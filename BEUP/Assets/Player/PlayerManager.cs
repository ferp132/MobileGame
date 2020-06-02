using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerManager :  UnitManager
{
    PlayerMovement  movement = null;
    PlayerInput     input    = null;
    PlayerAttack    attack   = null;

    public override void StartTurn()
    {
        input.MyTurn = true;
        movement.StartTurn();
        attack.StartTurn();
    }

    public override void Init(GameManager game)
    {
        this.Game = game;

        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        movement.Init(this);
        attack.Init(this);
        input.Init(this);

    }

    public override void EndTurn()
    {
        input.MyTurn = false;
        attack.EndTurn();
        movement.EndTurn();
        Game.EndTurn();
        
    }

    public void AddScore(int amount)
    {
        Game.AddScore(amount);
    }


}
