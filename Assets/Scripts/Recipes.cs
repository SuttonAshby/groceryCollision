using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeUpdateResult
{
    public bool allRecipesComplete;
    public string[] completedRecipes;
}

[CreateAssetMenu]
public class Recipes : ScriptableObject
{
    public ItemTree.Item[] recipeRoots;
    public ItemTree itemTree;

    private List<string> _completedRecipes;

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
    public void TestExport() {
        Debug.Log(ExportRecipeToJson());
    }

    public string ExportRecipeToJson() {
        string ret = "";
        foreach(var item in recipeRoots) {
            ret += "," + ItemJSON(item);
        }
        return "{" + ret.Substring(1) + "}";
    }

    private string ItemJSON(ItemTree.Item item) {
        string ret = "\"" + item.Name + "\":";
        if(item.Options.Count == 0) {
            return ret + "null";
        }
        List<ItemTree.Item> items = item.Options.ElementAt(0).Items;
        if(items.Count == 0) {
            return ret + "null";
        }
        ret += "{";
        foreach(var i in items) {
            ret += ItemJSON(i) + ",";
        }
        return ret.Substring(0, ret.Length-1) + "}";
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

    public void Reset()
    {
        itemTree.Reset();
        _completedRecipes = new List<string>();
    }

    public RecipeUpdateResult UpdateRecipes()
    {
        var allRecipesComplete = true;
        var completedRecipes = new List<string>();
        foreach (var recipe in recipeRoots)
        {
            var complete = itemTree.IsItemDone(recipe.Name);
            if (complete && !_completedRecipes.Contains(recipe.Name))
            {
                _completedRecipes.Add(recipe.Name);
                completedRecipes.Add(recipe.Name);
            }
            if (!complete) allRecipesComplete = false;
        }
        return new RecipeUpdateResult()
        {
            allRecipesComplete = allRecipesComplete,
            completedRecipes = completedRecipes.ToArray()
        };
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
