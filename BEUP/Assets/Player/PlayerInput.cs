using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerManager manager = null;

    Waypoint selectedWaypoint = null;

    PlayerMovement movement = null;

    PlayerAttack attack = null;

    bool myTurn = false;

    public bool MyTurn { get => myTurn; set => myTurn = value; }

    public void Init(PlayerManager manager)
    {
        this.manager = manager;
    }

    public void Start()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myTurn && !manager.Game.Paused)
        {
            if (Input.GetKeyDown("s")) manager.EndTurn();

            RaycastPointer();
            GetMovementInput();
            //GetAttackInput();
        }
    }

    private void GetAttackInput()
    {
        if (Input.GetMouseButtonUp(1))
        {
            attack.ExecuteAttack();
            
        }
    }

    private void GetMovementInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (attack.SelectedEnemy != null)
            {
                attack.ExecuteAttack();
            }
            //if the selected waypoint is a valid move location
            else if (movement.MoveableWaypoints.Contains(selectedWaypoint))
            {
                //set the current waypoint and move to it
                movement.SetCurrentWaypoint(selectedWaypoint);

                //Update attack detection
                attack.UpdateValidTargets();
            }
        }
    }

    private void RaycastPointer()
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

                EnemyMovement hitEnemy = hit.collider.GetComponentInParent<EnemyMovement>();

                if (hitEnemy)
                {
                    SetSelectedWaypoint(hitEnemy.CurrentWaypoint);
                    SetSelectedEnemy(hitEnemy);
                    return;
                }
                else
                {
                    SetSelectedEnemy(null);
                }


                Waypoint hitWaypoint = hit.collider.GetComponentInParent<Waypoint>();

                if (hitWaypoint)
                {
                    //Save a reference to the highlighted block
                    SetSelectedWaypoint(hitWaypoint);

                    GameObject foundObject = hitWaypoint.OccupiedBy;

                    if (foundObject)
                    {
                        EnemyMovement foundEnemy = foundObject.GetComponent<EnemyMovement>();
                        if (foundEnemy)
                        {
                            SetSelectedEnemy(foundEnemy);
                        }
                    }
                }
            }

        }
    }

    void SetSelectedWaypoint(Waypoint newWaypoint)
    {
        if (selectedWaypoint)
        {
            if (movement.MoveableWaypoints.Contains(selectedWaypoint)) selectedWaypoint.SetSelectionType(SelectionType.PLAYERMOVE);
            else selectedWaypoint.SetSelectionType(SelectionType.NONE);
        }

        selectedWaypoint = newWaypoint;

        selectedWaypoint.SetSelectionType(movement.MoveableWaypoints.Contains(selectedWaypoint) ? SelectionType.VALID : SelectionType.INVALID);
    }

    private void SetSelectedEnemy(EnemyMovement enemy)
    {


        if (enemy == null)
        {
            if (attack.SelectedEnemy != null)
            {
                attack.SelectedEnemy.SetColor(Color.yellow);

                attack.SelectedEnemy = null;
            }

        }
        else
        {
            EnemyHealth potentialEnemy = enemy.GetComponent<EnemyHealth>();

            if (attack.ValidTargets.Contains(potentialEnemy))
            {
                if (attack.SelectedEnemy != null)
                {
                    attack.SelectedEnemy.SetColor(Color.yellow);
                }

                potentialEnemy.SetColor(Color.green);
                attack.SelectedEnemy = potentialEnemy;
            }
        }


    }
}
