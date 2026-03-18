using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltraDirector.CameraLogic;
[PublicAPI]
[Serializable]
[RequireComponent(typeof(Camera))]
public sealed class UKCamera : MonoBehaviour
{
    [field: SerializeField]
    public Camera UnityCamera { get; private set; } = null!;
    public string Name => gameObject.name;
    public event Action<UKCamera>? OnUpdate = null;
    public event Action<UKCamera>? OnLateUpdate = null;
    public event Action<UKCamera>? OnFixedUpdate = null;

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
    private void Update() => OnUpdate?.Invoke(this);
    private void LateUpdate() => OnLateUpdate?.Invoke(this);

    private void FixedUpdate() => OnFixedUpdate?.Invoke(this);

    public void StartRecording(string outputPath) => throw new NotImplementedException("Recording functionality is not implemented yet.");
    public void StopRecording() => throw new NotImplementedException("Recording functionality is not implemented yet.");
    public void ClearUpdateCallbacks(bool update = true, bool lateUpdate = true, bool fixedUpdate = true)
    {
        if (update) OnUpdate = null;
        if (lateUpdate) OnLateUpdate = null;
        if (fixedUpdate) OnFixedUpdate = null;
    }
    public override string ToString()
    {
        return $"UKCamera: " +
               $"(Name: {Name}, " +
               $"Position: {UnityCamera.transform.position}, " +
               $"Rotation: {UnityCamera.transform.rotation.eulerAngles}, " +
               $"Depth: {UnityCamera.depth})";
    }
}