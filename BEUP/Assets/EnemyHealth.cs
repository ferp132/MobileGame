using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField]
    int maxHealth = 1;
    float currentHealth = 0;

    EnemyManager manager = null;

    public void Init(UnitManager manager)
    {
        this.manager = (EnemyManager)manager;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetColor(Color color)
    {
        MeshRenderer meshRenderer = transform.Find("Cube").GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            manager.DestroyEnemy(this);
        }
    }

}
