namespace PhiLibrary197.CloudSave.HttpModels;

/// <summary>
/// The simplified save info, trimmed down just for the basic (common) information 
/// of a save, which is more convenient to use in most cases.
/// </summary>
public class SimplifiedSaveInfo
{
	/// <summary>
	/// The URL of the game save.
	/// </summary>
	public required PhiCloudObj GameSave { get; set; }
	/// <summary>
	/// The creation date of the save.
	/// </summary>
	public required DateTime CreationDate { get; set; }
	/// <summary>
	/// The last modified time of the save.
	/// </summary>
	public required DateTime ModificationTime { get; set; }
	/// <summary>
	/// Player's summary in base64 format.
	/// </summary>
	public required string Summary { get; set; }
	/// <summary>
	/// The display name of the save, if available.
	/// </summary>
	public string? Name { get; init; }
	/// <summary>
	/// The cloud object id of the save, if available.
	/// </summary>
	public string? ObjectId { get; init; }
}
/// <summary>
/// A container containing the URL of object.
/// </summary>
public struct PhiCloudObj
{
	/// <summary>
	/// The URL of object.
	/// </summary>
	public required string? Url { get; set; }
}
