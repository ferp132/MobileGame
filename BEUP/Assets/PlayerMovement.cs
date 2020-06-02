using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : UnitMovement
{

    List<Waypoint> moveableWaypoints = new List<Waypoint>();

    public List<Waypoint> MoveableWaypoints { get => moveableWaypoints; set => moveableWaypoints = value; }

    public void Start()
    {
        manager = GetComponent<PlayerManager>();
    }

    public override void Init(UnitManager manager)
    {
        base.Init(manager);

        if (grid.PlayerSpawn.Count > 0)
        {
            currentWaypoint = grid.PlayerSpawn[Random.Range(0, grid.PlayerSpawn.Count - 1)];
        }
        else
        {
            currentWaypoint = grid.GetWaypoint(Vector3Int.zero);
        }

        currentWaypoint.OccupiedBy = gameObject;
        transform.position = currentWaypoint.transform.position;
    }

    void FindMoveableWaypoints()
    {
        grid.ResetGrid();
        moveableWaypoints.Clear();

        //if we still have movepoints
        if (MovePoints > 0)
        {
            //add the possible moves from my start position
            moveableWaypoints.AddRange(GetPossibleMoves(currentWaypoint));

            //If we have extra movement to spare, also add possible moves from list
            for (int i = 0; i < MovePoints - 1; i++)
            {
                //Make a tempoary list
                List<Waypoint> potentialWaypoints = new List<Waypoint>();

                foreach (Waypoint waypoint in moveableWaypoints)
                {

                    if (waypoint.Explored) continue;
                    else potentialWaypoints.AddRange(GetPossibleMoves(waypoint));
                }

                moveableWaypoints.AddRange(potentialWaypoints);
            }
        }

        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetSelectionType(SelectionType.PLAYERMOVE);
        }
    }


    protected override bool IsValidMove(Waypoint from, Waypoint to)
    {
        if (moveableWaypoints.Contains(to)) return false;

        return base.IsValidMove(from, to);
    }

    public void StartTurn()
    {
        MovePoints = MaxMovePoints;
        FindMoveableWaypoints();
    }

    public void SetCurrentWaypoint(Waypoint waypoint)
    {
        MoveTo(waypoint);

        if (MovePoints > 0)
        {
            FindMoveableWaypoints();
        }
        else
        {
            manager.EndTurn();
        }

    }

    public void EndTurn()
    {
        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.ResetWaypoint();
        }

        moveableWaypoints.Clear();
    }
}
