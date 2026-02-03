using UnityEngine;

public class BackgroundAspectFilter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        Camera camera = Camera.main;
        if (camera == null)
            return;

        // Передаём в метод текущие размеры экрана в юнитах камеры
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * camera.aspect;

        ResizeToCameraAspect(worldScreenWidth, worldScreenHeight);
    }
    public void ResizeToCameraAspect(float width, float height)
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

        // Размер видимой области камеры в мировых координатах
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * camera.aspect;

        // Размер спрайта в мировых координатах
        Sprite sprite = _spriteRenderer.sprite;

        // Размеры спрайта с учетом pixelsPerUnit
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;

        // Коэффициенты масштабирования, чтобы перекрыть экран по обеим осям
        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        // Берем больший коэффициент, чтобы спрайт гарантированно закрывал фон полностью
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

        // Передаём в метод текущие размеры экрана в юнитах камеры
        float worldScreenHeight = camera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * camera.aspect;

        ResizeToCameraAspect(worldScreenWidth, worldScreenHeight);
    }
#endif
    
}
