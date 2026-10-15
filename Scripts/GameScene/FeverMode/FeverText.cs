//フィーバーモード開始テキスト
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FeverText : MonoBehaviour
{
    [SerializeField] private Image image; //シーン上で配置したテキスト
    [SerializeField, Min(0f)] private float presentTime = 3f; //演出時間
    [SerializeField, Min(0f)] private float amplitude = 20f; //イージング範囲
    [SerializeField, Min(0f)] private float frequency = 2f; //揺れる頻度

    private Vector2 originalPosition; //各テキストの初期座標
    private Coroutine easingCoroutine; //コルーチン
    private bool initialized; //初期化済みフラグ

    private void Awake()
    {
        Initialize(); //初期化
    }

    //各テキストの初期座標を保存
    private void Initialize()
    {
        //重複初期化を防ぐ
        if (initialized) return;

        //配列が未設定の場合は空の配列を用意

        originalPosition = image.rectTransform.anchoredPosition;


        initialized = true; //初期化完了
        SetVisibility(false); //透明化
    }

    //テキスト演出開始
    public void StartFeverEnter()
    {
        Initialize(); //初期化されていない場合に備える
        StopFeverEnter(); //前回の演出を停止・リセット

        //無効状態、または演出時間がない場合は開始しない
        if (!isActiveAndEnabled || presentTime <= 0f)
            return;

        SetVisibility(true); //テキスト表示
        easingCoroutine = StartCoroutine(TextEasingRoutine()); //演出開始
    }

    //テキスト演出停止
    public void StopFeverEnter()
    {
        //実行中のコルーチンを停止
        if (easingCoroutine != null)
        {
            StopCoroutine(easingCoroutine);
            easingCoroutine = null;
        }

        //初期化前なら座標・表示を操作しない
        if (!initialized) return;

        ApplyOffset(0f); //初期座標に戻す
        SetVisibility(false); //透明化
    }

    //テキストを揺らすコルーチン
    private IEnumerator TextEasingRoutine()
    {
        float elapsed = 0f; //経過時間

        while (elapsed < presentTime)
        {
            //演出の進行度（0～1）
            float progress = Mathf.Clamp01(elapsed / presentTime);

            //開始時と終了時は揺れを小さく、中央で最大にする
            float envelope = Mathf.Sin(progress * Mathf.PI);

            //上下に揺れる値（-1～1）
            float wave = Mathf.Sin(
                elapsed * frequency * Mathf.PI * 2f
            );

            //全テキストに同じ移動量を適用
            ApplyOffset(wave * amplitude * envelope);

            yield return null; //次のフレームまで待機

            //ゲームの時間倍率に影響されずに更新
            elapsed += Time.unscaledDeltaTime;
        }

        ApplyOffset(0f); //初期座標に戻す
        SetVisibility(false); //透明化
        easingCoroutine = null; //コルーチン終了
    }

    //各テキストの初期座標を基準に上下移動
    private void ApplyOffset(float offsetY)
    {
        image.rectTransform.anchoredPosition =
        originalPosition + Vector2.up * offsetY;
  
    }

    //透明・または見えるようにする
    private void SetVisibility(bool visible)
    {
        Color c = image.color;
        c.a = visible ? 1f : 0f;
        image.color = c;

    }

    //無効化された際に演出を停止
    private void OnDisable()
    {
        StopFeverEnter();
    }
}