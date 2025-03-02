using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;
using VInspector.Libs;

[Serializable]
public class Node
{
    public bool isWall;
    public Node parentNode;
    public float x, y,z, G, H;
    public float F
    {
        get {return G + H; }
    }

    public Node(bool _isWall,float _x, float _y,float _z)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
        z = _z;
    }
}
public class PathFind : Singleton<PathFind>
{
    public Vector3Int bottomLeft, topRight;
 
    [SerializeField] private bool allowDiagonal, dontCrossCorner;

    [SerializeField] private float NodeIntervalSize = 15f;

    private int sizeX, sizeZ;
    [Tooltip("여러층일때")]
    [SerializeField] private bool floor; //나중에 enumState 전환으로
    public Node[,] NodeArray;//타일맵의 판의 크기 (이차원 배열이라는 뜻) 

    private void Awake()
    {
    }

    private void Start()
    {
        NodeSetting();
    }

    [Button]
    private void NodeSetting()
    {
        #region 처음 세팅
        sizeX = Mathf.RoundToInt((topRight.x - bottomLeft.x) / NodeIntervalSize) + 1;
        sizeZ = Mathf.RoundToInt((topRight.z - bottomLeft.z) / NodeIntervalSize) + 1;
        NodeArray = new Node[sizeX, sizeZ];
        
        //벽 찾기
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                int curfloor = 1;
                Vector3 nodePosition =
                    new Vector3(i * NodeIntervalSize + bottomLeft.x, 10,j * NodeIntervalSize + bottomLeft.z);
                bool isWall = !Physics.Raycast(nodePosition, Vector3.down, Mathf.Infinity, LayerMask.GetMask("MoveTile"));
                 
                
                NodeArray[i,j] = new Node(isWall, nodePosition.x, curfloor,nodePosition.z);
            }
        }
        #endregion
    }

      public int IntervalInt(float n)
    {
        return Mathf.RoundToInt(n / NodeIntervalSize);
    }

    [Button]
    public List<Node> PathFinding(Vector3Int startPos, Vector3Int targetPos)
    {
        targetPos = new Vector3Int(
            Mathf.Clamp(targetPos.x, bottomLeft.x, topRight.x),
            0,
            Mathf.Clamp(targetPos.z, bottomLeft.z, topRight.z)
        );

        Node StartNode = NodeArray[IntervalInt(startPos.x - bottomLeft.x), IntervalInt(startPos.z - bottomLeft.z)];
        Node TargetNode = NodeArray[IntervalInt(targetPos.x - bottomLeft.x), IntervalInt(targetPos.z - bottomLeft.z)];

        List<Node> OpenList = new List<Node> { StartNode };
        List<Node> ClosedList = new List<Node>();
        List<Node> FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            Node CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
                    CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.parentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                return FinalNodeList;
            }

            AddNeighbors(CurNode, TargetNode, OpenList, ClosedList);
        }
        return FinalNodeList;
    }

    private void AddNeighbors(Node CurNode, Node TargetNode, List<Node> OpenList, List<Node> ClosedList)
    {
        Vector3Int[] directions = allowDiagonal
            ? new Vector3Int[]
            {
                new Vector3Int(1, 0, 1), new Vector3Int(-1, 0, 1),
                new Vector3Int(-1, 0, -1), new Vector3Int(1, 0, -1),
                new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 0),
                new Vector3Int(0, 0, -1), new Vector3Int(-1, 0, 0)
            }
            : new Vector3Int[]
            {
                new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 0),
                new Vector3Int(0, 0, -1), new Vector3Int(-1, 0, 0)
            };

        foreach (var dir in directions)
        {
            int checkX = IntervalInt(CurNode.x - bottomLeft.x) + dir.x;
            int checkZ = IntervalInt(CurNode.z - bottomLeft.z) + dir.z;


            if (checkX >= 0 && checkX < sizeX && checkZ >= 0 && checkZ < sizeZ)
            {
                if (allowDiagonal)
                {
                    int curXIdx = IntervalInt(CurNode.x - bottomLeft.x);
                    int curZIdx = IntervalInt(CurNode.z - bottomLeft.z);

                    if (NodeArray[curXIdx, checkZ].isWall && NodeArray[checkX, curZIdx].isWall)
                        continue;
                }

                if (dontCrossCorner)
                {
                    int curXIdx = IntervalInt(CurNode.x - bottomLeft.x);
                    int curZIdx = IntervalInt(CurNode.z - bottomLeft.z);

                    if (NodeArray[curXIdx, checkZ].isWall || NodeArray[checkX, curZIdx].isWall)
                        continue;
                }

                Node NeighborNode = NodeArray[checkX, checkZ];
                if (NeighborNode.isWall || ClosedList.Contains(NeighborNode)) continue;

                float MoveCost = CurNode.G + (dir.x == 0 || dir.z == 0 ? 10 : 14);

                if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.G = MoveCost;
                    NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.z - TargetNode.z)) * 10;
                    NeighborNode.parentNode = CurNode;
                    OpenList.Add(NeighborNode);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (var n in NodeArray)
            {
                Gizmos.color = Color.green;
                if (!n.isWall)
                    Gizmos.DrawSphere(new Vector3(n.x, 1, n.z), 1f);
            }
        }
    }

}

