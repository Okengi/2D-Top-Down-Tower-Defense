using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrassGrid : Biom
{
	public GrassGrid(int id, Vector2 _gridPos, int width, int height, GridType gridType)
	   : base(id, _gridPos, width, height, gridType, TileType.Road, TileType.Boarder, TileType.Grass)
	{
		
		
	}
}