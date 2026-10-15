//フィーバーアイテムの生成座標を管理
using UnityEngine;

public class FeverSpawnLocator
{
    //各エリアの中心座標（実際のマップに合わせて変更）
    private readonly Vector3[] areaCenters =
    {
        new Vector3(-10f, 0f, -10f),
        new Vector3( 10f, 0f, -10f),
        new Vector3(-10f, 0f,  10f),
        new Vector3( 10f, 0f,  10f),
    };

    private readonly float offsetRange; //中心からの移動範囲
    private int currentIndex; //次に使用するエリア

    public FeverSpawnLocator(float offsetRange = 7f)
    {
        this.offsetRange = Mathf.Max(0f, offsetRange);
        ShuffleAreas();
    }

    //次の生成座標を返す
    public bool TryGetPosition(out Vector3 pos)
    {
        pos = default;

        if (areaCenters.Length == 0)
            return false;

        //全エリアを使用したら再シャッフル
        if (currentIndex >= areaCenters.Length)
            ShuffleAreas();

        Vector3 center = areaCenters[currentIndex];
        currentIndex++;

        //中心からX・Z方向にランダムなオフセット
        Vector3 offset = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            0f,
            Random.Range(-offsetRange, offsetRange)
        );

        //中心座標にオフセットを加えて返す
        pos = center + offset;
        return true;
    }

    //エリアの使用順をシャッフル
    private void ShuffleAreas()
    {
        for (int i = areaCenters.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            Vector3 temp = areaCenters[i];
            areaCenters[i] = areaCenters[j];
            areaCenters[j] = temp;
        }

        currentIndex = 0;
    }
}