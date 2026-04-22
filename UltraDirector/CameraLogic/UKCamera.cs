using System;
using JetBrains.Annotations;
using UltraDirector.CameraLogic.Input.Audio;
using UltraDirector.CameraLogic.Input.Video;
using UnityEngine;

namespace UltraDirector.CameraLogic;
[PublicAPI]
[Serializable]
public sealed partial class UKCamera : MonoBehaviour, IDisposable
{
    public string Name => gameObject.name;
    public Camera Camera { get; private set; } = null!;
    public event Action<UKCamera>? OnUpdate = null;
    public event Action<UKCamera>? OnLateUpdate = null;
    public event Action<UKCamera>? OnFixedUpdate = null;

    // TODO: make this spawn a window and have a record method too
    private void Awake()
    {
        // TODO: MAKE A FUCKING PREFAB FOR THIS
        Camera = gameObject.GetOrAddComponent<Camera>();
        Camera.cullingMask = (int)(GameObjectLayer.TransparentFX
                                 | GameObjectLayer.Water
                                 | GameObjectLayer.Unused6
                                 | GameObjectLayer.Projectile
                                 | GameObjectLayer.BrokenGlass
                                 | GameObjectLayer.VirtualScreen
                                 | GameObjectLayer.GroundCheck
                                 | GameObjectLayer.Item
                                 | GameObjectLayer.SandboxGrabbable
                                 | GameObjectLayer.Portal
                                 | GameObjectLayer.Unused31);
        // TODO: fix portal and blood splatter rendering (maybe treat these cams as a portal?)
        if (AudioSource == null)
            AudioSource = FindFirstObjectByType<AudioListener>()
                         .gameObject
                         .GetOrAddComponent<AudioCompReader>();
        if (AudioSource == null) LogWarning("Could not find AudioSource.");

        if (VideoSource == null)
        {
            VideoSource = gameObject.GetOrAddComponent<VideoRecorder>();
            (VideoSource as VideoRecorder)?.camera = Camera;
        }
        if (VideoSource == null) LogWarning("Could not find VideoSource.");
        // CameraManager.Instance!.AddCamera(this);
    }
    private void OnEnable() => Camera.enabled = true;
    private void OnDisable() => Camera.enabled = false;
    private void OnDestroy()
    {
        CameraManager.Instance!.Cameras.Remove(Name);
        Dispose();
    }

    private void Update() => OnUpdate?.Invoke(this);
    private void LateUpdate() => OnLateUpdate?.Invoke(this);

    private void FixedUpdate() => OnFixedUpdate?.Invoke(this);

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
               $"Position: {Camera.transform.position}, " +
               $"Rotation: {Camera.transform.rotation.eulerAngles}, " +
               $"Depth: {Camera.depth})";
    }

    void IDisposable.Dispose() => Dispose();

    internal void Dispose()
    {
        _output?.Dispose();
    }
}