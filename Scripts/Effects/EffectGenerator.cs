using UnityEngine;
using System.Collections.Generic;

public class EffectGenerator : MonoBehaviour
{
    public static EffectGenerator Instance;

    [SerializeField]private EffectDB database;

    private Dictionary<int, EffectData> dict;

    void Awake()
    {
        if(Instance!=null && Instance !=this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        dict = new Dictionary<int, EffectData>();

        foreach(var d in database.effects){
            if(dict.ContainsKey(d.id)){
                Debug.LogWarning("EffectGenerator : 重複IDのエフェクトが存在します。"+d.id);
            }

            dict[d.id] = d;
        }
    }

    //EffectGenerator.Instance.CreateEffect(id, 座標); で呼ぶこと
    public GameObject CreateEffect(int id, Vector3 pos)
    {
        GameObject go = null;

        if(dict.TryGetValue(id, out var d)){
            go = Instantiate(d.prefab);
            Destroy(go, d.duration);
            go.transform.position = pos;
        }else{
            Debug.LogWarning("EffectGenerator:求めたエフェクトが存在しません"+id);
        }

        return go;
    }
}
