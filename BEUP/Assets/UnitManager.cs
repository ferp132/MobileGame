using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private GameManager game = null;
    protected GridManager grid = null;

    public GameManager Game { get => game; set => game = value; }

    public virtual void StartTurn(){ }
    public virtual void Init(GameManager game) { }
    public virtual void EndTurn() { }


}
