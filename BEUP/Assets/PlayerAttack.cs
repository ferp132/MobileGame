using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] int attackRange = 2;
    [SerializeField] private float attackValue = 1;
    [SerializeField]
    List<Vector3Int> attackDirections = new List<Vector3Int>(){
        new Vector3Int(1, 0, 0),    // Right
        new Vector3Int(-1, 0, 0),   // Left
        new Vector3Int(0, 0, 1),    // Up
        new Vector3Int(0, 0, -1),   // Down
        new Vector3Int(1, 0, 1),    // Up-Left
        new Vector3Int(-1, 0, -1),  // Down-Right
        new Vector3Int(1, 0, -1),   // Down-Left
        new Vector3Int(-1, 0, 1)}   // Up- Right
    ;

    List<EnemyHealth> validTargets = new List<EnemyHealth>();
    EnemyHealth selectedEnemy = null;

    GridManager grid = null;
    PlayerMovement movement = null;
    PlayerManager manager = null;
    AudioSource audio = null;

    public EnemyHealth SelectedEnemy { get => selectedEnemy; set => selectedEnemy = value; }
    public List<EnemyHealth> ValidTargets { get => validTargets; set => validTargets = value; }

    public void StartTurn()
    {
        UpdateValidTargets();
    }

    public void Init(UnitManager manager)
    {
        this.manager = (PlayerManager)manager;

        grid = FindObjectOfType<GridManager>();
        movement = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
    }

    public void UpdateValidTargets()
    {
        ResetTargets();

        List<EnemyHealth> potentialTargets = FindInRange(attackDirections, attackRange);

        // Check if these targets are valid, as of now, they are always valid.
        validTargets.AddRange(potentialTargets);

        //Set the color of the enemy to show they are vulnerable
        foreach (EnemyHealth enemy in validTargets)
        {
            if(enemy) enemy.SetColor(Color.yellow);
        }
    }

    private List<EnemyHealth> FindInRange(List<Vector3Int> directions, int range)
    {
        List<Vector3Int> directionsToCheck = MultiplyDirectionsByRange(directions, range);
        List<EnemyHealth> targetsFound = new List<EnemyHealth>();

        foreach (Vector3Int direction in directionsToCheck)
        {
            Waypoint foundWaypoint = grid.GetWaypoint(movement.CurrentWaypoint.GetGridPos() + direction);

            if (foundWaypoint)
            {

                GameObject foundObject = foundWaypoint.OccupiedBy;

                if (foundObject)
                {

                    EnemyHealth foundEnemy = foundObject.GetComponent<EnemyHealth>();
                    if (foundEnemy)
                    {
                        if (!targetsFound.Contains(foundEnemy)) targetsFound.Add(foundEnemy);
                    }
                }
            }
        }

        return targetsFound;
    }

    private List<Vector3Int> MultiplyDirectionsByRange(List<Vector3Int> directions, int range)
    {
        if (range <= 1) return directions; 

        int numDir = directions.Count * range;
        List<Vector3Int> result = new List<Vector3Int>(directions);

        for (int i = 0; i < range; i++)
        {
            List<Vector3Int> temp = new List<Vector3Int>(result);

            foreach (Vector3Int x in result)
            {
                foreach (Vector3Int y in directions)
                {
                    Vector3Int potentialDirection = x + y;
                    if (potentialDirection == Vector3Int.zero)  continue;
                    if (result.Contains(potentialDirection))    continue;

                    temp.Add(potentialDirection);
                }
            }
            result.AddRange(temp);
        }

        print("Estimated Directions = " + numDir + " Actual Directions = " + result.Count);
        return result;
    }

    public void EndTurn()
    {
        ResetTargets();
    }

    private void ResetTargets()
    {
        selectedEnemy = null;

        foreach (EnemyHealth enemy in validTargets)
        {
            if(enemy) enemy.SetColor(Color.red);
        }

        validTargets.Clear();
    }

    public void ExecuteAttack()
    {
        if (SelectedEnemy == null) return;

        if (!audio.isPlaying) audio.Play();
        validTargets.Remove(selectedEnemy);
        SelectedEnemy.TakeDamage(attackValue);
        manager.EndTurn();
    }
}
