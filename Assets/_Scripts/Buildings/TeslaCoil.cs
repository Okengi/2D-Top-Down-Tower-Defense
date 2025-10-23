using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : Building
{
	[SerializeField] private GameObject ballAttackSprite;
	List<Enemy> enemyList;
	float timer = 3f;
	private void Start()
	{
		enemyList = new List<Enemy>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Enemy")
		{
			enemyList.Add(collision.GetComponent<Enemy>());
			Debug.Log("Enemy detacted");
		}
	}

	private void FixedUpdate()
	{
		timer -= Time.deltaTime;
		if (timer < 0)
		{
			foreach (Enemy enemy in enemyList)
			{
				Shoot(enemy);
			}
			timer = 3f;
		}
	}

	private void Shoot(Enemy enemy)
	{
		var ball = Instantiate(ballAttackSprite, transform.position, Quaternion.identity);
		ball.GetComponent<Ball>().SetTarget(enemy);
	}
}
