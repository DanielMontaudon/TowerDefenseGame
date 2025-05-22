using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private GameObject waypointPrefab;
    [SerializeField] private GameObject towerLocationPrefab;
    [SerializeField] private Canvas mapCanvas;


    [SerializeField] private int size = 20;
    private int[,] grid; // 2D array 
    public static List<GameObject> path;


    void Awake()
    {
        path = new List<GameObject>();
        grid = new int[size, size];
        createGrid();
    }

    void createGrid()
    {
        int x = 0, z = Random.Range(0,size), nextMove, prevMove = -1;

        FillGridWithPath(x,z);

        while(x < size-1)
        {
            //make sure we dont chose the previous plotted point
            // 0 up, 1 down, 2 right
            nextMove = Random.Range(0, 3);
            while((nextMove == 0 && prevMove == 1)||(nextMove == 1 && prevMove == 0))
                nextMove = Random.Range(0, 3);
            prevMove = nextMove;

            switch (nextMove)
            {
                case 0:
                    if (z < size - 1)
                        z++;
                    else
                    {
                        x++;
                        prevMove = 2;
                    }
                    break;
                case 1:
                    if (z > 0)
                        z--;
                    else
                    {
                        x++;
                        prevMove = 2;
                    }
                    break;
                case 2:
                    x++;
                    break;
            }
            FillGridWithPath(x, z);
        }
        CreateTowerLocations();
    }

    void CreateTowerLocations()
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                if (grid[x, z] == 0 && CanPlaceTower(x, z))
                {
                    grid[x, z] = 2;
                    Instantiate(towerLocationPrefab, new Vector3(x, FindTerrainHeight(x,z), z), Quaternion.Euler(90,0,0), mapCanvas.transform);
                }
            }
        }
    }

    private bool CanPlaceTower(int x, int z)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int a = i + x;
                int b = j + z;

                if (a >= 0 && b >= 0 && a < size && b < size && grid[a, b] == 1)
                    return true;
            }
        }
        return false;
    }

    void FillGridWithPath(int x, int z)
    {
        path.Add(Instantiate(waypointPrefab, new Vector3(x, FindTerrainHeight(x,z), z), Quaternion.identity, transform));
        path[path.Count - 1].name = (path.Count - 1).ToString();
        grid[x, z] = 1;
    }

    float FindTerrainHeight(int x, int z)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 0, z), Vector3.down, out hit))
            return hit.point.y + 1.0f;
        return 0;
    }
}
