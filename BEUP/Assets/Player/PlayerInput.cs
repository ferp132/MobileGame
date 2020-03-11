using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Waypoint currentWaypoint = null;
    GridManager grid;
    GameManager gman;
    int MovePoints;
    [SerializeField] int MaxMovePoints = 2;
    List<Waypoint> moveableWaypoints = new List<Waypoint>();

    Waypoint selectedWaypoint = null;

    // Start is called before the first frame update
    void Start()
    {
        gman = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gman.PlayerTurn)
        {
            GetMouseInput();
        }

    }

    public void Init()
    {
        grid = FindObjectOfType<GridManager>();

        currentWaypoint = grid.PlayerStart;

        transform.position = currentWaypoint.transform.position;

        StartPlayerTurn();

    }
    void FindMoveableWaypoints()
    {
        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetTopColor(Color.white);
        }

        moveableWaypoints.Clear();

        if (MovePoints > 0) 
        {
            //Add the current waypoint's attached waypoints to the moveableWaypoints list
            moveableWaypoints.AddRange(currentWaypoint.AttachedWaypoints);

            //If we have extra movement to spare, also add the attached waypoints of those waypoints in the moveable waypoitns list
            for (int i = 0; i < MovePoints - 1; i++)
            {
                //Make a tempoary list
                List<Waypoint> potentialWaypoints = new List<Waypoint>();

                //Add waypoints that are not currently in the actual moveable waypoints list to the temp list
                foreach (Waypoint waypoint in moveableWaypoints)
                {
                    foreach (Waypoint attachedWaypoint in waypoint.AttachedWaypoints)
                    {
                        if (moveableWaypoints.Contains(attachedWaypoint) || 
                            attachedWaypoint == currentWaypoint) continue;
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

    void GetMouseInput()
    {
        //if the player is holding the left mouse button
        //if (Input.GetMouseButton(0))
        {
            //Cast a ray from the mouse into the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            // Detect if this raycast hit a moveable block
            if (Physics.Raycast(ray, out hit))
            {

                Waypoint hitWaypoint = hit.collider.GetComponentInParent<Waypoint>();

                if (hitWaypoint)
                {
                    //Save a reference to the highlighted block
                    SetSelectedWaypoint(hitWaypoint);
                }
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            //if the selected waypoint is a valid move location
            if (moveableWaypoints.Contains(selectedWaypoint))
            {
                //set the current waypoint and move to it
                SetCurrentWaypoint(selectedWaypoint);
            }

        }

    }

    void SetSelectedWaypoint(Waypoint newWaypoint)
    {
        if (selectedWaypoint)
        {
            selectedWaypoint.SetTopColor(moveableWaypoints.Contains(selectedWaypoint) ? Color.yellow : Color.white);
        }
            

        selectedWaypoint = newWaypoint;

        if (moveableWaypoints.Contains(selectedWaypoint))
        {
            selectedWaypoint.SetTopColor(Color.green);
        }
        else selectedWaypoint.SetTopColor(Color.red);
    }

    void SetCurrentWaypoint(Waypoint newWaypoint)
    {
        MovePoints -= Mathf.RoundToInt(Vector3Int.Distance(currentWaypoint.GetGridPos(), newWaypoint.GetGridPos()));
        print("Move points remaining: " + MovePoints);

        currentWaypoint = newWaypoint;
        transform.position = currentWaypoint.transform.position;

        if (MovePoints > 0)
        {
            FindMoveableWaypoints();
        }
        else
        {
            EndPlayerTurn();
        }

    }

    void EndPlayerTurn()
    {
        gman.PlayerTurn = false;

        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetTopColor(Color.white);
        }

        moveableWaypoints.Clear();
    }

    public void StartPlayerTurn()
    {
        MovePoints = MaxMovePoints;
        FindMoveableWaypoints();
    }
}
