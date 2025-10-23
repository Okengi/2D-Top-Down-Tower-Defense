using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class EnemyGroundTile : Tile
{
	[SerializeField] private Color _baseColor, _offsetColor;
	public override void Init(int x, int y, int id)
	{
		base.Init(x, y, id);
		_tileType = TileType.Enemy;

		bool isOffset = ((x + y) % 2 == 0);

		if (isOffset)
		{
			_renderer.color = _offsetColor;
		}
		else
		{
			_renderer.color = _baseColor;
		}
	}





	public void Hide()
	{
		transform.position = new Vector3(_xPosition + 1000, _yPosition + 1000);
	}

	public void Show()
	{
		transform.position = new Vector3(_xPosition, _yPosition);
	}

	
}
