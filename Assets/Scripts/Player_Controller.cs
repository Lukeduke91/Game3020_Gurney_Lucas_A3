using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    [Header("Movment")]
    public float MoveSpeed;
    public float groundDrag;
    public float PlayerHeight;
    public LayerMask WhatIsGround;
    public bool grounded;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Advance Movement")]
    public float DashForce;
    public bool canIDash;
    public KeyCode DashKey = KeyCode.E;
    public float stompForce;
    public int stompDamage;
    KeyCode stompKey = KeyCode.LeftShift;
    KeyCode ChargedJumpKey = KeyCode.F;
    public float chargeCooldown = 3f;
    public bool stompKeyPressed = false;

    public GameObject trail;
    public GameObject StompEffect;
    private float stayingEffect;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public KeyCode jumpState = KeyCode.Space;

    public Transform orientation;
    // Update looking around so that no matter what the player is using, they have equal sensitivity.
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround);

        MyInput();

        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        

        if(Input.GetKey(jumpState) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJD), jumpCooldown);
        }
        if(Input.GetKey(DashKey) && canIDash && grounded || Input.GetKey(DashKey) && canIDash && !grounded)
        {
            canIDash = false;
            rb.AddForce(orientation.forward * DashForce, ForceMode.Impulse);
            Invoke(nameof(ResetJD), chargeCooldown);
        }
        //else if(Input.GetKey(DashKey) && canIDash && !grounded)
        //{
        //    canIDash = false;
        //    rb.AddForce(orientation.forward * DashForce, ForceMode.Impulse);
        //    Invoke(nameof(ResetJD), 10f);
        //}
        //else if (!canIDash && grounded)
        //{
        //    canIDash = true;
        //}
        if (Input.GetKey(stompKey) && !grounded)
        {
            stompKeyPressed = true;
            rb.AddForce(orientation.up * -stompForce, ForceMode.Impulse);
            trail.SetActive(true);
            stayingEffect = 0.5f;
        }
        else
        {
            stayingEffect -= Time.deltaTime;
            stompKeyPressed = false;
            StompEffect.SetActive(true);
            if (stayingEffect <= 0f)
            {
                StompEffect.SetActive(false);
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * MoveSpeed * 20.0f, ForceMode.Impulse);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80, ForceMode.Force);
            }
        }

        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);
            trail.SetActive(false);
            
        }

        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if(OnSlope())
        {
            if(rb.velocity.magnitude > MoveSpeed)
            {
                rb.velocity = rb.velocity.normalized * MoveSpeed;
            }
        }

        else 
        { 
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if(flatVel.magnitude > MoveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * MoveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (Input.GetKey(ChargedJumpKey) && grounded)
        {
            rb.AddForce(transform.up * jumpForce * 2, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void ResetJD()
    {
        readyToJump = true;
        canIDash = true;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, PlayerHeight * 0.5f + 0.3f))
        {
            float angles = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angles < maxSlopeAngle && angles != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Basic_Enemy>() != null && stompKeyPressed)
        {
            Basic_Enemy enemy = collision.gameObject.GetComponent<Basic_Enemy>();
            enemy.TakeDamage(stompDamage);
            stompKeyPressed = false;
            StompEffect.SetActive(true);
            if (stayingEffect <= 0f)
            {
                StompEffect.SetActive(false);
            }
        }
        else if (collision.gameObject.GetComponent<MovingEnemyAI>() != null && stompKeyPressed)
        {
            MovingEnemyAI movingEnemyAI = collision.gameObject.GetComponent<MovingEnemyAI>();
            movingEnemyAI.TakeDamage(stompDamage);
            stompKeyPressed = false;
            StompEffect.SetActive(true);
            if (stayingEffect <= 0f)
            {
                StompEffect.SetActive(false);
            }
        }
        else if(collision.gameObject.CompareTag("Exit"))
        {
            Debug.Log("works");
            SceneManager.LoadScene("EndScreen");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
