using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public PlayerStatus playerStatus;

    public CharacterController controller;
    public float speed = 12f;

    public float gravityMultiplier = 1;


    [SerializeField] private Transform groundCheck;
    [SerializeField] public LayerMask layerMask;
    bool isGrounded;

    public float jumpHight = 2;

    Vector3 playerGravity;

    void Update()
    {   
        if (playerStatus.alive == true)
        {
            float gravity = -9.81f * gravityMultiplier;

            isGrounded = Physics.CheckSphere(groundCheck.position, .3f, layerMask);
            if (isGrounded && playerGravity.y < 0)
            {

                playerGravity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (x != 0 && z != 0)
            {
                x = x / 2;
                z = z / 2;
            }

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                playerGravity.y = Mathf.Sqrt(jumpHight * -2 * gravity);
            }

            playerGravity.y += gravity * Time.deltaTime;
            controller.Move(playerGravity * Time.deltaTime);

        }
    }

}
