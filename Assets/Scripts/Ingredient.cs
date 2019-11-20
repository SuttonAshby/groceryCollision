using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string id;
    private bool _isCollected;
    private bool _isHeld;
    private float _releaseTime;

    public bool CanHold()
    {
        if (_isCollected) return false;
        if (_isHeld) return false;
        return true;
    }

    public void Hold()
    {
        _isHeld = true;
        gameObject.SetLayerRecursively("HeldItem");
    }

    public void Release()
    {
        _isHeld = false;
        _releaseTime = Time.time;
        gameObject.SetLayerRecursively("Default");
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
