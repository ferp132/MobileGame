using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerMovement : MonoBehaviour
{
    PlayerManager player = null;
    GridManager grid = null;
    Waypoint currentWaypoint = null;
    int MovePoints;
    [SerializeField] int MaxMovePoints = 2;
    List<Waypoint> moveableWaypoints = new List<Waypoint>();
    [SerializeField]
    List<Vector3Int> moveableDirections = new List<Vector3Int>() {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1)};

    public List<Waypoint> MoveableWaypoints { get => moveableWaypoints; set => moveableWaypoints = value; }

    public void Start()
    {
        player = GetComponent<PlayerManager>();
    }

    public void Init()
    {
        grid = FindObjectOfType<GridManager>();

        currentWaypoint = grid.PlayerStart;

        transform.position = currentWaypoint.transform.position;
    }

    void FindMoveableWaypoints()
    {
        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetTopColor(Color.white);
        }

        grid.SetGridUnexplored();
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
            waypoint.SetTopColor(Color.yellow);
        }
    }

    private List<Waypoint> GetPossibleMoves(Waypoint from)
    {
        List<Waypoint> list = new List<Waypoint>();

        foreach (Vector3Int direction in moveableDirections)
        {
            Waypoint potentialWaypoint = grid.GetWaypoint(from.GetGridPos() + direction);

            if (potentialWaypoint)
            {
                if (IsValidMove(from, potentialWaypoint) && !list.Contains(potentialWaypoint)) list.Add(potentialWaypoint);
            }
            else print("Could not find waypoint at" + (from.GetGridPos() + direction));
        }

        foreach (Waypoint toDestination in from.AttachedWaypoints)
        {
            if (IsValidMove(from, toDestination) && !list.Contains(toDestination)) list.Add(toDestination);
        }

        from.Explored = true;
        return list;
    }

    private bool IsValidMove(Waypoint from, Waypoint to)
    {
        if (moveableWaypoints.Contains(to)) return false;
        if (to == currentWaypoint) return false;
        if (to.BlockedPaths.Contains(from.GetGridPos() - to.GetGridPos())) return false;
        if (from.BlockedPaths.Contains(to.GetGridPos() - from.GetGridPos())) return false;

        return true;
    }

    public void StartTurn()
    {
        MovePoints = MaxMovePoints;
        FindMoveableWaypoints();
    }

    public void SetCurrentWaypoint(Waypoint newWaypoint)
    {
        Vector3Int move = newWaypoint.GetGridPos() - currentWaypoint.GetGridPos();
        int moveValue = Mathf.Abs(move.x) + Mathf.Abs(move.y) + Mathf.Abs(move.z);

        MovePoints -= moveValue;
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
        foreach (Waypoint waypoint in moveableWaypoints)
        {
            waypoint.SetTopColor(Color.white);
        }

        moveableWaypoints.Clear();
        player.EndTurn();
    }
}
