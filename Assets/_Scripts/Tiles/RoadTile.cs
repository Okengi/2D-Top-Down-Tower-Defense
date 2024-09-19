using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile : Tile
{
	public override void Init(int x, int y)
	{
		base.Init(x, y);
		_tileType = TileType.Road;
	}
}
