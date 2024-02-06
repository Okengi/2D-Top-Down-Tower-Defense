using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManger : MonoBehaviour
{
	public static CameraManger instance;
	private int _width, _height;
	[SerializeReference] float duration;
	[SerializeReference] float zoomSpeed;
	private float _targetZoom = 5f;
	private Camera _camera;
	private Vector2 _pointInGridMatrix;
	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		_width = GridManager._width;
		_height = GridManager._height;
		_camera = Camera.main;
	}
	public void MoveTo(Vector2 gridPos)
	{
		Vector3 targetPosition = new Vector3(gridPos.x * _width + _width / 2, gridPos.y * _height + _height / 2, -10);
		_pointInGridMatrix = gridPos;
		StartCoroutine(SmoothMove(targetPosition));
	}

	private IEnumerator SmoothMove(Vector3 targetPosition)
	{
		Vector3 startPosition = Camera.main.transform.position;
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Camera.main.transform.position = targetPosition;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			MoveTo(_pointInGridMatrix + new Vector2(0, 1));
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			MoveTo(_pointInGridMatrix + new Vector2(0, -1));
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			MoveTo(_pointInGridMatrix + new Vector2(-1, 0));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			MoveTo(_pointInGridMatrix + new Vector2(1, 0));
		}
		Zoom();
	}

	private void Zoom()
	{
		float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

		if (scrollWheelInput != 0f)
		{
			_targetZoom -= scrollWheelInput * zoomSpeed;
			_targetZoom = Mathf.Clamp(_targetZoom, 1f, 50f);
			
		}

		float currentZoom = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * zoomSpeed);
		_camera.orthographicSize = currentZoom;
	}
}