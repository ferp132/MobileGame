using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class GridManager : MonoBehaviour
{
    Dictionary<Vector3Int, Waypoint> grid = new Dictionary<Vector3Int, Waypoint>();
    List<Waypoint> playerSpawn = new List<Waypoint>();
    List<Waypoint> enemySpawn = new List<Waypoint>();
    List<Waypoint> enemyDestination = new List<Waypoint>();

    [SerializeField] [Range(1, 20)] const int gridSize = 10;

    [Header("Block Colors")]
    [SerializeField] public Material BlockColor_Default;
    [SerializeField] public Material BlockColor_Selected;
    [SerializeField] public Material BlockColor_InvalidSelection;
    [SerializeField] public Material BlockColor_PlayerStart;
    [SerializeField] public Material BlockColor_EnemySpawn;
    [SerializeField] public Material BlockColor_EnemyDestination;
    [SerializeField] public Material BlockColor_PlayerValidMove;

    public static int GridSize => gridSize;

    public List<Waypoint> PlayerSpawn { get => playerSpawn; set => playerSpawn = value; }
    public List<Waypoint> EnemySpawn { get => enemySpawn; set => enemySpawn = value; }
    public List<Waypoint> EnemyDestination { get => enemyDestination; set => enemyDestination = value; }

    public void Init()
    {
        LoadBlocks();

        foreach (Waypoint waypoint in grid.Values)
        {
            waypoint.GetComponentInChildren<TextMesh>().gameObject.SetActive(false);
        }
    }

    public void ResetGrid()
    {
        foreach (Waypoint waypoint in grid.Values)
        {
            waypoint.Explored = false;
            waypoint.SetSelectionType(SelectionType.NONE);
            waypoint.FoundBy = null;
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
            switch (waypoint.BlockType)
            {
                case BlockType.PLAYER_SPAWN:
                    playerSpawn.Add(waypoint);
                    break;
                case BlockType.ENEMY_SPAWN:
                    enemySpawn.Add(waypoint);
                    break;
                case BlockType.ENEMY_DESTINATION:
                    enemyDestination.Add(waypoint);
                    break;
            }

            //UnityEditor.EditorUtility.SetDirty(waypoint);
            grid.Add(waypoint.GetGridPos(), waypoint);
            waypoint.Grid = this;
        }
    }
}
