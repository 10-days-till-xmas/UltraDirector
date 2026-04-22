using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

// ReSharper disable ParameterHidesMember

namespace UltraDirector.CameraLogic;

/// <summary>
/// Manages all cameras in the scene.
/// </summary>
/// <remarks>Note that the camera name mustn't include whitespace or else it won't be parsed</remarks>
[Serializable]
[ConfigureSingleton(SingletonFlags.DestroyDuplicates)]
public sealed class CameraManager : MonoSingleton<CameraManager>
{
    internal KeyCode SpawnCameraKey { get; set; } = KeyCode.F1;

    public Dictionary<string, UKCamera> Cameras { get; } = new();

    // TODO: properly handle reinstantiation between scenes, and maybe setup events for if cameras are destroyed or added
    private void Awake()
    {
        DontDestroyOnLoad(this);
        LogInfo("CameraManager initialized.");
    }

    public void AddCamera(UKCamera camera)
    {

        if (!Cameras.TryAdd(camera.Name, camera))
        {
            LogWarning($"Camera {camera.Name} already exists!");
            return;
        }

        LogInfo($"Camera {camera.Name} added successfully.");
    }

    public bool TrySpawnCamera(string name, Vector3 position, Quaternion rotation, [NotNullWhen(true)] out UKCamera? camera)
    {
        if (Cameras.TryGetValue(name, out camera))
        {
            LogWarning($"Camera {name} already exists!");
            return false;
        }

        var camObject = new GameObject(name)
        {
            transform =
            {
                position = position,
                rotation = rotation
            }
        };
        var unityCamera = camObject.AddComponent<Camera>();
        unityCamera.depth = CameraDepth.Hidden;
        camera = camObject.AddComponent<UKCamera>();

        Cameras.Add(name, camera);

        return true;
    }

    public void RemoveCamera(string name)
    {
        if (!Cameras.TryGetValue(name, out var camera)) return;

        Destroy(camera.Camera.gameObject);
        Cameras.Remove(name);
    }

    private void OnDestroy() => LogInfo("CameraManager is being destroyed.");

    private void OnDisable() => LogInfo("CameraManager is being disabled.");
}