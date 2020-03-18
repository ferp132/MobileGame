using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerManager :  UnitManager
{
    PlayerMovement  movement = null;
    PlayerInput     input    = null;

    public void Start()
    {
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
    }

    public override void StartTurn()
    {
        input.MyTurn = true;
        movement.StartTurn();
    }

    public override void Init(GameManager game)
    {
        this.game = game;
        movement.Init();

    }

    public override void EndTurn()
    {
        input.MyTurn = false;
        game.EndTurn();
    }

}
