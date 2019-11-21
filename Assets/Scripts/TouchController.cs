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
        public GameObject obj;
        public Rigidbody rigidbody;
        public Ingredient ingredient;
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
            if (hit.collider == null) return;

            var cartOpener = hit.collider.gameObject.GetComponent<CartOpener>();
            if (cartOpener != null)
            {
                gameObjectsByFinger.Add(touch.fingerId, new TouchedObject()
                {
                    obj = hit.collider.gameObject,
                });

                cartOpener.cart.Extend();
            }
            else
            {
                var rb = hit.rigidbody;
                if (rb == null) return;

                var ingredient = rb.gameObject.GetComponent<Ingredient>();
                if (ingredient == null) return;
                if (!ingredient.CanHold()) return;

                gameObjectsByFinger.Add(touch.fingerId, new TouchedObject()
                {
                    obj = hit.rigidbody.gameObject,
                    rigidbody = rb,
                    ingredient = ingredient
                });

                ingredient.Hold();
                hit.rigidbody.useGravity = false;
                hit.rigidbody.isKinematic = true;
                var dest = Camera.main.ScreenToWorldPoint(new Vector3(
                    touch.position.x,
                    touch.position.y,
                    Camera.main.transform.position.y - holdHeight
                ));
                rb.DOMove(dest, 0.1f);
            }
        }
    }

    private void HandleMove(Touch touch)
    {
        ForTouchedObject(touch, (touchedObject, fingerId) =>
        {
            var obj = touchedObject.obj;
            var cartOpener = obj.GetComponent<CartOpener>();
            if (cartOpener != null)
            {
                // Nice to have but not worth the trouble of getting right currently
                // if (!IsTouchOverObject(touch, obj))
                // {
                //     cartOpener.cart.Retract();
                //     gameObjectsByFinger.Remove(fingerId);
                // }
            }
            else
            {
                if (touchedObject.obj.GetComponent<Rigidbody>() == null)
                {
                    gameObjectsByFinger.Remove(fingerId);
                    return;
                }
                touchedObject.TrackVelocity(touch);
                var touchPoint = Camera.main.ScreenToWorldPoint(new Vector3(
                    touch.position.x,
                    touch.position.y,
                    Camera.main.transform.position.y - holdHeight
                ));
                touchedObject.rigidbody.MovePosition(touchPoint);

                var rotation = Quaternion.identity;
                if (touch.deltaPosition.x > 0)
                {
                    touchedObject.rigidbody.MoveRotation(Quaternion.Euler(0, 90, 0));
                }
                else if (touch.deltaPosition.x < 0)
                {
                    touchedObject.rigidbody.MoveRotation(Quaternion.Euler(0, -90, 0));
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
            var obj = touchedObject.obj;
            gameObjectsByFinger.Remove(fingerId);
            
            var cartOpener = obj.GetComponent<CartOpener>();
            if (cartOpener != null)
            {
                cartOpener.cart.Retract();
            }
            else
            {
                touchedObject.ingredient.Release();

                touchedObject.rigidbody.useGravity = true;
                touchedObject.rigidbody.isKinematic = false;

                var velocityVector = touchedObject.averageVelocity * flingVelocityFactor;

                touchedObject.rigidbody.AddForce(new Vector3(
                    velocityVector.x,
                    touchedObject.rigidbody.velocity.y,
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
            hit.collider.gameObject == go;
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
