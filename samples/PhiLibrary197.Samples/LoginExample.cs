using PhiLibrary197.Authentication;

namespace PhiLibrary197.Samples;

public static class LoginExample
{
    public static async Task<int> RunAsync(string[] args)
    {
        bool useChinaEndpoint = !args.Contains("--intl", StringComparer.OrdinalIgnoreCase);

        TapTapQrCodeLogin login = await TapTapQrCodeLogin.CreateAsync(useChinaEndpoint: useChinaEndpoint);
        Console.WriteLine($"请用 TapTap App 扫描或打开: {login.Url}");
        Console.WriteLine($"二维码有效期: {login.ExpiresInSeconds}s");

        TapTapTokenData token = await login.WaitForTokenAsync();
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
