using System.Collections.Generic;
public class ItemTree
{
    public class Item
    {
        public string Name { get; }
        public bool Done { get; private set; }

        public Item Parent { get; private set; }
        private List<Item> Children { get; }
        private int ChildrenRemaining;

        public Item(string name, Item parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.Children = new List<Item>();
            this.ChildrenRemaining = 0;
        }

        public void AddChild(Item other)
        {
            other.Parent = this;
            this.Children.Add(other);
            this.ChildrenRemaining++;
        }

        public void SetDone()
        {
            this.Done = true;
            if(this.Parent != null) this.Parent.ChildDone();
        }

        public void ChildDone()
        {
            if (Done) return;
            ChildrenRemaining--;
            if(ChildrenRemaining <= 0) this.SetDone();
        }
    }

    private Item Root;
    private Dictionary<string, Item> itemDict;

    public ItemTree()
    {
        this.Root = new Item("root", null);
        this.itemDict = new Dictionary<string, Item>() { { "root", this.Root } };
    }

    public bool Append(string parent, string child)
    {
        if(this.itemDict.TryGetValue(parent, out Item par))
        {
            Item ret = new Item(child, null);
            par.AddChild(ret);
            itemDict.Add(child, ret);
            return true;
        }
        return false;
    }

    public bool HasItem(string itemName)
    {
        return this.itemDict.ContainsKey(itemName);
    }

    public bool ItemDone(string itemName)
    {
        if(this.itemDict.TryGetValue(itemName, out Item res))
        {
            res.SetDone();
            return true;
        }
        return false;
    }

    public bool IsTreeDone()
    {
        return Root.Done;
    }
}
