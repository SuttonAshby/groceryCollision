using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ItemTree
{

    [System.Serializable]
    public class Options
    {
        public List<Item> Items;
        public Options(params Item[] items)
        {
            if (items == null || items.Length == 0) Items = new List<Item>();
            else Items = items.ToList();
        }
    }

    [System.Serializable]
    public class Item
    {
        [SerializeField]
        private string _name;
        public string Name { get => _name; set => _name = value; }
        public bool Done { get; private set; }
        [SerializeField]
        private List<Options> _options;
        public List<Options> Options { get => _options; }

        public Item(string name)
        {
            _name = name;
            _options = new List<Options>();
        }

        public void AddOption(Item item)
        {
            this.Options.Add(new Options(item));
        }

        // public void RemoveOption(Option option)
        // {
        //     if (!Options.Contains(option)) return;
        //     Options.Remove(option);
        // }

        public void SetDone()
        {
            this.Done = true;
        }

    }

    [SerializeField]
    private List<Item> items;
    public List<Item> Items { get => items; }

    public ItemTree()
    {
        items = new List<Item>();
    }

    public bool AppendToRoot(string childName)
    {
        Item child = new Item(childName);
        if (!HasItem(childName)) Items.Add(child);
        return true;
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
        parent.AddOption(child);
        if (!HasItem(childName)) Items.Add(child);
        return true;
    }

    public void RemoveFromRoot(Item item)
    {
        if (!HasItem(item.Name)) return;
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
        foreach (var option in item.Options)
        {
            var anyDone = false;
            foreach (var child in option.Items)
            {
                if (IsItemDone(child.Name))
                {
                    anyDone = true;
                    break;
                }
            }
            if (!anyDone) return false;
        }
        return true;
    }

}
