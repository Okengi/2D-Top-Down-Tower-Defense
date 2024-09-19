using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
   public static TileManager Instance { get; private set; }

	[SerializeField] private EnemyGroundTile enemyGroundTilePrefap;

	private Dictionary<Vector2Int, Tile> _allTilesMatrix = new Dictionary<Vector2Int, Tile>(); 

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	public void Remove(Vector2Int pos)
	{
		_allTilesMatrix.Remove(pos);
	}

	public void AddTileToMatix(Tile tile, Vector2Int tilePosInMatrix)
	{
		if(tile._tileType == TileType.Castle)
		{
			SetUpCastleTile(tile);
			return;
		}
		_allTilesMatrix.Add(tilePosInMatrix, tile);
	}

	public void SpawnTile(TileType tileTyp, int x, int y)
	{
		
		Tile spawnedTile = Instantiate(enemyGroundTilePrefap, new Vector3(x, y), Quaternion.identity);
		spawnedTile.transform.parent = transform;

		spawnedTile.Init(x, y);

		_allTilesMatrix[new Vector2Int(x, y)] = spawnedTile;
	}

	public void MoveTileFromTo(Vector2Int from, Vector2Int to)
	{
		if (_allTilesMatrix.TryGetValue(from, out Tile tile))
		{
			_allTilesMatrix.Remove(from);

			if (_allTilesMatrix.ContainsKey(to))
			{
				// Option 1: Replace the existing tile at "to"
				Debug.Log($"The space {to} is ocupide and will be replaced");
				Destroy(_allTilesMatrix[to]);
				_allTilesMatrix.Remove(to);

				_allTilesMatrix.Add(to, tile);
			}
			else
			{
				_allTilesMatrix.Add(to, tile);
			}
		}
		else
		{
			Debug.LogError($"No tile found at position {from}.");
		}
	}

	private void SetUpCastleTile(Tile tile)
	{
		for(int i = 2; i < 5; i++)
		{
			for (int j = 2; j < 5; j++)
			{
				_allTilesMatrix.Add(new Vector2Int(i, j), tile);
			}
		}
	}

	public Tile GetTile(Vector2Int posInMatrix)
	{
		return _allTilesMatrix.TryGetValue(posInMatrix, out Tile tile) ? tile : null;
	}

	public Tile GetCastle()
	{
		return _allTilesMatrix[new Vector2Int(3, 3)];
	}

	public Dictionary<Vector2Int, Tile> GetTileMatrix()
	{
		return _allTilesMatrix;
	}
}
public enum TileType
{
	Road,
	Grass,
	Mouinten,
	Castle,
	EnemyGround
}