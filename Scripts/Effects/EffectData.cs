using UnityEngine;
public enum EffectType
{
    None = 0,
    Eat = 1,
    Hit = 2,
    Explosion = 3,
    Throw = 4,
    Warning = 5,
    Heart = 6,
    HeartBreak = 7,
};

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    public EffectType type;
    public GameObject prefab;
    public float duration = 3f;
}
