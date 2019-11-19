using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventSender : MonoBehaviour
{

    public static event Action<GameObject> MouseDown;
    public static event Action<GameObject> MouseDrag;
    public static event Action<GameObject> MouseUp;

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this);
#endif
    }

    private void OnMouseDown()
    {
        MouseDown?.Invoke(gameObject);
    }

    private void OnMouseDrag()
    {
        MouseDrag?.Invoke(gameObject);
    }

    private void OnMouseUp()
    {
        MouseUp?.Invoke(gameObject);
    }

}
