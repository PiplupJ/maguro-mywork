using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

//点滅するテキストを制御するクラス
public class BlinkingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mText;
    [SerializeField] private float minAlpha = 0f;
    [SerializeField] private float blinkInterval = 0.6f; 

    private Tween blinkTween;

    private void OnEnable() 
    {
        StartCoroutine(ExecuteAfterDelay());
    }

    IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        mText.alpha = 1f;

        blinkTween = mText.DOFade(minAlpha, blinkInterval)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDisable()
    {
        blinkTween?.Kill();
        blinkTween = null;
    }
}
