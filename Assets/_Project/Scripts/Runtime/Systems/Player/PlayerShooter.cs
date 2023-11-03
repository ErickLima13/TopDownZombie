using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private LineRenderer shootLine;

    public float timeBetweenBullets = 0.3f;
    public float range = 100f;
    public float displayEffect = 0.02f;

    private float timer;

    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootMask;

    public bool isShoot;

    private void Start()
    {
        shootLine = GetComponent<LineRenderer>();
        shootLine.enabled = false;

        shootMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isShoot = false;
        }

        if(timer >= timeBetweenBullets * displayEffect)
        {
            DisableEffect();
        }
    }

    private void Shoot()
    {
        timer = 0;
        isShoot = true;
        shootLine.enabled = true;
        shootLine.SetPosition(0, transform.position);
        
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootMask))
        {
            // comandos quando acertar um inimigo
            shootLine.SetPosition(1, shootHit.point);
        }
        else
        {
            shootLine.SetPosition(1,shootRay.origin + shootRay.direction * range);
        }
    }

    private void DisableEffect()
    {
        shootLine.enabled = false;
    }
}
