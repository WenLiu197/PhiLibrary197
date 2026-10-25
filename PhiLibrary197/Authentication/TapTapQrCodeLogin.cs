namespace PhiLibrary197.Authentication;

/// <summary>
/// The state of a TapTap QRCode login poll.
/// </summary>
public enum TapTapQrCodeStatus
{
	/// <summary>
	/// The user has not authorized the login yet.
	/// </summary>
	Pending,
	/// <summary>
	/// The server is still waiting for the user action.
	/// </summary>
	Waiting,
	/// <summary>
	/// The client should increase its polling interval.
	/// </summary>
	SlowDown,
	/// <summary>
	/// The user denied the authorization.
	/// </summary>
	Denied,
	/// <summary>
	/// The QRCode or device code has expired.
	/// </summary>
	Expired,
	/// <summary>
	/// Authorization succeeded and <see cref="TapTapQrCodeResult.Token"/> is available.
	/// </summary>
	Success,
	/// <summary>
	/// The server returned an unrecognized state.
	/// </summary>
	Unknown
}

/// <summary>
/// The result of one TapTap QRCode login poll.
/// </summary>
/// <param name="Status">The current QRCode login state.</param>
/// <param name="Token">The token data when <paramref name="Status"/> is <see cref="TapTapQrCodeStatus.Success"/>; otherwise <see langword="null"/>.</param>
public readonly record struct TapTapQrCodeResult(TapTapQrCodeStatus Status, TapTapTokenData? Token);

/// <summary>
/// A prefabricated TapTap QRCode login session.
/// </summary>
/// <remarks>
/// <para>
/// 创建后展示 <see cref="Url"/>（或把它生成二维码）给用户扫描，然后等待
/// <see cref="WaitForTokenAsync"/>。调用方也可以使用 <see cref="PollAsync"/>
/// 自己控制轮询节奏和状态展示。
/// </para>
/// <para>
/// 该类型只负责协议轮询，不生成二维码图片，也不输出任何 UI。
/// </para>
/// </remarks>
public sealed class TapTapQrCodeLogin
{
	private readonly CompleteQRCodeData _qrCode;
	private readonly bool _useChinaEndpoint;

	private TapTapQrCodeLogin(CompleteQRCodeData qrCode, bool useChinaEndpoint)
	{
		this._qrCode = qrCode;
		this._useChinaEndpoint = useChinaEndpoint;
	}

	/// <summary>
	/// The QRCode request data returned by TapTap.
	/// </summary>
	public CompleteQRCodeData QrCode => this._qrCode;

	/// <summary>
	/// The URL the user should open or encode as a QRCode.
	/// </summary>
	public string Url => this._qrCode.Url;

	/// <summary>
	/// The QRCode lifetime in seconds.
	/// </summary>
	public int ExpiresInSeconds => this._qrCode.ExpiresInSeconds;

	/// <summary>
	/// Requests a new TapTap QRCode login session.
	/// </summary>
	/// <param name="permissions">OAuth permissions; default <c>public_profile</c>.</param>
	/// <param name="useChinaEndpoint">Use China endpoints or not.</param>
	/// <param name="ct">The cancellation token to cancel the request.</param>
	/// <returns>A new QRCode login session.</returns>
	public static async Task<TapTapQrCodeLogin> CreateAsync(
		string[]? permissions = null,
		bool useChinaEndpoint = true,
		CancellationToken ct = default)
	{
		CompleteQRCodeData qrCode = await TapTapHelper.RequestLoginQrCode(permissions, useChinaEndpoint, ct).ConfigureAwait(false);
		return new TapTapQrCodeLogin(qrCode, useChinaEndpoint);
	}

	/// <summary>
	/// Performs one QRCode login poll.
	/// </summary>
	/// <param name="ct">The cancellation token to cancel the request.</param>
	/// <returns>The current QRCode login status.</returns>
	public Task<TapTapQrCodeResult> PollAsync(CancellationToken ct = default)
		=> TapTapHelper.CheckQRCodeResultDetailed(this._qrCode, this._useChinaEndpoint, ct);

	/// <summary>
	/// Polls until the QRCode login succeeds, is denied, or expires.
	/// </summary>
	/// <param name="ct">The cancellation token to stop polling.</param>
	/// <returns>The TapTap token data on success.</returns>
	/// <exception cref="InvalidOperationException">The login was denied or returned an unknown state.</exception>
	/// <exception cref="TimeoutException">The QRCode expired before authorization completed.</exception>
	/// <exception cref="OperationCanceledException">The operation was cancelled.</exception>
	public async Task<TapTapTokenData> WaitForTokenAsync(CancellationToken ct = default)
	{
		int intervalSeconds = Math.Max(1, this._qrCode.Interval);
		DateTimeOffset expiresAt = DateTimeOffset.UtcNow.AddSeconds(Math.Max(1, this._qrCode.ExpiresInSeconds));

		while (true)
		{
			TapTapQrCodeResult result = await this.PollAsync(ct).ConfigureAwait(false);
			switch (result.Status)
			{
				case TapTapQrCodeStatus.Success:
					return result.Token ?? throw new InvalidOperationException("TapTap QR login succeeded without token data.");
				case TapTapQrCodeStatus.Denied:
					throw new InvalidOperationException("TapTap QR login was denied.");
				case TapTapQrCodeStatus.Expired:
					throw new TimeoutException("TapTap QR code expired.");
				case TapTapQrCodeStatus.Unknown:
					throw new InvalidOperationException("TapTap QR login returned an unknown state.");
				case TapTapQrCodeStatus.SlowDown:
					intervalSeconds = Math.Min(intervalSeconds + 5, 30);
					break;
				case TapTapQrCodeStatus.Pending:
				case TapTapQrCodeStatus.Waiting:
					break;
			}

			TimeSpan remaining = expiresAt - DateTimeOffset.UtcNow;
			if (remaining <= TimeSpan.Zero)
				throw new TimeoutException("TapTap QR code expired.");

			TimeSpan delay = TimeSpan.FromSeconds(intervalSeconds);
			if (delay > remaining)
				delay = remaining;

			await Task.Delay(delay, ct).ConfigureAwait(false);
		}
	}
}
