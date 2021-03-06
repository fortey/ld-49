using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : MonoBehaviour
{
    private Hero hero;
    private Transform heroTrasform;
    private Animator anim;
    private AudioSource audios;

    [SerializeField] private float AtackDistance=3f;
    [SerializeField] private float AtackCoolDown = 3f;
    [SerializeField] private float ofssetY = 1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private bool isAlive = true;

    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private AudioClip DamageSound;

    private readonly int attack_anim = Animator.StringToHash("attack");

    void Start()
    {
        hero = FindObjectOfType<Hero>();
        heroTrasform = hero.GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            var target = new Vector2(heroTrasform.position.x + AtackDistance, heroTrasform.position.y + ofssetY);
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);

            if (attackTime > 0)
                attackTime -= Time.deltaTime;

            if (attackTime <= 0 && Vector2.Distance(transform.position, target) < 0.4f)
            {
                attackTime = AtackCoolDown;
                Invoke(nameof(Attack), 0.4f);
            }
        }
    }


    public void Attack()
	{
        if (isAlive)
        {
            anim.SetTrigger(attack_anim);

            hero.SendMessage("Damage");
            audios.clip = AttackSound;
            audios.Play();
        }
	}

    public void Damage()
	{
        isAlive = false;
        audios.clip = DamageSound;
        audios.Play();
	}
}
