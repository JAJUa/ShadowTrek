using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class KeyTile : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] int doorNum;

    public enum Type
    {
        Default,PlayerAnimation
    }

    public Type playerAnimationType;
    [SerializeField] string animationName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerControl") && InGameManager.Inst.isKey[doorNum])
        {
            Debug.Log("KeyDoor");
            door.GetComponent<Animator>().SetBool("doorOpen",true);
            
            if(playerAnimationType == Type.PlayerAnimation)
                StartCoroutine(WaitPlayerAnimation(1,other.gameObject));
            else
                gameObject.SetActive(false);
        }
    }

    IEnumerator WaitPlayerAnimation(float time,GameObject player)
    {
        yield return new WaitForSeconds(time);
        player.transform.LookAt(new Vector3(door.transform.position.x,player.transform.position.y,door.transform.position.z));
        player.transform.GetChild(0).GetComponent<Animator>().SetBool("isWalk",true);
        player.transform.DOMoveX(player.transform.position.x + 20,3f);
        yield return new WaitForSeconds(2f);
        FadeInFadeOut.Inst.NextScene();
    }

}
