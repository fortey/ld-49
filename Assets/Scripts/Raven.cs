using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : MonoBehaviour
{
    private Hero hero;
    private Transform heroTrasform;
    private Animator anim;

    [SerializeField] private float AtackDistance=3f;
    [SerializeField] private float AtackCoolDown = 3f;
    [SerializeField] private float ofssetY = 1f;
    [SerializeField] private float speed = 3f;

    void Start()
    {
        hero = FindObjectOfType<Hero>();
        heroTrasform = hero.GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var target = new Vector2(heroTrasform.position.x+AtackDistance, heroTrasform.position.y + ofssetY);
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }

	private void FixedUpdate()
	{
        
	}
}
