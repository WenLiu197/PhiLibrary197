using System.Text.Json;
using PhiLibrary197;
using PhiLibrary197.LocalSave;
using PhiLibrary197.Scores;

namespace PhiLibrary197.Samples;

public static class SerializationExample
{
    public static int Run()
    {
        RawScore score = new()
        {
            Score = 1_000_000,
            Accuracy = 100f,
            Status = ScoreStatus.Phi,
        };

        string json = JsonSerializer.Serialize(score, PhiLibrary197JsonSerializerContext.Default.RawScore);
        Console.WriteLine(json);

        RawScore roundTrip = JsonSerializer.Deserialize(
            json,
            PhiLibrary197JsonSerializerContext.Default.RawScore);
        Console.WriteLine($"round-trip: {roundTrip.Score}, {roundTrip.Accuracy}, {roundTrip.Status}");
        return 0;
    }
}
