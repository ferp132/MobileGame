using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool playerTurn = true;

    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn)
        {
            playerTurn = true;
            FindObjectOfType<PlayerInput>().StartPlayerTurn();
        } 
    }
}
