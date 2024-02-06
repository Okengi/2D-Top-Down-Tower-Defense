using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public abstract class Grid : MonoBehaviour
{
	[SerializeField] protected Tile _baseTilePrefap, _roadTilePrefap, _boarderTilePrefap;
	[SerializeField] private TextMeshPro _text;

	protected Dictionary<Vector2, Tile> _tiles;
	protected GridType _typeOfGrid;
	public Vector2 _pointInMatrix;
	protected int _width, _height;

	protected int id;

	public void Init(GridType typ, Vector2 pointInMatrix, int id)
	{
		_width = GridManager._width;
		_height = GridManager._height;

		_tiles = new Dictionary<Vector2, Tile>(_width * _height);
		_typeOfGrid = typ;
		_pointInMatrix = pointInMatrix;
		this.id = id;
		_text.text = id.ToString();
		StartCoroutine(GenerateGrid());
	}

	protected virtual IEnumerator GenerateGrid()
	{
		Vector2 pos = new Vector2(_pointInMatrix.x * _width, _pointInMatrix.y * _height);

		Vector2 offsetOne = Vector2.zero;
		Vector2 offsetTwo = Vector2.zero;

		for (int x  = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				Tile prefap;
				switch (_typeOfGrid)
				{
					case GridType.Horizontal:
						prefap = (y == _height / 2) ? _roadTilePrefap : (y == 0 || y == _height - 1) ? _boarderTilePrefap : _baseTilePrefap;
						break;
					case GridType.Vertical:
						prefap = (x == _width / 2) ? _roadTilePrefap : (x == 0 || x == _width - 1) ? _boarderTilePrefap : _baseTilePrefap;
						break;
					case GridType.TopLeft:
						prefap = (y == _height / 2 && x <= _width / 2 || x == _width / 2 && y > _height / 2) ? _roadTilePrefap : (y == 0 || x == _width - 1) ? _boarderTilePrefap : _baseTilePrefap;
						break;
					case GridType.TopRight:
						prefap = (y == _height / 2 && x >= _width / 2 || x == _width / 2 && y > _height / 2) ? _roadTilePrefap : (y == 0 || x == 0) ? _boarderTilePrefap : _baseTilePrefap;
						break;
					case GridType.BottomLeft:
						prefap = (y == _height / 2 && x <= _width / 2 || x == _width / 2 && y < _height / 2) ? _roadTilePrefap : (y == _height - 1 || x == _width - 1) ? _boarderTilePrefap : _baseTilePrefap;
						break;
					case GridType.BottomRight:
						prefap = (y == _height / 2 && x >= _width / 2 || x == _width / 2 && y < _height / 2) ? _roadTilePrefap : (y == _height - 1 || x == 0) ? _boarderTilePrefap : _baseTilePrefap;
						break;
					case GridType.BottomLeftRight:
						if (x <= 2 && (y <= 2 && y != 0)) prefap = _baseTilePrefap;
						else if (x > 3 && (y <= 2 && y != 0)) prefap = _baseTilePrefap;
						else if (y == 3) prefap = _roadTilePrefap;
						else if (x == 3 && y < 3) prefap = _roadTilePrefap;
						else if (y == 4 || y == 5) prefap = _baseTilePrefap;
						else if (0 <x && x < 3 && y == 0) prefap = _baseTilePrefap;
						else if (y == 0 && x > 3 && x < 6) prefap = _baseTilePrefap;
						else prefap = _boarderTilePrefap;
						break;
					case GridType.TopLeftRight:
						if (x <= 2 && (y >=4 && y != 6)) prefap = _baseTilePrefap;
						else if (x > 3 && (y >= 4 && y != 6)) prefap = _baseTilePrefap;
						else if (y == 3) prefap = _roadTilePrefap;
						else if (x == 3 && y > 3) prefap = _roadTilePrefap;
						else if (y == 1 || y == 2) prefap = _baseTilePrefap;
						else prefap = _boarderTilePrefap;
						break;
					case GridType.LeftTopBottom:
						if (x == 0 && y != 3 || x == 6) prefap = _boarderTilePrefap;
						else if (x == 3) prefap = _roadTilePrefap;
						else if (y == 3 && x < 4) prefap = _roadTilePrefap;
						else prefap = _baseTilePrefap;
						break;
					case GridType.RightTopBottom:
						if (x == 0 || y != 3 && x == 6) prefap = _boarderTilePrefap;
						else if (x == 3) prefap = _roadTilePrefap;
						else if (y == 3 && x > 3) prefap = _roadTilePrefap;
						else prefap = _baseTilePrefap;
						break;
					default:
						prefap = _baseTilePrefap;
						break;
				}
				yield return new WaitForSeconds(0.0075f);
				HandelSpawningTile(prefap, x, y, pos);
			}
		}
	}
	protected void HandelSpawningTile(Tile tile, int x, int y, Vector2 gridPos)
	{
		if (tile == null) return;
		var spawnedTile = Instantiate(tile, new Vector3(gridPos.x + x, gridPos.y + y), Quaternion.identity);
		spawnedTile.transform.parent = transform;

		spawnedTile.Init(x, y, _pointInMatrix);
		spawnedTile.name = $"{spawnedTile.GetName()} {x} {y}";

		_tiles[new Vector2(x, y)] = spawnedTile;
	}

	public bool HasConnectionToRight()
	{
		Tile tile = GetTileAtPosition(new Vector2(_width - 1, _height / 2));
		return tile != null && tile.GetType() == typeof(RoadTile);
	}
	public bool HasConnectionToLeft()
	{
		Tile tile = GetTileAtPosition(new Vector2(0, _height / 2));
		return tile != null && tile.GetType() == typeof(RoadTile);
	}
	public bool HasConnectionToTop()
	{
		Tile tile = GetTileAtPosition(new Vector2(_width / 2, _height - 1));
		return tile != null && tile.GetType() == typeof(RoadTile);
	}
	public bool HasConnectionToBottom()
	{
		Tile tile = GetTileAtPosition(new Vector2(_width / 2, 0));
		return tile != null && tile.GetType() == typeof(RoadTile);
	}
	public Tile GetTileAtPosition(Vector2 position)
	{
		if (_tiles.TryGetValue(position, out var tile))
		{
			return tile;
		}
		return null;
	}

	public Tile GetLastRoadTile()
	{
		switch (_typeOfGrid)
		{
			case GridType.TopLeft:
				if(GridManager.instance.GetGrid(_pointInMatrix+new Vector2(0,1)) != null)
				{
					return GetTileAtPosition(new Vector2(0, 3));
				}
				else if(GridManager.instance.GetGrid(_pointInMatrix + new Vector2(-1, 0)) != null)
				{
					return GetTileAtPosition(new Vector2(3, 6));
				}
				break;
			case GridType.TopRight:
				if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(0, 1)) != null)
				{
					return GetTileAtPosition(new Vector2(6, 3));
				}
				else if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(1, 0)) != null)
				{
					return GetTileAtPosition(new Vector2(3, 6));
				}
				break;
			case GridType.BottomLeft:
				if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(0, -1)) != null)
				{
					return GetTileAtPosition(new Vector2(0, 3));
				}
				else if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(-1, 0)) != null)
				{
					return GetTileAtPosition(new Vector2(3, 0));
				}
				break;
			case GridType.BottomRight:
				if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(0, -1)) != null)
				{
					return GetTileAtPosition(new Vector2(6,3));
				}
				else if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(1, 0)) != null)
				{
					return GetTileAtPosition(new Vector2(3, 0));
				}
				break;
			case GridType.Vertical:
				if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(0, -1)) != null)
				{
					return GetTileAtPosition(new Vector2(3, 6));
				}
				else if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(0,1)) != null)
				{
					return GetTileAtPosition(new Vector2(3, 0));
				}
				break;
			case GridType.Horizontal:
				if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(1, 0)) != null)
				{
					return GetTileAtPosition(new Vector2(0, 3));
				}
				else if (GridManager.instance.GetGrid(_pointInMatrix + new Vector2(-1, 0)) != null)
				{
					return GetTileAtPosition(new Vector2(6, 3));
				}
				break;
		}
		return null;
	}

}