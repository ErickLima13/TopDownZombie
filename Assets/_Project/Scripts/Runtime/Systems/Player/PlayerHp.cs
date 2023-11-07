using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    private UIManager uiManager;


    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
       
        currentHp = maxHp;

    }

    public void MakeHit(int dmg)
    {
        currentHp -= dmg;

        float perc = currentHp / (float)maxHp;
        if(perc < 0)
        {
            perc = 0;
        }

        uiManager.UpdateHpBarPlayer(perc);

        if (currentHp <= 0)
        {
            // game over
        }
    }
}
