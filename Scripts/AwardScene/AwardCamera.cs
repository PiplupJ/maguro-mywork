//アワードシーン用カメラ
using UnityEngine;

public class AwardCamera : MonoBehaviour
{
    [SerializeField] private AwardCameraPath path; //カメラ経路

    //カメラ状態
    enum CameraState
    {
        None,
        Moving,
        End,
        Stop
    }
    CameraState cameraState = CameraState.None;

    private Transform target;
    private float elapsedTime;

    //カメラ実行
    public void RunAwardCamera(Transform target)
    {
        this.target = target;
        this.transform.position = path.startPosition;
        elapsedTime = 0;
        cameraState = CameraState.Moving;
    }

    void Update()
    {
        if(target==null||cameraState!=CameraState.Moving){
            return;
        }

        transform.position = GetNextCameraPosition();

        //カメラの回転
        LookAtTarget();

        elapsedTime += Time.deltaTime;

        //時間が経ったら終了
        if (elapsedTime >= path.duration)
        {
            elapsedTime = path.duration;                 
            transform.position = path.endPosition;
            LookAtTarget();
            cameraState = CameraState.End;
        }
    }

    //次の座標を求める
    private Vector3 GetNextCameraPosition()
    {
        Vector3 rtv = Vector3.zero;

        rtv.x = EasingLinear(elapsedTime, path.startPosition.x, path.endPosition.x, path.duration);
        rtv.y = EasingLinear(elapsedTime, path.startPosition.y, path.endPosition.y, path.duration);
        rtv.z = EasingLinear(elapsedTime, path.startPosition.z, path.endPosition.z, path.duration);

        return rtv;
    }

    //線型イージング
    private float EasingLinear(float elapsed, float start, float end, float duration)
    {   
        elapsed = Mathf.Clamp(elapsed, 0.0f, duration);
        return (end - start) * elapsed / duration + start;
    }

    //ターゲットへ向く
    private void LookAtTarget()
    {
        transform.LookAt(target);
        Vector3 dir = target.position - transform.position;
        if (dir.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
