using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private LineRenderer shootLine;
    private PlayerController playerController;

    public float timeBetweenBullets = 0.3f;
    public float range = 100f;
    public float displayEffect = 1f;
    public float delayKnifeAttack = 0.6f;

    private float timer;

    private Ray shootRay = new();
    private RaycastHit shootHit = new();
    private int shootMask;

    public bool isShoot;
    public bool isKnifeAttack;

    public int idWeapon;
    public Transform[] shootPos;
    public List<GameObject> weapons;

    public Transform hitBoxKnifePos;
    public GameObject hitBoxKnifePrefab;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        shootLine = GetComponent<LineRenderer>();
        shootLine.enabled = false;

        shootMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && idWeapon == 0 && !isKnifeAttack)
        {
            isKnifeAttack = true;
            playerController.TriggerShoot();
            StartCoroutine(EndKnifeAttack());
        }

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && idWeapon != 0)
        {
            Shoot();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isShoot = false;
        }

        if (timer >= timeBetweenBullets * displayEffect)
        {
            DisableEffect();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !isKnifeAttack)
        {
            idWeapon++;
            if (idWeapon > weapons.Count - 1)
            {
                idWeapon = 0;
            }

            playerController.ChangeAnimationLayer(idWeapon);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !isKnifeAttack)
        {
            idWeapon--;
            if (idWeapon < 0)
            {
                idWeapon = weapons.Count - 1;
            }

            playerController.ChangeAnimationLayer(idWeapon);
        }

    }

    private void Shoot()
    {
        playerController.TriggerShoot();

        timer = 0;
        isShoot = true;
        shootLine.enabled = true;
        shootLine.SetPosition(0, shootPos[idWeapon].position);

        shootRay.origin = shootPos[idWeapon].position;
        shootRay.direction = transform.forward;

        Debug.DrawRay(shootRay.origin, shootRay.direction * range, Color.red, 1);

        if (Physics.Raycast(shootRay, out shootHit, range, shootMask))
        {
            print(shootHit.transform.name);

            // comandos quando acertar um inimigo
            shootLine.SetPosition(1, shootHit.point);

            shootHit.transform.gameObject.GetComponentInChildren<EnemyHp>().MakeHit(1);
        }
        else
        {
            shootLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    private void DisableEffect()
    {
        shootLine.enabled = false;
    }

    private IEnumerator EndKnifeAttack()
    {
        yield return new WaitForSeconds(0.2f);

        Instantiate(hitBoxKnifePrefab, hitBoxKnifePos.position, transform.localRotation);

        yield return new WaitForSeconds(delayKnifeAttack);

        isKnifeAttack = false;
    }
}
