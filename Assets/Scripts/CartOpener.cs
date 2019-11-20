using UnityEngine;
using System.Collections.Generic;

public class CartOpener : MonoBehaviour
{
    public Cart cart;

    private void OnMouseDown()
    {
        cart.Extend();
    }

    private void OnMouseUp()
    {
        cart.Retract();
    }

}
