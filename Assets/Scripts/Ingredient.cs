using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string id;
    private bool _isCollected;
    private bool _isHeld;
    private float _releaseTime;

    public void Hold()
    {
        _isHeld = true;
        gameObject.layer = LayerMask.NameToLayer("HeldItem");
    }

    public void Release()
    {
        _isHeld = false;
        _releaseTime = Time.time;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public bool CanCollect()
    {
        if (_isCollected) return false;
        if (_isHeld) return false;
        return Time.time - _releaseTime < 1f;
    }

    public void Collect()
    {
        _isCollected = true;
    }

}
