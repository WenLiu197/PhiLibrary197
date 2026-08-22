using PhiLibrary197.CloudSave;
using PhiLibrary197.LocalSave;
using System.Globalization;

namespace PhiLibrary197;

/// <summary>
/// A helper class for doing <see cref="ScoreStatus"/> related operations.
/// </summary>
public static class ScoreHelper
{
	/// <summary>
	/// Get <see cref="ScoreStatus"/> of a raw record.
	/// </summary>
	/// <param name="record">The game record.</param>
	/// <returns>The status of the record.</returns>
	public static ScoreStatus ParseStatus(RawScore record)
		=> ParseStatus(record.Score, record.Accuracy, record.Status == ScoreStatus.Fc);
	/// <summary>
	/// Get <see cref="ScoreStatus"/> from score, accuracy and full combo status.
	/// </summary>
	/// <param name="accuracy">The accuracy of the score, ex. 11.45, 99.114514, 100</param>
	/// <param name="isFc">If full combo'ed, <see langword="true"/>, otherwise <see langword="false"/>.</param>
	/// <param name="score">The score, ex. 920000, 1000000, 69420, 1145</param>
	/// <returns>The status of the record.</returns>
	public static ScoreStatus ParseStatus(int score, double accuracy, bool isFc)
	{
		if (accuracy == 100)
		{
			if (score == 1000000) { return ScoreStatus.Phi; }
			return ScoreStatus.Bugged;
		}
		if (isFc) { return ScoreStatus.Fc; }
		if (score >= 960000) { return ScoreStatus.Vu; }
		if (score >= 920000) { return ScoreStatus.S; }
		if (score >= 880000) { return ScoreStatus.A; }
		if (score >= 820000) { return ScoreStatus.B; }
		if (score >= 700000) { return ScoreStatus.C; }
		if (score >= 0) { return ScoreStatus.False; }
		return ScoreStatus.Bugged;
	}
	/// <summary>
	/// Convert difficulty string to index, ex. EZ, HD, IN...
	/// </summary>
	/// <param name="diff">Difficulty string, ex EZ, HD, IN...</param>
	/// <returns>A <see cref="byte"/> presenting the difficulty index.</returns>
	public static byte DifficultyStringToIndex(string diff)
	{
		return (byte)(int)Enum.GetValues<Difficulty>().First(x => x.ToString().Equals(diff, StringComparison.OrdinalIgnoreCase));
	}

	/// <summary>
	/// Loads a chart constant table from a <c>difficulty.tsv</c> file.
	/// </summary>
	/// <remarks>
	/// The tsv format is <c>songName\tEZ\tHD\tIN\tAT</c>, one line per song. Columns may be omitted
	/// (a song without an AT chart simply has no AT column) and the table stays sparse — missing keys
	/// are only an issue if the save actually contains a score for that (song, difficulty) pair.
	/// <para>
	/// Line handling: empty lines, <c>#</c>/<c>//</c> comments and lines without at least one constant
	/// column are skipped; unparsable numeric cells are skipped. The song name gets the conventional
	/// <c>.0</c> suffix appended to form the full song id (e.g. <c>Glaciaxion.SunsetRay</c> →
	/// <c>Glaciaxion.SunsetRay.0</c>).
	/// </para>
	/// </remarks>
	/// <param name="tsvPath">Path to the difficulty.tsv file.</param>
	/// <returns>A mapping from <see cref="ChartConstantKey"/> to chart constant.</returns>
	public static Dictionary<ChartConstantKey, float> LoadConstantTable(string tsvPath)
	{
		Dictionary<ChartConstantKey, float> map = [];
		Difficulty[] difficulties = [Difficulty.EZ, Difficulty.HD, Difficulty.IN, Difficulty.AT];

		foreach (string rawLine in File.ReadLines(tsvPath))
		{
			string line = rawLine.Trim();
			if (line.Length == 0 || line.StartsWith('#') || line.StartsWith("//")) continue;

			string[] cols = line.Split('\t');
			if (cols.Length < 2) continue; // no constant column, skip

			string id = cols[0] + ".0";
			for (int i = 1; i < cols.Length && i <= difficulties.Length; i++)
			{
				if (float.TryParse(cols[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float constant))
					map[new ChartConstantKey(id, difficulties[i - 1])] = constant;
			}
		}

		return map;
	}

	/// <summary>
	/// 从 info.tsv 加载歌名表（完整曲目 id → 显示名）。
	/// </summary>
	/// <remarks>
	/// info.tsv 每行第一列是曲目 id 前缀（如 <c>Glaciaxion.SunsetRay</c>），第二列是显示名
	/// （如 <c>Glaciaxion</c>）；其余列（曲师/画师/谱师等元数据）本方法忽略。
	/// 曲目名自动补 <c>.0</c> 后缀形成完整 id，与 <see cref="LoadConstantTable"/> 的键对齐。
	/// 空行、<c>#</c>/<c>//</c> 注释行、以及不足两列的行会被跳过。
	/// </remarks>
	/// <param name="tsvPath">info.tsv 文件路径。</param>
	/// <returns>完整曲目 id 到显示名的映射。</returns>
	public static Dictionary<string, string> LoadSongInfo(string tsvPath)
	{
		Dictionary<string, string> map = [];

		foreach (string rawLine in File.ReadLines(tsvPath))
		{
			string line = rawLine.Trim();
			if (line.Length == 0 || line.StartsWith('#') || line.StartsWith("//")) continue;

			string[] cols = line.Split('\t');
			if (cols.Length < 2) continue; // 没有显示名列，跳过

			map[cols[0] + ".0"] = cols[1];
		}

		return map;
	}
}
