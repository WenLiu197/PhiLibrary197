using System.Net;
using System.Xml.Linq;
using PhiLibrary197.LocalSave;
using PhiLibrary197.Scores;
using LocalSaveApi = PhiLibrary197.LocalSave.LocalSave;

namespace PhiLibrary197.Samples;

public static class LocalSaveExample
{
    public static int Run(string[] args)
    {
        string? path = args.Skip(1).FirstOrDefault()
            ?? Environment.GetEnvironmentVariable("PHIGROS_PREFS_PATH");

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("usage: local <path-to-playerPrefsV2.xml>");
            Console.WriteLine("or set PHIGROS_PREFS_PATH");
            return 2;
        }

        if (!File.Exists(path))
        {
            Console.WriteLine($"file not found: {path}");
            return 2;
        }

        XDocument doc = XDocument.Load(path);
        List<SongScore> scores = [];

        foreach (XElement element in doc.Descendants("string"))
        {
            string? rawName = element.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(rawName))
                continue;

            string decryptedName = LocalSaveApi.DecryptLocalSaveStringNew(WebUtility.UrlDecode(rawName));
            int marker = decryptedName.LastIndexOf(".Record.", StringComparison.Ordinal);
            if (marker < 0)
                continue;

            string songId = decryptedName[..marker];
            string difficultyText = decryptedName[(marker + ".Record.".Length)..];
            if (!Enum.TryParse(difficultyText, ignoreCase: true, out Difficulty difficulty))
                continue;

            string decryptedValue = LocalSaveApi.DecryptLocalSaveStringNew(WebUtility.UrlDecode(element.Value));
            RawScore raw = RawScore.FromJson(decryptedValue);
            scores.Add(raw.ToSongScore(songId, difficulty));
        }

        Console.WriteLine($"records: {scores.Count}");
        foreach (SongScore score in scores.Take(10))
            Console.WriteLine($"  {score.Id} [{score.Difficulty}] {score.Score} {score.Accuracy:F4}% {score.Status}");

        return 0;
    }
}
