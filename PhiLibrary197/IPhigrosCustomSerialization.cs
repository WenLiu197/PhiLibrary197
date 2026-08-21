namespace PhiLibrary197;

/// <summary>
/// Interface for Phigros custom serialization, which is used for some complex objects that 
/// cannot be serialized/deserialized by <see cref="BinaryReader"/> and <see cref="BinaryWriter"/> directly.
/// </summary>
/// <typeparam name="TSelf">The type to serialize to and deserialize from. Usually the class type itself.</typeparam>
public interface IPhigrosCustomSerialization<out TSelf>
{
	/// <summary>
	/// Constructs an object of type <typeparamref name="TSelf"/> from the given <see cref="BinaryReader"/>.
	/// The reader is expected to be at the correct position for reading the object, and should be at the end of the object after reading.
	/// </summary>
	/// <param name="reader">A <see cref="BinaryReader"/> to read data from.</param>
	/// <returns>A constructed instance of <typeparamref name="TSelf"/>.</returns>
	/// <param name="objectVersion">The version of the object being deserialized.</param>
	static abstract TSelf FromReader(BinaryReader reader, byte objectVersion);
	/// <summary>
	/// Serializes the current object to the given <see cref="BinaryWriter"/>. 
	/// The writer is expected to be at the correct position for writing the object, and should be at the end of the object after writing.
	/// </summary>
	/// <param name="writer">A <see cref="BinaryWriter"/> to write data to.</param>
	/// <param name="objectVersion">This will be set to the version of the object being serialized. 
	/// If the object does not have a version, it will be set to <see cref="byte.MaxValue"/>.</param>
	void Serialize(BinaryWriter writer, out byte objectVersion);
}
