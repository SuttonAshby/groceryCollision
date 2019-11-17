using UnityEngine;

public class Manager : MonoBehaviour
{
    private ItemTree cart1State;
    private Cart cart1;
    private Cart cart2;
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

        Cart[] carts = FindObjectsOfType<Cart>();
        foreach (Cart c in carts)
        {
            if (c.name == "Cart1") cart1 = c;
            else if (c.name == "Cart2") cart2 = c;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) cart1.Lockout();
        if (Input.GetMouseButton(1)) cart2.Extend();
        else cart2.Retract();
    }
}
