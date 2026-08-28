//アワードシーン用キャラクター
using UnityEngine;
using System;

public class AwardSceneCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator; //アニメーター

    public GameObject camTarget;

    protected const float transitionDuration = 0.1f; //モーション切り替えにかかる時間

    public event Action OnMotionEnd; //モーション終了イベント

    //モーションの状況
    enum PlayState
    {
        None,//なし
        OnPlay,//実行中
        End //終了
    }
    PlayState state;

    private readonly string[] Motions = { "YMCAceremony", "HipHopCeremony", "ChickenCeremony"};

    //Animatorからnameに相当するモーションを再生
    public void PlayMotion()
    {
        int hash = Animator.StringToHash(Motions[UnityEngine.Random.Range(0,Motions.Length)]);
        animator.CrossFadeInFixedTime(hash, transitionDuration);
        state = PlayState.OnPlay;
    }

    private void Update()
    {
        if(state!=PlayState.OnPlay){
            return;
        }

        var motionState = animator.GetCurrentAnimatorStateInfo(0);

        //現在実行中のモーションが終わったら終了処理
        if (!animator.IsInTransition(0) && motionState.normalizedTime >= 1f)
        {
            state = PlayState.End;
            OnMotionEnd?.Invoke();   
        }
    }
}
