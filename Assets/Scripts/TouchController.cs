using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour 
{

    public float flingVelocityFactor = 0.5f;
    public float holdHeight = 5f;
    private Dictionary<int, GameObject> gameObjectsByFinger = new Dictionary<int, GameObject>();

    private void Update()
    {
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    HandleGrab(touch);
                    break;
                case TouchPhase.Moved:
                    HandleMove(touch);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    HandleRelease(touch);
                    break;
            }
        }
    }

    private void HandleGrab(Touch touch)
    {
        var ray = Camera.main.ScreenPointToRay(touch.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var gameObject = hit.rigidbody.gameObject;
            gameObjectsByFinger.Add(touch.fingerId, gameObject);

            var cart = gameObject.GetComponent<Cart>();
            if (cart != null)
            {
                cart.Extend();
            }
            else 
            {
                hit.rigidbody.useGravity = false;
                gameObject.transform.DOMoveY(holdHeight, 0.1f);
            }
        }
    }

    private void HandleMove(Touch touch)
    {   
        var fingerId = touch.fingerId;
        if (gameObjectsByFinger.ContainsKey(fingerId))
        {
            var gameObject = gameObjectsByFinger[fingerId];

            var cart = gameObject.GetComponent<Cart>();
            if (cart != null)
            {
                if (!IsTouchOverObject(touch, gameObject))
                {
                    cart.Retract();
                    gameObjectsByFinger.Remove(fingerId);
                }
            }
            else
            {
                var touchPoint = ScreenToWorldPoint(touch.position);
                gameObject.transform.position = new Vector3(
                    touchPoint.x,
                    holdHeight,
                    touchPoint.z
                );

                var rotation = Quaternion.identity;
                if (touch.deltaPosition.x > 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                } 
                else if (touch.deltaPosition.x < 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
        }
    }

    private void HandleRelease(Touch touch)
    {
        var fingerId = touch.fingerId;
        if (gameObjectsByFinger.ContainsKey(fingerId))
        {
            var gameObject = gameObjectsByFinger[fingerId];

            var cart = gameObject.GetComponent<Cart>();
            if (cart != null)
            {
                cart.Retract();
                gameObjectsByFinger.Remove(fingerId);
            }
            else
            {
                gameObjectsByFinger.Remove(fingerId);
                
                var rigidbody = gameObject.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                
                var velocityVector = touch.deltaPosition / touch.deltaTime * flingVelocityFactor;
                
                rigidbody.AddForce(new Vector3(
                    velocityVector.x,
                    rigidbody.velocity.y,
                    velocityVector.y
                ));         
            }
        }
    }

    private bool IsTouchOverObject(Touch touch, GameObject go)
    {
        var ray = Camera.main.ScreenPointToRay(touch.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.rigidbody.gameObject == go;
        }
        else
        {
            return false;
        }
    }

    private Vector3 ScreenToWorldPoint(Vector2 screenPos)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(
            screenPos.x,
            screenPos.y,
            holdHeight
        ));
    }
}
