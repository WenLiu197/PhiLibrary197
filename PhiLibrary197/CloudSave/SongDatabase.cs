using System.Globalization;

namespace PhiLibrary197.CloudSave;

/// <summary>
/// 单曲完整信息，由 difficulty.tsv 与 info.tsv 按曲目 id 前缀合并而成。
/// </summary>
/// <param name="Id">完整曲目 id，如 <c>Glaciaxion.SunsetRay.0</c>。</param>
/// <param name="Name">显示名（info.tsv 第 2 列），表中缺失时为 <see langword="null"/>。</param>
/// <param name="Composer">曲师（info.tsv 第 3 列）。</param>
/// <param name="Illustrator">曲绘画师（info.tsv 第 4 列）。</param>
/// <param name="EzCharter">EZ 谱师（info.tsv 第 5 列），老曲可能没有。</param>
/// <param name="HdCharter">HD 谱师（info.tsv 第 6 列）。</param>
/// <param name="InCharter">IN 谱师（info.tsv 第 7 列）。</param>
/// <param name="AtCharter">AT 谱师（info.tsv 第 8 列），无 AT 谱的曲目为 <see langword="null"/>。</param>
/// <param name="Constants">各难度定数（difficulty.tsv），仅收录表中存在的难度。</param>
public sealed record SongEntry(
	string Id,
	string? Name,
	string? Composer,
	string? Illustrator,
	string? EzCharter,
	string? HdCharter,
	string? InCharter,
	string? AtCharter,
	IReadOnlyDictionary<Difficulty, float> Constants)
{
	/// <summary>
	/// 取指定难度的定数，表中不存在该难度时返回 <see langword="null"/>。
	/// </summary>
	public float? GetConstant(Difficulty difficulty)
		=> this.Constants.TryGetValue(difficulty, out float c) ? c : null;
}

/// <summary>
/// 曲目数据库：合并 difficulty.tsv（定数）与 info.tsv（歌名/曲师/画师/谱师），支持按 id 或显示名查询。
/// 两表以曲目 id 前缀为键做并集合并，任一表缺失的字段为 <see langword="null"/>（引用类型）或空表（定数）。
/// </summary>
public sealed class SongDatabase
{
	private readonly Dictionary<string, SongEntry> _byId;

	private SongDatabase(Dictionary<string, SongEntry> byId)
	{
		this._byId = byId;
	}

	/// <summary>
	/// 加载曲目数据库。
	/// </summary>
	/// <remarks>
	/// <para>difficulty.tsv 每行：<c>曲目id前缀\tEZ\tHD\tIN\tAT</c>（定数列按存在情况可选）。</para>
	/// <para>info.tsv 每行：<c>曲目id前缀\t显示名\t曲师\t画师\tEZ谱师\tHD谱师\tIN谱师\tAT谱师</c>（后列可选）。</para>
	/// 两表的容错规则一致：空行、<c>#</c>/<c>//</c> 注释行跳过；列不足的字段留 <see langword="null"/>；
	/// 数值解析失败跳过该列；曲目前缀自动补 <c>.0</c> 后缀形成完整 id。
	/// </remarks>
	/// <param name="difficultyTsvPath">difficulty.tsv 路径。</param>
	/// <param name="infoTsvPath">info.tsv 路径。</param>
	/// <returns>构建好的 <see cref="SongDatabase"/>。</returns>
	public static SongDatabase Load(string difficultyTsvPath, string infoTsvPath)
	{
		string[] difficultyLines = File.ReadAllLines(difficultyTsvPath);
		string[] infoLines = File.ReadAllLines(infoTsvPath);

		Dictionary<string, SongInfoRow> infos = ParseInfoRows(infoLines);
		Dictionary<string, Dictionary<Difficulty, float>> constants = ParseConstantRows(difficultyLines);

		Dictionary<string, SongEntry> byId = [];
		foreach (string prefix in infos.Keys.Union(constants.Keys))
		{
			string id = prefix + ".0";
			SongInfoRow info = infos.GetValueOrDefault(prefix);
			byId[id] = new SongEntry(
				id,
				info.Name,
				info.Composer,
				info.Illustrator,
				info.EzCharter,
				info.HdCharter,
				info.InCharter,
				info.AtCharter,
				constants.GetValueOrDefault(prefix) ?? []);
		}

		return new SongDatabase(byId);
	}

