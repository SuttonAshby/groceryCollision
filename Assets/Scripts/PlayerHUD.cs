using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public AnimatedText gameOverTextPrefab;
    public AnimatedText collectedItemTextPrefab;
    public AnimatedText completedRecipeTextPrefab;
    public AnimatedText collectedTrashTextPrefab;

    private List<AnimatedText> _completedRecipeTexts;
    private List<AnimatedText> _collectedItemTexts;
    private List<AnimatedText> _collectedTrashTexts;

    private int _trashTextIndex;
    private string[] _trashTexts = new string[]
    {
        "OOPS",
        "NOPE",
        "TRASH",
        "WRONG",
        "TRY AGAIN",
        "WASTE OF $$",
        "DON'T NEED IT"
    };

    public void PlayerWon()
    {
        var gameOverText = Instantiate(gameOverTextPrefab, transform);
        gameOverText.Play("YOU WON!");
    }

    public void PlayerLost()
    {
        var gameOverText = Instantiate(gameOverTextPrefab, transform);
        gameOverText.Play("YOU LOST!");
    }

    public void CollectedItem(string itemName)
    {
        var text = GetAnimatedText(collectedItemTextPrefab, _collectedItemTexts);
        text.Play(string.Format("{0}!", itemName));
    }

    public void CompletedRecipe(string itemName)
    {
        var text = GetAnimatedText(completedRecipeTextPrefab, _completedRecipeTexts);
        text.Play(string.Format("{0}!", itemName));
    }

    public void CollectedTrash()
    {
        var text = GetAnimatedText(collectedTrashTextPrefab, _collectedTrashTexts);
        var msg = _trashTexts[_trashTextIndex];
        _trashTextIndex++;
        if (_trashTextIndex >= _trashTexts.Length) _trashTextIndex = 0;
        text.Play(msg);
    }

    private AnimatedText GetAnimatedText(AnimatedText prefab, List<AnimatedText> pool)
    {
        if (pool == null) pool = new List<AnimatedText>();
        if (pool.Count == 0) return CreateAnimatedText(prefab, pool);
        var existing = GetExistingAnimatedText(prefab, pool);
        if (existing == null) return CreateAnimatedText(prefab, pool );
        return existing;
    }

    private AnimatedText GetExistingAnimatedText(AnimatedText prefab, List<AnimatedText> pool)
    {
        foreach (var text in pool)
        {
            if (!text.gameObject.activeSelf) return text;
        }
        return null;
    }

    private AnimatedText CreateAnimatedText(AnimatedText prefab, List<AnimatedText> pool)
    {
        var text = Instantiate(prefab, transform);
        pool.Add(text);
        return text;
    }

}
