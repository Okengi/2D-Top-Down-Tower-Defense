using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CastelGrid : Grid
{
	[SerializeField] private Tile _castelTilePrefap;
	protected override IEnumerator GenerateGrid()
	{
		Vector2 pos = new Vector2(_pointInMatrix.x * _width, _pointInMatrix.y * _height);

		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				Tile prefap;
				switch (_typeOfGrid)
				{
					case GridType.Horizontal:
						prefap = (y == _height / 2) ? _roadTilePrefap : (y == 0 || y == _height - 1) ? _boarderTilePrefap : _baseTilePrefap;
						prefap = DecideForCastelTile(x, y, prefap);
						break;
					case GridType.Vertical:
						prefap = (x == _width / 2) ? _roadTilePrefap : (x == 0 || x == _width - 1) ? _boarderTilePrefap : _baseTilePrefap;
						prefap = DecideForCastelTile(x, y, prefap);
						break;
					case GridType.TopLeft:
						prefap = (y == _height / 2 && x <= _width / 2 || x == _width / 2 && y > _height / 2) ? _roadTilePrefap : (y == 0 || x == _width - 1) ? _boarderTilePrefap : _baseTilePrefap;
						prefap = DecideForCastelTile(x, y, prefap);
						break;
					case GridType.TopRight:
						prefap = (y == _height / 2 && x >= _width / 2 || x == _width / 2 && y > _height / 2) ? _roadTilePrefap : (y == 0 || x == 0) ? _boarderTilePrefap : _baseTilePrefap;
						prefap = DecideForCastelTile(x, y, prefap);
						break;
					case GridType.BottomLeft:
						prefap = (y == _height / 2 && x <= _width / 2 || x == _width / 2 && y < _height / 2) ? _roadTilePrefap : (y == _height - 1 || x == _width - 1) ? _boarderTilePrefap : _baseTilePrefap;
						prefap = DecideForCastelTile(x, y, prefap);
						break;
					case GridType.BottomRight:
						prefap = (y == _height / 2 && x >= _width / 2 || x == _width / 2 && y < _height / 2) ? _roadTilePrefap : (y == _height - 1 || x == 0) ? _boarderTilePrefap : _baseTilePrefap;
						prefap = DecideForCastelTile(x, y, prefap);
						break;
					default:
						prefap = DecideForCastelTile(x, y, _baseTilePrefap);
						break;
				}
				yield return new WaitForSeconds(0.0025f);
				HandelSpawningTile(prefap, x, y, pos);
			}
		}
	}
	private Tile DecideForCastelTile(int x, int y, Tile currentTile)
	{
		if (x >= (_width - 3) / 2 && x < _width - (_width - 3) / 2 && y >= (_height - 3) / 2 && y < _height - (_height - 3) / 2)
		{
			if (x == _width / 2 && y == _height / 2)
			{
				return _castelTilePrefap;
			}
			else
			{
				return null;
			}
		}
		return currentTile;
	}

	public Vector2[] LastRoadTile()
	{
		Vector2[] ret = new Vector2[2];
		switch (_typeOfGrid)
		{
			case GridType.TopLeft:
				ret[0] = new Vector2(0, 3);
				ret[1] = new Vector2(3, 6);
				break;
			case GridType.TopRight:
				ret[0] = new Vector2(6, 3);
				ret[1] = new Vector2(3, 6);
				break;
			case GridType.BottomLeft:
				ret[0] = new Vector2(0, 3);
				ret[1] = new Vector2(3, 0);
				break;
			case GridType.BottomRight:
					ret[0]= new Vector2(6, 3);
					ret[1]= new Vector2(3, 0);
				break;
			case GridType.Vertical:
					ret[0] = new Vector2(3, 6);
					ret[1] = new Vector2(3, 0);
				break;
			case GridType.Horizontal:
					ret[0] = new Vector2(0, 3);
					ret[1] = new Vector2(6, 3);
				break;
		}
		return ret;
	}
}