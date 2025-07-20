using System;
using UnityEngine;

namespace UltraDirector.CameraLogic;
[Serializable]
public sealed class UKCamera(Camera camera)
{
    // TODO: fix skybox rendering
    public Camera UnityCamera { get; } = camera;
    public string Name => UnityCamera.gameObject.name;
    
    public void SetActive(bool isActive = true)
    {
        UnityCamera.enabled = isActive;
    }

    public override string ToString()
    {
        return $"UKCamera: " +
               $"(Name: {Name}, " +
               $"Position: {UnityCamera.transform.position}, " +
               $"Rotation: {UnityCamera.transform.rotation.eulerAngles}, " +
               $"Depth: {UnityCamera.depth})";
    }
    // TODO: implement an event for when the camera is removed
}