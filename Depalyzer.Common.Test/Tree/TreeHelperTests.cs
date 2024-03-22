namespace Depalyzer.Common.Test.Tree;

using Depalyzer.Common.Tree;
using Shouldly;

public class TreeHelperTests
{
    [Fact]
    public void FindLeaves()
    {
        var root = new TreeNode<string>("I am (g)root!");

        var two = root.AddOrGetNodeFor("#2");
        two.AddOrGetNodeFor("#5");

        var three = two.AddOrGetNodeFor("#3");
        three.AddOrGetNodeFor("#4");

        var leaves = TreeHelper.FindLeaves(root);

        leaves.Length.ShouldBe(2);
        leaves.ShouldContain(leaf => leaf.Item == "#5");
        leaves.ShouldContain(leaf => leaf.Item == "#4");
    }

    [Fact]
    public void MergeLeaves()
    {
        var root = new TreeNode<string>("I am (g)root!");

        var systemHttp = root.AddOrGetNodeFor("System.Net.Http/4.3.0");
        var netstandardLibrary = systemHttp.AddOrGetNodeFor("NETStandard.Library/1.6.1");
        var xunitExtCore = netstandardLibrary.AddOrGetNodeFor("xunit.extensibility.core/2.4.2");
        var autoFixXUnit = xunitExtCore.AddOrGetNodeFor("AutoFixture.Xunit2/4.17.0");

        autoFixXUnit.AddOrGetNodeFor("My.Cool.Dependency/1.0.11");

        var xunitExtExec = netstandardLibrary.AddOrGetNodeFor("xunit.extensibility.execution/2.4.2");
        var xunitCore = xunitExtExec.AddOrGetNodeFor("xunit.core/2.4.2");
        var xunit = xunitCore.AddOrGetNodeFor("xunit/2.4.2");

        xunit.AddOrGetNodeFor("My.Cool.Dependency/1.0.11");

        var leaves = TreeHelper.FindLeaves(root);

        leaves.Length.ShouldBe(2);

        var mergedLeaves = TreeHelper.MergeLeavesIntoTree(leaves);

        mergedLeaves.Children.Count.ShouldBe(1);

        var firstChild = mergedLeaves.Children.First();
        firstChild.Item.ShouldBe("My.Cool.Dependency/1.0.11");

        firstChild.Children.Count.ShouldBe(2);
        firstChild.Children.ShouldContain(child => child.Item == "AutoFixture.Xunit2/4.17.0");
        firstChild.Children.ShouldContain(child => child.Item == "xunit/2.4.2");

        var autoFixXUnitChild = firstChild.Children.First(child => child.Item == "AutoFixture.Xunit2/4.17.0");
        autoFixXUnitChild.Children.Count.ShouldBe(1);

        var xunitExtCoreChild = autoFixXUnitChild.Children.First();
        xunitExtCoreChild.Item.ShouldBe("xunit.extensibility.core/2.4.2");
        xunitExtCoreChild.Children.Count.ShouldBe(1);

        var netstandardLibraryChild = xunitExtCoreChild.Children.First();
        netstandardLibraryChild.Item.ShouldBe("NETStandard.Library/1.6.1");
        netstandardLibraryChild.Children.Count.ShouldBe(1);

        var systemHttpChild = netstandardLibraryChild.Children.First();
        systemHttpChild.Item.ShouldBe("System.Net.Http/4.3.0");
        systemHttpChild.Children.Count.ShouldBe(1);

        var grootChild = systemHttpChild.Children.First();
        grootChild.Item.ShouldBe("I am (g)root!");
        grootChild.Children.Count.ShouldBe(0);

        var xunitChild = firstChild.Children.First(child => child.Item == "xunit/2.4.2");
        xunitChild.Children.Count.ShouldBe(1);

        var xunitCoreChild = xunitChild.Children.First();
        xunitCoreChild.Item.ShouldBe("xunit.core/2.4.2");
        xunitCoreChild.Children.Count.ShouldBe(1);

        var xunitExtExecChild = xunitCoreChild.Children.First();
        xunitExtExecChild.Item.ShouldBe("xunit.extensibility.execution/2.4.2");
        xunitExtExecChild.Children.Count.ShouldBe(1);

        var netstandardLibraryChild2 = xunitExtExecChild.Children.First();
        netstandardLibraryChild2.Item.ShouldBe("NETStandard.Library/1.6.1");
        netstandardLibraryChild2.Children.Count.ShouldBe(1);

        var systemHttpChild2 = netstandardLibraryChild2.Children.First();
        systemHttpChild2.Item.ShouldBe("System.Net.Http/4.3.0");
        systemHttpChild2.Children.Count.ShouldBe(1);

        var grootChild2 = systemHttpChild2.Children.First();
        grootChild2.Item.ShouldBe("I am (g)root!");
        grootChild2.Children.Count.ShouldBe(0);
    }
}