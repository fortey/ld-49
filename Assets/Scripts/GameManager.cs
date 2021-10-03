using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SimpleHero hero;
    public PlayableDirector timeline;
	public PlayableDirector climbTimeline;

	void Start()
    {
        hero = FindObjectOfType<SimpleHero>();
    }



    public void StartGame()
	{
        timeline.Play();
        hero.canMove = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<SimpleHero>())
		{
			
			hero.canMove = false;
			FindObjectOfType<CameraFollow>().follow = false;
			climbTimeline.Play();

			//SceneManager.LoadScene(1);
		}
	}

	public void EndLevel()
	{
		SceneManager.LoadScene(1);
	}
}
