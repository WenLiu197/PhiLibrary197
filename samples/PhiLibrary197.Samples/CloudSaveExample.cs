using PhiLibrary197.CloudSave;
using PhiLibrary197.CloudSave.HttpModels;
using PhiLibrary197.Scores;
using PhiLibrary197.Songs;

namespace PhiLibrary197.Samples;

public static class CloudSaveExample
{
    public static async Task<int> RunAsync()
    {
        string? token = Environment.GetEnvironmentVariable("PHIGROS_SESSION_TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            Console.WriteLine("set PHIGROS_SESSION_TOKEN to a 25-character LeanCloud session token");
            return 2;
        }

        bool international = Environment.GetEnvironmentVariable("PHIGROS_INTERNATIONAL") is "1" or "true";
        using Save save = new(token, isInternational: international);

        SaveInfoContainer container = await save.GetSaveInfoFromCloudAsync();
        Console.WriteLine($"saves: {container.Results.Count}");
        if (container.Results.Count == 0)
        {
            Console.WriteLine("no save found");
            return 3;
        }

        SaveContext context = await save.GetSaveContextAsync(0);
        Summary summary = context.ReadSummary();
        GameRecord record = context.ReadGameRecord();

        SongDatabase db = SongDatabase.LoadFromBuiltIn();
        (List<CompleteScore> phis, List<CompleteScore> sorted, double rks) =
            record.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());

        Console.WriteLine($"game version: {summary.GameVersion}, save version: {summary.SaveVersion}");
        Console.WriteLine($"summary rks: {summary.Rks:F6}, computed rks: {rks:F6}");
        Console.WriteLine($"raw records: {record.Records.Count}, complete: {sorted.Count}, phis: {phis.Count}");

        foreach (CompleteScore score in sorted.Take(10))
            Console.WriteLine($"  {score.NameOrDefault} [{score.Score.Difficulty}] {score.Score.Score} {score.Rks:F4}");

        return 0;
    }
}
