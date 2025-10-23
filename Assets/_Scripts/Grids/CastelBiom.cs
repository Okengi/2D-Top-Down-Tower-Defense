using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CastelBiom : Biom
{
	[SerializeField] private Tile _castelTilePrefap;
	public CastelBiom(int id, Vector2 _gridPos, int width, int height, GridType gridType)
	   : base(id, _gridPos, width, height, gridType, TileType.Road, TileType.Grass, TileType.Grass)
	{


	}
	protected override TileType GetTileTyp(int x, int y)
	{
		TileType tileType = TileType.Grass;
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
		}
		tileType = DecideForCastelTile(x,y, tileType);
		return tileType;
	}
	
	private TileType DecideForCastelTile(int x, int y, TileType currentTile)
	{
		if (x >= (width - 3) / 2 && x < width - (width - 3) / 2 && y >= (height - 3) / 2 && y < height - (height - 3) / 2)
		{

				return TileType.Castle;

		}
		return currentTile;
	}

	public Vector2Int[] LastRoadTile()
	{
		Vector2Int[] positions = new Vector2Int[2];
		switch (_gridType)
		{
			case GridType.TopLeft:
				positions[0] = new Vector2Int(0, 3);
				positions[1] = new Vector2Int(3, 6);
				break;
			case GridType.TopRight:
				positions[0] = new Vector2Int(6, 3);
				positions[1] = new Vector2Int(3, 6);
				break;
			case GridType.BottomLeft:
				positions[0] = new Vector2Int(0, 3);
				positions[1] = new Vector2Int(3, 0);
				break;
			case GridType.BottomRight:
				positions[0]= new Vector2Int(6, 3);
				positions[1]= new Vector2Int(3, 0);
				break;
			case GridType.Vertical:
				positions[0] = new Vector2Int(3, 6);
				positions[1] = new Vector2Int(3, 0);
				break;
			case GridType.Horizontal:
				positions[0] = new Vector2Int(0, 3);
				positions[1] = new Vector2Int(6, 3);
				break;
		}
		return positions;
	}
}