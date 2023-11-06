using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public Sprite picture;


    private EnemyMovement movement;
    private UIManager uiManager;

    private void Start()
    {
        movement = GetComponent<EnemyMovement>();
        uiManager = FindObjectOfType<UIManager>();

        currentHp = maxHp;

    }

    public void MakeHit(int dmg)
    {
        currentHp -= dmg;

        float perc = currentHp / (float)maxHp;

        uiManager.UpdateHpBar(perc,picture);

        if (currentHp <= 0)
        {
            movement.TriggerDead();
        }
    }
}
