using UnityEngine;
using System.Collections.Generic;

public class CartCatcher : MonoBehaviour
{
    public Cart cart;
    private HashSet<Collider> caughtObjects = new HashSet<Collider>();

    private void OnTriggerStay(Collider other)
    {
        if (!cart.CanCollect()) return;

        if (caughtObjects.Contains(other)) return;

        if (other.attachedRigidbody == null) return;

        var ingredient = other.attachedRigidbody.GetComponent<Ingredient>();
        if (ingredient == null) return;

        if (!ingredient.CanCollect()) return;

        ingredient.Collect();
        caughtObjects.Add(other);
        cart.AddItem(ingredient);
    }

}
