using UnityEngine;
using DG.Tweening;
using VInspector;

public class CameraMove : MonoBehaviour
{
    //집 안을 사용했던 씬의 카메라에서 사용한 코드 - 현재는 사용될 곳이 없을 것 같음
    
    Camera m_camera;


    [Space(10)] [Header("-- Player Follow --")]

    public bool PlayerFollow;
    public Transform Player;
    public Vector3 offset = new Vector3(-112, 112, -112);
    public float followDuration = 0.5f;  // 따라오는 시간

    Vector3 targetPosition;
    Tweener cameraTweener;


    private void Awake()
    {
        m_camera= GetComponent<Camera>();
       // Player = GameObject.Find("BS_Transform").transform;
    }




    private void FixedUpdate()
    {
        // Player Follow
        if (PlayerFollow)
        {
            targetPosition = Player.position + offset;

            // DOTween의 Tweener가 이미 있다면 중지
            if (cameraTweener != null && cameraTweener.IsActive())
            {
                cameraTweener.Kill();
            }

            cameraTweener = transform.DOMove(targetPosition, followDuration).SetEase(Ease.InOutSine);
        }
    }


    // Room Change Camera Setting
    public void CameraSetting(bool playerFollow, Vector3 cameraPosition, float cameraSize)
    {
        if (cameraTweener != null && cameraTweener.IsActive())
        {
            cameraTweener.Kill();
        }

        m_camera.orthographicSize = cameraSize;
        PlayerFollow = playerFollow;

        if(!playerFollow)
            transform.localPosition = cameraPosition;
        else
            transform.position = Player.position + offset;
    }

}
