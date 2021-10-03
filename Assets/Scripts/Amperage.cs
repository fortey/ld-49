using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amperage : MonoBehaviour
{
    [SerializeField] private float speed = -2f;
	public Transform leftPoint;
	public Transform rightPoint;

	private bool facingRight;
	private Transform trform;
	public int wire;

	private void Start()
	{
		trform = transform;
	}
	void Update()
    {
        trform.Translate(speed * Time.deltaTime * trform.right);

			if(rightPoint.position.x< trform.position.x && facingRight || leftPoint.position.x > trform.position.x && !facingRight)
			{
				Flip();
			}
    }

	private void Flip()
	{
		facingRight = !facingRight;
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		speed *= -1;
	}

	public void Init(int wire)
	{
		this.wire = wire;
		GetComponent<SpriteRenderer>().sortingOrder = wire+1;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Hero>())
		{
			collision.SendMessage("Damage");
		}
	}
}
