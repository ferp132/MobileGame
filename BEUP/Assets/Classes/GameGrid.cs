using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField][Min(0f)]
    private int Cols = 5;
    [SerializeField] [Min(0f)]
    private int Rows = 8;
    [SerializeField][Range(0f, 100f)]
    private float TileSize = 1f;

    private List<GridPoint> Points = new List<GridPoint>();
    

    private void Awake()
    {
        GridPoint newPoint;

        for (float x = 0; x < Cols; x += 1)
        {
            for (float z = 0; z < Rows; z += 1)
            {
                Vector3 point = new Vector3(x, 0f, z);
                newPoint = new GridPoint();
                Points.Add(newPoint);

                newPoint.InitGridPosition(point, TileSize);
            }
        }

        foreach (GridPoint point in Points)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    if (point.GridPosition.x + x < 0 || point.GridPosition.x + x > Cols) continue;
                    if (point.GridPosition.y + z < 0 || point.GridPosition.y + z > Rows) continue;

                    Vector3 newConnection = new Vector3(x, 0f, z);

                    Debug.Log(newConnection.ToString());
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
    }
}
