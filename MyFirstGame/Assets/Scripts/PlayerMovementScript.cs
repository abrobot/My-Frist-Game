using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public CharacterController controller;
    Vector3 playerGravity;

    [SerializeField] private Transform groundCheck;
    [SerializeField] public LayerMask layerMask;

    bool isGrounded;
    public float gravityMultiplier = 1;
    public float jumpHight = 2;

    Vector3 movePosition;
    public MoveDirection moveDirection;


    public void SetMoveDirection()
    {

        MoveDirection newMoveDirection = MoveDirection.N;

        bool wPressed = false;
        bool sPressed = false;
        bool aPressed = false;
        bool dPressed = false;

        if (Input.GetKey(KeyCode.W))
        {
            wPressed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            sPressed = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            aPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dPressed = true;
        }

        switch (wPressed, sPressed, aPressed, dPressed)
        {
            case (true, false, false, false):
                newMoveDirection = MoveDirection.F;
                break;
            case (false, true, false, false):
                newMoveDirection = MoveDirection.B;
                break;
            case (false, false, true, false):
                newMoveDirection = MoveDirection.L;
                break;
            case (false, false, false, true):
                newMoveDirection = MoveDirection.R;
                break;
            case (true, false, true, false):
                newMoveDirection = MoveDirection.FL;
                break;
            case (true, false, false, true):
                newMoveDirection = MoveDirection.FR;
                break;
            case (false, true, true, false):
                newMoveDirection = MoveDirection.BL;
                break;
            case (false, true, false, true):
                newMoveDirection = MoveDirection.BR;
                break;
            default:
                newMoveDirection = MoveDirection.N;
                break;
        }

        moveDirection = newMoveDirection;
        playerStatus.moveDirection = newMoveDirection;

    }


    void Update()
    {
        SetMoveDirection();
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
            if (moveDirection != MoveDirection.N)
            {
                movePosition = transform.right * x + transform.forward * z;
            } else {
                movePosition = movePosition / 1.1f;
            }

            //controller.Move(movePosition * playerStatus.speed * Time.deltaTime);


            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                playerGravity.y = Mathf.Sqrt(jumpHight * -2 * gravity);
            }

            playerGravity.y += gravity * Time.deltaTime;
            controller.Move((playerGravity * Time.deltaTime) + (movePosition * playerStatus.speed * Time.deltaTime));
            //gameObject.transform.position.Set(transform.position.x, 0, transform.position.z);
        }
    }

}

public enum MoveDirection
{
    N,

    F,
    B,
    L,
    R,

    FR,
    FL,
    BR,
    BL,
}
