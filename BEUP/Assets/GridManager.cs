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

    public void Init()
    {
        LoadBlocks();
    }

    public void SetGridUnexplored()
    {
        foreach (Waypoint waypoint in grid.Values)
        {
            waypoint.Explored = false;
        }
    }

    public Waypoint GetWaypoint(Vector3Int gridPos)
    {
        if (grid.ContainsKey(gridPos)) return grid[gridPos];
        else return null;
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

            waypoint.BlockedPaths.Clear();
        }
    }

    [ExposeMethodInEditor]
    private void LoadBlocks()
    {
        if (null == grid)
        {
            grid = new Dictionary<Vector3Int, Waypoint>();
        }
        else grid.Clear();

        Waypoint[] foundChildren = GetComponentsInChildren<Waypoint>();

        foreach (Waypoint waypoint in foundChildren)
        {
            UnityEditor.EditorUtility.SetDirty(waypoint);
            grid.Add(waypoint.GetGridPos(), waypoint);
        }
    }
}
