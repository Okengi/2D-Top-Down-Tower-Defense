using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewTile : MonoBehaviour
{
	[SerializeField] protected SpriteRenderer _renderer;
	[SerializeField] private GameObject _highLight;
	[SerializeField] private TextMeshPro _text;
	public Vector2 _gridPos;
	public int id;

	public void Init(Vector2 gridPos, int id)
	{
		_gridPos = gridPos;
		this.id = id;
		_text.text = id.ToString();
	}

	public void Move(Vector2 offset)
	{
		Vector3 pos = transform.position+ new Vector3(offset.x * GridManager._width, offset.y * GridManager._height, 0);
		_gridPos += offset;
		transform.position += new Vector3(0, 0, 1000);
		StartCoroutine(MoveWithDelay(pos));
	}

	private IEnumerator MoveWithDelay(Vector2 pos)
	{
		yield return new WaitForSeconds(0.4f);
		transform.position = pos;
	}

	private void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject()) return;
		GridManager.instance.GenerateGrid(_gridPos , id);
	}

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


}