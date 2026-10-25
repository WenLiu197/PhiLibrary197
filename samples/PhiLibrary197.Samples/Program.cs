using PhiLibrary197.Samples;

string mode = args.FirstOrDefault()?.ToLowerInvariant() ?? "builtin";

return mode switch
{
    "builtin" => BuiltInExample.Run(),
    "local" => LocalSaveExample.Run(args),
    "cloud" => await CloudSaveExample.RunAsync(),
    "qr" => await LoginExample.RunAsync(args),
    "callback" => await CallbackLoginExample.RunAsync(args),
    "json" => SerializationExample.Run(),
    "-h" or "--help" or "help" => Usage(),
    _ => Usage(),
};

static int Usage()
{
    Console.WriteLine("PhiLibrary197 samples");
    Console.WriteLine("  dotnet run --project samples/PhiLibrary197.Samples -- builtin");
    Console.WriteLine("  dotnet run --project samples/PhiLibrary197.Samples -- local <playerPrefsV2.xml>");
    Console.WriteLine("  set PHIGROS_SESSION_TOKEN=... && dotnet run ... -- cloud");
    Console.WriteLine("  dotnet run ... -- qr [--intl]");
    Console.WriteLine("  dotnet run ... -- callback [--intl]");
    Console.WriteLine("  dotnet run ... -- json");
    return 0;
}
