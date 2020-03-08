using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

[ExecuteAlways]
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
        SnapToGrid();
        RenameLabel();
    }

    private void SnapToGrid()
    {
        int gridSize = waypoint.GridSize;


        transform.position = waypoint.GetGridPos();
    }
    private void RenameLabel()
    {
        TextMesh label = GetComponentInChildren<TextMesh>();

        int gridSize = waypoint.GridSize;
        Vector3Int gridPos = waypoint.GetGridPos();

        string xCoord = (gridPos.x / gridSize).ToString();
        string zCoord = (gridPos.z / gridSize).ToString();
        string newName = xCoord + "," + zCoord;

        name = newName;
        label.text = newName;
    }


}
