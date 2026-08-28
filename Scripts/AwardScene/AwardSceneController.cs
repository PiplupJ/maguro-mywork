//アワードシーンコントローラー
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AwardSceneController : MonoBehaviour
{
    [SerializeField] private AwardSceneCharacter playerone;
    [SerializeField] private AwardSceneCharacter playertwo;
    private AwardSceneCharacter winner; //勝者
    [SerializeField] private ScreenTransition screenTransition;
    [SerializeField] private AwardCamera awardCamera;
    [SerializeField] private GameObject winnerText;
    [SerializeField] private GameObject pressBkeyBlink;
    
    [SerializeField] private SceneLoader sceneLoader;

    enum AwardState
    {
        Start,
        Running,
        Finished
    }
    AwardState awardState;

    void Start()
    {
        PlacePlayer();    
        RunAwardScene();
        screenTransition.FadeInScreen(2.0f);
    }

    private void OnDisable() 
    {
        if(winner!=null){
            winner.OnMotionEnd -= ShowWinnerText;
        }
    }

    //ゲーム結果に相当するキャラクターを配置
    private void PlacePlayer()
    {
        //対戦終了後static変数に保存されている勝者情報を読み込み、それに相当するキャラクターを配置。勝者として保存
        //勝者のモーション終了イベント購読

        if(GameResult.Winner == PlayerSlot.Player1){
            winner = Instantiate(playerone, Vector3.zero, Quaternion.identity);
        }
        else{
            winner = Instantiate(playertwo, Vector3.zero, Quaternion.identity);
        }
        //テスト用
        winner.OnMotionEnd += ShowWinnerText;
    }

    //シーン実行
    private void RunAwardScene()
    {
        //サウンド再生
        awardCamera.RunAwardCamera(winner.camTarget.transform);
        winner.PlayMotion();
        if (SoundPlayer.Instance != null)
        {
            SoundPlayer.Instance.PlaySE(SoundType.ApplauseSE);
            SoundPlayer.Instance.PlaySE(SoundType.AwardEntrySE);
        }
        awardState = AwardState.Running;
    }

    private void ShowWinnerText()
    {
        winnerText.SetActive(true);
        pressBkeyBlink.SetActive(true);
        awardState = AwardState.Finished;
        GameResult.Clear();
    }

    //ボタン入力イベント・タイトルシーンへ遷移
    public void OnAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if(awardState!=AwardState.Finished) return;
        if (SoundPlayer.Instance != null)
        {
            SoundPlayer.Instance.PlaySE(SoundType.SelectSE);
        }
        sceneLoader.LoadScene();
    }
    
}
