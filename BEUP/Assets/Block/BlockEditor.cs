using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class BlockEditor : MonoBehaviour
{
    GridManager grid;
    Waypoint waypoint = null;

    [Header("Block Path")]
    [SerializeField]
    Vector3Int pathToBlock;

    [SerializeField]
    bool blockPathNeighbours = false;

    [SerializeField]
    bool blockSelf = true;

    private void Start()
    {
        if (!grid) grid = GetComponentInParent<GridManager>();
        waypoint = GetComponent<Waypoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            SnapToGrid();
            RenameLabel();
        }
    }

    private void SnapToGrid()
    {
        int gridSize = GridManager.GridSize;
        transform.position = waypoint.GetGridPos() * gridSize;
    }

    private void RenameLabel()
    {
        TextMesh label = GetComponentInChildren<TextMesh>();

        int gridSize = GridManager.GridSize;
        Vector3Int gridPos = waypoint.GetGridPos();

        string xCoord = (gridPos.x).ToString();
        string zCoord = (gridPos.z).ToString();
        string newName = xCoord + "," + zCoord;

        name = newName;
        label.text = newName;
    }


    [ExposeMethodInEditor]
    public void BlockThisPath()
    {
        BlockPath(pathToBlock, blockPathNeighbours, blockSelf);
        pathToBlock = Vector3Int.zero;
    }


    /*
    * Block a pathfinder from travelling from this block to the path specified
    * @param path: The path to be blocked
    * @param blockNeighbours: if the method should also block the neighbouring paths. 
    *   Blocking to the south would also block southeast and southwest paths.
    *   Blocking to the northeast would also block north and east.
    * @param alsoBlockSelf: if true, get any block that is positioned at the end of a blocked path and also block a return path to this block.
    * @note will only block paths that directly neighbour this block. a vectors will have their members clamped to a length of one.
    */
    public void BlockPath(Vector3Int path, bool blockHorizontalNeighbours, bool alsoBlockSelf)
    {
        if (path == Vector3Int.zero) return;

        //Clamp Path
        Vector3Int clampedPath = ClampVector3Int(path, -1, 1);

        // Fill an array with paths to block
        Vector3Int[] blockedPaths = new Vector3Int[blockHorizontalNeighbours ? 3 : 1];

        blockedPaths[0] = clampedPath;

        if (blockHorizontalNeighbours)
        {
            bool zMove = clampedPath.z != 0; 
            bool xMove = clampedPath.x != 0; 

            if (zMove && xMove) //neighbours are (-x, 0, 0) && (0, 0, -z);
            {
                blockedPaths[1] = clampedPath + new Vector3Int(-clampedPath.x, 0, 0);
                blockedPaths[2] = clampedPath + new Vector3Int(0, 0, -clampedPath.z);
            }
            else if (xMove)     //neighbours are x = 1, x = -1
            {
                blockedPaths[1] = clampedPath + new Vector3Int(1, 0, 0) ;
                blockedPaths[2] = clampedPath + new Vector3Int(-1, 0, 0);
            }
            else if (zMove)     // neighbours are z = 1, z = -1
            {
                blockedPaths[1] = clampedPath + new Vector3Int(0, 0, 1);
                blockedPaths[2] = clampedPath + new Vector3Int(0, 0, -1);
            }
        }

        //Block Self from paths in array
        if (alsoBlockSelf)
        {
            foreach (Vector3Int neighbour in blockedPaths)
            {
                Waypoint potentialWaypoint = grid.GetWaypoint(neighbour + waypoint.GetGridPos());

                if (potentialWaypoint)
                {
                    UnityEditor.EditorUtility.SetDirty(potentialWaypoint);
                    potentialWaypoint.BlockedPaths.Add(-neighbour);
                }
                else print("Could not find neigbour at" + (waypoint.GetGridPos() + neighbour));
            }
        }

        //add blocked paths to this waypoint
        UnityEditor.EditorUtility.SetDirty(waypoint);
        waypoint.BlockedPaths.AddRange(blockedPaths);
    }

    public Vector3Int ClampVector3Int(Vector3Int vector, int min, int max)
    {
        return new Vector3Int(
        Mathf.Clamp(vector.x, min, max),
        Mathf.Clamp(vector.y, min, max),
        Mathf.Clamp(vector.z, min, max));
    }
}
