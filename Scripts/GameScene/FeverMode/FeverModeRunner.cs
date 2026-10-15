//フィーバーモード実行用クラス
using Unity.VisualScripting;
using UnityEngine;

public class FeverModeRunner : MonoBehaviour
{
    [SerializeField] private FeverItemSpawnTable spawnTable; //テーブル(ScriptableObjectで作成)
    [SerializeField] private float spawnStart = 20; //開始瞬間の生成週期
    [SerializeField] private float spawnEnd = 5; //終了が近い時の生成週期
    private float spawnTimer; //生成タイマー
    private FeverItemSpawner spawner; //生成用
    private FeverItemRegistry registry; //アイテムリスト

    private FeverSpawnLocator locator; //座標返却
    bool onClimax; //クライマックスモードフラグ

    [SerializeField] private MaguroSpawner maguroSpawner; //マグロ生成用
    [SerializeField] private FeverLighting lighting;

    private void Awake()
    {
        spawner = new FeverItemSpawner(); //スポナー生成
        registry = new FeverItemRegistry(); //レジストリ生成
        locator = new FeverSpawnLocator();
        //タイマー初期化
        spawnTimer = 0;
        //間違えた入力を防ぐ
        if (spawnEnd <= 0) spawnEnd = 1;
        if(spawnStart < spawnEnd)
        {
            spawnStart = spawnEnd*2;
        }
    }

    //フィーバーモードに入る時、Handlerが実行する
    public void OnEnter()
    {
        spawner.SetSpawnTable(spawnTable); //生成データを設定
        SoundPlayer.Instance.PlayBGM(SoundType.FeverBGM);　//フィーバーBGMを設定
        onClimax = false; //クライマックスではない
        lighting.Dim();
    }
    
    //フィーバーモード中、Handlerが実行
    public void Tick(float remainingTime, float totalTime)
    {
        //生成する時間ではないなら、リターン
        if (spawnTimer > 0)
        {
            //ヒットストップ中も起動
            spawnTimer -= Time.unscaledDeltaTime; ;
            return;
        }

        //生成するアイテム
        FeverItem data = spawner.GetNextSpawn();

        if(!locator.TryGetPosition(out Vector3 spawnPos))
        {
            spawnTimer = 0.25f;
            return;
        }
        FeverItem placed = Instantiate(data, spawnPos , Quaternion.identity);//生成
        
        //クライマックスモード中の生成なら点滅させる
        if (onClimax)
        {
            placed.SetBlinking(true);
        }
        registry.Add(placed); //レジストリに登録

        //マグロ生成に追加
        Maguro maguro = placed.GetComponent<Maguro>();
        if (maguro != null)
        {
            maguroSpawner.AddFeverMaguro(maguro);
        }

        //次の生成時間設定
        spawnTimer = GetNextSpawnTime(remainingTime/totalTime);
        
    }

    //次の生成までの時間返却
    //ラウンド時間が経過した分、頻繫に生成
    private float GetNextSpawnTime(float remainingRatio)
    {
        if (onClimax) return spawnEnd;

        return spawnStart - ((1-remainingRatio) * (spawnStart - spawnEnd));
    }

    //クライマックスモード実行
    public void StartClimax()
    {
        onClimax = true;
        registry.StartBlink(); //現在シーン上のアイテムが点滅しはじめる
        SoundPlayer.Instance.SetSpeedBGM(2f); //BGM再生速度が2倍
    }

    //フィーバータイム終了
    public void OnExit()
    {
        //全てのアイテムを削除
        registry.ReleaseAll();
        maguroSpawner.RemoveAllFeverMaguro();
        //BGMを元に戻す
        SoundPlayer.Instance.SetSpeedBGM(1f);
        SoundPlayer.Instance.PlayBGM(SoundType.GameBGM);
        lighting.Brighten();
    }

    //シーン終了時に実行
    private void OnDestroy()
    {
        //全てのアイテムを削除
        registry.ReleaseAll();
        maguroSpawner.RemoveAllFeverMaguro();
        //BGMを元に戻す
        SoundPlayer.Instance.SetSpeedBGM(1f);
        lighting.BrightenImmidiate();
    }
}
