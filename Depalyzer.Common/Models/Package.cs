namespace Depalyzer.Common.Models;

using Depalyzer.Common.Models.VulnerabilityCheck;
using NuGet.Versioning;

public readonly record struct Package(string Name, string Version, string? Sdk = null, Vulnerability? Vulnerability = null)
{
    public NuGetVersion GetNuGetVersion() => NuGetVersion.Parse(this.Version);

    public VersionRange GetNuGetVersionRange() => VersionRange.Parse(this.Version);
}