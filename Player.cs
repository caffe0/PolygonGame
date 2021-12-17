using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variaveis
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    public Transform cam;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
   




    private Vector3 moveDir;
    private Vector3 velocity;


    //referencias

    public CharacterController controller;
    private void Start()
    {
        controller = GetComponent<CharacterController>();


    }

    private void Update()
    {

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDir = new Vector3(moveX, 0, moveZ);


        if (isGrounded)
        {
            if (moveDir != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                //ta andando
                moveSpeed = walkSpeed;
            }
            else if (moveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                //ta correndo
                moveSpeed = runSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            moveDir *= moveSpeed;
        }

        controller.Move(moveDir * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(moveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 DirectionMove = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(DirectionMove * moveSpeed * Time.deltaTime);
        }
    }


    private void Idle()
    {

    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
