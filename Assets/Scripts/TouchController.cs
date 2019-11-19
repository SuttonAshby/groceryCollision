using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public float flingVelocityFactor = 0.1f;
    public float holdHeight = 5f;

    private class TouchedObject
    {
        public GameObject gameObject;
        public Vector2 averageVelocity = Vector3.zero;

        private float averageWeightingFactor = 0.33f;

        public void TrackVelocity(Touch touch)
        {
            averageVelocity = (averageVelocity * (1 - averageWeightingFactor)) +
                (averageWeightingFactor * touch.deltaPosition / touch.deltaTime);
        }
    }

    private Dictionary<int, TouchedObject> gameObjectsByFinger = new Dictionary<int, TouchedObject>();

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
                case TouchPhase.Stationary:
                    HandleStationary(touch);
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
            if (hit.rigidbody == null) return;
            var gameObject = hit.rigidbody.gameObject;
            gameObjectsByFinger.Add(touch.fingerId, new TouchedObject()
            {
                gameObject = gameObject
            });

            var cart = gameObject.GetComponent<Cart>();
            if (cart != null)
            {
                cart.Extend();
            }
            else
            {
                hit.rigidbody.useGravity = false;
                hit.rigidbody.isKinematic = true;
                gameObject.transform.DOMoveY(holdHeight, 0.1f);
            }
        }
    }

    private void HandleMove(Touch touch)
    {
        ForTouchedObject(touch, (touchedObject, fingerId) =>
        {
            var gameObject = touchedObject.gameObject;
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
                touchedObject.TrackVelocity(touch);
                var touchPoint = Camera.main.ScreenToWorldPoint(new Vector3(
                    touch.position.x,
                    touch.position.y,
                    Camera.main.transform.position.y - holdHeight
                ));
                gameObject.transform.position = touchPoint;

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
        });
    }

    private void HandleStationary(Touch touch) {
        ForTouchedObject(touch, (gameObject, fingerId) =>
        {

        });
    }

    private void HandleRelease(Touch touch)
    {
        ForTouchedObject(touch, (touchedObject, fingerId) =>
        {
            var gameObject = touchedObject.gameObject;
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
                rigidbody.isKinematic = false;

                var velocityVector = touchedObject.averageVelocity * flingVelocityFactor;

                rigidbody.AddForce(new Vector3(
                    velocityVector.x,
                    rigidbody.velocity.y,
                    velocityVector.y
                ));
            }
        });
    }

    private bool IsTouchOverObject(Touch touch, GameObject go)
    {
        var ray = Camera.main.ScreenPointToRay(touch.position);

        RaycastHit hit;
        return Physics.Raycast(ray, out hit) &&
            hit.rigidbody.gameObject == go;
    }

    private void ForTouchedObject(Touch touch, Action<TouchedObject, int> action)
    {
        var fingerId = touch.fingerId;
        if (gameObjectsByFinger.ContainsKey(fingerId))
        {
            action(gameObjectsByFinger[fingerId], fingerId);
        }
    }
}
