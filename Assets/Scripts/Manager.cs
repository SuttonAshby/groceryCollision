using UnityEngine;

public class Manager : MonoBehaviour
{
    public Recipes player1Recipes;
    public Recipes player2Recipes;
    public Cart cart1;
    public Cart cart2;  
    public WinLoseCanvas winLoseCanvas;

    public void GotItem(Cart cart, string itemName)
    {
        print("Got " + itemName);
        if (cart == cart1 && player1Recipes.itemTree.HasItem(itemName))
        {
            player1Recipes.itemTree.SetItemDone(itemName);
            if (player1Recipes.AreRecipesComplete())
            {
                winLoseCanvas.Player1Win();
            }
        }
        else if (cart == cart2 && player2Recipes.itemTree.HasItem(itemName))
        {
            player2Recipes.itemTree.SetItemDone(itemName);
            if (player2Recipes.AreRecipesComplete())
            {
                winLoseCanvas.Player2Win();
            }
        }
        else
        {
            print("Bad Stuff");
        }

    }
}
