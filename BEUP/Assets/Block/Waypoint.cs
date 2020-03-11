using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class Waypoint : MonoBehaviour
{
    Vector3Int gridPos = Vector3Int.zero;


    [SerializeField] List<Waypoint> attachedWaypoints;

    public List<Waypoint> AttachedWaypoints { get => attachedWaypoints; set => attachedWaypoints = value; }

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

                string NameToSearch = ((transform.position.x / gridSize) + x) + "," + ((transform.position.z / gridSize) + z);

                GameObject potentialWaypoint = GameObject.Find(NameToSearch);

                if (potentialWaypoint)
                {
                    Waypoint foundNeighbour = potentialWaypoint.GetComponent<Waypoint>();

                    if (foundNeighbour)
                    {
                        if (attachedWaypoints.Contains(foundNeighbour))
                        {
                        }
                        else
                        {
                            attachedWaypoints.Add(foundNeighbour);
                        }
                    }
                }
                else continue;



            }
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ExposeMethodInEditor]
    public void ClearAllAttachments()
    {
       
        attachedWaypoints.Clear();
        UnityEditor.EditorUtility.SetDirty(this);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Waypoint waypoint in attachedWaypoints)
        {
            Gizmos.DrawLine(transform.position, waypoint.transform.position);
        }
    }

    public void SetTopColor(Color color)
    {
        MeshRenderer meshRenderer = transform.Find("Up").GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
    }
}
