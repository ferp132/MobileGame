using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : UnitMovement
{
    Waypoint destination = null;

    public override void Init(UnitManager manager)
    {
        base.Init(manager);
        if (currentWaypoint)
        {
            transform.position = currentWaypoint.transform.position;
            currentWaypoint.OccupiedBy = gameObject;
        }
    }

    public void ProcessMovement()
    {
        // Check if I have move points
        if (MovePoints <= 0) return;

        Waypoint potentialWaypoint = grid.GetWaypoint(currentWaypoint.GetGridPos() + new Vector3Int(0, 0, -1));

        if (potentialWaypoint != null && IsValidMove(currentWaypoint, potentialWaypoint))
        {
             MoveTo(potentialWaypoint);
        }
        else
        {
            potentialWaypoint = null;

            // Else Pathfind
            destination = GetClosestWaypoint(grid.EnemyDestination);

            if (destination != null)
            {
                potentialWaypoint = BreadthFirstPathFind(currentWaypoint, destination);
            }

            if (potentialWaypoint != null) MoveTo(potentialWaypoint);
        }

        grid.ResetGrid();

        if (grid.EnemyDestination.Contains(currentWaypoint))
        {
            ((EnemyManager)manager).EnemiesToDestroy.Add(this);
        }
    }

    private Waypoint GetClosestWaypoint(List<Waypoint> list)
    {
        Waypoint closestWaypoint = null;
        int clostestDistance = int.MaxValue;

        foreach (Waypoint waypoint in list)
        {
            int potentailDistance = GetMoveValue(currentWaypoint, waypoint);

            if (potentailDistance < clostestDistance)
            {
                closestWaypoint = waypoint;
                clostestDistance = potentailDistance;
            }
        }

        return closestWaypoint;
    }

    private Waypoint BreadthFirstPathFind(Waypoint from, Waypoint to)
    {
        Waypoint nextWaypoint = null;
        Queue<Waypoint> queue = new Queue<Waypoint>();
        queue.Enqueue(from);

        while (nextWaypoint == null)
        {
            Waypoint checking = (queue.Dequeue());

            List<Waypoint> moves = GetPossibleMoves(checking);

            foreach (Waypoint waypoint in moves)
            {
                if (waypoint.FoundBy == null)
                {
                    waypoint.FoundBy = checking;
                    queue.Enqueue(waypoint);
                }
                    

                if (waypoint == destination)
                {
                    Waypoint previous = waypoint;

                    while (nextWaypoint == null)
                    {
                        if (previous.FoundBy != from) previous = previous.FoundBy;
                        else 
                        {
                            nextWaypoint = previous;
                        }
                    }
                }
            }

            if (queue.Count <= 0)
            {
                break;
            }
        }

        return nextWaypoint;
    }
}
