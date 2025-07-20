using UnityEngine;

namespace UltraDirector.CameraLogic.Extensions;

public static class UKCameraExtensions
{
    public static void SetRectSize(this UKCamera camera, Vector2 size) 
        => camera.UnityCamera.rect = camera.UnityCamera.rect with { size = size };
    public static void SetRectSize(this UKCamera camera, float width, float height) 
        => camera.SetRectSize(new Vector2(width, height));

    public static void SetRectPosition(this UKCamera camera, Vector2 position) 
        => camera.UnityCamera.rect = camera.UnityCamera.rect with { position = position };

    public static void SetRectPosition(this UKCamera camera, float x, float y) 
        => camera.SetRectPosition(new Vector2(x, y));
}