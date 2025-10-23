using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class Tile : MonoBehaviour
{
	[SerializeField] protected SpriteRenderer _renderer;
	[SerializeField] private GameObject _highLight;

	protected string _name;
	protected int _xPosition, _yPosition;
	protected Vector2Int Position;

	public TileType _tileType;
	protected int id;
	public string GetName() { return _name; }
	public virtual void Init(int x, int y, int id)
	{
		_name = GetType().Name + $" ({x}|{y}";
		_xPosition = x;
		_yPosition = y;
		Position = new Vector2Int(x, y);
		gameObject.name = _name;
		this.id = id;
	}
	public void SelfDestroy()
	{
		TileManager.Instance.Remove(Position);
		Destroy(gameObject);
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
	private void OnMouseExit()
	{
		_highLight.SetActive(false);
	}
	public int GetXPosition() { return _xPosition;}
	public int GetYPosition() { return _yPosition;}
	private void OnMouseOver()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			_highLight.SetActive(false); return;
		}
		_highLight.SetActive(true);
	}

	protected virtual void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;
		//GridManager.Instance.FocusOnGrid(_gridPos);
	}
	// A Star Path Finding ____________________________________________
	public float gCost;
	public float hCost;
	public float fCost;

	public Tile previousTile;

	public void CalculateFCost()
	{
		fCost = gCost + hCost;
	}

	public void Spawn(GameObject entity)
	{
		var monster_entity = Instantiate(entity, transform.position, Quaternion.identity);
		Enemy en = monster_entity.GetComponent<Enemy>();
		en.Initialize(this);
	}

}