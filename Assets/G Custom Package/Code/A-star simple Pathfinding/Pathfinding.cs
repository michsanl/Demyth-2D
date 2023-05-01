using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomCode.GridClass2D;

//Source : Codemonkey
namespace CustomCode.Pathfinding
{
    public class Pathfinding
    {
        private const int STRAIGHT_COST = 10;
        private const int DIAGONAL_COST = 14;

        private GridClass<PathNode> grid;

        private List<PathNode> openList;
        private List<PathNode> closeList;

        public Pathfinding(int width, int height, float size, Vector3 position)
        {
            grid = new GridClass<PathNode>(width, height, size, position, 
                    (GridClass<PathNode> g, int x, int y) => new PathNode(g, x, y)
                );
        }

        public GridClass<PathNode> Grid()
        {
            return grid;
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetGridObject(startX, startY);
            PathNode endNode = grid.GetGridObject(endX, endY);
            
            if (endNode == null) return null;

            openList = new List<PathNode> { startNode };
            closeList = new List<PathNode>();

            for (int i = 0; i < grid.GetWidth(); i++)
            {
                for (int j = 0; j < grid.GetHeight(); j++)
                {
                    PathNode node = grid.GetGridObject(i, j);
                    node.gCost = int.MaxValue;
                    node.CalculateFCost();
                    node.parentNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while(openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                openList.Remove(currentNode);
                closeList.Add(currentNode);

                if (currentNode == endNode)
                    return CalculatePath(endNode);

                foreach (PathNode neighbour in GetNeighbourNode(currentNode))
                {
                    if (closeList.Contains(neighbour)) continue;
                    if (!neighbour.isWalkable) continue;

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                    if(tentativeGCost < neighbour.gCost)
                    {
                        neighbour.parentNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if (!openList.Contains(neighbour))
                            openList.Add(neighbour);
                    }
                }
            }

            return null;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);

            PathNode currentNode = endNode;
            while(currentNode.parentNode != null)
            {
                path.Add(currentNode.parentNode);
                currentNode = currentNode.parentNode;
            }

            path.Reverse();
            return path;
        }

        private List<PathNode> GetNeighbourNode(PathNode currentNode)
        {
            List<PathNode> neighbours = new List<PathNode>();

            //CEK SEBELAH KIRI
            if(currentNode.x - 1 >= 0)
            {
                //Ambil node disebelah kiri
                neighbours.Add(GetNode(currentNode.x - 1, currentNode.y));
                //ambil node disebelah kiri bawah
                if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                //ambil node disebelah kiri atas
                if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
            //CEK SEBELAH KANAN
            if(currentNode.x + 1 < grid.GetWidth())
            {
                //Ambil node disebelah kanan
                neighbours.Add(GetNode(currentNode.x + 1, currentNode.y));
                //ambil node disebelah kanan bawah
                if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                //ambil node disebelah kanan atas
                if (currentNode.y + 1 < grid.GetHeight()) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
            //TOP
            if (currentNode.y + 1 < grid.GetHeight())
                neighbours.Add(GetNode(currentNode.x, currentNode.y + 1));
            //BOTTOM
            if (currentNode.y - 1 >= 0)
                neighbours.Add(GetNode(currentNode.x, currentNode.y - 1));

            return neighbours;
        }

        private PathNode GetNode(int v, int y)
        {
            return grid.GetGridObject(v, y);
        }

        private PathNode GetLowestFCostNode(List<PathNode> openList)
        {
            PathNode lowest = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < lowest.fCost)
                    lowest = openList[i];
            }

            return lowest;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
        }
    }

    public class PathNode
    {
        private GridClass<PathNode> grid;
        public int x;
        public int y;
        public bool isWalkable;

        public int gCost;
        public int hCost;
        public int fCost;

        public PathNode parentNode;

        public PathNode(GridClass<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            isWalkable = true;
        }

        public void SetWalkable()
        {
            isWalkable = !isWalkable;
            grid.TriggerChangeEvent(x, y);
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public override string ToString()
        {
            return x + " " + y + '\n' + isWalkable;
        }
    }
}

