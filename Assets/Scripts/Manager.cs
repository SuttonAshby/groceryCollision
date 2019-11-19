using UnityEngine;

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

    public void GotItem(Cart cart, string itemName)
    {
        print("Got " + itemName);
        PlayerRefs player = null;
        if (cart == player1Refs.cart) player = player1Refs;
        else if (cart == player2Refs.cart) player = player2Refs;
        else Debug.LogError("no player for cart");
        if (player.recipes.itemTree.HasItem(itemName))
        {
            player.recipes.itemTree.SetItemDone(itemName);
            if (player.recipes.AreRecipesComplete())
            {
                player.hud.PlayerWon();
            }
        }
    }

}
