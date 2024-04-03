namespace Depalyzer.Common.Tree;

public record TreeNode<TItem>(TItem Item, TreeNode<TItem>? Parent = null)
    where TItem : notnull
{
    public List<TreeNode<TItem>> Children { get; } = [];

    public bool IsLeaf => this.Children.Count == 0;

    public bool IsRoot => this.Parent is null;

    public TreeNode<TItem> Root
    {
        get
        {
            var currentNode = this;
            while (currentNode.Parent is not null)
            {
                currentNode = currentNode.Parent!;
            }

            return currentNode;
        }
    }

    public TreeNode<TItem> AddOrGetNodeFor(TItem item)
    {
        var child = this.Children.Find(child => child.Item.Equals(item));

        if (child == null)
        {
            child = new(item, this);
            this.Children.Add(child);
        }

        return child;
    }

    public void AddNode(TreeNode<TItem> node)
    {
        this.Children.Add(node);
    }

    public void AddNodes(IEnumerable<TreeNode<TItem>> nodes)
    {
        this.Children.AddRange(nodes);
    }
}