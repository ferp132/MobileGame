using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class GridManager : MonoBehaviour
{
    Dictionary<Vector3Int, Waypoint> grid = new Dictionary<Vector3Int, Waypoint>();
    [SerializeField] [Range(1, 20)] const int gridSize = 10;

    [SerializeField] Waypoint playerStart = null;

    //[SerializeField] Waypoint start = null;
    //[SerializeField] Waypoint end = null;

    public static int GridSize => gridSize;

    public Waypoint PlayerStart { get => playerStart; set => playerStart = value; }

    private void Start()
    {
        LoadBlocks();
        ClearAllChildrenAttachments();
        AttachAllChildren();
        FindObjectOfType<PlayerInput>().Init();
    }

    [ExposeMethodInEditor]
    private void AttachAllChildren()
    {
        foreach (Waypoint waypoint in grid.Values)
        {
            waypoint.AttachToNeighbours();
        }
    }

    [ExposeMethodInEditor]
    private void ClearAllChildrenAttachments()
    {
        foreach (Waypoint waypoint in grid.Values)
        {
            waypoint.ClearAllAttachments();
        }
    }

    [ExposeMethodInEditor]
    private void ResetChildPosition()
    {
        foreach (Vector3Int pos in grid.Keys)
        {
            Waypoint waypoint = grid[pos];

            waypoint.transform.position = pos;
        }
    }

    [ExposeMethodInEditor]
    private void LoadBlocks()
    {
        grid.Clear();

        Waypoint[] foundChildren = GetComponentsInChildren<Waypoint>();

        foreach (Waypoint waypoint in foundChildren)
        {
            Vector3Int gridPos = waypoint.GetGridPos();
            if (grid.ContainsKey(gridPos))
            {
                Debug.Log("Skipping overlapping waypoint at: " + gridPos);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }
        }
    }
}
