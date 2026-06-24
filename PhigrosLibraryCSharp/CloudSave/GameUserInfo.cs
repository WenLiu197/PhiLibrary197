namespace PhigrosLibraryCSharp.CloudSave;

/// <summary>
/// The user's info in game.
/// </summary>
public class GameUserInfo : IPhigrosCustomSerialization<GameUserInfo>
{
	/// <summary>
	/// Gets or sets the version of user info file. Latest: 1.
	/// </summary>
	public byte Version { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the user has the user name expanded.
	/// </summary>
	public bool ShowUserId { get; set; }

	/// <summary>
	/// Gets or sets the user's intro.
	/// </summary>
	public string Intro { get; set; }

	/// <summary>
	/// Gets or sets the user's avatar id.
	/// </summary>
	public string AvatarId { get; set; }

	/// <summary>
	/// Gets or sets the user's background id.
	/// </summary>
	public string BackgroundId { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="GameUserInfo"/> class.
	/// </summary>
	/// <param name="version">The version of user info file.</param>
	/// <param name="showUserId"><see langword="true"/> if user has the user name expanded, otherwise <see langword="false"/>.</param>
	/// <param name="intro">User's intro.</param>
	/// <param name="avatarId">User's avatar id.</param>
	/// <param name="backgroundId">User's background id.</param>
	public GameUserInfo(byte version, bool showUserId, string intro, string avatarId, string backgroundId)
	{
		this.Version = version;
		this.ShowUserId = showUserId;
		this.Intro = intro;
		this.AvatarId = avatarId;
		this.BackgroundId = backgroundId;
	}

	/// <inheritdoc/>
	public static GameUserInfo FromReader(BinaryReader reader, byte objectVersion)
	{
		//string tmp;
		return new(
			objectVersion,
			reader.ReadBoolean(),
			reader.ReadString(),
			reader.ReadString(),
			reader.ReadString());
		//string.IsNullOrWhiteSpace(tmp = reader.ReadString()) ? "Introduction" : tmp,
	}
	/// <inheritdoc/>
	public void Serialize(BinaryWriter writer, out byte objectVersion)
	{
		objectVersion = this.Version;

		writer.Write(this.ShowUserId);
		writer.Write(this.Intro);
		writer.Write(this.AvatarId);
		writer.Write(this.BackgroundId);
	}
}
