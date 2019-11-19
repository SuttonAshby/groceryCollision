using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{

    public Camera camera;

    private void Start()
    {
        #if UNITY_IOS
        camera.rect = new Rect(0f, 0.1f, 1f, 0.8f);
        #endif
    }

}
