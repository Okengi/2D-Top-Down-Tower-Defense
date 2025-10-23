using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawnBiom : Biom
{
	public EnemySpawnBiom(int id, Vector2 _gridPos, int width, int height, GridType gridType)
		: base(id,_gridPos, width, height, gridType)
	{

	}
	protected override TileType GetTileTyp(int x, int y)
	{
		return TileType.Enemy;
	}

	protected override void InitializeGrid()
	{
		ForEachTile((x, y) =>
		{
			int xPos = x + (int)(_gridPos.x) * width;
			int yPos = y + (int)(_gridPos.y) * height;
			TileManager.Instance.SpawnTile(TileType.Enemy, xPos, yPos, id);
			tiles[x, y] = TileManager.Instance.GetTile(new Vector2Int(xPos, yPos), false);
		});
	}
	public void MoveTo(Vector2 gridPos)
	{
		this._gridPos = gridPos;
		ForEachTile((x, y) =>
		{
			int xPos = x + (int)(_gridPos.x * width);
			int yPos = y + (int)(_gridPos.y * width);
			tiles[x, y].Move(new Vector2Int(xPos, yPos));
		});
	}

	public void Show()
	{
		ForEachTile((x, y) =>
		{
			int xPos = x + (int)(_gridPos.x);
			int yPos = y + (int)(_gridPos.y);
			tiles[x, y].Move(new Vector2Int(xPos, yPos));
		});
	}

	public void Hide()
	{
		ForEachTile((x, y) =>
		{
			int xPos = x + (int)(_gridPos.x * 1000);
			int yPos = y + (int)(_gridPos.y * 1000);
			tiles[x, y].Move(new Vector2Int(xPos, yPos));
		});
	}

	public void SelfDestroy()
	{
		ForEachTile((x, y) => tiles[x, y].SelfDestroy());
	}

	public void SpawnEnemys(GameObject monster, int count = 1)
	{
		int x = 3;
		int y = 6;
		tiles[x, y].Spawn(monster);
	}
}
