using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] decals;
    public Color[] bloodColor;
    public float fadeSpeed;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        spriteRenderer.sprite = decals[Random.Range(0,decals.Length)];
        spriteRenderer.color = bloodColor[0];
        StartCoroutine(Fading());
    }

    private IEnumerator Fading()
    {
        while (spriteRenderer.color != bloodColor[1])
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, bloodColor[1], fadeSpeed);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
