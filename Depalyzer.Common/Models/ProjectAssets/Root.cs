namespace Depalyzer.Common.Models.ProjectAssets;

using System.Collections.Immutable;
using System.Text.Json.Serialization;

public readonly record struct Root(
    [property: JsonPropertyName("version")]
    int Version,
    [property: JsonPropertyName("targets")]
    IImmutableDictionary<string, IImmutableDictionary<string, Package>> Targets);