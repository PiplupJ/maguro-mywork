using UnityEngine;
using System.Collections.Generic;
public class FeverItemSpawner
{
    private FeverItemSpawnTable spawnTable; //生成用テーブル

    private List<FeverItem> spawnEntry; //生成待機リスト

    private int currentIndex;

    public FeverItemSpawner()
    {
        spawnEntry = new List<FeverItem>();
        currentIndex = 0;
    }

    //テーブル設定、テーブル情報を基に生成待機リスト作成
    public void SetSpawnTable(FeverItemSpawnTable spawnTable)
    {
        this.spawnTable = spawnTable;
        spawnEntry.Clear();

        //生成待機リスト作成
        for(int i = 0; i < spawnTable.data.Count; i++)
        {
            for(int j = 0; j < spawnTable.data[i].weight; j++)
            {
                spawnEntry.Add(spawnTable.data[i].item);
            }
        }
        //シャッフル
        ShuffleEntry();
    }

    private void ShuffleEntry()
    {
        if(spawnEntry.Count <= 0) { return;  }

        //ランダム化
        for (int i = spawnEntry.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            var temp = spawnEntry[i];
            spawnEntry[i] = spawnEntry[j];
            spawnEntry[j] = temp;
        }
        currentIndex = 0; //インデックス初期化
    }

    //次の生成アイテムを返却
    public FeverItem GetNextSpawn()
    {
        FeverItem rtv = spawnEntry[currentIndex];
        
        currentIndex++;
        //次の生成アイテムが配列範囲外を指していたらまたシャッフルする
        if (currentIndex >= spawnEntry.Count)
        {
            ShuffleEntry();
        }

        return rtv;
    }

}
