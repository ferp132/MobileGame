using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : UnitManager
{
    [SerializeField] GameObject enemy = null;
    List<EnemyMovement> enemies = new List<EnemyMovement>();
    private List<EnemyMovement> enemiesToDestroy = new List<EnemyMovement>();
    AudioSource audio = null;

    public List<EnemyMovement> Enemies { get => enemies; set => enemies = value; }
    public List<EnemyMovement> EnemiesToDestroy { get => enemiesToDestroy; set => enemiesToDestroy = value; }

    public List<Waypoint> DifficultyBlocks1 = new List<Waypoint>();
    public List<Waypoint> DifficultyBlocks2 = new List<Waypoint>();

    public Color difficultyColor1;
    public Color difficultyColor2;

    void SpawnEnemies(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Waypoint spawnLocation = GetRandomSpawnLocation();

            if (spawnLocation != null)
            {
               
                EnemyMovement movement = Instantiate(enemy).GetComponent<EnemyMovement>();
                movement.CurrentWaypoint = spawnLocation;
                movement.Init(this);
                enemies.Add(movement);
                movement.GetComponent<EnemyHealth>().Init(this);
            }
        }
    }

    public void DestroyEnemy(EnemyHealth toDestroy)
    {
        Game.AddScore(10);
        EnemyMovement movement = (toDestroy.GetComponent<EnemyMovement>());
        enemies.Remove(movement);
        movement.CurrentWaypoint.OccupiedBy = null;
        Destroy(toDestroy.gameObject);
    }

    private Waypoint GetRandomSpawnLocation()
    {
        
        List<Waypoint> possibleSpawns = new List<Waypoint>(grid.EnemySpawn);
        Waypoint validSpawn = null;

        while (validSpawn == null)
        {
            int numberPoints = possibleSpawns.Count - 1;
            Waypoint potentialWaypoint = possibleSpawns[Random.Range(0, numberPoints)];

            if (potentialWaypoint.OccupiedBy == null) validSpawn = potentialWaypoint;
            else possibleSpawns.Remove(potentialWaypoint);

            if (possibleSpawns.Count == 0) break;
        }

        return validSpawn;

    }

    public override void StartTurn()
    {

        foreach (EnemyMovement enemy in enemies)
        {
            enemy.ResetMovement();
            enemy.ProcessMovement();
        }

        int numEnemiesToSpawn = 0;

        if (Game.Score == 300)
        {
            Camera.main.backgroundColor = difficultyColor1;

            foreach (Waypoint block in DifficultyBlocks1)
            {
                block.gameObject.SetActive(true);
            }
        }

        if (Game.Score == 600)
        {
            Camera.main.backgroundColor = difficultyColor2;

            foreach (Waypoint block in DifficultyBlocks2)
            {
                block.gameObject.SetActive(true);
            }
        }

        if (Game.Score < 250)
        {
            numEnemiesToSpawn = Mathf.RoundToInt(Random.Range(0, 2));
        }
        else if (Game.Score < 500)
        {
            numEnemiesToSpawn = 1;
        }
        else
        {
            numEnemiesToSpawn = Mathf.RoundToInt(Random.Range(0, 3));
        }

        SpawnEnemies(numEnemiesToSpawn);

        DestroyEnemiesAtDestination();
        EndTurn();
    }

    private void DestroyEnemiesAtDestination()
    {
        while (enemiesToDestroy.Count > 0)
        {
            if (!audio.isPlaying) audio.Play();
            Game.LoseLives(1);
            EnemyMovement toDestroy = enemiesToDestroy[0];
            DestroyEnemy(toDestroy.GetComponent<EnemyHealth>());
            enemiesToDestroy.RemoveAt(0);
        }
    }

    public override void EndTurn()
    {
        Game.EndTurn();
    }

    public override void Init(GameManager game)
    {
        foreach (Waypoint block in DifficultyBlocks1)
        {
            block.gameObject.SetActive(false);
        }

        foreach (Waypoint block in DifficultyBlocks2)
        {
            block.gameObject.SetActive(false);
        }

        this.Game = game;

        audio = GetComponent<AudioSource>();

        grid = GameObject.FindObjectOfType<GridManager>();

        EnemyMovement[] children = GetComponentsInChildren<EnemyMovement>();

        enemies.AddRange(children);

        foreach (EnemyMovement enemy in enemies)
        {
            enemy.Init(this);
        }
    }
}
