using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField]
    protected Waypoint currentWaypoint = null;
    [SerializeField]
    protected int MaxMovePoints = 2;
    [SerializeField]
    protected List<Vector3Int> moveableDirections = new List<Vector3Int>() {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1)};

    protected GridManager grid = null;

    protected int MovePoints;

    protected UnitManager manager = null;

    public Waypoint CurrentWaypoint { get => currentWaypoint; set => currentWaypoint = value; }

    public void ResetMovement()
    {
        MovePoints = MaxMovePoints;
    }

    public virtual void Init(UnitManager manager)
    {
        this.manager = manager;
        grid = FindObjectOfType<GridManager>();
    }

    protected virtual void MoveTo(Waypoint validWaypoint)
    {
        Waypoint oldWaypoint = currentWaypoint;
        currentWaypoint = validWaypoint;

        // Set occupation values of new and old waypoint
        oldWaypoint.OccupiedBy = null;
        currentWaypoint.OccupiedBy = gameObject;

        // Get the value of the move and subtract it from the units move points
        MovePoints -= GetMoveValue(oldWaypoint, validWaypoint);


        //move the unit to the new waypoint
        transform.position = currentWaypoint.transform.position;
    }

    protected int GetMoveValue(Waypoint from, Waypoint to)
    {
        Vector3Int move = to.GetGridPos() - from.GetGridPos();
        return Mathf.Abs(move.x) + Mathf.Abs(move.y) + Mathf.Abs(move.z);
    }

    protected virtual bool IsValidMove(Waypoint from, Waypoint to)
    {
        if (to == null) return false;
        if (to == currentWaypoint) return false;                                            // if this is our current position, return false
//        if (to.BlockedPaths.Contains(from.GetGridPos() - to.GetGridPos())) return false;    // if the destination blocks moves to our current waypoint
        if (from.BlockedPaths.Contains(to.GetGridPos() - from.GetGridPos())) return false;  // if our current waypoint blocks moves to the destination, return false
        if (to.OccupiedBy != null) return false;                                                    // if our destination is occupied, return false
        if (to.gameObject.activeInHierarchy == false) return false;

        return true;
    }


    protected List<Waypoint> GetPossibleMoves(Waypoint from)
    {
        List<Waypoint> list = new List<Waypoint>();

        foreach (Vector3Int direction in moveableDirections)
        {
            Waypoint potentialWaypoint = grid.GetWaypoint(from.GetGridPos() + direction);

            if (potentialWaypoint != null)
            {
                if (IsValidMove(from, potentialWaypoint) && !list.Contains(potentialWaypoint))
                {
                    list.Add(potentialWaypoint);
                }
            }
        }

        foreach (Waypoint toDestination in from.AttachedWaypoints)
        {
            if (IsValidMove(from, toDestination) && !list.Contains(toDestination)) list.Add(toDestination);
        }

        from.Explored = true;
        return list;
    }
}
