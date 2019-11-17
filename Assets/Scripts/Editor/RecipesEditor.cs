using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Recipes))]
public class RecipesEditor : Editor
{

    private Recipes _recipes;
    private string _itemToAdd;
    private string[] _itemNames;
    private int _recipeToAdd;
    private Dictionary<string,int> _selectedItems;
    private bool _dirty;

    private void OnEnable()
    {
        _recipes = target as Recipes;
        _selectedItems = new Dictionary<string, int>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var header = new GUIStyle(EditorStyles.largeLabel);
        header.fontSize = 15;
        header.fontStyle = FontStyle.Bold;

        if (_recipes.itemTree == null || _recipes.itemTree.Items.Count == 0)
        {
            GUILayout.BeginHorizontal();
            _itemToAdd = EditorGUILayout.TextField("", _itemToAdd);
            if (GUILayout.Button("Add New Item")) AddItem();
            GUILayout.EndHorizontal();
            return;
        }

        var items = _recipes.itemTree.Items;
        _itemNames = items.Select((i) => i.Name).ToArray();

        EditorGUILayout.LabelField("Recipes", header, GUILayout.Height(24));
        if (_recipes.recipeRoots != null)
        {
            for (var i = _recipes.recipeRoots.Length - 1; i >= 0; i--)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(_recipes.recipeRoots[i].Name);
                if (GUILayout.Button("-")) RemoveRecipe(i);
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        _recipeToAdd = EditorGUILayout.Popup(_recipeToAdd, _itemNames);
        if (GUILayout.Button("Add")) AddRecipe();
        GUILayout.EndHorizontal();

        GUILayout.Space(28);
        EditorGUILayout.LabelField("Items", header, GUILayout.Height(24));

        GUILayout.BeginHorizontal();
        _itemToAdd = EditorGUILayout.TextField("", _itemToAdd);
        if (GUILayout.Button("Add New Item")) AddItem();
        GUILayout.EndHorizontal();
        GUILayout.Space(15);
        for (var i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            DrawItem(item, null, 1);

            GUILayout.Space(12);
        }
        EditorGUI.indentLevel = 0;

        if (_dirty)
        {
            EditorUtility.SetDirty(_recipes);
            _dirty = false;
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void AddRecipe()
    {
        var item = _recipes.itemTree.GetItem(_itemNames[_recipeToAdd]);
        if (item == null) return;
        if (_recipes.recipeRoots == null || _recipes.recipeRoots.Length == 0)
        {
            _recipes.recipeRoots = new ItemTree.Item[] { item };
            return;
        }
        var existing = _recipes.recipeRoots;
        var expanded = new ItemTree.Item[existing.Length + 1];
        existing.CopyTo(expanded, 0);
        _recipes.recipeRoots = expanded;
        _recipes.recipeRoots[_recipes.recipeRoots.Length - 1] = item;
        _dirty = true;
    }

    private void RemoveRecipe(int index)
    {
        var list = _recipes.recipeRoots.ToList();
        list.RemoveAt(index);
        _recipes.recipeRoots = list.ToArray();
        _dirty = true;
    }

    private void DrawItem(ItemTree.Item item, ItemTree.Item parent, int depth)
    {
        if (depth > 5) return;
        EditorGUI.indentLevel = 2 * (depth - 1);

        if (depth == 2 || depth == 1)
        {
            GUILayout.BeginHorizontal();
            if (depth == 1)
            {
                var prevItemName = item.Name;
                item.Name = EditorGUILayout.TextField(item.Name);
                if (prevItemName != item.Name) _dirty = true;
            }
            else EditorGUILayout.LabelField(item.Name);
            // if (GUILayout.Button("-")) RemoveChild(parent, item);
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField(item.Name);
        }
        for (var i = item.Options.Count - 1; i >= 0; i--)
        {
            EditorGUI.indentLevel = 2 * depth;
            if (item.Options.Count > 1)
            {
                EditorGUI.indentLevel = 2 * depth - 1;
                EditorGUILayout.LabelField(string.Format("Option {0}", i + 1));
            }
            var option = item.Options[i];
            for (var j = option.Items.Count - 1; j >= 0; j--)
            {
                var childName = option.Items[j].Name;
                var child = _recipes.itemTree.GetItem(childName);
                if (child != null) DrawItem(child, item, depth + 1);
            }

            if (depth == 1)
            {
                EditorGUI.indentLevel = 2 * depth;
                GUILayout.BeginHorizontal();
                if (!_selectedItems.ContainsKey(item.Name)) _selectedItems[item.Name] = 0;
                _selectedItems[item.Name] = EditorGUILayout.Popup(_selectedItems[item.Name], _itemNames);
                if (GUILayout.Button("+")) AddChild(item, option, _itemNames[_selectedItems[item.Name]]);
                GUILayout.EndHorizontal();
            }
            // if (i > 0) EditorGUILayout.LabelField(" - OR - ");
        }

        if (depth == 1)
        {

            GUILayout.Space(8);
            EditorGUI.indentLevel = 2 * depth;
            var key = string.Format("{0}-Or", item.Name);
            GUILayout.BeginHorizontal();
            if (!_selectedItems.ContainsKey(key)) _selectedItems[key] = 0;
            _selectedItems[key] = EditorGUILayout.Popup(_selectedItems[key], _itemNames);
            var label = "+";
            if (item.Options.Count > 0) label = "+OR";
            if (GUILayout.Button(label)) AddOption(item, _itemNames[_selectedItems[key]]);
            GUILayout.EndHorizontal();

        }
    }

    private void AddItem()
    {
        if (string.IsNullOrEmpty(_itemToAdd))
        {
            Debug.LogError("no name specified");
            return;
        }
        if (_recipes.itemTree == null) _recipes.itemTree = new ItemTree();
        if (_recipes.itemTree.HasItem(_itemToAdd))
        {
            Debug.LogError("name already exists");
            return;
        }
        _recipes.itemTree.AppendToRoot(_itemToAdd);
        _itemToAdd = "";
        _dirty = true;
    }

    private void AddChild(ItemTree.Item item, ItemTree.Options options, string childName)
    {

        if (string.IsNullOrEmpty(childName))
        {
            Debug.LogError("no name specified");
            return;
        }
        if (childName == item.Name)
        {
            Debug.LogError("can't add self as parent");
            return;
        }
        if (!_recipes.itemTree.HasItem(childName))
        {
            Debug.LogError("no such item");
            return;
        }
        var child = _recipes.itemTree.GetItem(childName);
        if (child == null)
        {
            Debug.LogError("child is null");
            return;
        }
        if (options.Items.Contains(child))
        {
            Debug.LogError("child already added");
            return;
        }
        options.Items.Add(child);
        _dirty = true;
    }

    private void AddOption(ItemTree.Item item, string optionName)
    {
        if (optionName == item.Name)
        {
            Debug.LogError("can't add self as parent");
            return;
        }
        if (!_recipes.itemTree.HasItem(optionName))
        {
            Debug.LogError("no such item");
            return;
        }
        var child = _recipes.itemTree.GetItem(optionName);
        item.AddOption(child);
        _dirty = true;
    }

    private void RemoveChild(ItemTree.Item parent, ItemTree.Item child)
    {
        if (parent == null)
        {
            if (EditorUtility.DisplayDialog("Delete?", "Really delete recipe?", "Bye Bye Forever", "Cancel"))
            {
                _recipes.itemTree.RemoveFromRoot(child);
            }
        }
        else
        {
            // parent.RemoveOption(child);
        }
        _dirty = true;
    }

}
