namespace Depalyzer.Common.Models.ProjectAssets;

using System.Collections.Immutable;
using System.Text.Json.Serialization;

public readonly record struct Package(
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("dependencies")]
    IImmutableDictionary<string, string>? Dependencies);