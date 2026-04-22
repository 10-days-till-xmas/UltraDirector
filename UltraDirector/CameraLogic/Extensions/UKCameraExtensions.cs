using UnityEngine;

namespace UltraDirector.CameraLogic.Extensions;

public static class UKCameraExtensions
{
    extension(UKCamera camera)
    {
        public void SetRectSize(Vector2 size)
            => camera.Camera.rect = camera.Camera.rect with { size = size };

        public void SetRectSize(float width, float height)
            => camera.SetRectSize(new Vector2(width, height));

        public void SetRectPosition(Vector2 position)
            => camera.Camera.rect = camera.Camera.rect with { position = position };

        public void SetRectPosition(float x, float y)
            => camera.SetRectPosition(new Vector2(x, y));

        public CameraRenderGizmo AddCameraRenderGizmo()
        {
            return camera.gameObject.GetOrAddComponent<CameraRenderGizmo>();
        }
    }
}