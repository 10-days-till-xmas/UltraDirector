using System.Collections.Generic;
using UnityEngine;

// ReSharper disable ParameterHidesMember

namespace UltraDirector.CameraLogic;

/// <summary>
/// Manages all cameras in the scene.
/// </summary>
/// <remarks>Note that the camera name mustn't include whitespace or else it won't be parsed</remarks>
[ConfigureSingleton(SingletonFlags.DestroyDuplicates)]
public sealed class CameraManager : MonoSingleton<CameraManager>
{
    internal KeyCode SpawnCameraKey { get; set; } = KeyCode.F1;
    internal UKCamera DefaultCamera { get; private set; } = null!;

    public Dictionary<string, UKCamera> Cameras { get; } = new();

    // TODO: properly handle reinstantiation between scenes, and maybe setup events for if cameras are destroyed or added
    protected override void Awake()
    {
        DefaultCamera = new UKCamera(CameraController.Instance.cam);
        Cameras.Add(DefaultCamera.Name, DefaultCamera);
        DontDestroyOnLoad(this);
        Log("CameraManager initialized.");
        Log($"Default camera set to: {DefaultCamera.Name}");
    }
    
    public void AddCamera(UKCamera camera)
    {
        
        if (!Cameras.TryAdd(camera.Name, camera))
        {
            LogWarning($"Camera {camera.Name} already exists!");
            return;
        }

        Log($"Camera {camera.Name} added successfully.");
    }
    
    public bool TrySpawnCamera(string name, Vector3 position, Quaternion rotation, out UKCamera? camera)
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
        unityCamera.CopyFrom(DefaultCamera.UnityCamera);
        unityCamera.depth = CameraDepth.Hidden;
        camera = new UKCamera(unityCamera);
        
        Cameras.Add(name, camera);
        
        return true;
    }
    
    public void RemoveCamera(string name)
    {
        if (!Cameras.TryGetValue(name, out var camera)) return;
        
        Destroy(camera.UnityCamera.gameObject);
        Cameras.Remove(name);
    }

    protected override void OnDestroy()
    {
        Log("CameraManager is being destroyed.");
    }

    private void OnDisable()
    {
        Log("CameraManager is being disabled.");
    }
}