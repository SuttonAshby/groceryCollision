using UnityEngine;
using DG.Tweening;

public class Cart : MonoBehaviour
{
    private Manager manager;
    private bool Moving;
    void Start()
    {
        manager = FindObjectOfType<Manager>();
        if(!manager)
        {
            Debug.LogError("no Manager");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Lockout();
    }

    void Lockout()
    {
        if (!Moving)
        {
            Moving = true;
            transform.DOMoveZ(-5, 1).SetLoops(2, LoopType.Yoyo).SetRelative(true).OnComplete(() => Moving = false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        manager.GotItem(otherName);
        Destroy(other.gameObject);
    }
}
