using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class FeverLighting : MonoBehaviour
{
    [SerializeField] private Light sceneLight; //シーンのライトを参照する変数
    [SerializeField, Range(0f, 1f)] private float dimRatio = 0.4f;　//ライトの暗さの比率を設定する変数
    [SerializeField, Min(0f)] private float dimTime = 0.5f; //ライトを暗くする時間を設定する変数
    [SerializeField, Min(0f)] private float brightenTime = 0.5f; //ライトを明るくする時間を設定する変数

    private float originalIntensity; //ライトの元の強度を保存する変数
    private float originalAmbientIntensity; //ライトの元の環境光強度を保存する変数
    private Color originalAmbientLight; //ライトの元の環境光を保存する変数

    private float currentRatio = 1; //現在の明るさ倍率を保存する変数

    private Coroutine dimCoroutine; //ライトを暗くするコルーチンを保存する変数

    private void Awake()
    {
        if (sceneLight == null)
        {
            sceneLight = FindObjectOfType<Light>();
        }
        originalIntensity = sceneLight.intensity;
        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalAmbientLight = RenderSettings.ambientLight;
    }

    public void Dim()
    {
        FadeTo(dimRatio, dimTime);
    }

    public void Brighten()
    {
        FadeTo(1f, brightenTime);
    }
    
    public void BrightenImmidiate()
    {
        if (dimCoroutine != null)
        {
            StopCoroutine(dimCoroutine);
            dimCoroutine = null;
        }

        ApplyLighting(1f);
    }

    private void FadeTo(float target, float time)
    {
        if (dimCoroutine != null)
        {
            StopCoroutine(dimCoroutine);
            dimCoroutine = null;
        }

        dimCoroutine = StartCoroutine(FadeRoutine(target, time));
    }

    private IEnumerator FadeRoutine(float target, float time)
    {
        float start = currentRatio;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.unscaledDeltaTime; //

            float t = Mathf.Clamp01(elapsedTime / time);
            t = t * t * (3f - 2f * t); //スムーズステップ

            ApplyLighting(Mathf.Lerp(start, target, t));

            yield return null;
        }

        ApplyLighting(target);
        dimCoroutine = null;
    }

    private void ApplyLighting(float ratio)
    {
        currentRatio = ratio;

        sceneLight.intensity = originalIntensity * ratio;

        RenderSettings.ambientIntensity = originalAmbientIntensity * ratio;
        RenderSettings.ambientLight = originalAmbientLight*ratio;
    }

    
}
