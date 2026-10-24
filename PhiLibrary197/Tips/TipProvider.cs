using System.Reflection;
using System.Text;

namespace PhiLibrary197.Tips;

/// <summary>
/// 提供内置的 Phigros 简体中文 Tips。
/// </summary>
/// <remarks>
/// 数据随程序集嵌入，不读写文件，适用于 Native AOT、单文件发布和只读环境。
/// Tips 数量与文本来自内置的 <c>tips.tsv</c>。
/// </remarks>
public static class TipProvider
{
	private const string ResourceName = "PhiLibrary197.Data.tips.tsv";

	private static readonly string[] _tips = LoadTips();

	/// <summary>
	/// 随机获取一条内置简体中文 Tip。
	/// </summary>
	/// <returns>一条非空的 Tip 文本。</returns>
	/// <exception cref="InvalidOperationException">内置 Tips 资源缺失或没有可用条目。</exception>
	public static string GetTip()
		=> _tips[Random.Shared.Next(_tips.Length)];

	private static string[] LoadTips()
	{
		Assembly assembly = typeof(TipProvider).Assembly;
		using Stream stream = assembly.GetManifestResourceStream(ResourceName)
			?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' was not found in '{assembly.GetName().Name}'.");

		using StreamReader reader = new(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
		List<string> tips = [];
		string? line;
		while ((line = reader.ReadLine()) is not null)
		{
			if (line.Length == 0 || line[0] == '#')
				continue;

			int tab = line.IndexOf('\t');
			if (tab < 1)
				continue;

			// 第一列是 index；表头或脏行会被 TryParse 过滤掉。
			if (!int.TryParse(line.AsSpan(0, tab), out _))
				continue;

			string text = line[(tab + 1)..];
			if (text.Length > 0)
				tips.Add(text);
		}

		if (tips.Count == 0)
			throw new InvalidOperationException("The built-in tips resource contains no usable entries.");

		return [.. tips];
	}
}
