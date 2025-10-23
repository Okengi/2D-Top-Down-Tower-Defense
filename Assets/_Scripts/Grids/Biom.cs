using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public abstract class Biom
{

	protected String name;
	protected int id;
	protected Tile[,] tiles;
	protected Vector2 _gridPos;
	protected int width, height;
	protected GridType _gridType;

	protected TileType road;
	protected TileType boarder;
	protected TileType ground;
	
	public Biom(int id, Vector2 _gridPos, int width, int height, GridType gridTyp, TileType road, TileType boarder, TileType ground) 
	{
		this.id = id;
		this._gridPos = _gridPos;
		this.width = width;
		this.height = height;
		this._gridType = gridTyp;
		this.road = road;
		this.boarder = boarder;
		this.ground = ground;
		tiles = new Tile[width, height];
		name = typeof(This).Name + id;
		InitializeGrid();
	}
	public Biom(int id, Vector2 _gridPos, int width, int height, GridType gridTyp)
	{
		this.id = id;
		this._gridPos = _gridPos;
		this.width = width;
		this.height = height;
		this._gridType = gridTyp;
		name = typeof(This).Name + id;
		tiles = new Tile[width, height];
		InitializeGrid();
	}

	public Vector2 GridPos{ get { return _gridPos; } }

	protected virtual void InitializeGrid()
	{
		
		ForEachTile((x, y) =>
		{
		TileType tileType = GetTileTyp(x, y);
		int xPos = x + (int)_gridPos.x * width;
		int yPos = y + (int)_gridPos.y * height;
			
		TileManager.Instance.SpawnTile(tileType, xPos, yPos, id);
		tiles[x,y] = TileManager.Instance.GetTile(new Vector2Int(xPos, yPos));
		});
		
	}
	protected virtual TileType GetTileTyp(int x, int y)
	{
		TileType tileType;
		switch (_gridType)
		{
			case GridType.Horizontal:
				tileType = (y == height / 2) ? road : (y == 0 || y == height - 1) ? boarder : ground;
				break;
			case GridType.Vertical:
				tileType = (x == width / 2) ? road : (x == 0 || x == width - 1) ? boarder : ground;
				break;
			case GridType.TopLeft:
				tileType = (y == height / 2 && x <= width / 2 || x == width / 2 && y > height / 2) ? road : (y == 0 || x == width - 1) ? boarder : ground;
				break;
			case GridType.TopRight:
				tileType = (y == height / 2 && x >= width / 2 || x == width / 2 && y > height / 2) ? road : (y == 0 || x == 0) ? boarder : ground;
				break;
			case GridType.BottomLeft:
				tileType = (y == height / 2 && x <= width / 2 || x == width / 2 && y < height / 2) ? road : (y == height - 1 || x == width - 1) ? boarder : ground;
				break;
			case GridType.BottomRight:
				tileType = (y == height / 2 && x >= width / 2 || x == width / 2 && y < height / 2) ? road : (y == height - 1 || x == 0) ? boarder : ground;
				break;
			case GridType.BottomLeftRight:
				if (x <= 2 && (y <= 2 && y != 0)) tileType = ground;
				else if (x > 3 && (y <= 2 && y != 0)) tileType = ground;
				else if (y == 3) tileType = road;
				else if (x == 3 && y < 3) tileType = road;
				else if (y == 4 || y == 5) tileType = ground;
				else if (0 < x && x < 3 && y == 0) tileType = ground;
				else if (y == 0 && x > 3 && x < 6) tileType = ground;
				else tileType = boarder;
				break;
			case GridType.TopLeftRight:
				if (x <= 2 && (y >= 4 && y != 6)) tileType = ground;
				else if (x > 3 && (y >= 4 && y != 6)) tileType = ground;
				else if (y == 3) tileType = road;
				else if (x == 3 && y > 3) tileType = road;
				else if (y == 1 || y == 2) tileType = ground;
				else tileType = boarder;
				break;
			case GridType.LeftTopBottom:
				if (x == 0 && y != 3 || x == 6) tileType = boarder;
				else if (x == 3) tileType = road;
				else if (y == 3 && x < 4) tileType = road;
				else tileType = ground;
				break;
			case GridType.RightTopBottom:
				if (x == 0 || y != 3 && x == 6) tileType = boarder;
				else if (x == 3) tileType = road;
				else if (y == 3 && x > 3) tileType = road;
				else tileType = ground;
				break;
			default:
				tileType = ground;
				break;
		}
		return tileType;
	}
	protected void ForEachTile(System.Action<int, int> action)
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				action(x, y);
			}
		}
	}


	public bool HasConnectionToRight()
	{
		Tile tile = GetTileAtPosition(new Vector2(width - 1, height / 2));
		bool b = tile != null && tile.GetType() == typeof(RoadTile);
		//Debug.Log($"Grid: {_gridPos} Has connection to right: {b}");
		return b;
	}
	public bool HasConnectionToLeft()
	{
		Tile tile = GetTileAtPosition(new Vector2(0, height / 2));
		bool b = tile != null && tile.GetType() == typeof(RoadTile);
		//Debug.Log($"Grid: {_gridPos} Has connection to left: {b}");
		return b;
	}
	public bool HasConnectionToTop()
	{
		Tile tile = GetTileAtPosition(new Vector2(width / 2, height - 1));
		bool b = tile != null && tile.GetType() == typeof(RoadTile);
		//Debug.Log($"Grid: {_gridPos} Has connection to top: {b}");
		return b;
	}
	public bool HasConnectionToBottom()
	{
		Tile tile = GetTileAtPosition(new Vector2(width / 2, 0));
		bool b = tile != null && tile.GetType() == typeof(RoadTile);
		//Debug.Log($"Grid: {_gridPos} Has connection to bottom: {b}");
		return b;
	}
	public Tile GetTileAtPosition(Vector2 position)
	{
		return tiles[(int)position.x, (int)position.y];
	}

	public GridType GetGridType() { return _gridType; }
}