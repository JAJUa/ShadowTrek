using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

[System.Serializable]
public class PointInTime
{

    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }


}

public class RePlay : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    public static RePlay Inst;
    
    Player player;
    private bool isReverse = false;
    public List<PointInTime> pointsInTime;
    [SerializeField]private List<PointInTime> savedPointsInTime;
    private List<PointInTime> savedPointsInLine;
    public List<PointInTime> pointsInLine;


    private void Awake()
    {
        Inst = this;
        player = GetComponent<Player>();
    }

    public void Init(List<PointInTime> _pointsInTime)
    {
        if (!isReverse)
        {
            _pointsInTime.Reverse();
            isReverse = true;
        }
           
      
        pointsInTime = new List<PointInTime>(_pointsInTime);
        pointsInTime.RemoveAt(0);
       
        pointsInLine = new List<PointInTime>(pointsInTime);
        savedPointsInLine = new List<PointInTime>(pointsInTime);
        savedPointsInTime = new List<PointInTime>(pointsInTime);
        
        transform.position = pointsInTime[0].position;
        transform.rotation = pointsInTime[0].rotation;
    }
    

    private void Update()
    {
        if(InGameManager.Inst == null)
            Debug.Log("인게임 매니저가 없음");
        if (InGameManager.Inst.CurState() == GameState.ShadowTurn ) {  LineRenderer(); }

    }

    public void ResetReplayLine()
    {
        lineRenderer.positionCount = 0;
    }



    public void ReMove(bool isPapaStay)
    {
        if (pointsInTime.Count <= 0) return;
        Debug.Log(pointsInTime[0].position);
      
    }


    

    // LineRenderer
    public void LineRenderer()
    {
        lineRenderer.positionCount = (pointsInLine.Count + 1);
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 2.7f, transform.position.z));
        for (int i = 0; i < pointsInLine.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, new Vector3(pointsInLine[i].position.x, 2.7f, pointsInLine[i].position.z));
        }
    }
}
