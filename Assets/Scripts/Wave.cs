using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public GameObject[] enemy;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Hero>())
		{
			foreach(var en in enemy)
			{
				en.SetActive(true);
			}
		}

		Destroy(this);
	}
}
