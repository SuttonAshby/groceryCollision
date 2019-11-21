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
                player.hud.CollectedTrash("Extra!");
                player.cart.Retract();
            }
            if (player.recipes.AreRecipesComplete())
            {
                player.hud.PlayerWon();
                otherPlayer.hud.PlayerWon();
            }
        }
        else
        {
            player.hud.CollectedTrash("Unneeded!");
            player.cart.Retract();
        }
    }

}
