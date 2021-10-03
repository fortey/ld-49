using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimation : MonoBehaviour
{
    private Hero hero;

    void Start()
    {

        hero = GetComponentInParent<Hero>();
    }

    public void Attack()
	{
        hero.AttackEnd();
	}
}
