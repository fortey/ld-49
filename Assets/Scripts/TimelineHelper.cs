using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineHelper : MonoBehaviour
{
    public void EndOfClimb()
	{
		FindObjectOfType<GameManager>().EndLevel();
	}
}
