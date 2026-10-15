//プール用エフェクトクラス
using UnityEngine;

public class PooledEffect : MonoBehaviour
{
    private ParticleSystem rootParticle; //ルートパーティクルシステムのキャッシュ

    [SerializeField] protected SoundType soundType = SoundType.None; //再生するサウンドの種類

    private void Cache()
    {
        if (rootParticle == null)
            rootParticle = GetComponent<ParticleSystem>();
    }

    //エフェクトを指定した座標で再生する
    public void Show(Vector3 position)
    {
        Cache();

        rootParticle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        transform.position = position;
        gameObject.SetActive(true);
        rootParticle.Play(true);
        //指定されたサウンドがあれば再生
        if (soundType != SoundType.None)
        {
            SoundPlayer.Instance.PlaySE(soundType);
        }
    }


    //エフェクトを停止して非表示にする
    public void ResetAndHide()
    {
        Cache();

        rootParticle.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        gameObject.SetActive(false);
    }
}