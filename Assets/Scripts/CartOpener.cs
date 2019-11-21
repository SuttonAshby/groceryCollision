using UnityEngine;
using System.Collections.Generic;

public class CartOpener : MonoBehaviour
{
    public Cart cart;

    private void OnMouseDown()
    {
        if (Input.touchCount > 0) return;
        cart.Extend();
    }

    private void OnMouseUp()
    {
        if (Input.touchCount > 0) return;
        cart.Retract();
    }

}
