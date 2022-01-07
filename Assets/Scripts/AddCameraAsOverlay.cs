using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class AddCameraAsOverlay : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = this.GetComponent<Camera>();
    }

    private void Start()
    {
        UniversalAdditionalCameraData mainCameraData = Camera.main.GetUniversalAdditionalCameraData();
        mainCameraData.cameraStack.Add(_camera);
    }
}
