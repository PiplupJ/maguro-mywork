//プレイヤーから呼び出す。StartGameOverRoutine(座標)で実行
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameOverEffect : MonoBehaviour
{
    [SerializeField] private float totalRoutineTime = 2.0f; // ゲームオーバー演出の総時間
    [SerializeField] private int explosionCount = 5; //爆発の数
    [SerializeField] private float effectGenerateOffset = 2.0f;　//爆発生成オフセット
    [SerializeField] private float hitStopSeconds = 0.25f; //開始時のヒットストップ

    private Coroutine routine; //現在実行中のルーチンを保存

    //呼び出す。引数はプレイヤーの座標
    public void StartGameOverRoutine(Vector3 playerPos)
    {
        if (routine != null) StopCoroutine(routine);　//既に実行中のルーチンがあれば停止
        HitStop.Play(hitStopSeconds);　//ヒットストップ
        routine = StartCoroutine(GameOverRoutine(playerPos));
    }

    //コルーチン
    IEnumerator GameOverRoutine(Vector3 playerPos)
    {
        float elapsedTime = 0f;

        int next = 0;

        List<float> schedule = BuildEffectSchedule();

        while (elapsedTime < totalRoutineTime)
        {
            elapsedTime += Time.deltaTime;

            //爆発生成時間なら生成
            while (next < schedule.Count && elapsedTime >= schedule[next])
            {
                if (SpawnExplosion(playerPos))
                {
                    next++;
                }
            }
            yield return null;
        }

        routine = null; 
    }

    //爆発生成
    private bool SpawnExplosion(Vector3 playerPos)
    {

        EffectGenerator.Instance.CreateEffect(EffectType.Explosion, GetRandomOffset(playerPos));
        SoundPlayer.Instance.PlaySE(SoundType.ExplodeSE);

        return true;
    }

    //プレイヤー座標を基にランダムなオフセットを取得
    private Vector3 GetRandomOffset(Vector3 pos)
    {
        return new Vector3(pos.x + Random.Range(-effectGenerateOffset, effectGenerateOffset),
                           pos.y + Random.Range(-effectGenerateOffset, effectGenerateOffset),
                           pos.z + Random.Range(-effectGenerateOffset, effectGenerateOffset));
    }

    //爆発生成テーブル作成
    private List<float> BuildEffectSchedule()
    {
        var times = new List<float>(explosionCount);

        for (int i = 0; i < explosionCount; i++)
        {
            times.Add(i * totalRoutineTime / explosionCount); 
        }

        return times;
    }
}