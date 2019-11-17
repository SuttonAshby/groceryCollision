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

    private void OnEnable()
    {
        _recipes = target as Recipes;
        _selectedItems = new Dictionary<string, int>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (_recipes.itemTree == null || _recipes.itemTree.Items.Count <= 1)
        {
            GUILayout.BeginHorizontal();
            _itemToAdd = EditorGUILayout.TextField("", _itemToAdd);
            if (GUILayout.Button("Add New Item")) AddItem();
            GUILayout.EndHorizontal();
            return;
        }

        var items = _recipes.itemTree.Items;
        _itemNames = items.Where((i) => i.Name != "root").Select((i) => i.Name).ToArray();

        EditorGUILayout.LabelField("Recipes", EditorStyles.boldLabel);
        if (_recipes.recipeRoots != null)
        {
            for (var i = _recipes.recipeRoots.Length - 1; i >= 0; i--)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextField(_recipes.recipeRoots[i].Name);
                if (GUILayout.Button("-")) RemoveRecipe(i);
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.BeginHorizontal();
        _recipeToAdd = EditorGUILayout.Popup(_recipeToAdd, _itemNames);
        if (GUILayout.Button("Add")) AddRecipe();
        GUILayout.EndHorizontal();

        GUILayout.Space(28);
        EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        _itemToAdd = EditorGUILayout.TextField("", _itemToAdd);
        if (GUILayout.Button("Add New Item")) AddItem();
        GUILayout.EndHorizontal();
        GUILayout.Space(15);
        for (var i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            if (item.Name == "root") continue;
            DrawItem(item, null, 1);

            GUILayout.Space(12);
        }
        EditorGUI.indentLevel = 0;

        serializedObject.ApplyModifiedProperties();
    }

    private void AddRecipe()
    {
        var existing = _recipes.recipeRoots;
        var expanded = new ItemTree.Item[existing.Length + 1];
        existing.CopyTo(expanded, 0);
        _recipes.recipeRoots = expanded;
        var item = _recipes.itemTree.GetItem(_itemNames[_recipeToAdd]);
        if (item == null) return;
        _recipes.recipeRoots[_recipes.recipeRoots.Length - 1] = item;
    }

    private void RemoveRecipe(int index)
    {
        var list = _recipes.recipeRoots.ToList();
        list.RemoveAt(index);
        _recipes.recipeRoots = list.ToArray();
    }

    private void DrawItem(ItemTree.Item item, ItemTree.Item parent, int depth)
    {
        if (depth > 5) return;
        EditorGUI.indentLevel = 2 * (depth - 1);

        if (depth == 2 || depth == 1)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(item.Name);
            if (GUILayout.Button("-")) RemoveChild(parent, item);
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField(item.Name);
        }
        EditorGUI.indentLevel = 2 * depth;
        for (var i = item.Children.Count - 1; i >= 0; i--)
        {
            var childName = item.Children[i];
            var child = _recipes.itemTree.GetItem(childName);
            DrawItem(child, item, depth + 1);
        }

        if (depth == 1)
        {
            EditorGUI.indentLevel = 2 * depth;
            GUILayout.BeginHorizontal();
            if (!_selectedItems.ContainsKey(item.Name)) _selectedItems[item.Name] = 0;
            _selectedItems[item.Name] = EditorGUILayout.Popup(_selectedItems[item.Name], _itemNames);
            if (GUILayout.Button("+")) AddChild(item);
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
    }

    private void AddChild(ItemTree.Item item)
    {
        var name = _itemNames[_selectedItems[item.Name]];
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("no name specified");
            return;
        }
        if (name == item.Name)
        {
            Debug.LogError("can't add self as parent");
            return;
        }
        if (!_recipes.itemTree.HasItem(name))
        {
            Debug.LogError("no such item");
            return;
        }
        var child = _recipes.itemTree.GetItem(name);
        if (item.Children.Contains(child.Name))
        {
            Debug.LogError("child already added");
            return;
        }
        item.AddChild(child);
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
            parent.RemoveChild(child);
        }
    }

}
