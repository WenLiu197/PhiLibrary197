namespace PhiLibrary197;

/// <summary>
/// An <see cref="ArgumentNullException"/> that includes an  
/// <inheritdoc cref="AdditionalMessage"/>
/// </summary>
public class DebugArgumentNullException : ArgumentNullException, IFormattable
{
	/// <summary>
	/// Additional message for debugging purposes. 
	/// The additional message by default will not be included in the standard <see cref="ToString()"/> output, 
	/// but can be accessed via the <see cref="ToStringDetailed"/> method, or use the <see cref="IFormattable"/> interface 
	/// with the "D" format specifier.
	/// </summary>
	public string? AdditionalMessage { get; }

	/// <summary>
	/// Construct a new instance of <see cref="DebugArgumentNullException"/> with the specified parameter name and an additional message.
	/// </summary>
	/// <param name="paramName">The name of the parameter that caused the exception.</param>
	/// <param name="additionalMessage">The additional message.</param>
	public DebugArgumentNullException(string? paramName, string? additionalMessage)
		: base(paramName, $"Object is null. Additional message will be included with the {nameof(ToStringDetailed)} method.")
	{
		this.AdditionalMessage = additionalMessage;
		this.Data.Add(nameof(this.AdditionalMessage), additionalMessage);
	}

	/// <summary>
	/// Creates a detailed string representation of the exception, including the additional message.
	/// </summary>
	/// <returns>A string representation of the exception, including the additional message.</returns>
	public string ToStringDetailed()
	{
		return $"{base.ToString()}\nAdditional message: {this.AdditionalMessage ?? "<none>"}";
	}
	/// <summary>
	/// Creates a string representation of the exception. The additional message will not be included in this output.
	/// </summary>
	/// <returns>A string representation of the exception.</returns>
	public override string ToString()
	{
		return base.ToString();
	}

	/// <summary>
	/// Creates a string representation of the exception, with the option to include the additional message if the "D" format specifier is used.
	/// </summary>
	/// <param name="format">The format specifier. Use "D" to include the additional message.</param>
	/// <param name="formatProvider">The format provider, unused.</param>
	/// <returns>A string representation of the exception.</returns>
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		if (format == "D")
		{
			return this.ToStringDetailed();
		}
		else
		{
			return this.ToString();
		}
	}
}
