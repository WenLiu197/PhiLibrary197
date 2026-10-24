using System.Collections;
using System.Globalization;

namespace PhiLibrary197.CloudSave;

/// <summary>
/// 曲目 id 的工具方法。Phigros 的曲目 id 形如 <c>Glaciaxion.SunsetRay.0</c>，
/// 末尾的数字后缀表示同一曲目的不同谱面版本（例如 <c>Random.SobremSilentroom.1</c>）。
/// </summary>
internal static class SongIdUtility
{
	/// <summary>
	/// 尝试去掉 id 末尾的数字版本后缀。
	/// </summary>
	/// <param name="id">完整曲目 id，例如 <c>Random.SobremSilentroom.3</c>。</param>
	/// <param name="baseId">去掉末尾 <c>.<i>数字</i></c> 后的前缀；没有后缀时与 <paramref name="id"/> 相同。</param>
	/// <returns>存在数字后缀时为 <see langword="true"/>。</returns>
	internal static bool TryGetBaseId(string id, out string baseId)
	{
		int dot = id.LastIndexOf('.');
		if (dot > 0 && dot < id.Length - 1)
		{
			ReadOnlySpan<char> suffix = id.AsSpan(dot + 1);
			if (int.TryParse(suffix, NumberStyles.None, CultureInfo.InvariantCulture, out _))
			{
				baseId = id[..dot];
				return true;
			}
		}

		baseId = id;
		return false;
	}
}

/// <summary>
/// 在定数表查找失败时，尝试把变体 id（如 <c>Song.1</c>）回退到基础 id（<c>Song.0</c>）的只读字典视图。
/// 仅用于 <see cref="CompleteScore"/> 的 RKS/名称解析，不改变 <see cref="GameRecord.GetCompleteScores"/> 返回的原始成绩。
/// </summary>
internal sealed class VariantConstantMap : IReadOnlyDictionary<ChartConstantKey, float>
{
	private readonly IReadOnlyDictionary<ChartConstantKey, float> _inner;

	internal VariantConstantMap(IReadOnlyDictionary<ChartConstantKey, float> inner)
	{
		this._inner = inner;
	}

	public float this[ChartConstantKey key]
		=> this.TryGetValue(key, out float value)
			? value
			: throw new KeyNotFoundException($"Chart constant for '{key.SongId}' ({key.Difficulty}) was not found.");

	public IEnumerable<ChartConstantKey> Keys => this._inner.Keys;
	public IEnumerable<float> Values => this._inner.Values;
	public int Count => this._inner.Count;

	public bool ContainsKey(ChartConstantKey key) => this.TryGetValue(key, out _);

	public bool TryGetValue(ChartConstantKey key, out float value)
	{
		if (this._inner.TryGetValue(key, out value))
			return true;

		if (SongIdUtility.TryGetBaseId(key.SongId, out string baseId))
		{
			if (this._inner.TryGetValue(new ChartConstantKey(baseId, key.Difficulty), out value))
				return true;
			if (this._inner.TryGetValue(new ChartConstantKey(baseId + ".0", key.Difficulty), out value))
				return true;
		}

		value = default;
		return false;
	}

	public IEnumerator<KeyValuePair<ChartConstantKey, float>> GetEnumerator() => this._inner.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

/// <summary>
/// 在歌名表查找失败时，尝试把变体 id 回退到基础 id 的只读字典视图。
/// </summary>
internal sealed class VariantNameMap : IReadOnlyDictionary<string, string>
{
	private readonly IReadOnlyDictionary<string, string> _inner;

	internal VariantNameMap(IReadOnlyDictionary<string, string> inner)
	{
		this._inner = inner;
	}

	public string this[string key]
		=> this.TryGetValue(key, out string? value)
			? value
			: throw new KeyNotFoundException($"Song name for '{key}' was not found.");

	public IEnumerable<string> Keys => this._inner.Keys;
	public IEnumerable<string> Values => this._inner.Values;
	public int Count => this._inner.Count;

	public bool ContainsKey(string key) => this.TryGetValue(key, out _);

	public bool TryGetValue(string key, out string value)
	{
		if (this._inner.TryGetValue(key, out value!))
			return true;

		if (SongIdUtility.TryGetBaseId(key, out string baseId))
		{
			if (this._inner.TryGetValue(baseId, out value!))
				return true;
			if (this._inner.TryGetValue(baseId + ".0", out value!))
				return true;
		}

		value = string.Empty;
		return false;
	}

	public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => this._inner.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
