using System;
using System.Threading;
using UnityEngine;

//HitStop.Play(0.15f);　で呼び出すと0.15秒間スローモーションになる
public static class HitStop 
{
    static CancellationTokenSource cts;

    public static async void Play(float duration, float scale = 0f)
    {
        //すでに進行中の場合はキャンセルする
        cts?.Cancel();
        cts?.Dispose();
        cts = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);
        var token = cts.Token;

        Time.timeScale = scale;
        try
        {
            float end = Time.realtimeSinceStartup + duration;
            while (Time.realtimeSinceStartup < end)
                await Awaitable.NextFrameAsync(token);

            Time.timeScale = 1f;
        }
        catch (OperationCanceledException) { }
    }
}
