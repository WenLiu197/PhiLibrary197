using PhiLibrary197.Authentication;

namespace PhiLibrary197.Samples;

public static class CallbackLoginExample
{
    public static async Task<int> RunAsync(string[] args)
    {
        bool useChinaEndpoint = !args.Contains("--intl", StringComparer.OrdinalIgnoreCase);

        using TapTapCallbackServer server = TapTapHelper.StartCallbackLogin(
            port: 0,
            useChinaEndpoint: useChinaEndpoint);

        Console.WriteLine($"callback: {server.CallbackUrl}");
        Console.WriteLine($"打开授权地址: {server.BeginUrl}");

        TapTapTokenData token = await server.WaitForTokenAsync();
        TapTapProfileData profile = await TapTapHelper.GetProfile(
            token.Data,
            useChinaEndpoint: useChinaEndpoint);

        string sessionToken = await LCHelper.LoginAndGetToken(
            new LCCombinedAuthData(profile.Data, token.Data),
            useChinaEndpoint: useChinaEndpoint);

        Console.WriteLine($"name: {profile.Data.Name}");
        Console.WriteLine($"bio: {profile.Data.Bio ?? "(empty)"}");
        Console.WriteLine($"sessionToken: {sessionToken}");
        return 0;
    }
}
