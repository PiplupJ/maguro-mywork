//アワードシーン用カメラの経路設定
using UnityEngine;

[CreateAssetMenu(fileName = "AwardCameraPath", menuName = "Scriptable Objects/AwardCameraPath")]
public class AwardCameraPath : ScriptableObject
{
    public Vector3 startPosition; //開始座標
    public Vector3 endPosition; //目標座標
    public float duration; //カット終了までの時間
}
