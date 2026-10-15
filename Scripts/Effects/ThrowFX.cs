using UnityEngine;

public class ThrowFX : MonoBehaviour
{
    //投げたプレイヤーによって色を変えるためのID
    public const int Player1 = 0;   // オレンジ
    public const int Player2 = 1;   // 白

    [SerializeField] private TrailRenderer[] trailRenderers;

    // [Player1] オレンジ マテリアル, [Player2] 白 マテリアル
    [SerializeField] private Material[] playerMaterials;

    // コンポーネント追加時、子の TrailRenderer を自動的に埋める
    private void Reset()
    {
        trailRenderers = GetComponentsInChildren<TrailRenderer>(true);
    }

    private void Awake()
    {
        foreach (var t in trailRenderers)
        {
            t.emitting = false;
            t.Clear();
        }
    }

    public void Begin(int playerId)
    {
        if (playerId < 0 || playerId >= playerMaterials.Length)
        {
            Debug.LogWarning($"[ThrowFX] 不正な playerId: {playerId}", this);
            return;
        }

        Material mat = playerMaterials[playerId];

        foreach (var t in trailRenderers)
        {
            t.sharedMaterial = mat;
            t.Clear();
            t.emitting = true;
        }
    }
    public void End()
    {
        foreach (var t in trailRenderers)
        {
            t.emitting = false;
        }
    }
}