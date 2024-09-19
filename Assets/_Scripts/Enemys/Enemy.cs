using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private Tile curentlyOccupiedTile;
	private List<Tile> path;
	public float moveSpeed = 1f;
	bool reachedCastle = false;

	public void Initialize(Tile tile)
	{
		curentlyOccupiedTile = tile;
		Debug.Log("Bevore Pathfinding Begins");
		path = PathFinding.instance.ShortesPath_ToCastle(curentlyOccupiedTile);

		/*foreach (Tile t in path)
		{
			Debug.Log($"<color=green>{t.GetName()}| {t._positionInTileMatrix}</color>");
		}*/
	}


	private void FixedUpdate()
	{
		Move();

	}

	public void Move()
	{
		if (path != null && !reachedCastle)
		{
			if (transform.position == new Vector3(path[0].GetXPosition(), path[0].GetYPosition(), 0)) { path.RemoveAt(0); }
			transform.position = Vector3.Lerp(transform.position, new Vector3(path[0].GetXPosition(), path[0].GetYPosition(), 0), moveSpeed / 10);
			if (path.Count == 1 ) { reachedCastle = true; }
		}
		
	}
}