using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{

    [System.Serializable]
    public class PlayerRefs
    {
        public Recipes recipes;
        public Cart cart;
        public PlayerHUD hud;
    }

    public PlayerRefs player1Refs;
    public PlayerRefs player2Refs;

    private void Start()
    {
        player1Refs.recipes.Reset();
        player2Refs.recipes.Reset();
    }

    public void GotItem(Cart cart, string itemName)
    {
        PlayerRefs player = null;
        PlayerRefs otherPlayer = null;

        if (cart == player1Refs.cart)
        {
            player = player1Refs;
            otherPlayer = player2Refs;
        }
        else if (cart == player2Refs.cart)
        {
            player = player2Refs;
            otherPlayer = player1Refs;
        }

        if (player.recipes.itemTree.HasItem(itemName))
        {
            var itemNeeded = player.recipes.itemTree.SetItemDone(itemName);
            if (itemNeeded)
            {
                player.hud.CollectedItem(itemName);
            }
            else
            {
                player.hud.CollectedTrash();
            }
            var result = player.recipes.UpdateRecipes();
            var delay = 1f;
            foreach (var completedRecipe in result.completedRecipes)
            {
                StartCoroutine(ShowCompletedRecipe(player.hud, completedRecipe, delay));
                delay += 2f;
            }
            if (result.allRecipesComplete)
            {
                player.hud.PlayerWon();
                otherPlayer.hud.PlayerLost();
                StartCoroutine(GameOver());
            }
        }
        else
        {
            player.hud.CollectedTrash();
        }
    }

    private IEnumerator ShowCompletedRecipe(PlayerHUD hud, string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        hud.CompletedRecipe(name);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(1);
    }

}
