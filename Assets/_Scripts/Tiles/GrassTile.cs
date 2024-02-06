using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class GrassTile : Tile
{
	[SerializeField] private Color _baseColor, _offsetColor;
	public override void Init(int x, int y, Vector2 gridPos)
	{
		base.Init(x, y, gridPos);
		_name = "Grass Lands";
		bool isOffset;
		if (gridPos.x == 0 && gridPos.y == 0)
		{
			isOffset = (x % 2 != y % 2);
		}
		else
		{
			isOffset = (x % 2 != y % 2) != (Math.Abs(gridPos.x % 2) !=Math.Abs( gridPos.y % 2));
		}

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
		if (GameManager.instance.state == GameState.PlaceUnits) BuildingManager.instance.Build(_gridPos, _position);	
	}
}
