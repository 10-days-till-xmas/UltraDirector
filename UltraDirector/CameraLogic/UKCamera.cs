using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UltraDirector.CameraLogic;
[PublicAPI]
[Serializable]
[RequireComponent(typeof(Camera))]
public sealed class UKCamera : MonoBehaviour
{
    [field: SerializeField]
    public Camera UnityCamera { get; private set; } = null!;
    public string Name => gameObject.name;
    public event Action? OnUpdate = null;
    public event Action? OnLateUpdate = null;
    public event Action? OnFixedUpdate = null;

    // TODO: make this spawn a window and have a record method too

    private void Awake()
    {
        // TODO: MAKE A FUCKING PREFAB FOR THIS
        UnityCamera = gameObject.GetOrAddComponent<Camera>();
        // CameraManager.Instance!.AddCamera(this);
    }
    private void OnEnable() => UnityCamera.enabled = true;
    private void OnDisable() => UnityCamera.enabled = false;
    private void OnDestroy() => CameraManager.Instance!.Cameras.Remove(Name);
    private void Update() => OnUpdate?.Invoke();
    private void LateUpdate() => OnLateUpdate?.Invoke();
    private void FixedUpdate() => OnFixedUpdate?.Invoke();

    public void StartRecording(string outputPath) => throw new NotImplementedException("Recording functionality is not implemented yet.");
    public void StopRecording() => throw new NotImplementedException("Recording functionality is not implemented yet.");

    public override string ToString()
    {
        return $"UKCamera: " +
               $"(Name: {Name}, " +
               $"Position: {UnityCamera.transform.position}, " +
               $"Rotation: {UnityCamera.transform.rotation.eulerAngles}, " +
               $"Depth: {UnityCamera.depth})";
    }
}