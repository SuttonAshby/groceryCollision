using UnityEngine;

public class Manager : MonoBehaviour
{
    public ItemTree items;
    public string cart1ListItem;
    public string cart2ListItem;
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
        if (cart == cart1 && items.HasItem(itemName))
        {
            // this.cart1List.SetItemDone(itemName);
            // if (this.cart1List.IsTreeDone())
            // {
            //     print("Game Done");
            // }
        }
        else
        {
            print("Bad Stuff");
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) cart1.Lockout();
        if (Input.GetMouseButton(1) && cart2) cart2.Extend();
        else if(cart2) cart2.Retract();
    }
}
