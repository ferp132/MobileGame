using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Waypoint currentWaypoint = null;
    GridManager grid;
    int MovePoints = 2;
    List<Waypoint> moveableWaypoints = new List<Waypoint>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void Init()
    {
        grid = FindObjectOfType<GridManager>();
        currentWaypoint = grid.PlayerStart;
        transform.position = currentWaypoint.transform.position;
        FindMoveableWaypoints();
    }
    void FindMoveableWaypoints()
    {
        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetTopColor(Color.green);
        }

        moveableWaypoints.Clear();

        if (MovePoints > 0) 
        {
            //Add the current waypoint's attached waypoints to the moveableWaypoints list
            moveableWaypoints.AddRange(currentWaypoint.AttachedWaypoints.Values);

            //If we have extra movement to spare, also add the attached waypoints of those waypoints in the moveable waypoitns list
            for (int i = 0; i < MovePoints - 1; i++)
            {
                //Make a tempoary list
                List<Waypoint> potentialWaypoints = new List<Waypoint>();

                //Add waypoints that are not currently in the actual moveable waypoints list to the temp list
                foreach (Waypoint waypoint in moveableWaypoints)
                {
                    foreach (Waypoint attachedWaypoint in waypoint.AttachedWaypoints.Values)
                    {
                        if (moveableWaypoints.Contains(attachedWaypoint) || attachedWaypoint == currentWaypoint) continue;
                        else potentialWaypoints.Add(attachedWaypoint);
                    }

                }
                //add those waypoints to the main list
                moveableWaypoints.AddRange(potentialWaypoints);
            }
        }

        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetTopColor(Color.yellow);
        }
    }
}
