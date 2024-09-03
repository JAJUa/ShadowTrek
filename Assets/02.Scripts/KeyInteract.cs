using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteract : MonoBehaviour
{
    [SerializeField] GameObject colliderTrans;
    [SerializeField] Vector3 colliderSize;
    [SerializeField] LayerMask playerMask;
    [SerializeField] GameObject keyObj, textBillBoard;
    [SerializeField] int keyNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CheckPlayer()
    {
        Collider[] hit = Physics.OverlapBox(colliderTrans.transform.position, colliderSize, Quaternion.identity, playerMask);
        if (hit.Length > 0)
        {
            Debug.Log("ss");
            if (hit[0].CompareTag("PlayerControl"))
            {
                textBillBoard.GetComponent<TextBillBoard>().Interact(true);
            }
        }
        else
        {
            textBillBoard.GetComponent<TextBillBoard>().Interact(false);
        }
    }

    public void GetKey()
    {
        keyObj.SetActive(false);
        InGameManager.Inst.isKey[keyNum] = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(colliderTrans.transform.position, colliderSize * 2);
    }
}
