using GameCamera;
using UnityEngine;
using VContainer;

public class BackgroundAspectFilter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Inject]
    private void Construct(CameraContainer cameraContainer)
    {
        Camera camera = cameraContainer.GameCamera;
        if (camera == null)
            return;
        
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * camera.aspect;

        ResizeToCameraAspect(worldScreenWidth, worldScreenHeight);
    }
  
    private void ResizeToCameraAspect(float width, float height)
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("BackgroundAspectFilter: SpriteRenderer is not assigned and not found on GameObject.");
                return;
            }
        }

        Camera camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError("BackgroundAspectFilter: Main Camera not found.");
            return;
        }
        
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * camera.aspect;
        
        Sprite sprite = _spriteRenderer.sprite;
        
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;
        
        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        float scale = Mathf.Max(scaleX, scaleY);
        
        Vector3 baseScale = Vector3.one;

        transform.localScale = new Vector3(baseScale.x * scale, baseScale.y * scale, baseScale.z);
    }

#if UNITY_EDITOR
    public void OnEditorResize()
    {
        Camera camera = Camera.main;
        if (camera == null)
            return;
        
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * camera.aspect;

        ResizeToCameraAspect(worldScreenWidth, worldScreenHeight);
    }
#endif
    
}
