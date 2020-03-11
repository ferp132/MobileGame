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

    private void Start()
    {
        if(!grid) grid = GetComponentInParent<GridManager>();
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


}
