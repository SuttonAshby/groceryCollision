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
        public GameObject gameObject;
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
        _heldObject = new HeldObject()
        {
            gameObject = obj
        };

        rb.useGravity = false;
        rb.isKinematic = true;
        gameObject.transform.DOMoveY(holdHeight, 0.1f);
        _lastMousePosition = Input.mousePosition;
        _lastMouseTime = Time.time;
    }

    private void MouseDrag(GameObject obj)
    {
        if (_heldObject == null) return;
        if (_heldObject.gameObject != obj) return;
        if (_heldObject.gameObject.GetComponent<Rigidbody>() == null)
        {
            _heldObject = null;
            return;
        }
        var gameObject = _heldObject.gameObject;
        var deltaPosition = (Vector2)Input.mousePosition - _lastMousePosition;
        var deltaTime = Time.time - _lastMouseTime;

        _heldObject.TrackVelocity(deltaPosition, deltaTime);
        var touchPoint = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.transform.position.y - holdHeight
        ));
        gameObject.transform.position = touchPoint;

        var rotation = Quaternion.identity;
        if (deltaPosition.x > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (deltaPosition.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        _lastMousePosition = Input.mousePosition;
        _lastMouseTime = Time.time;
    }

    private void MouseUp(GameObject obj)
    {
        if (_heldObject == null) return;
        if (_heldObject.gameObject != obj) return;
        if (_heldObject.gameObject.GetComponent<Rigidbody>() == null)
        {
            _heldObject = null;
            return;
        }

        var gameObject = _heldObject.gameObject;

        var rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;

        var velocityVector = _heldObject.averageVelocity * flingVelocityFactor;

        rigidbody.AddForce(new Vector3(
            velocityVector.x,
            rigidbody.velocity.y,
            velocityVector.y
        ));
        _heldObject = null;
    }

    private bool IsTouchOverObject(Touch touch, GameObject go)
    {
        var ray = Camera.main.ScreenPointToRay(touch.position);

        RaycastHit hit;
        return Physics.Raycast(ray, out hit) &&
            hit.rigidbody.gameObject == go;
    }

}
