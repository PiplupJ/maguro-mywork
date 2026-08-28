using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectDB", menuName = "Scriptable Objects/EffectDB")]
public class EffectDB : ScriptableObject
{
    public List<EffectData> effects;
}
