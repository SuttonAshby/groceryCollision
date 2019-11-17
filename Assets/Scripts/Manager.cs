using UnityEngine;

public class Manager : MonoBehaviour
{
    private ItemTree cart1State;
    // Start is called before the first frame update
    void Start()
    {
        cart1State = new ItemTree();
        cart1State.Append("root", "ApplePie");
        cart1State.Append("ApplePie", "Apples");
        cart1State.Append("ApplePie", "Sugar");
        cart1State.Append("ApplePie", "Crust");
        cart1State.Append("Crust", "Milk");
        cart1State.Append("Crust", "Flour");
    }

    public void GotItem(ItemTree tree, string itemName)
    {
        print("Got " + itemName);
        if (tree.HasItem(itemName))
        {
            tree.ItemDone(itemName);
            if (tree.IsTreeDone())
            {
                print("Game Done");
            }
        }
        else
        {
            print("Bad Stuff");
        }

    }

    public void GotItem(string itemName)
    {
        print("Got " + itemName);
        if (this.cart1State.HasItem(itemName))
        {
            this.cart1State.ItemDone(itemName);
            if (this.cart1State.IsTreeDone())
            {
                print("Game Done");
            }
        }
        else
        {
            print("Bad Stuff");
        }

    }
}
