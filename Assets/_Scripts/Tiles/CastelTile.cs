using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class CastelTile : Tile
{
	public override void Init(int x, int y, int id)
	{
		base.Init(x, y, id);
		_tileType = TileType.Castle;
	}
}
