using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class GrassTile : Tile
{
	[SerializeField] private Color _baseColor, _offsetColor;
	public override void Init(int x, int y)
	{
		base.Init(x, y);
		_name = "Grass Lands";
		_tileType = TileType.Grass;

		bool isOffset = (x % 2 != y % 2);
		
		if (isOffset)
		{
			_renderer.color = _offsetColor;
		}
		else
		{
			_renderer.color = _baseColor;
		}
	}

	protected override void OnMouseDown()
	{
		base.OnMouseDown();
		//if (GameManager.instance.state == GameState.PlaceUnits) BuildingManager.instance.Build(_gridPos, _positionInGrid);	
	}
}
