using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class PointInTime
{
    public static PointInTime Inst;

    public Vector3 position;
    public Quaternion rotation;
    public List<PointInTime> pointsInTime;

    public PointInTime (Vector3 _position, Quaternion _rotation)
    {
        position= _position;
        rotation= _rotation;
    }

    
}


public class ReWind : MonoBehaviour
{
    public static ReWind Inst;
    public bool isRewinding = false;
    public Animator animator;
    public List<PointInTime> pointsInTime;

    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        pointsInTime = new List<PointInTime>();
       
    }

    // Update is called once per frame
    void Update()
    {
 

        if (Input.GetKeyUp(KeyCode.U))
        {
           
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach(PointInTime point in pointsInTime) 
            {
                Debug.Log(point.position);
            }

            Debug.Log(pointsInTime[0].position);
           
        }
        /*
        if(Input.GetKeyUp(KeyCode.U))
        {
            StopRewind();
        }*/
            
    }

    private void FixedUpdate()
    {
      
    }

    public void StopRewind(GameObject character, Animator animCharacter, List<PointInTime> pointsInTime)
    {
        PointInTime pointInTime = pointsInTime[1];
        if (transform.position != pointInTime.position)
        {
            animCharacter.SetBool("RewindWalk", false);
            StartCoroutine(MoveToFrontTile(character, animCharacter,  pointsInTime));
        }
    }

    public void Rewinding(GameObject character,Animator animCharacter,List<PointInTime> pointsInTime)
    {
        if(pointsInTime.Count > 0)
        {
            Time.timeScale = 1;
            MoveToPosition(character,animCharacter,pointsInTime);
        }
 
        
    }
    private void MoveToPosition(GameObject character, Animator animCharacter, List<PointInTime> pointsInTime)
    {
        PointInTime pointInTime = pointsInTime[1];
        animCharacter.SetBool("RewindWalk", true);
        // Position
        Vector3 startPosition = character.transform.position;
        float distance = Vector3.Distance(startPosition, pointInTime.position);
       // float timeToMove = distance / 20;


        character.transform.rotation = pointInTime.rotation;

        character.transform.position = Vector3.MoveTowards(startPosition, pointInTime.position,20*Time.deltaTime);
        if(character.transform.position == pointInTime.position)
        {
            animCharacter.SetBool("RewindWalk", false);
            pointsInTime.RemoveAt(0);
        }
    
    }
    private IEnumerator MoveToFrontTile(GameObject character, Animator animCharacter, List<PointInTime> pointsInTime)
    {
        PointInTime pointInTime = pointsInTime[0];
        Vector3 targetPosition = pointInTime.position;
        animCharacter.SetBool("isWalk", true);
        // Position
        Vector3 startPosition = character.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float timeToMove = distance / 20;


        character.transform.rotation = pointInTime.rotation;

        float elapsedTime = 0;

        // Walking
        while (elapsedTime < timeToMove)
        {
            character.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        character.transform.position = targetPosition;
        animCharacter.SetBool("isWalk", false);
    }


    public void Record()
    {
    
        pointsInTime.Insert(0, new PointInTime(transform.position,transform.rotation));
    }

    public void StartRewind()
    {
        isRewinding= true;
       
    }

    public void StopRewind()
    {
        isRewinding= false;
        animator.SetBool("RewindWalk", false);
    }
}
