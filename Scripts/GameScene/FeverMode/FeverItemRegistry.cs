//シーン上のフィーバーアイテム管理用クラス
using UnityEngine;
using System.Collections.Generic;

public class FeverItemRegistry
{
    List<FeverItem> items = new(); //現在シーン上のアイテムリスト

    //リストにアイテム追加(生成した際に必ず呼ぶこと
    public void Add(FeverItem toAdd)
    {
        items.Add(toAdd); //リストに追加
        toAdd.Released += Remove; //アイテム消費イベント講読
    }

    //リストからアイテムを削除(デリゲートによって実行)
    private void Remove(FeverItem toRemove) { 
        toRemove.Released -= Remove; //講読解除
        items.Remove(toRemove); //リストから削除
        //Object.Destroy(toRemove.gameObject); //破棄する
    }

    //フィーバータイム終了。全てのアイテム削除
    public void ReleaseAll()
    {
        for(int i = items.Count -1; i >= 0; i--)
        {
            FeverItem toRelease = items[i];
            Remove(items[i]);
        }
    }

    //フィーバーアイテムが点滅しはじめる(残り時間がない）
    public void StartBlink()
    {
        foreach(var item in items)
        {
            item.SetBlinking(true);
        }
    }
}
