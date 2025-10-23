using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
	public override void Init(int x, int y, int id)
	{
		base.Init(x, y, id);
		_tileType = TileType.Road;
	}
}
