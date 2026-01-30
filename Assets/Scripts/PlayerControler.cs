using System;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //PLAYER COMPONENTS
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;
    private AudioSource jumpSound;

    [Header("Move and Jump Settings")]

    [SerializeField] private  float speed = 5f;
    private int direction = 1;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float extraJumps;
    [SerializeField] private float counterExtraJumps;
    private int IdSpeed;
    
    [Header("Ground Check Settings")]
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
    private int idIsGrounded;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
        IdSpeed = Animator.StringToHash("Speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        lFoot = GameObject.Find("LFoot").GetComponent<Transform>();
        rFoot = GameObject.Find("RFoot").GetComponent<Transform>();
        jumpSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        SetAnimatorValues();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
        CheckGround();
    }

    private void Move()
    {
        Flip();
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, m_rigidbody2D.linearVelocityY);
    }

    private void Flip()
    {
        if(m_gatherInput.ValueX * direction < 0)    
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, 1,1);
            direction *= -1;
        }
       
    }

    private void SetAnimatorValues()
    {
        m_animator.SetFloat(IdSpeed, Math.Abs(m_rigidbody2D.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
    }

    private void Jump()
    {
        if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                jumpSound.Play();
            if (counterExtraJumps > 0 && !isGrounded)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                counterExtraJumps--;
            }
        }
        m_gatherInput.IsJumping = false;
    }

    private void CheckGround()
    {
        RaycastHit2D lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);

        if (lFootRay || rFootRay)
        {
            counterExtraJumps = extraJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
