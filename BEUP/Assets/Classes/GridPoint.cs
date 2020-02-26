using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    public Vector3 GridPosition = Vector3.zero;
    float GridSizeMultiplier = 0f;
    List<GridPoint> ConnectedTiles = new List<GridPoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGridPosition(Vector3 position, float multiplier)
    {
        GridPosition = position;
        SetMultiplier(multiplier);
    }

    public void SetMultiplier(float newMultiplier)
    {
        GridSizeMultiplier = newMultiplier;
        transform.position = GridPosition * GridSizeMultiplier;
    }

    public void AddConnection(GridPoint Connection)
    {
        if (ConnectedTiles.Contains(Connection)) return;
        else ConnectedTiles.Add(Connection);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.black;
        foreach (GridPoint Connection in ConnectedTiles)
        {
            Gizmos.DrawLine(transform.position, Connection.transform.position);
        }

    }
}
