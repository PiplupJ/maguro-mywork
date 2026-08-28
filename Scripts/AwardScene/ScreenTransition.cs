//画面転換用スクーリン操作
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private Image maskImage; //スクリーン＊シーンに配置すること

    //画面が見えるようになる
    public void FadeInScreen(float duration, Action onComplete = null)
    {
        StartCoroutine(ScreenFade(1.0f,0.0f, duration, onComplete));
    }

    //画面が見えないようになる
    public void FadeOutScreen(float duration, Action onComplete = null)
    {
        StartCoroutine(ScreenFade(0.0f,1.0f, duration, onComplete));
    }

    //スクリーン転換。終了後予約したメソッドを実行
    private IEnumerator ScreenFade(float from, float to, float duration, Action onComplete)
    {
        float elapsedTime = 0.0f;
        Color color = maskImage.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            color.a = Mathf.Lerp(from, to, t);
            maskImage.color = color;
            yield return null;
        }

        color.a = to;
        maskImage.color = color;

        onComplete?.Invoke();
    }
}
