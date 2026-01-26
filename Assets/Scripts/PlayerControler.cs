using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;
    private int IdSpeed;
    [SerializeField] private  float speed = 5f;
    private int direction = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_animator = GetComponent<Animator>();
        IdSpeed = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        SetAnimatorValues();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
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
        
    }
}
