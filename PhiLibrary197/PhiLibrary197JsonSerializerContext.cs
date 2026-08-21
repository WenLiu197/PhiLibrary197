using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using PhiLibrary197.CloudSave;
using PhiLibrary197.CloudSave.HttpModels;
using PhiLibrary197.CloudSave.Login;
using PhiLibrary197.LocalSave;

namespace PhiLibrary197;

/// <summary>
/// The source-generated <see cref="JsonSerializerContext"/> used for all JSON serialization in this library.
/// Required for Native AOT compatibility, as reflection-based serialization is not supported.
/// </summary>
[JsonSourceGenerationOptions(
	AllowTrailingCommas = true,
	PropertyNameCaseInsensitive = true,
	IncludeFields = true)]
[JsonSerializable(typeof(SaveInfoContainer))]
[JsonSerializable(typeof(SaveInfo))]
[JsonSerializable(typeof(RawScore))]
[JsonSerializable(typeof(LCCombinedAuthData))]
[JsonSerializable(typeof(TapTapProfileData))]
[JsonSerializable(typeof(TapTapTokenData))]
[JsonSerializable(typeof(PartialTapTapQRCodeData))]
[JsonSerializable(typeof(JsonNode))]
public partial class PhiLibrary197JsonSerializerContext : JsonSerializerContext
{
}
