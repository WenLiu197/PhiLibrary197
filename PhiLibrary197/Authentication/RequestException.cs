using System.Net;

namespace PhiLibrary197.Authentication;
/// <summary>
/// An exception that will be thrown to user if server sent unknown response.
/// </summary>
public class RequestException : Exception
{
	/// <summary>
	/// The kind of failure reported by the login endpoint.
	/// </summary>
	public enum FailingType
	{
		/// <summary>
		/// The user has not finished the authorization yet.
		/// </summary>
		Pending,
		/// <summary>
		/// The user denied the authorization.
		/// </summary>
		Denied,
		/// <summary>
		/// The server is waiting for the user to act.
		/// </summary>
		Waiting,
		/// <summary>
		/// No failure code was returned.
		/// </summary>
		None,
		/// <summary>
		/// The failure code is unknown.
		/// </summary>
		Unknown
	}

	/// <summary>
	/// The parsed failure kind, useful for callers that need to distinguish pending/denied states.
	/// </summary>
	public FailingType Failing { get; }

	/// <summary>
	/// The HTTP status code returned by the server.
	/// </summary>
	public HttpStatusCode HttpStatus { get; }

	internal RequestException(string message, HttpStatusCode code, FailingType type)
		: base(message)
	{
		this.Failing = type;
		this.HttpStatus = code;
	}

	/// <summary>
	/// Returns a <see cref="string"/> form of this instance.
	/// </summary>
	/// <returns>A <see cref="string"/> representing this instance.</returns>
	public override string ToString()
	{
		return $"{this.Failing}, {this.HttpStatus}: {this.Message}";
	}
}
