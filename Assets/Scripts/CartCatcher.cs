using UnityEngine;
using System.Collections.Generic;

public class CartCatcher : MonoBehaviour
{
    private Cart cart;
    private HashSet<Collider> caughtObjects = new HashSet<Collider>();

    public void Start()
    {
        cart = GetComponentInParent<Cart>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!caughtObjects.Contains(other))
        {
            caughtObjects.Add(other);
            if (other.attachedRigidbody != null)
            {
                cart.AddItem(other.attachedRigidbody);
            }
        }
    }
}
