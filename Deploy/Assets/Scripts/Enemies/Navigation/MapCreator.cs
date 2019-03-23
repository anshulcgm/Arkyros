using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapClass
{
    public class MapCreator: MonoBehaviour
    {
        public int xMax;
        public int yMax;
        public int zMax;

        public float offsetX = 1;
        public float offsetY = 1;
        public float offsetZ = 1;

        public GridNode[,,] grid;

        public GameObject gridFloorPrefab;

        public Vector3 startNodePosition;
        public Vector3 endNodePosition;

        public int numAgents;

        private void Start()
        {
            grid = new GridNode[xMax, yMax, zMax];

            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    for (int z = 0; z < zMax; z++)
                    {
                        float posX = x * offsetX;
                        float posY = y * offsetY;
                        float posZ = z * offsetZ;
                        GameObject floor = Instantiate(gridFloorPrefab, new Vector3(posX, posY
                            , posZ), Quaternion.identity) as GameObject;
                        floor.transform.name = x.ToString() + " " + y.ToString() + " " + z.ToString();
                        floor.transform.parent = transform;

                        GridNode node = new GridNode();
                        node.x = x;
                        node.y = y;
                        node.z = z;
                        node.worldObject = floor;

                        RaycastHit[] hits = Physics.BoxCastAll(new Vector3(posX, posY, posZ), new Vector3(1, 0, 1), Vector3.forward);

                        for (int i = 0; i < hits.Length; i++)
                        {
                            node.isWalkable = false;
                        }

                        grid[x, y, z] = node;
                    }
                }
            }
        }
        public bool start;
        private void Update()
        {
            if (start)
            {
                start = false;

                grid[1, 0, 1].isWalkable = false;

                GridNode startNode = GetNodeFromVector3(startNodePosition);
                GridNode end = GetNodeFromVector3(endNodePosition);

                startNode.worldObject.SetActive(false);

                for (int i = 0; i < numAgents; i++)
                {
                    Pathfinding.PathfindMaster.GetInstance().RequestPathfind(startNode, end, ShowPath);
                }

            }
        }

        public void ShowPath(List<GridNode> path)
        {
            foreach (GridNode n in path)
            {
                n.worldObject.SetActive(false);
            }
        }

        public GridNode GetNode(int x, int y, int z)
        {
            GridNode retVal = null;

            if (x < xMax && x >= 0 && y >= 0 && y < yMax && z >= 0 && z < zMax)
            {
                retVal = grid[x, y, z];
            }

            return retVal;
        }
        public GridNode GetNodeFromVector3(Vector3 pos)
        {
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            int z = Mathf.RoundToInt(pos.z);

            GridNode retVal = GetNode(x, y, z);
            return retVal;
        }

        //Singleton
        public static MapCreator instance;
        public static MapCreator GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            instance = this;
        }
    }
}

