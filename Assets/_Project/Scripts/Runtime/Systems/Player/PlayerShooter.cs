using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    private LineRenderer shootLine;
    private PlayerController playerController;

    [Range(0, 1)] public float[] timeBetweenBullets;
    public float range = 100f;
    public float displayEffect = 1f;
    public float delayKnifeAttack = 0.6f;

    private float timer;

    private Ray shootRay = new();
    private RaycastHit shootHit = new();
    private int shootMask;

    public bool isShoot;
    public bool isKnifeAttack;
    private bool isReload;

    public int idWeapon;
    public Transform[] shootPos;
    public List<GameObject> weapons;

    public Transform hitBoxKnifePos;
    public GameObject hitBoxKnifePrefab;

    public Sprite[] iconWeapon;
    public Image pictureWeapon;

    public TextMeshProUGUI totalMunitions;
    public TextMeshProUGUI currentBullets;

    public int[,] munitions = new int[3, 3];

    private AudioManager audioManager;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        shootLine = GetComponent<LineRenderer>();
        audioManager = AudioManager.Instance;
        shootLine.enabled = false;

        shootMask = LayerMask.GetMask("Enemy");

        ChangeWeapon();

        //munitions[idweapon, 0 = qtd pentes, 1 = munição, 2 = qtdmaxmuniçãoporpente

        munitions[0, 0] = 0;
        munitions[0, 1] = 0;
        munitions[0, 2] = 0;

        munitions[1, 0] = 0;
        munitions[1, 1] = 99;
        munitions[1, 2] = 99;

        munitions[2, 0] = 1;
        munitions[2, 1] = 12;
        munitions[2, 2] = 12;
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

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets[idWeapon] && idWeapon != 0  && !isReload)
        {
            Shoot();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isShoot = false;
        }

        if (timer >= timeBetweenBullets[idWeapon] * displayEffect)
        {
            DisableEffect();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !isKnifeAttack && !isReload)
        {
            idWeapon++;
            if (idWeapon > weapons.Count - 1)
            {
                idWeapon = 0;
            }

            playerController.ChangeAnimationLayer(idWeapon);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !isKnifeAttack && !isReload)
        {
            idWeapon--;
            if (idWeapon < 0)
            {
                idWeapon = weapons.Count - 1;
            }

            playerController.ChangeAnimationLayer(idWeapon);
        }

        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        if (idWeapon == 0)
        {
            totalMunitions.enabled = false;
            currentBullets.enabled = false;
        }
        else
        {
            totalMunitions.enabled = true;
            currentBullets.enabled = true;
        }


        pictureWeapon.sprite = iconWeapon[idWeapon];
        UpdateHud();
    }

    private void UpdateHud()
    {
        totalMunitions.text = munitions[idWeapon, 0].ToString();
        currentBullets.text = munitions[idWeapon, 1].ToString();
    }

    private bool CheckMunition()
    {
        bool hasMunition = munitions[idWeapon, 1] > 0;
        return hasMunition;
    }

    private void Shoot()
    {
        timer = 0;
        isShoot = true;

        if (CheckMunition())
        {
            playerController.TriggerShoot();
            shootLine.enabled = true;
            shootLine.SetPosition(0, shootPos[idWeapon].position);

            shootRay.origin = shootPos[idWeapon].position;
            shootRay.direction = transform.forward;

            Debug.DrawRay(shootRay.origin, shootRay.direction * range, Color.red, 1);

            if (Physics.Raycast(shootRay, out shootHit, range, shootMask))
            {
                // comandos quando acertar um inimigo
                shootLine.SetPosition(1, shootHit.point);

                shootHit.transform.gameObject.GetComponentInChildren<EnemyHp>().MakeHit(1);
            }
            else
            {
                shootLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }

            munitions[idWeapon, 1]--;

            if (munitions[idWeapon, 1] <= 0 && munitions[idWeapon, 0] > 0)
            {
                StartCoroutine(Reload());
            }
        }
        else
        {
            audioManager.PlaySfx(audioManager.noAmmo);
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

    private IEnumerator Reload()
    {
        isReload = true;
        audioManager.PlaySfx(audioManager.reloads[idWeapon]);
        yield return new WaitForSeconds(audioManager.reloads[idWeapon].length);
        munitions[idWeapon, 1] = munitions[idWeapon, 2];
        munitions[idWeapon, 0]--;
        isReload = false;
        UpdateHud();
    }
}
