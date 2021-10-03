using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2 : MonoBehaviour
{
	public GameObject FinishPanel;
	public GameObject LosePanel;
	public RectTransform LivesPanel;

	public int Lives;

	const int MaxLives = 3;

	private void Start()
	{
		FindObjectOfType<Hero>().OnDamaged += OnDamage;
		Lives = MaxLives;
	}
	public void FinishGame()
	{
		FinishPanel.SetActive(true);
		Time.timeScale = 0f;
	}

	public void Restart(int i)
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(i);
	}

	void OnDamage()
	{
		Lives--;
		RefreshLives();
		if (Lives <= 0)
		{
			Invoke(nameof(Lose), .5f);
		}
	}
	public void RefreshLives()
	{
		for (int i = 0; i < MaxLives; i++)
		{
			LivesPanel.GetChild(i).gameObject.SetActive(i < Lives);
		}
	}

	public void Lose()
	{
		LosePanel.SetActive(true);
		Time.timeScale = 0f;
	}

	public void InvokeLose()
	{
		Invoke(nameof(Lose), 1f);
	}
}
