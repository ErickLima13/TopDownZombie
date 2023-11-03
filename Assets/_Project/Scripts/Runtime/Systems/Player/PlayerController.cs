using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    [Range(0,5)] public float movementSpeed;

    private Vector3 movement;

    private int groundMask;

    private float camRayLength = 100f;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
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
        movement.Set(h,0f,v);
        movement = movement.normalized * movementSpeed * Time.deltaTime;
        playerRb.MovePosition(transform.position + movement);

    }
}
