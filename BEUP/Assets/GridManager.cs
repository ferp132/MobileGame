using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class GridManager : MonoBehaviour
{
    [SerializeField] List<Waypoint> grid;
    [SerializeField] [Range(1, 20)] const int gridSize = 10;

    [SerializeField] Waypoint playerStart = null;

    //[SerializeField] Waypoint start = null;
    //[SerializeField] Waypoint end = null;

    public static int GridSize => gridSize;

    public Waypoint PlayerStart { get => playerStart; set => playerStart = value; }

    private void Start()
    {
        if (grid == null)
        {
            grid = new List<Waypoint>();
        }

        print(grid.Count);
        FindObjectOfType<PlayerInput>().Init();
    }

    [ExposeMethodInEditor]
    private void AttachAllChildren()
    {
        foreach (Waypoint waypoint in grid)
        {
            waypoint.AttachToNeighbours();
        }
    }

    [ExposeMethodInEditor]
    private void ClearAllChildrenAttachments()
    {
        foreach (Waypoint waypoint in grid)
        {
            waypoint.ClearAllAttachments();
        }
    }

    [ExposeMethodInEditor]
    private void LoadBlocks()
    {
        grid.Clear();

        Waypoint[] foundChildren = GetComponentsInChildren<Waypoint>();

        foreach (Waypoint waypoint in foundChildren)
        {
            UnityEditor.EditorUtility.SetDirty(waypoint);
            grid.Add(waypoint);
        }
    }
}
