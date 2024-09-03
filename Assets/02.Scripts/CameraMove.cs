using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public GameObject cameraCenter;

    public float rotationSpeed,zoomSpeed,maxOthograpicSize,minOthograpicSize;

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
        Player = GameObject.Find("BS_Transform").transform;
    }

    private void Start()
    {
        //transform.LookAt(cameraCenter.transform.position);
    }

    void Update()
    {

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
