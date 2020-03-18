using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInput : MonoBehaviour
{
    Waypoint selectedWaypoint = null;

    PlayerMovement movement = null;

    bool myTurn = false;

    public bool MyTurn { get => myTurn; set => myTurn = value; }

    public void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myTurn) GetMouseInput();
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
            if (movement.MoveableWaypoints.Contains(selectedWaypoint))
            {
                //set the current waypoint and move to it
               movement.SetCurrentWaypoint(selectedWaypoint);
            }

        }

    }

    void SetSelectedWaypoint(Waypoint newWaypoint)
    {
        if (selectedWaypoint)
        {
            selectedWaypoint.SetTopColor(movement.MoveableWaypoints.Contains(selectedWaypoint) ? Color.yellow : Color.white);
        }
            

        selectedWaypoint = newWaypoint;

        if (movement.MoveableWaypoints.Contains(selectedWaypoint))
        {
            selectedWaypoint.SetTopColor(Color.green);
        }
        else selectedWaypoint.SetTopColor(Color.red);
    }
}
