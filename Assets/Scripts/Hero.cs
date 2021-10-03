using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public event Action<float> OnBalanceChanged;
    public event Action OnDamaged;

    [Header("Movement")]
    public float speed = 2f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float switchWireForce = 3000f;
    [SerializeField] private LayerMask whatIsGround;                     
    [SerializeField] private Transform groundCheck;
    //[Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;
    const float groundedRadius = 0.2f;
    private bool grounded;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;

    [Space]
    public int currentWire = 2;

    [Header("Attack")]
    [SerializeField] private float AtackCoolDown = 3f;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private Transform targetChecker;

    [Header("Balancing")]
    [SerializeField] private bool balancing = true;
    public float Balance { get=>balance;
        set {
            if ((state == State.OnWire || state == State.Waving)&&balancing)
            {
                balance = Mathf.Clamp(value, -1f, 1f);
                OnBalanceChangerHandler();
            }
        } }
    [SerializeField] private float balance;
    
    public float balanceScale = 1.01f;
    public AnimationCurve impulseCurve;
    public float impulse = 0.001f;

    
    private Rigidbody2D rb;
    private float horizontalInput;
    private State state = State.OnWire;
    private SpriteRenderer spriteRenderer;

    private Animator anim;
    private readonly int balanceCode = Animator.StringToHash("balance");
    private readonly int speedCode = Animator.StringToHash("speed");
    private readonly int speedYCode = Animator.StringToHash("speedY");
    private readonly int jumpCode = Animator.StringToHash("jump");
    private readonly int onGroundCode = Animator.StringToHash("onGround");
    private readonly int damageCode = Animator.StringToHash("damage");
    private readonly int attackCode = Animator.StringToHash("attack");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if(state == State.OnWire || state == State.Waving)
		{
            Balancing();
		}
        if (state == State.Waving && Mathf.Abs(balance) < 0.45f)
            state = State.OnWire;

        if(state == State.OnWire || state == State.Jumping)
		{
            Move(horizontalInput*Time.deltaTime*10f, Input.GetKeyDown(KeyCode.Space), Input.GetKeyDown(KeyCode.W), Input.GetKeyDown(KeyCode.S));
		}
        if (state == State.Jumping)
        {
            //anim.SetFloat(speedYCode, rb.velocity.y);
        }

        if (attackTime > 0)
            attackTime -= Time.deltaTime;

        if (state == State.OnWire && Input.GetKeyDown(KeyCode.E))
		{
            Attack();
		}
    }

	private void FixedUpdate()
	{
        //var currentSpeed = Vector2.zero;
        //if(state==State.OnProvod)
        //    currentSpeed = new Vector2(horizontalInput * speed, 0);
        //rb.velocity = currentSpeed;
        //anim.SetFloat(speedCode, currentSpeed.x);

		if (balance == 0)
		{
            balance = UnityEngine.Random.Range(-0.05f, 0.05f);
		}
        if(Mathf.Abs(balance) >= 0.45)
            state = State.Waving;

        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded) {
                    state = State.OnWire;
                    anim.SetTrigger(onGroundCode);
                }
            }
        }
    }

    public void Move(float move, bool jump, bool switchUp, bool swithDown)
    {
        if (grounded|| state == State.Jumping)
        {
            Vector3 targetVelocity = new Vector2(move * speed, rb.velocity.y);
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
            rb.velocity = targetVelocity;
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

        if (grounded && state==State.OnWire && jump)
        {
            grounded = false;
            state = State.Jumping;
            rb.AddForce(new Vector2(0f, jumpForce));
            anim.SetTrigger(jumpCode);
        }

        if (grounded && state == State.OnWire && switchUp && currentWire>0)
        {
            grounded = false;
            state = State.Jumping;
            ChangeWire(currentWire - 1, switchWireForce);
            anim.SetTrigger(jumpCode);
        }

        if (grounded && state == State.OnWire && swithDown && currentWire < 2)
        {
            grounded = false;
            state = State.Jumping;
            ChangeWire(currentWire +1, switchWireForce/2);
            anim.SetTrigger(jumpCode);
        }
    }

    private void Balancing()
	{
        Balance *= balanceScale;

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
            StartCoroutine(AddImpulseToBalance(-1f));
		}
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(AddImpulseToBalance(1f));
        }
    }

    private IEnumerator AddImpulseToBalance(float value)
    {
        var time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            Balance += impulseCurve.Evaluate(time) * value * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void ChangeWire(int newWire, float force)
	{
        rb.AddForce(new Vector2(0f, force));
        Wires.instance.ChangeWire(currentWire, newWire);
        currentWire = newWire;
        spriteRenderer.sortingOrder = newWire;
    }

    private void OnBalanceChangerHandler()
	{
        OnBalanceChanged?.Invoke(Balance);
        anim.SetFloat(balanceCode, Mathf.Abs(Balance));
        if(Balance==1f || Balance == -1f)
		{
            state = State.Lose;
            Wires.instance.TurnOffAll();
            GetComponent<AudioSource>().Play();
            FindObjectOfType<Level2>().SendMessage("InvokeLose");
		}
    }

    public void Damage()
	{
        anim.SetTrigger(damageCode);
        OnDamaged?.Invoke();
	}

    private void Attack()
	{
		if (attackTime <= 0)
		{
            attackTime = AtackCoolDown;
            anim.SetTrigger(attackCode);

        }
	}

    public void AttackEnd()
	{
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetChecker.position, 0.2f);
        for (int i = 0; i < colliders.Length; i++)
        {
            var raven = colliders[i].GetComponent<Raven>();
            if (raven)
            {
                raven.Damage();
                Destroy(raven.gameObject, 0.5f);
            }
        }
    }
}

public enum State { OnGround, OnWire, Waving, Jumping, Lose}