using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnBiom
{

	[SerializeField] private EnemyGroundTile enemyGroungTilePrefap;
	private Vector2 _gridPos;
	private EnemyGroundTile[,] tiles;
	private int width;
	private int height;
	public EnemySpawnBiom(Vector2 _gridPos, int width, int height)
	{
		this._gridPos = _gridPos;
		this.width = width;
		this.height = height;
		tiles = new EnemyGroundTile[width, height];
		InitializeGrid();
	}

	public void InitializeGrid()
	{
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int xPos = x + (int)_gridPos.x;
				int yPos = y + (int)_gridPos.y;
				TileManager.Instance.SpawnTile(TileType.EnemyGround, xPos, yPos);
				tiles[x, y] = (EnemyGroundTile)TileManager.Instance.GetTile(new Vector2Int(xPos, yPos));
			}
		}
	}
	public void MoveTo(Vector2 gridPos)
	{
		this._gridPos = gridPos;
		UpdateLocation();
	}

	public void Hide()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int xPos = x + (int)(_gridPos.x * 1000);
				int yPos = y + (int)(_gridPos.y * 1000);
				tiles[x, y].Move(new Vector2Int(xPos, yPos));
			}
		}
	}

	public void SelfDestroy()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				tiles[x, y].SelfDestroy();
			}
		}
	}

	public void Show()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int xPos = x + (int)_gridPos.x;
				int yPos = y + (int)_gridPos.y;
				tiles[x, y].Show();
			}
		}
	}

	private void UpdateLocation() 
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int xPos = x + (int)(_gridPos.x * width);
				int yPos = y + (int)(_gridPos.y * width);
				tiles[x, y].Move(new Vector2Int(xPos, yPos));
			}
		}
	}
}
