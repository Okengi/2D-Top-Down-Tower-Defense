using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Enemy target;
    float damage = 10f;

    public void SetTarget(Enemy enemy) { target = enemy; }
    [SerializeField]
    float speed = 0.1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, speed / 10);
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Enemy")
		{
            collision.GetComponent<Enemy>().Damage(damage);
			Destroy(this.gameObject);
		}
	}
}
