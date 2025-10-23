using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
   public static TileManager Instance { get; private set; }

	[SerializeField] private EnemyGroundTile enemyGroundTilePrefap;
	[SerializeField] private CastelTile castelTilePrefap;
	[SerializeField] private RoadTile roadTilePrefap;
	[SerializeField] private BoarderTile boarderTilePrefap;
	[SerializeField] private GrassTile grassTilePrefap;

	Dictionary<int, GameObject> tileHolders = new Dictionary<int, GameObject>();

	private CastelTile CastelTile = null;

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

	public void SpawnTile(TileType tileTyp, int x, int y, int id)
	{
		Tile spawnedTile;
		switch (tileTyp)
		{
			case TileType.Castle:
				SetUpCastleTile(x, y);
				break;
			case TileType.Boarder:
				spawnedTile = Instantiate(boarderTilePrefap, new Vector3(x, y), Quaternion.identity);
				if (!tileHolders.ContainsKey(id))
				{
					tileHolders[id] = new GameObject();
					tileHolders[id].name = $"Biom {id}";
				}
				
				spawnedTile.transform.parent = tileHolders[id].transform;
				spawnedTile.Init(x, y, id);
				
				_allTilesMatrix[new Vector2Int(x, y)] = spawnedTile;
				break;
			case TileType.Grass:
				spawnedTile = Instantiate(grassTilePrefap, new Vector3(x, y), Quaternion.identity);
				if (!tileHolders.ContainsKey(id))
				{
					tileHolders[id] = new GameObject();
					tileHolders[id].name = $"Biom {id}";
				}

				spawnedTile.transform.parent = tileHolders[id].transform;

				spawnedTile.Init(x, y, id);
				_allTilesMatrix[new Vector2Int(x, y)] = spawnedTile;
				break;
			case TileType.Road:
				spawnedTile = Instantiate(roadTilePrefap, new Vector3(x, y), Quaternion.identity);
				if (!tileHolders.ContainsKey(id))
				{
					tileHolders[id] = new GameObject();
					tileHolders[id].name = $"Biom {id}";
				}

				spawnedTile.transform.parent = tileHolders[id].transform;

				spawnedTile.Init(x, y, id);
				_allTilesMatrix[new Vector2Int(x, y)] = spawnedTile;
				break;
			case TileType.Enemy:
				spawnedTile = Instantiate(enemyGroundTilePrefap, new Vector3(x, y), Quaternion.identity);
				if (!tileHolders.ContainsKey(id))
				{
					tileHolders[id] = new GameObject();
					tileHolders[id].name = $"Biom {id}";
				}

				spawnedTile.transform.parent = tileHolders[id].transform;

				spawnedTile.Init(x, y, id);
				_allTilesMatrix[new Vector2Int(x, y)] = spawnedTile;
				break;
		
		}

		
	}

	private void SetUpCastleTile(int x, int y)
	{
		if(CastelTile == null)
		{
			Tile castleTile = Instantiate(castelTilePrefap, new Vector3(x +1, y + 1), Quaternion.identity);
			castleTile.transform.parent = tileHolders[0].transform;

			castleTile.Init(x, y, 0);

			for (int i = 2; i < 5; i++)
			{
				for (int j = 2; j < 5; j++)
				{
					_allTilesMatrix[new Vector2Int(i, j)] = castleTile;
				}
			}
			CastelTile = castleTile.GetComponent<CastelTile>();
		}
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
			//Debug.LogError($"No tile found at position {from}.");
		}
	}

	public Tile GetTile(Vector2Int posInMatrix, bool debug = false)
	{
		if (debug) Debug.Log($"Geting tile at {posInMatrix}");
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
	Boarder,
	Castle,
	Enemy
}