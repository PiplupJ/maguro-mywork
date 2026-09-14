//マグロ生成
using UnityEngine;
using System.Collections.Generic;

//プレイヤーがフィールドとしてこれを持つこと
//GameSceneでプレイヤー初期配置の際、MaguroSpawnerをIMaguroProviderとしてプレイヤーに渡す
public interface IMaguroProvider
{
    bool TryPickMaguro(Vector3 pos, float targetDist, out Maguro maguro);
}
public class MaguroSpawner : MonoBehaviour, IMaguroProvider
{
    [SerializeField] private GameObject[] spawnPoints; //マップ上の生成ポイント
    [SerializeField] private float spawnOffset = 5.0f; //生成時オフセット

    [SerializeField] private int firstSpawnCount = 10; //最初生成量
    [SerializeField] private int maxSpawnLimit = 30; //最大生成量

    [SerializeField] private float spawnInterval = 3.0f; //生成周期
    private float spawnTimer;
    
    [SerializeField] private Maguro maguroPrefab; //マグロのプリハブ
    private List<Maguro> maguroList = new List<Maguro>(); //マグロのリスト

    enum SpawnState
    {
        Spawning, //生成中
        Waiting   //停止
    }
    SpawnState spawnState;

    void Start()
    {
        for(int i = 0; i < firstSpawnCount; i++)
        {
            SpawnMaguro();
        }
        spawnTimer = 0;
        spawnState = SpawnState.Spawning;
    }

    void Update()
    {
        switch(spawnState)
        {
            case SpawnState.Spawning :
                spawnTimer += Time.deltaTime;
                if(spawnTimer >= spawnInterval){
                    SpawnMaguro();
                    spawnTimer = 0;
                    if(maguroList.Count >= maxSpawnLimit)
                    {
                        spawnState = SpawnState.Waiting;
                    }
                }
                break;
            case SpawnState.Waiting :
                if(maguroList.Count<maxSpawnLimit)
                {
                    spawnTimer = 0;
                    spawnState = SpawnState.Spawning;
                }
                break;
            default :
                break;
        }
    }

    //生成座標を生成ポイントの中から選ぶ
    private Vector3 SelectSpawnPosition()
    {
        Vector3 rtv = Vector3.zero;
        rtv = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

        return rtv;
    }
    //生成座標にオフセット適用
    private Vector3 GetSpawnOffset()
    {
        Vector3 rtv = Vector3.zero;
        rtv.x += Random.Range(-spawnOffset, spawnOffset);
        rtv.z += Random.Range(-spawnOffset, spawnOffset);

        return rtv;
    }

    //マグロ生成
    private void SpawnMaguro()
    {
        Vector3 spawnPosition = SelectSpawnPosition();
        spawnPosition += GetSpawnOffset();

        Maguro maguro = Instantiate(maguroPrefab, spawnPosition, Quaternion.identity);
        maguroList.Add(maguro); //リストに追加
    }

    //マグロを拾う
    public bool TryPickMaguro(Vector3 pos, float targetDist, out Maguro maguro)
    {
        maguro = FindNearestMaguro(pos, targetDist);

        if(maguro==null){
            return false;
        }

        maguroList.Remove(maguro);

        return true;
    }

    //指定座標から最も近いマグロを探す
    private Maguro FindNearestMaguro(Vector3 pos, float targetDist)
    {
        Maguro nearestMaguro = null;

        for(int i = maguroList.Count - 1; i>=0; i-- ){
            var maguro = maguroList[i];

            float dist = Vector3.Distance(pos, maguro.transform.position);

            if(dist > targetDist){
                continue;
            }

            if(dist <= targetDist){
                nearestMaguro = maguro;
                targetDist = dist;
            }
        }
        return nearestMaguro;
    }

}
