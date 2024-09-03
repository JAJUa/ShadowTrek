using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileMoveScript;
using UnityEngine.Rendering;

public class PathFind : MonoBehaviour
{
    public List<Nodee> OpenList, ClosedList;
    public List<Nodee> FinalNodeList;
    public Nodee StartNode, TargetNode, CurNode;
    public Nodee[,] NodeArray;

    private Vector3Int bottomLeft, topRight;
    private int gridDistance;
    private bool allowDiagonal, dontCrossCorner;
    private LayerMask groundLayer;
    

    public PathFind(Vector3Int bottomLeft, Vector3Int topRight, int gridDistance, bool allowDiagonal, bool dontCrossCorner, LayerMask groundLayer)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        this.gridDistance = gridDistance;
        this.allowDiagonal = allowDiagonal;
        this.dontCrossCorner = dontCrossCorner;
        this.groundLayer = groundLayer;
    }

    public List<Nodee> FindPath(Vector3Int startPos, Vector3Int targetPos)
    {
        int sizeX = topRight.x - bottomLeft.x + 15;
        int sizeZ = topRight.z - bottomLeft.z + 15;

        if ((startPos.x - targetPos.x) % gridDistance != 0 || (startPos.z - targetPos.z) % gridDistance != 0)
            Debug.Log("gridDistance Doesn't fit the size");
        sizeX /= 15;
        sizeZ /= 15;
        NodeArray = new Nodee[sizeX, sizeZ];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                bool isWall = false;
                Collider[] mTile = Physics.OverlapSphere(new Vector3(((i * 15) + bottomLeft.x), 3, ((j * 15) + bottomLeft.z)), 0.4f, LayerMask.GetMask("MoveTile"));
                if (mTile.Length == 0) isWall = true;

               // if (!Physics.Raycast(new Vector3(((i * 15) + bottomLeft.x), 3, ((j*15) + bottomLeft.z)), Vector3.down, 6f, groundLayer)) isWall = true;
 
                NodeArray[i, j] = new Nodee(isWall, (i*15) + bottomLeft.x, (j*15) + bottomLeft.z);
                    
            }
        }
        //print(NodeArray.Length);
        StartNode = NodeArray[(startPos.x - bottomLeft.x)/15, (startPos.z - bottomLeft.z)/15];
        TargetNode = NodeArray[(targetPos.x - bottomLeft.x)/15, (targetPos.z - bottomLeft.z)/15];

        OpenList = new List<Nodee>() { StartNode };
        ClosedList = new List<Nodee>();
        FinalNodeList = new List<Nodee>();

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            if (CurNode == TargetNode)
            {
                Nodee TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }

                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                return FinalNodeList;
            }

            // Diagonal
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + gridDistance, CurNode.z + gridDistance);
                OpenListAdd(CurNode.x - gridDistance, CurNode.z + gridDistance);
                OpenListAdd(CurNode.x - gridDistance, CurNode.z - gridDistance);
                OpenListAdd(CurNode.x + gridDistance, CurNode.z - gridDistance);
            }

            // Up, down, left, right
            OpenListAdd(CurNode.x, CurNode.z + gridDistance);
            OpenListAdd(CurNode.x + gridDistance, CurNode.z);
            OpenListAdd(CurNode.x, CurNode.z - gridDistance);
            OpenListAdd(CurNode.x - gridDistance, CurNode.z);
        }

        return null;
    }

    private void OpenListAdd(int checkX, int checkZ)
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x + 15 && checkZ >= bottomLeft.z && checkZ < topRight.z + 15 && !ClosedList.Contains(NodeArray[(checkX - bottomLeft.x)/15, (checkZ - bottomLeft.z)/15]))
        {
            
            if(Inst.checkWall)
            {
                if (allowDiagonal)
                {
                    if (NodeArray[(CurNode.x - bottomLeft.x)/15, (checkZ - bottomLeft.z)/15].isWall && NodeArray[(checkX - bottomLeft.x)/15, (CurNode.z - bottomLeft.z)/15].isWall)
                        return;
                }

                if (dontCrossCorner)
                {
                    if (NodeArray[(CurNode.x - bottomLeft.x) / 15, (checkZ - bottomLeft.z) / 15].isWall || NodeArray[(checkX - bottomLeft.x) / 15, (CurNode.z - bottomLeft.z) / 15].isWall)
                        return;
                }
            }
         
            
            Nodee NeighborNode = NodeArray[(checkX - bottomLeft.x) / 15, (checkZ - bottomLeft.z) / 15];
            
            if (NeighborNode.isWall && Inst.checkWall)
                return;

            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.z - checkZ == 0 ? 10 : 14);

            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                if (!OpenList.Contains(NeighborNode))
                {
                    NeighborNode.G = MoveCost;
                    NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.z - TargetNode.z)) * 10;
                    NeighborNode.ParentNode = CurNode;
                    OpenList.Add(NeighborNode);
                }
            }
        }
    }


  
   

}
