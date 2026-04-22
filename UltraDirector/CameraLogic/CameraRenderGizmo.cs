using System;
using System.Diagnostics.CodeAnalysis;
using UltraDirector.Utils;
using UnityEngine;

namespace UltraDirector.CameraLogic;

[RequireComponent(typeof(Camera))]
public sealed class CameraRenderGizmo : MonoBehaviour
{
    private MeshFilter _meshFilter = null!;
    private MeshRenderer _meshRenderer = null!;
    private Mesh _mesh = null!;
    public Camera camera = null!;
    private Material _frustumMaterial = null!;
    private void Awake()
    {
        camera = GetComponent<Camera>()
              ?? throw new Exception("CameraRenderGizmo is attached to object without a camera");
        var frustumObject = new GameObject("FrustumVisual");
        frustumObject.transform.SetParent(transform, false);
        frustumObject.layer = gameObject.layer;

        _meshFilter = frustumObject.AddComponent<MeshFilter>();
        _meshRenderer = frustumObject.AddComponent<MeshRenderer>();

        _mesh = new Mesh { name = "CameraFrustumMesh" };
        _meshFilter.sharedMesh = _mesh;

        var shader = Shader.Find("Sprites/Default")
                  ?? throw new Exception("Shader not found: Sprites/Default");
        _frustumMaterial = new Material(shader)
        {
            color = new Color(0.2f, 0.8f, 1f, 0.2f)
        };
        _meshRenderer.sharedMaterial = _frustumMaterial;
        RebuildFrustumMesh();
    }

    private void OnEnable()
    {
        _meshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _meshRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        _frustumMaterial.Destroy();
        _mesh.Destroy();
    }
    private void LateUpdate()
    {
        // Keep this if camera properties can change in runtime.
        RebuildFrustumMesh();
    }

    private readonly float[] dataCompare = new float[4];
    private readonly Vector3[] vecBuffer = new Vector3[8];
    private readonly int[] triangles =
    [
        0, 3, 1, 0, 2, 3, // near:   quad of (0, 1, 2, 3)
        4, 5, 7, 4, 7, 6, // far:    quad of (4, 5, 6, 7)
        0, 6, 2, 0, 4, 6, // left:   quad of (0, 3, 4, 7)
        1, 3, 7, 1, 7, 5, // right:  quad of (1, 2, 5, 6)
        2, 6, 7, 2, 7, 3, // top:    quad of (2, 3, 6, 7)
        0, 1, 5, 0, 5, 4  // bottom: quad of (0, 1, 4, 5)
    ];
    [SuppressMessage("ReSharper", "MultipleSpaces")]
    private void RebuildFrustumMesh()
    {
        ReadOnlySpan<float> sp = stackalloc float[4]
        {
            camera.nearClipPlane,
            camera.farClipPlane,
            camera.fieldOfView,
            camera.aspect
        };
        if (dataCompare.SequenceEqual(sp)) return;
        sp.CopyTo(dataCompare);

        var near = camera.nearClipPlane;
        var far = camera.farClipPlane;
        var halfFovRad = camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
        var aspect = camera.aspect;

        var nearHalfHeight = Mathf.Tan(halfFovRad) * near;
        var nearHalfWidth = nearHalfHeight * aspect;
        var farHalfHeight = Mathf.Tan(halfFovRad) * far;
        var farHalfWidth = farHalfHeight * aspect;


        for (var i = 0; i < 8; i++)
        {
            var v = i < 4
                ? new Vector3(nearHalfWidth, nearHalfHeight, near)
                : new Vector3(farHalfWidth, farHalfHeight, far);
            if (i % 2 == 0) v.x = -v.x;
            if (i % 4 < 2) v.y = -v.y;
            vecBuffer[i] = v;
        }

        _mesh.Clear();
        _mesh.vertices = vecBuffer;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
}