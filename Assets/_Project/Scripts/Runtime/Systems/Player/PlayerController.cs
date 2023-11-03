using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private PlayerShooter shooter;
    private Animator animator;

    [Range(0,5)] public float movementSpeed;

    private Vector3 movement;

    private int groundMask;

    private float camRayLength = 100f;

    private bool isWalk;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        shooter = GetComponent<PlayerShooter>();
        animator = GetComponentInChildren<Animator>();
        groundMask = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Turning();
        Move(horizontal, vertical);
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit;
        if (Physics.Raycast(camRay, out groundHit,camRayLength,groundMask))
        {
            Vector3 playerToMouse = groundHit.point - transform.position;
            playerToMouse.y = 0;
            Quaternion newRotation  = Quaternion.LookRotation(playerToMouse);
            playerRb.MoveRotation(newRotation);
        }
    }

    private void Move(float h,float v)
    {
        if (!shooter.isShoot)
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

        movement = movement.normalized * movementSpeed * Time.deltaTime;
        playerRb.MovePosition(transform.position + movement);
        animator.SetBool("isWalk", isWalk);
    }

    public void TriggerShoot()
    {
        animator.SetTrigger("attack");
    }

    public void ChangeAnimationLayer(int idLayer)
    {
        for(int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(idLayer, 1);
    }
}
