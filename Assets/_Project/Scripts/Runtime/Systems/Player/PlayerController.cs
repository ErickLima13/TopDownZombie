using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private PlayerShooter shooter;
    private Animator animator;
    private AudioManager audioManager;

    [Range(0, 5)] public float movementSpeed;

    private Vector3 movement;

    private int groundMask;

    private float camRayLength = 100f;

    private bool isWalk;
    private bool isStep;

    [Range(0, 100)] public float limitCam;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        playerRb = GetComponent<Rigidbody>();
        shooter = GetComponent<PlayerShooter>();
        animator = GetComponentInChildren<Animator>();
        groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if (isWalk)
        {
            LimitMove();
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Turning();
        Move(horizontal, vertical);
    }

    private void LimitMove()
    {
        if (transform.position.x >= limitCam)
        {
            transform.position = new(limitCam, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -limitCam)
        {
            transform.position = new(-limitCam, transform.position.y, transform.position.z);
        }

        if (transform.position.z >= limitCam)
        {
            transform.position = new(transform.position.x, transform.position.y, limitCam);
        }
        else if (transform.position.z <= -limitCam)
        {
            transform.position = new(transform.position.x, transform.position.y, -limitCam);
        }
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit;
        if (Physics.Raycast(camRay, out groundHit, camRayLength, groundMask))
        {
            Vector3 playerToMouse = groundHit.point - transform.position;
            playerToMouse.y = 0;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            if (!shooter.isKnifeAttack)
            {
                playerRb.MoveRotation(newRotation);
            }
        }
    }

    private void Move(float h, float v)
    {
        if (!shooter.isShoot && !shooter.isKnifeAttack)
        {
            movement.Set(h, 0f, v);

            if (h == 0 && v == 0)
            {
                isWalk = false;
            }
            else
            {
                isWalk = true;
            }
        }
        else
        {
            isWalk = false;
            movement.Set(0f, 0f, 0f);
        }

        if (isWalk && !isStep)
        {
            StartCoroutine(StepFx());
        }



        movement = movement.normalized * movementSpeed * Time.deltaTime;
        playerRb.MovePosition(transform.position + movement);
        animator.SetBool("isWalk", isWalk);
    }

    private IEnumerator StepFx()
    {
        isStep = true;
        int idStep = Random.Range(0, audioManager.steps.Length);
        audioManager.PlaySfx(audioManager.steps[idStep]);
        yield return new WaitForSeconds(audioManager.steps[idStep].length);
        isStep = false;
    }

    public void TriggerShoot()
    {
        animator.SetTrigger("attack");
        audioManager.PlaySfx(audioManager.fires[shooter.idWeapon]);
    }

    public void ChangeAnimationLayer(int idLayer)
    {
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(idLayer, 1);
    }

    public AudioManager GetAudioManager()
    {
        return audioManager;
    }
}
