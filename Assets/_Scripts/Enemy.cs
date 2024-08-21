using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	Vector2 gridPos = Vector2.zero;
	Vector2 tilePos = Vector2.zero;

	Grid activeGrid = null;
	List<Grid> gridsToCastle = null;

	public void Initialize(Grid grid)
	{
		activeGrid = grid;
	}

	public void MoveOneTileForward()
	{	
		gridsToCastle = Dijkstra.instance.Dijkstra_ShortesPath_ToCastle(activeGrid);
		if (gridsToCastle == null ) { return; }
		Grid nextGrid = gridsToCastle[0];
		foreach (Grid grid in gridsToCastle)
		{
			Debug.Log($"<color=red>Grids to caste{grid._pointInMatrix} from EnemyScript</color>");
		}
		Vector2 pos = nextGrid.GetLastRoadTile()._position;

		transform.position = nextGrid._pointInMatrix * 7 + pos;
	}
}