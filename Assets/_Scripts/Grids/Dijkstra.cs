using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
	public static Dijkstra instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else { Destroy(gameObject); }
	}
	public List<Grid> GetNeighbors(Grid grid)
	{
		List<Grid> neighbors = new List<Grid>();
		Vector2 gridPos = grid._pointInMatrix;

		List<Vector2> directions = GridTypManager.instance.GetDirectionsOfType(grid.GetGridType());

		foreach (Vector2 dir in directions)
		{
			Grid neighbor = GridManager.instance.GetGrid(gridPos + dir);
			if (neighbor != null)
			{
				neighbors.Add(neighbor);
			}
		}

		return neighbors;
	}
	public List<Grid> Dijkstra_ShortesPath_ToCastle(Grid start)
	{
		List<int> cost = new List<int>();
		List<Grid> Neigbores = GetNeighbors(start);

		return null;
	}
	public int GetTransitionCost(Grid from, Grid to)
	{
		return 1;
	}
}
