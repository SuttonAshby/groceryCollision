using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    private Manager manager;
    void Start()
    {
        manager = FindObjectOfType<Manager>();
        if(!manager)
        {
            Debug.LogError("no Manager");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        manager.GotItem(otherName);
        Destroy(other.gameObject);
    }
}
