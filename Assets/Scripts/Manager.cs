using UnityEngine;

public class Manager : MonoBehaviour
{
    public Recipes player1Recipes;
    public Recipes player2Recipes;
    private Cart cart1;
    private Cart cart2;
    // Start is called before the first frame update
    void Start()
    {
        Cart[] carts = FindObjectsOfType<Cart>();
        foreach (Cart c in carts)
        {
            if (c.name == "Cart1") cart1 = c;
            else if (c.name == "Cart2") cart2 = c;
        }
    }

    public void GotItem(Cart cart, string itemName)
    {
        print("Got " + itemName);
        if (cart == cart1 && player1Recipes.itemTree.HasItem(itemName))
        {
            player1Recipes.itemTree.SetItemDone(itemName);
            if (player1Recipes.AreRecipesComplete())
            {
                print("Player 1 Won");
            }
        }
        else if (cart == cart2 && player2Recipes.itemTree.HasItem(itemName))
        {
            player2Recipes.itemTree.SetItemDone(itemName);
            if (player2Recipes.AreRecipesComplete())
            {
                print("Player 2 Won");
            }
        }
        else
        {
            print("Bad Stuff");
        }

    }

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0)) cart1.Lockout();
        // if (Input.GetMouseButton(1)) cart2.Extend();
        // else cart2.Retract();
    }
}
