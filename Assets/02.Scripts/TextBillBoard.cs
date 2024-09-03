using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBillBoard : MonoBehaviour
{
    Transform cam;

    public enum Type
    {
        button,onlyText
    }

    [SerializeField] Type textType;

    [SerializeField] TMP_Text text;
    [SerializeField] GameObject button;
    [SerializeField] float interactionDistance;
    Transform player;
    // Start is called before the first frame update

    private void Awake()
    {
       // text.enabled = false;
       //button.SetActive(false);
    }
    void Start()
    {
        cam = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            Interact(true);
        }

        float squaredDistance = (transform.position - player.position).sqrMagnitude;
        float squaredInteractionDistance = interactionDistance * interactionDistance;
        if (squaredDistance <= squaredInteractionDistance)
        {
            Interact(true);
        }
    }

    public void Interact(bool isInteract)
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
       // text.enabled = isInteract;
        /*
        if (isInteract)
        {
            if (textType == Type.button)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
        }
        else button.SetActive(false);*/
        
    }

    void OnDrawGizmos()
    {
        // 상호작용 반경을 시각적으로 표시합니다.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }


}
