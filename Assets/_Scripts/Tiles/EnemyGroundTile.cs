using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyGroundTile : Tile
{
	public override void Init(int x, int y)
	{
		base.Init(x, y);
		_tileType = TileType.EnemyGround;
	}

	public void Move(Vector2Int newPosition)
	{
		TileManager.Instance.MoveTileFromTo(Position, newPosition);
		_xPosition = newPosition.x;
		_yPosition = newPosition.y;
		Position = newPosition;
		transform.position = new Vector3(_xPosition, _yPosition);
		_name = GetType().Name + $" ({_xPosition}|{_yPosition}";
		gameObject.name = _name;
	}

	public void SelfDestroy()
	{
		TileManager.Instance.Remove(Position);
		Destroy(gameObject);
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
