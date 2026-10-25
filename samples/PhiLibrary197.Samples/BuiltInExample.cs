using PhiLibrary197.Scores;
using PhiLibrary197.Songs;
using PhiLibrary197.Tips;

namespace PhiLibrary197.Samples;

public static class BuiltInExample
{
    public static int Run()
    {
        SongDatabase db = SongDatabase.LoadFromBuiltIn();

        Console.WriteLine($"songs: {db.Count}");
        Console.WriteLine($"data version: {SongDatabase.BuiltInDataVersion}");

        SongEntry? song = db.GetSong("Glaciaxion.SunsetRay");
        if (song is null)
        {
            Console.WriteLine("song not found");
            return 1;
        }

        Console.WriteLine($"{song.Name} / {song.Composer} / IN={song.GetConstant(Difficulty.IN)}");
        Console.WriteLine($"IN charter: {song.GetCharter(Difficulty.IN)}");

        SongEntry? variant = db.GetSong("Random.SobremSilentroom.3");
        if (variant is not null)
        {
            Console.WriteLine($"variant: {variant.Id}, base={variant.BaseId}, isVariant={variant.IsVariant}");
        }

        foreach (SongEntry hit in db.FindByFullName("Glaciaxion"))
            Console.WriteLine($"name hit: {hit.Id}");

        // 用合成成绩演示 B30/RKS；真实成绩可来自 SaveContext.ReadGameRecord()。
        List<SongScore> records = [];
        foreach (SongEntry entry in db.Songs.Where(x => x.Constants.ContainsKey(Difficulty.IN)).Take(30))
        {
            records.Add(new SongScore(1_000_000, 100f, entry.Id, Difficulty.IN, ScoreStatus.Phi));
        }

        GameRecord gameRecord = new(records, version: 1);
        (List<CompleteScore> phis, List<CompleteScore> sorted, double rks) =
            gameRecord.GetSortedListForRks(db.ToConstantMap(), db.ToNameMap());

        Console.WriteLine($"phi count: {phis.Count}, sorted: {sorted.Count}, rks: {rks:F4}");
        foreach (CompleteScore score in sorted.Take(3))
            Console.WriteLine($"  {score.NameOrDefault} [{score.Score.Difficulty}] {score.Rks:F4}");

        Console.WriteLine($"tip: {TipProvider.GetTip()}");
        return 0;
    }
}
