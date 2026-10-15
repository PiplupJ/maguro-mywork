using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct FeverSpawnInfo
{
    public FeverItem item;　//アイテム
    public int weight; //登場確率。くじ引きボックスの中に何個入れるか
}

[CreateAssetMenu(fileName = "FeverItemSpawnTable", menuName = "Scriptable Objects/FeverItemSpawnTable")]
public class FeverItemSpawnTable : ScriptableObject
{
    public List<FeverSpawnInfo> data;　//データリスト
}
