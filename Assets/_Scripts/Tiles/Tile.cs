using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tile : MonoBehaviour
{
	[SerializeField] protected SpriteRenderer _renderer;
	[SerializeField] private GameObject _highLight;

	protected string _name;
	public Vector2 _position;
	public string GetName() { return _name; }

	public Vector2 _gridPos;

	public virtual void Init(int x, int y, Vector2 gridPos)
	{
		_name = this.GetType().Name;
		_gridPos = gridPos;
		_position = new Vector2(x, y);
	}

	/*private void OnMouseEnter()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;
		_highLight.SetActive(true);
	}*/

	private void OnMouseExit()
	{
		_highLight.SetActive(false);
	}

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
		GridManager.instance.FocusOnGrid(_gridPos);
	}
}
