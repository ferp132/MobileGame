using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    protected GameManager game = null;

    public virtual void StartTurn(){ }
    public virtual void Init(GameManager game) { }
    public virtual void EndTurn() { }


}
