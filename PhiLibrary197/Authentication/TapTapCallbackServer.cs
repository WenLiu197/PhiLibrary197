using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PhiLibrary197.Authentication;

/// <summary>
/// The result of a local OAuth callback.
/// </summary>
/// <param name="Code">The OAuth authorization code.</param>
/// <param name="State">The state value that was validated against the login request.</param>
public readonly record struct TapTapCallbackResult(string Code, string State);

/// <summary>
/// A loopback HTTP callback server for TapTap callback login.
/// </summary>
/// <remarks>
/// <para>
/// 该类型只监听 <c>localhost</c> 的 IPv4/IPv6 回环地址，不会暴露到局域网；
/// 它负责生成回调登录 URL、接收一次 OAuth 回调、校验 <c>state</c>，
/// 并可选地直接换取 TapTap token。
/// </para>
/// <para>
/// 浏览器不会自动打开，调用方应展示 <see cref="BeginUrl"/> 或自行打开它。
/// 服务端返回一个简单的 HTML 结果页，不需要调用方实现 HTTP 监听器。
/// </para>
/// </remarks>
public sealed class TapTapCallbackServer : IDisposable
{
	private const string DefaultPath = "/callback";

	private readonly TcpListener _listenerV4;
	private readonly TcpListener? _listenerV6;
	private readonly string _path;
	private readonly bool _useChinaEndpoint;
	private bool _disposed;

	private TapTapCallbackServer(
		TcpListener listenerV4,
		TcpListener? listenerV6,
		string path,
		bool useChinaEndpoint,
		CallbackLoginData loginData,
		int port)
	{
		this._listenerV4 = listenerV4;
		this._listenerV6 = listenerV6;
		this._path = path;
		this._useChinaEndpoint = useChinaEndpoint;
		this.LoginData = loginData;
		this.Port = port;
	}

	/// <summary>
	/// The callback login data generated for this server.
	/// </summary>
	public CallbackLoginData LoginData { get; }

	/// <summary>
	/// The actual local port, useful when <c>port = 0</c> was requested.
	/// </summary>
	public int Port { get; }

	/// <summary>
	/// The callback URL registered with TapTap.
	/// </summary>
	public string CallbackUrl => this.LoginData.RedirectUrl;

	/// <summary>
	/// The URL the user should open to begin authorization.
	/// </summary>
	public string BeginUrl => this.LoginData.BeginUrl;

	/// <summary>
	/// Starts a loopback callback server and generates the login URL.
	/// </summary>
	/// <param name="port">Local loopback port, or <c>0</c> to let the OS choose one.</param>
	/// <param name="useChinaEndpoint">Use China endpoints or not.</param>
	/// <param name="path">Callback path, default <c>/callback</c>.</param>
	/// <param name="permissions">OAuth permissions; default <c>public_profile</c>.</param>
	/// <returns>A running callback server.</returns>
	public static TapTapCallbackServer Start(
		int port = 0,
		bool useChinaEndpoint = true,
		string path = DefaultPath,
		string[]? permissions = null)
	{
		if (port is < 0 or > 65535)
			throw new ArgumentOutOfRangeException(nameof(port), port, "Port must be between 0 and 65535.");

		path = string.IsNullOrWhiteSpace(path) ? DefaultPath : path;
		if (!path.StartsWith('/'))
			path = "/" + path;

		TcpListener listenerV4 = new(IPAddress.Loopback, port);
		TcpListener? listenerV6 = null;
		try
		{
			listenerV4.Start(1);
			int actualPort = ((IPEndPoint)listenerV4.LocalEndpoint).Port;

			// localhost may resolve to ::1 or 127.0.0.1 depending on the browser/system.
			// IPv6 is optional; if the platform cannot bind ::1, IPv4 still works.
			try
			{
				listenerV6 = new TcpListener(IPAddress.IPv6Loopback, actualPort);
				listenerV6.Server.DualMode = false;
				listenerV6.Start(1);
			}
			catch (Exception ex) when (ex is SocketException or NotSupportedException or PlatformNotSupportedException)
			{
				listenerV6?.Stop();
				listenerV6 = null;
			}

			string callbackUrl = $"http://localhost:{actualPort}{path}";
			CallbackLoginData loginData = TapTapHelper.GenerateCallbackLoginUrl(callbackUrl, useChinaEndpoint, permissions);
			return new TapTapCallbackServer(listenerV4, listenerV6, path, useChinaEndpoint, loginData, actualPort);
		}
		catch
		{
			listenerV4.Stop();
			listenerV6?.Stop();
			throw;
		}
	}

