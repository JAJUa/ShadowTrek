using UnityEngine;
using DG.Tweening;
using VInspector;

public class CameraMove : MonoBehaviour
{
    public GameObject cameraCenter;

    public float rotationSpeed,zoomSpeed,maxOthograpicSize,minOthograpicSize;

    Camera m_camera;

    public float shakeDuration = 0.5f; // 흔들리는 시간
    public float shakeMagnitude = 0.1f; // 흔들리는 강도
    public float dampingSpeed = 1.0f; // 흔들림이 사라지는 속도
    Vector3 initialPosition;
    float shakeTime = 0f;

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
        initialPosition = transform.localPosition;
        //transform.LookAt(cameraCenter.transform.position);
    }

    void Update()
    {
        if (shakeTime > 0)
        {
            // 카메라 위치에 랜덤한 진동을 더해줌
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            // 흔들림 시간이 지나면 줄어듬
            shakeTime -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeTime = 0f;
            transform.localPosition = initialPosition;
        }
    }

    [Button]
    public void TriggerShake(float duration)
    {
        shakeTime = duration;
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
