//フィーバーモード制御用クラス
using UnityEngine;

public class FeverModeHandler : MonoBehaviour
{
    //全てのタイマーはインスペクター上で設定可能
    [SerializeField] float feverModeInterval = 30.0f;　//フィーバーモードに入るまでの時間
    [SerializeField] float feverModeDuration = 10.0f; //フィーバーモードの制限時間
    [SerializeField] private FeverModeRunner runner; //フィーバーモード実行
    [SerializeField] private FeverText feverText;
    private float feverCountdownTimer; //フィーバーモードに入るまで残り時間
    private float feverModeTimer;   //フィーバーモードが終わり、クライマックスモードになるまでの残り時間

    [SerializeField] private float climaxDuration = 10f; //クライマックスの制限時間
    private float climaxTimer;  //クライマックスが終わるまでの時間

    private int initialThrowCount; //初発動までに投げられたマグロの数
    [SerializeField] private float firstFeverThrowThreshold = 10; //初発動までに投げられたマグロの数の閾値

    //イベント状態
    enum EventState
    {
        Wait, //ゲーム開始前の状態
        InitialWait, //初めてのフィーバーモードになるまでの待機状態
        Countdown, //フィーバーモードになるのを待つ時間
        Fever,  //フィーバーモード
        Climax　//クライマックスモード。全てのアイテムが点滅し、残り時間が少ないのを見せる
    }
    EventState eventState;

    //初期化
    private void Awake()
    {
        //タイマー初期設定
        feverCountdownTimer = feverModeInterval;
        feverModeTimer = feverModeDuration;
        climaxTimer = climaxDuration;
        initialThrowCount = 0;
        eventState = EventState.Wait;
    }

    //GameManagerが実行
    public void EnableCountdown()
    {
        eventState = EventState.InitialWait;
    }

    //初発動までに投げられたマグロの数をカウントする
    public void OnMaguroThrown()
    {
        if (eventState != EventState.InitialWait)
            return;

        initialThrowCount++;

        if (initialThrowCount >= firstFeverThrowThreshold)
        {
            eventState = EventState.Fever; //状態更新
            runner.OnEnter(); //ランナーの初期化
            feverText.StartFeverEnter();
        }
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime; //リアルタイムを基準に経過

        switch (eventState)
        {
            
            //カウントダウン中
            case EventState.Countdown:
                //タイマーをカウントダウン
                if (feverCountdownTimer > 0)
                {
                    feverCountdownTimer -= dt;
                }
                //フィーバーモードに入る
                if (feverCountdownTimer <= 0)
                {
                    runner.OnEnter(); //ランナーの初期化
                    feverCountdownTimer = feverModeInterval;　//タイマー初期化
                    feverText.StartFeverEnter();
                    eventState = EventState.Fever; //状態更新
                }
                break;
            //フィーバーモード
            case EventState.Fever:
                if(feverModeTimer > 0)
                {
                    feverModeTimer -= dt;
                }
                //クライマックスモードに入る
                if(feverModeTimer <= 0)
                {
                    //タイマー初期化
                    feverModeTimer = feverModeDuration;
                    climaxTimer = climaxDuration;
                    runner.StartClimax();　//ランナーのクライマックスモード設定
                    eventState = EventState.Climax;　//状態更新
                }
                runner.Tick(feverModeTimer, feverModeDuration); //ランナーの更新(フィーバーモードの実行)
                break;
            case EventState.Climax:
                if(climaxTimer > 0)
                {
                    climaxTimer -= dt;
                }
                if(climaxTimer <= 0)
                {
                    runner.OnExit(); //ランナーの終了
                    eventState = EventState.Countdown; //状態更新
                }
                runner.Tick(feverModeTimer, feverModeInterval); //ランナーの更新(フィーバーモードの実行)
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        
    }
}
