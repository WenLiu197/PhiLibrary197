namespace PhiLibrary197.CloudSave;

/// <summary>
/// The summary of play counts, including cleared count, full combo count and phi count.
/// Note: This may not be accurate, please always calculate from <see cref="GameRecord"/>. 
/// </summary>
public struct PlayCountSummary : IPhigrosCustomSerialization<PlayCountSummary>
{
	/// <summary>
	/// The cleared count of current difficulty, including full combo and Phis.
	/// </summary>
	public short ClearedCount { get; set; }
	/// <summary>
	/// The full combo count of current difficulty, including Phis.
	/// </summary>
	public short FullComboCount { get; set; }
	/// <summary>
	/// The phi count of current difficulty.
	/// </summary>
	public short PhiCount { get; set; }

	/// <summary>
	/// Construct a new instance of <see cref="PlayCountSummary"/>.
	/// </summary>
	/// <param name="cleared">The cleared count of current difficulty, including full combo and Phis.</param>
	/// <param name="fullCombo">The full combo count of current difficulty, including Phis.</param>
	/// <param name="phi">The phi count of current difficulty.</param>
	public PlayCountSummary(short cleared, short fullCombo, short phi)
	{
		this.ClearedCount = cleared;
		this.FullComboCount = fullCombo;
		this.PhiCount = phi;
	}

	/// <inheritdoc/>
	public static PlayCountSummary FromReader(BinaryReader reader, byte objectVersion)
	{
		return new(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
	}
	/// <inheritdoc/>
	public readonly void Serialize(BinaryWriter writer, out byte objectVersion)
	{
		objectVersion = byte.MaxValue;

		writer.Write(this.ClearedCount);
		writer.Write(this.FullComboCount);
		writer.Write(this.PhiCount);
	}
}
/// <summary>
/// The player's summary.
/// </summary>
public class Summary : IPhigrosCustomSerialization<Summary>
{
	/// <summary>
	/// The default player <see cref="Summary"/>.
	/// </summary>
	public static Summary Default =>
		new(0, default, 0, default, "", default, default, default, default);

	/// <summary>
	/// The version of save.
	/// </summary>
	public byte SaveVersion { get; set; }
	/// <summary>
	/// The version of game.
	/// </summary>
	public int GameVersion { get; set; }
	/// <summary>
	/// The rks value of the player. Note: This may not be accurate, please always calculate from <see cref="GameRecord"/>.
	/// </summary>
	public float Rks { get; set; }
	/// <summary>
	/// The player challenge code, example: 123 <br/>
	/// 1 is the type of challenge, 0 = none, 1 = green... etc. <br/>
	/// And the 23 part is level.
	/// </summary>
	public Challenge Challenge { get; set; }
	/// <summary>
	/// Avatar id. Example: <c>Introduction</c>, <c>-SURREALISM-</c>
	/// </summary>
	public string Avatar { get; set; }
	/// <summary>
	/// The play count summary of easy difficulty.
	/// </summary>
	public PlayCountSummary EZPlayRecord { get; set; }
	/// <summary>
	/// The play count summary of hard difficulty.
	/// </summary>
	public PlayCountSummary HDPlayRecord { get; set; }
	/// <summary>
	/// The play count summary of insane difficulty.
	/// </summary>
	public PlayCountSummary INPlayRecord { get; set; }
	/// <summary>
	/// The play count summary of another difficulty.
	/// </summary>
	public PlayCountSummary ATPlayRecord { get; set; }

	/// <summary>
	/// Construct a new instance of <see cref="Summary"/>.
	/// </summary>
	/// <param name="saveVersion"></param>
	/// <param name="gameVersion"></param>
	/// <param name="challenge"></param>
	/// <param name="rks"></param>
	/// <param name="avatar"></param>
	/// <param name="ez"></param>
	/// <param name="hd"></param>
	/// <param name="in"></param>
	/// <param name="at"></param>
	public Summary(
		byte saveVersion,
		Challenge challenge,
		float rks,
		int gameVersion,
		string avatar,
		PlayCountSummary ez,
		PlayCountSummary hd,
		PlayCountSummary @in,
		PlayCountSummary at)
	{
		this.SaveVersion = saveVersion;
		this.GameVersion = gameVersion;
		this.Rks = rks;
		this.Challenge = challenge;
		this.Avatar = avatar;
		this.EZPlayRecord = ez;
		this.HDPlayRecord = hd;
		this.INPlayRecord = @in;
		this.ATPlayRecord = at;
	}

	/// <inheritdoc/>
	public static Summary FromReader(BinaryReader reader, byte objectVersion)
	{
		return new(
			reader.ReadByte(),
			Challenge.FromReader(reader, 0),
			reader.ReadSingle(),
			reader.Read7BitEncodedInt(),
			reader.ReadString(),
			PlayCountSummary.FromReader(reader, 0),
			PlayCountSummary.FromReader(reader, 0),
			PlayCountSummary.FromReader(reader, 0),
			PlayCountSummary.FromReader(reader, 0));
	}
	/// <inheritdoc/>
	public void Serialize(BinaryWriter writer, out byte objectVersion)
	{
		objectVersion = byte.MaxValue;

		writer.Write(this.SaveVersion);
		this.Challenge.Serialize(writer, out _);
		writer.Write(this.Rks);
		writer.Write7BitEncodedInt(this.GameVersion);
		writer.Write(this.Avatar);
		this.EZPlayRecord.Serialize(writer, out _);
		this.HDPlayRecord.Serialize(writer, out _);
		this.INPlayRecord.Serialize(writer, out _);
		this.ATPlayRecord.Serialize(writer, out _);
	}
}
