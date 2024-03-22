namespace Depalyzer.Common.Tree;

using System.Collections.Immutable;

public static class TreeHelper
{
    public static ImmutableArray<TreeNode<TItem>> FindLeaves<TItem>(TreeNode<TItem> previousNode) where TItem : notnull
    {
        var roots = new List<TreeNode<TItem>>();

        if (previousNode.IsLeaf)
        {
            roots.Add(previousNode);
        }
        else
        {
            foreach (var foundRoots in previousNode.Children.Select(FindLeaves))
            {
                roots.AddRange(foundRoots);
            }
        }

        return roots.ToImmutableArray();
    }

    public static TreeNode<TItem> MergeLeavesIntoTree<TItem>(ImmutableArray<TreeNode<TItem>> leaves, TreeNode<TItem>? previousNode = null) where TItem : notnull
    {
        previousNode ??= new(default);

        var nextLeaves = leaves
            .Where(leaf => !leaf.IsRoot)
            .Select(leaf => leaf.Parent!)
            .ToImmutableArray();

        foreach (var leaf in leaves)
        {
            var node = previousNode.AddOrGetNodeFor(leaf.Item);
            var nextLeavesForLeaf = nextLeaves
                .Where(
                    nextLeaf => nextLeaf.Children
                        .Any(child => child.Item.Equals(leaf.Item)))
                .ToImmutableArray();

            MergeLeavesIntoTree(nextLeavesForLeaf, node);
        }

        return previousNode;
    }

    public static TreeNode<TItem> CollapseTreeFirstBranches<TItem>(TreeNode<TItem> root) where TItem : notnull
    {
        var newRoot = new TreeNode<TItem>(root.Item);

        foreach (var child in root.Children)
        {
            var node = newRoot.AddOrGetNodeFor(child.Item);
            var foundLeaves = FindLeaves(child);

            foreach (var foundLeaf in foundLeaves)
            {
                node.AddOrGetNodeFor(foundLeaf.Item);
            }
        }

        return newRoot;
    }
}