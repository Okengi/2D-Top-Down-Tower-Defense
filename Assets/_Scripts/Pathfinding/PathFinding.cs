using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
	public static PathFinding instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else { Destroy(gameObject); }
	}
	public List<Grid> GetNeighborGrids(Grid grid)
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

	public List<Tile> GetNeighborTiles(Tile tile, List<TileType> dontSearchfor)
	{
		List<Tile> neighbors = new List<Tile>();
		Vector2Int tileMatrixPos = new Vector2Int(tile.GetXPosition(), tile.GetYPosition());

		List<Vector2Int> directions = new List<Vector2Int>
		{
			new Vector2Int(0,1),
			new Vector2Int(1,0),
			new Vector2Int(-1,0),
			new Vector2Int(0,-1),
		};

		foreach (Vector2Int dir in directions)
		{

			Tile neighbor = TileManager.Instance.GetTile(tileMatrixPos + dir);
			if (neighbor != null)
			{
				if (dontSearchfor.Contains(neighbor._tileType)) { continue; }
				neighbors.Add(neighbor);
			}
		}
		if (tileMatrixPos == new Vector2(1, 3) || tileMatrixPos == new Vector2(3, 1) || tileMatrixPos == new Vector2(5, 3)|| tileMatrixPos == new Vector2(3, 5))
		{
			neighbors.Add(TileManager.Instance.GetCastle());
		}

		return neighbors;
	}
	public List<Tile> ShortesPath_ToCastle(Tile start)
	{
		
		return AStarPathFindingAlgorthem(start);
	}
	private List<Tile> openList;
	private List<Tile> closedList;
	int straightTravelCost = 10;
	int diagonelTravelCost = 14;

	private List<Tile> AStarPathFindingAlgorthem(Tile startTile)
	{
		//Debug.Log($"StartNode: {startTile.name} {startTile._positionInTileMatrix}");
		
		Tile startNode = startTile;
		Tile endNode = TileManager.Instance.GetCastle();
		//Debug.Log($"EndNode: {endNode.name} {endNode._positionInTileMatrix}");

		openList = new List<Tile> { startNode };
		closedList = new List<Tile>();

		foreach (KeyValuePair<Vector2Int, Tile> entry in TileManager.Instance.GetTileMatrix())
		{
			Tile tile = entry.Value;
			tile.gCost = float.MaxValue;
			tile.CalculateFCost();
			tile.previousTile = null;
		}

		startNode.gCost = 0;
		startNode.hCost = CalculateDistance(startTile, endNode);
		startNode.CalculateFCost();
		int i = 0;

		while (openList.Count > 0)
		{
			i++;
			Tile currentTile = CalculateLowestFCostNode(openList);
			if (currentTile == endNode)
			{
				return CalculatePath(endNode);
			}
			if (i > 1000)
			{
				Debug.Log("<color=red>---------Abruch-------</color>");
				break;
			}

			openList.Remove(currentTile);
			closedList.Add(currentTile);

			foreach (Tile neigbore in GetNeighborTiles(currentTile, new List<TileType> { TileType.Mouinten}))
			{
				if (closedList.Contains(neigbore)) 
				{
					continue; 
				}
				float tentativGCost = currentTile.gCost + CalculateDistance(currentTile, neigbore);
				
				if (tentativGCost < neigbore.gCost) {
					neigbore.previousTile = currentTile;
					neigbore.gCost = tentativGCost;
					neigbore.hCost = CalculateDistance(neigbore, endNode);
					neigbore.CalculateFCost();

					if(!openList.Contains(neigbore))
					{
						openList.Add(neigbore);
					}
				}
			}

		}
		return null;
	}

	private List<Tile> CalculatePath(Tile endTile)
	{
		List<Tile> path = new List<Tile>();
		path.Add(endTile);
		Tile currentTile = endTile;
		while (currentTile.previousTile != null) {
			path.Add(currentTile.previousTile);
			currentTile = currentTile.previousTile;
		}
		path.Reverse();
		return path;
	}

	private float CalculateDistance(Tile start, Tile end)
	{
		float x = Mathf.Abs(start.GetXPosition() - end.GetXPosition());
		float y = Mathf.Abs(start.GetYPosition() - end.GetYPosition());
		float remaining = Mathf.Abs(x - y);
		return diagonelTravelCost * Mathf.Min(x, y) + straightTravelCost * remaining;
	}
	
	private Tile CalculateLowestFCostNode(List<Tile> tileList)
	{
		Tile lowestFCostTile = tileList[0];
		foreach (Tile tile in tileList)
		{
			if (tile.fCost < lowestFCostTile.fCost)
			{
				lowestFCostTile = tile;
			}
		}
		return lowestFCostTile;
	}
}
