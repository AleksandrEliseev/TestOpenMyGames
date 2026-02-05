using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText; 
    // public TextMeshProUGUI fpsText;

    private float pollingTime = 0.1f; // Обновлять текст раз в секунду
    private float timeAccumulator;
    private int frameAccumulator;
    private float currentFps;

    void Update()
    {
        timeAccumulator += Time.deltaTime;
        frameAccumulator++;

        if (timeAccumulator >= pollingTime)
        {
            // Формула: количество кадров / прошедшее время
            currentFps = frameAccumulator / timeAccumulator;

            // Выводим значение, округлив до целого числа
            if (fpsText != null)
            {
                fpsText.text = "FPS: " + Mathf.RoundToInt(currentFps);
            }

            // Сбрасываем счётчики
            timeAccumulator = 0f;
            frameAccumulator = 0;
        }
    }
}
