using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarderTile : Tile
{
	public override void Init(int x, int y, int id)
	{
		base.Init(x, y, id);
		_tileType = TileType.Boarder;
	}
}
