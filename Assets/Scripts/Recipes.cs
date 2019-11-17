using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using

[CreateAssetMenu]
public class Recipes : ScriptableObject
{
    [System.Serializable]
    public class RecipeWrapper
    {
        public ItemTree.Item[] recipes;
    }

    public ItemTree.Item[] recipeRoots;
    public ItemTree itemTree;

    [ContextMenu("Log All Asset Ingredients")]
    public void LogAllAssetIngredients()
    {
        var ingredients = GetAllIngredients(true);
        foreach (var item in ingredients) Debug.Log(item.Name);
    }

    [ContextMenu("Log All Ingredients")]
    public void LogAllIngredients()
    {
        var ingredients = GetAllIngredients(false);
        foreach (var item in ingredients) Debug.Log(item.Name);
    }

    [ContextMenu("Export JSON")]
    public void ExportJson()
    {
        var wrapper = new RecipeWrapper();
        wrapper.recipes = recipeRoots;
        Debug.Log(JsonUtility.ToJson(wrapper));
    }

    [ContextMenu("Sort Recipes A-Z")]
    public void SortRecipesAlphabetically()
    {
        recipeRoots = recipeRoots.ToList().OrderByDescending( (i) => i.Name).ToArray();
    }

    [ContextMenu("Sort Items A-Z")]
    public void SortItemsAlphabetically()
    {
        itemTree.Items.Sort(delegate (ItemTree.Item item1, ItemTree.Item item2)
        {
            if (item1 == null && item2 == null) return 0;
            if (item1 == null) return -1;
            if (item2 == null) return 1;
            return item2.Name.CompareTo(item1.Name);
        });
    }

    public string[] GetAllIngredientNames(bool onlyWithAssets)
    {
        var ingredients = GetAllIngredients(onlyWithAssets);
        return ingredients.Select( (i) => i.Name).ToArray();
    }

    public ItemTree.Item[] GetAllIngredients(bool onlyWithAssets)
    {
        var ingredients = new List<ItemTree.Item>();
        foreach (var item in recipeRoots)
        {
            AddItemToIngredients(item, ingredients, onlyWithAssets);
        }
        return ingredients.ToArray();
    }

    private void AddItemToIngredients(ItemTree.Item item, List<ItemTree.Item> ingredients, bool onlyWithAssets)
    {
        if (item == null) return;
        item = itemTree.GetItem(item.Name);
        if (!ingredients.Contains(item))
        {
            if (!onlyWithAssets || item.HasAsset)
            {
                ingredients.Add(item);
            }
        }
        foreach (var option in item.Options)
        {
            foreach (var child in option.Items)
            {
                AddItemToIngredients(child, ingredients, onlyWithAssets);
            }
        }
    }

}
