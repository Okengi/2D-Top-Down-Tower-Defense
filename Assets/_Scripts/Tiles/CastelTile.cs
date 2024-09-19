using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class CastelTile : Tile
{
	public override void Init(int x, int y)
	{
		base.Init(x, y);
		_tileType = TileType.Castle;
	}
}
