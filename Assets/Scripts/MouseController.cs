using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{


    public float flingVelocityFactor = 0.1f;
    public float holdHeight = 5f;
    public KeyCode player1CartKey;
    public KeyCode player2CartKey;
    public Cart player1Cart;
    public Cart player2Cart;

    private class HeldObject
    {
        public GameObject colliderGameObject;
        public Rigidbody rigidbody;
        public Ingredient ingredient;
        public Vector2 averageVelocity = Vector3.zero;

        private float averageWeightingFactor = 0.33f;

        public void TrackVelocity(Vector2 deltaPosition, float deltaTime)
        {
            averageVelocity = (averageVelocity * (1 - averageWeightingFactor)) +
                (averageWeightingFactor * deltaPosition / deltaTime);
        }
    }

    private HeldObject _heldObject;
    private Vector2 _lastMousePosition;
    private float _lastMouseTime;

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif
    }

    private void OnEnable()
    {
        MouseEventSender.MouseDown += MouseDown;
        MouseEventSender.MouseDrag += MouseDrag;
        MouseEventSender.MouseUp += MouseUp;
    }

    private void OnDisable()
    {
        MouseEventSender.MouseDown -= MouseDown;
        MouseEventSender.MouseDrag -= MouseDrag;
        MouseEventSender.MouseUp -= MouseUp;
    }

    private void Update()
    {
        if (Input.GetKeyDown(player1CartKey)) player1Cart.Extend();
        if (Input.GetKeyDown(player2CartKey)) player2Cart.Extend();
        if (Input.GetKeyUp(player1CartKey)) player1Cart.Retract();
        if (Input.GetKeyUp(player2CartKey)) player2Cart.Retract();
    }

    private void MouseDown(GameObject obj)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (rb == null) return;

        var ingredient = rb.gameObject.GetComponent<Ingredient>();
        if (ingredient == null) return;
        if (!ingredient.CanHold()) return;

        _heldObject = new HeldObject()
        {
            colliderGameObject = obj,
            rigidbody = rb,
            ingredient = ingredient
        };

        ingredient.Hold();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.DOMoveY(Camera.main.transform.position.y - holdHeight, 0.1f);
        _lastMousePosition = Input.mousePosition;
        _lastMouseTime = Time.time;
    }

    private void MouseDrag(GameObject obj)
    {
        if (_heldObject == null) return;
        if (_heldObject.colliderGameObject != obj) return;

        var deltaPosition = (Vector2)Input.mousePosition - _lastMousePosition;
        var deltaTime = Time.time - _lastMouseTime;

        _heldObject.TrackVelocity(deltaPosition, deltaTime);
        var touchPoint = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.transform.position.y - holdHeight
        ));
        _heldObject.rigidbody.MovePosition(touchPoint);

        var rotation = Quaternion.identity;
        if (deltaPosition.x > 0)
        {
            _heldObject.rigidbody.MoveRotation(Quaternion.Euler(0, 90, 0));
        }
        else if (deltaPosition.x < 0)
        {
            _heldObject.rigidbody.MoveRotation(Quaternion.Euler(0, -90, 0));
        }

        _lastMousePosition = Input.mousePosition;
        _lastMouseTime = Time.time;
    }

    private void MouseUp(GameObject obj)
    {
        if (_heldObject == null) return;
        if (_heldObject.colliderGameObject != obj) return;

        _heldObject.ingredient.Release();
        _heldObject.rigidbody.useGravity = true;
        _heldObject.rigidbody.isKinematic = false;

        var velocityVector = _heldObject.averageVelocity * flingVelocityFactor;

        _heldObject.rigidbody.AddForce(new Vector3(
            velocityVector.x,
            _heldObject.rigidbody.velocity.y,
            velocityVector.y
        ));
        _heldObject = null;
    }

}
