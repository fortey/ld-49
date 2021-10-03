using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHero : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove;
    public float speed = 2f;
    //[Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
    private Rigidbody2D rb;
    private float horizontalInput;

    private Animator anim;
    private readonly int speedCode = Animator.StringToHash("speed");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        Move(horizontalInput * Time.deltaTime * 10f);
        
}

    public void Move(float move)
    {
        if (canMove)
        {
            Vector3 targetVelocity = new Vector2(move * speed, rb.velocity.y);
            rb.velocity = targetVelocity;//Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
            anim.SetFloat(speedCode, Mathf.Abs(rb.velocity.x));
        }
        else
        {
            anim.SetFloat(speedCode, 0f);
        }
    
    }

    private void Flip()
    {
        facingRight = !facingRight;

        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
