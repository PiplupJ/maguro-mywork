
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool; //10/13追加

public class EffectGenerator : MonoBehaviour
{
    public static EffectGenerator Instance;

    [SerializeField]private EffectDB database;

    private Dictionary<EffectType, EffectData> dict;

    //オブジェクトプールを使う場合のための辞書
    private readonly Dictionary<EffectType, ObjectPool<PooledEffect>> pools = new();
    //再生中のエフェクトを管理するためのリスト
    private readonly List<(PooledEffect effect, EffectType type, float returnAt)> playing = new();

    void Awake()
    {
        if(Instance!=null && Instance !=this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dict = new Dictionary<EffectType, EffectData>();

        foreach (var d in database.effects) {
            if (dict.ContainsKey(d.type)) {
                Debug.LogWarning("EffectGenerator : 重複IDのエフェクトが存在します。" + d.type);
            }

            dict[d.type] = d;

            //オブジェクトプールを作成する
            pools[d.type] = new ObjectPool<PooledEffect>(
                //プールから取得する際の処理
                createFunc: () =>
                {
                    return CreatePooledEffect(d.prefab);
                },
                //プールに戻す際の処理->後で設定するのでここではnull
                actionOnGet: null,
                //プールから取得したオブジェクトを返却する際の処理
                actionOnRelease: (effect) =>
                {
                    effect.ResetAndHide();
                },
                //プールから取得したオブジェクトを破棄する際の処理
                actionOnDestroy: (effect) =>
                {
                    Destroy(effect.gameObject);
                },
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 40
            );
        }
    }


    public GameObject CreateEffect(int id, Vector3 pos)
    {
        return CreateEffect((EffectType)id, pos);
    }

    //EffectGenerator.Instance.CreateEffect(type, 座標); で呼ぶこと
    public GameObject CreateEffect(EffectType type, Vector3 pos)
    {

        if (!dict.TryGetValue(type, out var data) || !pools.TryGetValue(type, out var pool))
        {
            Debug.LogWarning("EffectGenerator:求めたエフェクトが存在しません" + type);
            return null;
        }

        //プールの内部関数。プールにある場合はそれを返す。なければ生成して返す
        var effect = pool.Get();  //プールから取得
        effect.Show(pos); //座標を設定

        playing.Add((effect, type, Time.time + Mathf.Max(0,data.duration))); //再生中リストに追加

        return effect.gameObject;
    }

    //プールから取得する際の処理。ないなら生成
    private PooledEffect CreatePooledEffect(GameObject prefab)
    {
        var go = Instantiate(prefab);
        go.SetActive(false);

        if (!go.TryGetComponent<PooledEffect>(out var effect))
        {
            Destroy(go);
            throw new System.InvalidOperationException(
                $"{prefab.name}: PooledEffectコンポーネントがありません."
            );
        }

        return effect;
    }

    //寿命管理のためにUpdateで再生中のエフェクトをチェックする
    private void Update()
    {
        //再生中のエフェクトをチェックして、終了したものをプールに戻す
        for (int i = playing.Count - 1; i >= 0; i--)
        {
            var (effect, type, returnAt) = playing[i];
            if (Time.time >= returnAt)
            {
                pools[type].Release(effect); //プールに戻す
                playing.RemoveAt(i); //リストから削除
            }
        }
    }
}
