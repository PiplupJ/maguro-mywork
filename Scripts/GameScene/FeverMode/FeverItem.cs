using UnityEngine;
using System;

public class FeverItem : MonoBehaviour
{
    public event Action<FeverItem> Released; //消費された
    //protected abstract void ApplyEffect(); //固有の効果。継承したクラスで定義すること
    //protected bool OnProcess = false; //重複実行を防ぐためのフラグ

    protected Renderer[] renderers; //アイテム点滅用。レンダラー情報
    protected const int blinkInterval = 8; //点滅周期。フレーム単位
    protected int blinkTimer; //点滅タイマー
    protected bool isBlinking; //点滅中
    
    protected virtual void Awake()
    {
        //レンダラーを読み込む
        renderers = GetComponentsInChildren<Renderer>();
        //初期は点滅しない
        isBlinking = false;
        blinkTimer = 0;
    }

    //アイテムを消費
    public void Consume()
    {
        //重複使用を防ぐ
        //if (OnProcess) { return; }
        //OnProcess = true;

        //ApplyEffect(); //効果適用

        Released?.Invoke(this); //デリケート実行。Registryから除外
    }

    //当たり判定
    //protected void OnTriggerEnter(Collider other)
    //{
        //プレイヤーに当たったら消費処理
      //  if (other.CompareTag("Player"))
       // {
         //   Consume();　//消費
        //}
    //}

    //フィーバータイムが終了が近いなら点滅するため
    public void SetBlinking(bool on) 
    {
        isBlinking = on;

        //点滅が終わった場合
        if (!isBlinking)
        {
            //見えるようにする
            foreach (var r in renderers)
            {
                r.enabled = true;
            }
        }
    }

    //アイテムが獲得できる時間が少ないなら点滅
    protected void Update()
    {
        //点滅中ではないなら、リターン
        if (!isBlinking) return;

        //タイマー更新
        blinkTimer = (blinkTimer + 1) % (blinkInterval * 2);

        //見える時間
        if (blinkTimer == 0)
        {
            foreach (var r in renderers)
            {
                r.enabled = true;
            }
        }
        //消える時間
        else if (blinkTimer == blinkInterval)
        {
            foreach (var r in renderers)
            {
                r.enabled = false;
            }
        }
    }
}