	private readonly record struct SongInfoRow(
		string? Name,
		string? Composer,
		string? Illustrator,
		string? EzCharter,
		string? HdCharter,
		string? InCharter,
		string? AtCharter);

	private static Dictionary<string, SongInfoRow> ParseInfoRows(string[] lines)
	{
		Dictionary<string, SongInfoRow> rows = [];
		foreach (string rawLine in lines)
		{
			string line = rawLine.Trim();
			if (line.Length == 0 || line.StartsWith('#') || line.StartsWith("//")) continue;

			string[] cols = line.Split('\t');
			if (cols.Length < 2) continue; // 没有显示名，跳过

			rows[cols[0]] = new SongInfoRow(
				cols[1],
				cols.Length > 2 ? cols[2] : null,
				cols.Length > 3 ? cols[3] : null,
				cols.Length > 4 ? cols[4] : null,
				cols.Length > 5 ? cols[5] : null,
				cols.Length > 6 ? cols[6] : null,
				cols.Length > 7 ? cols[7] : null);
		}
		return rows;
	}

	private static Dictionary<string, Dictionary<Difficulty, float>> ParseConstantRows(string[] lines)
	{
		Dictionary<string, Dictionary<Difficulty, float>> rows = [];
		Difficulty[] difficulties = [Difficulty.EZ, Difficulty.HD, Difficulty.IN, Difficulty.AT];

		foreach (string rawLine in lines)
		{
			string line = rawLine.Trim();
			if (line.Length == 0 || line.StartsWith('#') || line.StartsWith("//")) continue;

			string[] cols = line.Split('\t');
			if (cols.Length < 2) continue; // 没有定数列，跳过

			Dictionary<Difficulty, float> map = [];
			for (int i = 1; i < cols.Length && i <= difficulties.Length; i++)
			{
				if (float.TryParse(cols[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float c))
					map[difficulties[i - 1]] = c;
			}
			rows[cols[0]] = map;
		}
		return rows;
	}

	/// <summary>
	/// 全部曲目，按完整 id 排序。
	/// </summary>
	public IReadOnlyList<SongEntry> Songs => this._byId.Values.OrderBy(x => x.Id).ToArray();

	/// <summary>
	/// 曲目数量。
	/// </summary>
	public int Count => this._byId.Count;

	/// <summary>
	/// 按完整曲目 id 查询（也接受不带 <c>.0</c> 后缀的前缀形式）。
	/// </summary>
	/// <param name="id">完整 id 或 id 前缀。</param>
	/// <returns>匹配的曲目，不存在返回 <see langword="null"/>。</returns>
	public SongEntry? GetSong(string id)
		=> this._byId.TryGetValue(id, out SongEntry? entry)
			? entry
			: this._byId.GetValueOrDefault(id + ".0");

	/// <summary>
	/// 按显示名搜索（不区分大小写），返回所有显示名匹配的曲目。
	/// </summary>
	/// <param name="name">显示名关键字。</param>
	/// <returns>匹配的曲目集合（可能为空）。</returns>
	public IEnumerable<SongEntry> FindByName(string name)
		=> this._byId.Values.Where(x => x.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);

	/// <summary>
	/// 导出定数表（与 <see cref="ScoreHelper.LoadConstantTable"/> 同构），供 <see cref="GameRecord.GetSortedListForRks(IReadOnlyDictionary{ChartConstantKey, float}, IReadOnlyDictionary{string, string})"/> 使用。
	/// </summary>
	public Dictionary<ChartConstantKey, float> ToConstantMap()
	{
		Dictionary<ChartConstantKey, float> map = [];
		foreach (SongEntry song in this._byId.Values)
		{
			foreach ((Difficulty difficulty, float constant) in song.Constants)
				map[new ChartConstantKey(song.Id, difficulty)] = constant;
		}
		return map;
	}

	/// <summary>
	/// 导出歌名表（与 <see cref="ScoreHelper.LoadSongInfo"/> 同构），供 <see cref="GameRecord.GetSortedListForRks(IReadOnlyDictionary{ChartConstantKey, float}, IReadOnlyDictionary{string, string})"/> 使用。
	/// 显示名为 <see langword="null"/> 的曲目不纳入（RKS 输出会回退显示 id）。
	/// </summary>
	public Dictionary<string, string> ToNameMap()
	{
		Dictionary<string, string> map = [];
		foreach (SongEntry song in this._byId.Values)
		{
			if (song.Name is not null)
				map[song.Id] = song.Name;
		}
		return map;
	}
}
