using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public enum BlockType
{
    NONE,
    ENEMY_SPAWN,
    ENEMY_DESTINATION,
    PLAYER_SPAWN
}

public enum SelectionType
{
    NONE,
    VALID,
    INVALID,
    PLAYERMOVE
}

public class Waypoint : MonoBehaviour
{
    [SerializeField] BlockType blockType = BlockType.NONE;
    [SerializeField] bool explored = false;
    [SerializeField] List<Waypoint> attachedWaypoints;
    [SerializeField] List<Vector3Int> blockedPaths;

    SelectionType selected = SelectionType.NONE;
    [SerializeField] GameObject occupiedBy = null;
    Vector3Int gridPos = Vector3Int.zero;
    GridManager grid;

    Waypoint foundBy = null;

    public List<Waypoint> AttachedWaypoints { get => attachedWaypoints; set => attachedWaypoints = value; }
    public List<Vector3Int> BlockedPaths { get => blockedPaths; set => blockedPaths = value; }
    public bool Explored { get => explored; set => explored = value; }
    public GameObject OccupiedBy { get => occupiedBy; set => occupiedBy = value; }
    public BlockType BlockType { get => blockType; }
    public GridManager Grid { set => grid = value; }
    public Waypoint FoundBy { get => foundBy; set => foundBy = value; }

    public void SetSelectionType(SelectionType selection)
    {
        selected = selection;
        SetTopColor();
    }

    public void SetBlockType(BlockType type)
    {
        blockType = type;
        SetTopColor();
    }

    public void ResetWaypoint()
    {
        explored = false;
        SetSelectionType(SelectionType.NONE);
    }

    private void Start()
    {
        if (attachedWaypoints == null)
        {
            attachedWaypoints = new List<Waypoint>();
        }
    }

    public Vector3Int GetGridPos()
    {
        float gridSize = GridManager.GridSize;

        return new Vector3Int(
            (int)(Mathf.RoundToInt(transform.position.x / gridSize)),
            0, //Mathf.RoundToInt(transform.position.y / gridSize) * gridSize
            (int)(Mathf.RoundToInt(transform.position.z / gridSize))
        );
    }

    [ExposeMethodInEditor]
    public void AttachToNeighbours()
    {
        float gridSize = GridManager.GridSize;

        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                if (x == 0 && z == 0) continue;


                else continue;
            }
        }
        //UnityEditor.EditorUtility.SetDirty(this);
    }

    [ExposeMethodInEditor]
    public void ClearAllAttachments()
    {
       
        attachedWaypoints.Clear();
       // UnityEditor.EditorUtility.SetDirty(this);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Waypoint waypoint in attachedWaypoints)
        {
            Gizmos.DrawLine(transform.position, waypoint.transform.position);
        }
    }

    public void SetTopColor()
    {
        if (grid == null) grid = GetComponentInParent<GridManager>();

        Material mat = null;

        if (selected != SelectionType.NONE)
        {
            switch (selected)
            {
                case SelectionType.VALID:
                    mat = grid.BlockColor_Selected;
                    break;
                case SelectionType.INVALID:
                    mat = grid.BlockColor_InvalidSelection;
                    break;
                case SelectionType.PLAYERMOVE:
                    mat = grid.BlockColor_PlayerValidMove;
                    break;
            }
        }
        else
        {
            switch (blockType)
            {
                case BlockType.NONE:
                    mat = grid.BlockColor_Default;
                    break;
                case BlockType.PLAYER_SPAWN:
                    mat = grid.BlockColor_PlayerStart;
                    break;
                case BlockType.ENEMY_SPAWN:
                    mat = grid.BlockColor_EnemySpawn;
                    break;
                case BlockType.ENEMY_DESTINATION:
                    mat = grid.BlockColor_EnemyDestination;
                    break;
            }
        }

        MeshRenderer meshRenderer = transform.Find("Up").GetComponent<MeshRenderer>();
        meshRenderer.material = mat;
    }
}
