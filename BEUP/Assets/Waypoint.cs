using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class Waypoint : MonoBehaviour
{
    Vector3Int gridPos = Vector3Int.zero;
    const int gridSize = 10;

    [SerializeField] Dictionary<Vector3Int, Waypoint> attachedWaypoints = new Dictionary<Vector3Int, Waypoint>();

    public int GridSize { get => gridSize; }

    public Vector3Int GetGridPos()
    {
        return new Vector3Int(
            Mathf.RoundToInt(transform.position.x / gridSize) * gridSize,
            0, //Mathf.RoundToInt(transform.position.y / gridSize) * gridSize
            Mathf.RoundToInt(transform.position.z / gridSize) * gridSize
        );
    }

    [ExposeMethodInEditor]
    public void AttachToNeighbours()
    {
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
                        Vector3Int gridPos = foundNeighbour.GetGridPos();
                        if (attachedWaypoints.ContainsKey(gridPos))
                        {
                        }
                        else
                        {
                            attachedWaypoints.Add(gridPos, foundNeighbour);
                        }
                    }
                }
                else continue;



            }
        }
    }

    [ExposeMethodInEditor]
    public void ClearAllAttachments()
    {
        attachedWaypoints.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Waypoint waypoint in attachedWaypoints.Values)
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
