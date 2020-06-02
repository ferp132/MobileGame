using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    private bool playerTurn = true;

    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }
    public int Lives { get => lives; }
    public int Score { get => score; }
    public bool Paused { get => paused; set => paused = value; }

    UnitManager player = null;
    UnitManager enemies = null;
    GridManager grid = null;
    UIManager UI = null;

    bool paused = false;

    int lives = 3;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        player = FindObjectOfType<PlayerManager>();
        enemies = FindObjectOfType<EnemyManager>();
        UI = FindObjectOfType<UIManager>();


        grid.Init();
        player.Init(this);
        enemies.Init(this);
        UI.Init(this);

        StartGame();
    }

    public void EndTurn()
    {
        playerTurn = !playerTurn;
        StartTurn();
    }

    void StartGame()
    {
        string gameId = "3537242";
        Advertisement.Initialize(gameId, true);

        playerTurn = true;
        StartTurn();
    }

    private void StartTurn()
    {
        UnitManager manager = playerTurn ? player : enemies;

        manager.StartTurn();
    }

    public void LoseLives(int amount)
    {
        lives -= amount;
        UI.UpdateUI();


        if (lives <= 0)
        {
            //Player Has Lost The Game
            print("Player Has Lost!!");

            LoseGame();
        }


    }

    private static void LoseGame()
    {
        int timesPlayedValue = PlayerPrefs.GetInt("Times Played");
        timesPlayedValue++;

        ShowAd();

        PlayerPrefs.SetInt("Times Played", timesPlayedValue);
        SceneManager.LoadScene("Menu");
    }

    private static void ShowAd()
    {
        const string PlacementId = "video";

        print("Trying To Play Ad");
        if (Advertisement.IsReady())
        {
            print("Ad is Ready!");
            Advertisement.Show(PlacementId);
        }

    }

    public void adViewResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Player Viewed Complete Ad");
                break;
            case ShowResult.Skipped:
                Debug.Log("Player Skipped Ad");
                break;
            case ShowResult.Failed:
                Debug.Log("Problem Showing Ad");

                break;
        }
    }

        public void AddScore(int amount)
    {
        score += amount;
        UI.UpdateUI();
    }
}
