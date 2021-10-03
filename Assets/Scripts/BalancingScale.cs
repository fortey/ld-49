using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalancingScale : MonoBehaviour
{
    public RectTransform arrow;
    
    void Start()
    {
        FindObjectOfType<Hero>().OnBalanceChanged += OnBalanceChanged;
    }

    public void OnBalanceChanged(float value)
	{
        arrow.localPosition = new Vector3(value * 450, 0, 0);
	}

	private void OnDestroy()
	{
        var hero = FindObjectOfType<Hero>();
        if(hero)
            hero.OnBalanceChanged -= OnBalanceChanged;
    }
}
