using UnityEngine;
using System.Collections.Generic;

public class CartOpener : MonoBehaviour
{
    public Cart cart;

    private void OnMouseDown()
    {
#if !UNITY_EDITOR
        return;
#endif
        if (Input.touchCount > 0) return;
        cart.Extend();
    }

    private void OnMouseUp()
    {
#if !UNITY_EDITOR
        return;
#endif
        if (Input.touchCount > 0) return;
        cart.Retract();
    }

}
