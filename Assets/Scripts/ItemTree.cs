using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemTree
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        private string _name;
        public string Name { get => _name; }
        public bool Done { get; private set; }
        [SerializeField]
        private List<string> _children;
        public List<string> Children { get => _children; }

        public Item(string name)
        {
            _name = name;
            _children = new List<string>();
        }

        public void AddChild(Item other)
        {
            this.Children.Add(other.Name);
        }

        public void RemoveChild(Item other)
        {
            if (!Children.Contains(other.Name)) return;
            Children.Remove(other.Name);
        }

        public void SetDone()
        {
            this.Done = true;
        }

    }

    public Item Root;
    [SerializeField]
    private List<Item> items;
    public List<Item> Items { get => items; }
    public ItemTree()
    {
        this.Root = new Item("root");
        this.items = new List<Item>() { this.Root };
    }

    public bool AppendToRoot(string childName)
    {
        return Append(Root, childName);
    }

    public bool Append(string parentName, string childName)
    {
        var parent = GetItem(parentName);
        return Append(parent, childName);
    }

    public bool Append(Item parent, string childName)
    {
        if (parent == null) return false;
        Item child = new Item(childName);
        parent.AddChild(child);
        if (!HasItem(childName)) Items.Add(child);
        return true;
    }

    public void RemoveFromRoot(Item item)
    {
        if (!HasItem(item.Name)) return;
        Root.RemoveChild(item);
        Items.Remove(item);
    }

    public bool HasItem(string itemName)
    {
        var item = this.GetItem(itemName);
        return item != null;
    }

    public Item GetItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return null;
        foreach (var item in Items)
        {
            if (item.Name == itemName) return item;
        }
        return null;
    }

    public bool SetItemDone(string itemName)
    {
        var item = GetItem(itemName);
        if (item == null) return false;
        item.SetDone();
        return true;
    }

    public bool IsItemDone(string itemName)
    {
        var item = GetItem(itemName);
        if (item == null) return false;
        if (!item.Done) return false;
        foreach (var child in item.Children)
        {
            if (!IsItemDone(child)) return false;
        }
        return true;
    }

    public bool IsTreeDone()
    {
        return IsItemDone(Root.Name);
    }
}
