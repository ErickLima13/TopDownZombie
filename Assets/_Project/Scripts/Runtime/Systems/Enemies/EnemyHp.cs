using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class EnemyHp : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public Sprite picture;
    public GameObject bloodPrefab;
    public GameObject decalPrefab;

    private EnemyMovement movement;
    private UIManager uiManager;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        movement = GetComponent<EnemyMovement>();
        uiManager = FindObjectOfType<UIManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentHp = maxHp;
    }

    public void MakeHit(int dmg)
    {
        GameObject temp = Instantiate(bloodPrefab, transform.position, bloodPrefab.transform.localRotation);
        Destroy(temp, 1);

        currentHp -= dmg;

        float perc = currentHp / (float)maxHp;

        if (perc < 0)
        {
            perc = 0;
        }

        uiManager.UpdateHpBar(perc,picture);

        if (currentHp <= 0)
        {
            movement.TriggerDead();
            StartCoroutine(Dissapear());
        }
    }

    private IEnumerator Dissapear()
    {
        spriteRenderer.sortingOrder--;
        Instantiate(decalPrefab,transform.position,decalPrefab.transform.localRotation);

        yield return new WaitForSeconds(3);

        for(int i = 0; i < 15; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.02f);
        }

        Destroy(decalPrefab);
        Destroy(gameObject); 
        
        
    }
}
