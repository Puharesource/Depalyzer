namespace Depalyzer.Cli.DataHandlers;

using System.Collections.Immutable;
using Depalyzer.Common.Models.ProjectAssets;
using Depalyzer.Common.Tree;
using Package = Depalyzer.Common.Models.Package;

public class ProjectAssetsDataHandler(Root projectAssets)
{
    public ProjectAssetsTargetDataHandler GetTarget(string targetSdk) => new(targetSdk, projectAssets.Targets[targetSdk]);
}

public class ProjectAssetsTargetDataHandler(string targetSdk, IImmutableDictionary<string, Common.Models.ProjectAssets.Package> projectAssetsPackages)
{
    public TreeNode<Package> FindAllParents(string packageName, string packageVersion, TreeNode<Package>? previousNode = null)
    {
        TreeNode<Package> currentNode;
        if (previousNode == null)
        {
            currentNode = new(new(packageName, packageVersion));
        }
        else
        {
            currentNode = previousNode.AddOrGetNodeFor(new(packageName, packageVersion));
        }

        var parents = this.FindParents(packageName, packageVersion);
        if (!parents.IsEmpty)
        {
            foreach (var parent in parents)
            {
                this.FindAllParents(parent.Name, parent.Version, currentNode);
            }
        }

        return currentNode.Root.Children.First();
    }

    private ImmutableArray<Package> FindDependencies(string packageName, string packageVersion)
    {
        var assetPackage = projectAssetsPackages[$"{packageName}/{packageVersion}"];

        return assetPackage.Dependencies
                   ?.Select(keyValue => new Package(keyValue.Key, keyValue.Value, targetSdk))
                   .ToImmutableArray()
               ?? ImmutableArray<Package>.Empty;
    }

    private ImmutableArray<Package> FindParents(string packageName, string packageVersion)
    {
        var parents = new List<Package>();
        var findForPackage = new Package(packageName, packageVersion, targetSdk);

        foreach (var parentKey in projectAssetsPackages.Keys)
        {
            var parentKeyPair = parentKey.Split('/', 2);
            var currentParent = new Package(parentKeyPair[0], parentKeyPair[1], targetSdk);
            var dependencies = this.FindDependencies(currentParent.Name, currentParent.Version);
            var isNameAndVersionEqual = dependencies.Any(
                dependency => dependency.Name.Equals(findForPackage.Name, StringComparison.OrdinalIgnoreCase)
                              && dependency.GetNuGetVersionRange().Satisfies(findForPackage.GetNuGetVersion()));

            if (isNameAndVersionEqual)
            {
                parents.Add(currentParent);
            }
        }

        return parents.ToImmutableArray();
    }
}