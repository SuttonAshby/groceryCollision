using UnityEngine;
using System.Collections.Generic;

public class CartCatcher : MonoBehaviour
{
    public Cart cart;
    public AudioClip[] sounds;

    private HashSet<Collider> caughtObjects = new HashSet<Collider>();
    private AudioSource _audioSource;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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

        _audioSource.clip = sounds[Random.Range(0, sounds.Length)];
        _audioSource.Play();

        cart.AddItem(ingredient);
    }

}