	/// <summary>
	/// Waits for one valid OAuth callback and returns its code and state.
	/// </summary>
	/// <param name="ct">The cancellation token to stop waiting.</param>
	/// <returns>The validated callback result.</returns>
	/// <exception cref="InvalidOperationException">The callback contained an OAuth error.</exception>
	/// <exception cref="OperationCanceledException">The operation was cancelled.</exception>
	public async Task<TapTapCallbackResult> WaitForCallbackAsync(CancellationToken ct = default)
	{
		if (this._disposed)
			throw new ObjectDisposedException(nameof(TapTapCallbackServer));

		using CancellationTokenSource acceptCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
		Task<TcpClient> v4Task = this._listenerV4.AcceptTcpClientAsync(acceptCts.Token).AsTask();
		Task<TcpClient>? v6Task = this._listenerV6?.AcceptTcpClientAsync(acceptCts.Token).AsTask();

		try
		{
			while (true)
			{
				Task<TcpClient> completed = v6Task is null
					? v4Task
					: await Task.WhenAny(v4Task, v6Task).ConfigureAwait(false);

				using TcpClient client = await completed.ConfigureAwait(false);
				if (ReferenceEquals(completed, v4Task))
					v4Task = this._listenerV4.AcceptTcpClientAsync(acceptCts.Token).AsTask();
				else if (v6Task is not null)
					v6Task = this._listenerV6!.AcceptTcpClientAsync(acceptCts.Token).AsTask();

				TapTapCallbackResult? result = await HandleClientAsync(client, acceptCts.Token).ConfigureAwait(false);
				if (result.HasValue)
					return result.Value;
			}
		}
		finally
		{
			acceptCts.Cancel();
			await IgnoreAcceptExceptionAsync(v4Task).ConfigureAwait(false);
			await IgnoreAcceptExceptionAsync(v6Task).ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Waits for one valid callback and exchanges its code for TapTap token data.
	/// </summary>
	/// <param name="ct">The cancellation token to stop waiting or cancel the exchange.</param>
	/// <returns>The TapTap token data.</returns>
	public async Task<TapTapTokenData> WaitForTokenAsync(CancellationToken ct = default)
	{
		TapTapCallbackResult result = await this.WaitForCallbackAsync(ct).ConfigureAwait(false);
		return await TapTapHelper.HandleCallbackLogin(
			this.LoginData,
			result.Code,
			result.State,
			this._useChinaEndpoint,
			ct).ConfigureAwait(false);
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (this._disposed)
			return;

		this._disposed = true;
		this._listenerV4.Stop();
		this._listenerV6?.Stop();
	}

	private async Task<TapTapCallbackResult?> HandleClientAsync(TcpClient client, CancellationToken ct)
	{
		using NetworkStream stream = client.GetStream();
		using StreamReader reader = new(stream, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);

		string? requestLine = await reader.ReadLineAsync(ct).ConfigureAwait(false);
		if (string.IsNullOrWhiteSpace(requestLine))
			return null;

		// Consume headers; GET has no body.
		string? header;
		while (!string.IsNullOrEmpty(header = await reader.ReadLineAsync(ct).ConfigureAwait(false)))
		{
		}

		string[] parts = requestLine.Split(' ');
		if (parts.Length < 2 || !parts[0].Equals("GET", StringComparison.OrdinalIgnoreCase))
		{
			await WriteResponseAsync(stream, 405, "Method Not Allowed", "Only GET is supported.", ct).ConfigureAwait(false);
			return null;
		}

		if (!TryParseTarget(parts[1], out string requestPath, out Dictionary<string, string> query))
		{
			await WriteResponseAsync(stream, 400, "Bad Request", "Invalid callback URL.", ct).ConfigureAwait(false);
			return null;
		}

		if (!requestPath.Equals(this._path, StringComparison.Ordinal))
		{
			await WriteResponseAsync(stream, 404, "Not Found", "Not a callback endpoint.", ct).ConfigureAwait(false);
			return null;
		}

		if (query.TryGetValue("error", out string? error))
		{
			string description = query.GetValueOrDefault("error_description") ?? error;
			await WriteResponseAsync(stream, 400, "Login Failed", description, ct).ConfigureAwait(false);
			throw new InvalidOperationException($"TapTap login failed: {description}");
		}

		if (!query.TryGetValue("code", out string? code) || string.IsNullOrEmpty(code))
		{
			await WriteResponseAsync(stream, 400, "Bad Request", "Missing authorization code.", ct).ConfigureAwait(false);
			return null;
		}

		if (!query.TryGetValue("state", out string? state)
			|| !string.Equals(state, this.LoginData.State, StringComparison.Ordinal))
		{
			await WriteResponseAsync(stream, 400, "Bad Request", "Invalid OAuth state.", ct).ConfigureAwait(false);
			return null;
		}

		await WriteResponseAsync(stream, 200, "Login Successful", "Login successful. You can close this window.", ct).ConfigureAwait(false);
		return new TapTapCallbackResult(code, state);
	}

	private static bool TryParseTarget(string target, out string path, out Dictionary<string, string> query)
	{
		path = string.Empty;
		query = [];

		if (Uri.TryCreate(target, UriKind.Absolute, out Uri? absolute)
			&& (absolute.Scheme == Uri.UriSchemeHttp || absolute.Scheme == Uri.UriSchemeHttps))
		{
			path = absolute.AbsolutePath;
			ParseQuery(absolute.Query.TrimStart('?'), query);
			return true;
		}

		int queryIndex = target.IndexOf('?');
		if (queryIndex >= 0)
		{
			path = target[..queryIndex];
			ParseQuery(target[(queryIndex + 1)..], query);
		}
		else
		{
			path = target;
		}

		return path.Length > 0;
	}

	private static void ParseQuery(string queryString, Dictionary<string, string> query)
	{
		foreach (string pair in queryString.Split('&', StringSplitOptions.RemoveEmptyEntries))
		{
			int equals = pair.IndexOf('=');
			string key = equals >= 0 ? pair[..equals] : pair;
			string value = equals >= 0 ? pair[(equals + 1)..] : string.Empty;
			key = WebUtility.UrlDecode(key);
			value = WebUtility.UrlDecode(value);
			if (key.Length > 0)
				query[key] = value;
		}
	}

	private static async Task WriteResponseAsync(
		NetworkStream stream,
		int statusCode,
		string reason,
		string message,
		CancellationToken ct)
	{
		string html = $"""
			<!doctype html>
			<html><head><meta charset="utf-8"><title>TapTap Login</title></head>
			<body><h3>{WebUtility.HtmlEncode(reason)}</h3><p>{WebUtility.HtmlEncode(message)}</p></body></html>
			""";
		byte[] body = Encoding.UTF8.GetBytes(html);
		string headers = $"HTTP/1.1 {statusCode} {reason}\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n";
		await stream.WriteAsync(Encoding.ASCII.GetBytes(headers), ct).ConfigureAwait(false);
		await stream.WriteAsync(body, ct).ConfigureAwait(false);
		await stream.FlushAsync(ct).ConfigureAwait(false);
	}

	private static async Task IgnoreAcceptExceptionAsync(Task<TcpClient>? acceptTask)
	{
		if (acceptTask is null)
			return;

		try
		{
			(await acceptTask.ConfigureAwait(false)).Dispose();
		}
		catch (OperationCanceledException)
		{
		}
		catch (ObjectDisposedException)
		{
		}
		catch (SocketException)
		{
		}
	}
}
