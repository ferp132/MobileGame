using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool playerTurn = true;

    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

    UnitManager player = null;
    UnitManager enemies = null;
    GridManager grid = null;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        player = FindObjectOfType<PlayerManager>();
        enemies = FindObjectOfType<EnemyManager>();

        grid.Init();
        player.Init(this);
        enemies.Init(this);

        StartGame();
    }

    public void EndTurn()
    {
        //playerTurn = !playerTurn;
        StartTurn();
    }

    void StartGame()
    {
        playerTurn = true;
        StartTurn();
    }

    private void StartTurn()
    {
        //UnitManager man = playerTurn ? player : enemies;

        //man.StartTurn();

        player.StartTurn();
    }
}
