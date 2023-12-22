using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerspeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationSpeed = 10f ;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;
    // Start is called before the first frame update

   public Animator animator;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this; 
    }


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        switch (CheackWinner.instrace.isWinner)
        {
            case true:
                animator.SetBool("victory", CheackWinner.instrace.isWinner);
                break;
            case false:
                Movement();
                break;
        }
    }
    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if (characterController.isGrounded && playerVelocity.y < -2f) 
        {
            playerVelocity.y = -1f;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0)
        *new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;

        characterController.Move(movementDirection*playerspeed*Time.deltaTime);

        if(movementDirection != Vector3.zero) 
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotation,rotationSpeed * Time.deltaTime);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        groundedPlayer = characterController.isGrounded;

        if(Input.GetButtonDown("Jump")&&groundedPlayer) 
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetTrigger("jumping");
        
        }
        animator.SetFloat("speed",Mathf.Abs(movementDirection.x)+Mathf.Abs(movementDirection.z));
        animator.SetBool("ground",characterController.isGrounded);
       
    }
}
