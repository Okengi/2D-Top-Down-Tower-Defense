using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTilesManager : MonoBehaviour
{	public static PreviewTilesManager Instance;

	[SerializeField] private PreViewGrid _preViewGrid;
	public List<PreViewGrid> _previewList;
	public Dictionary<Vector2, PreViewGrid> _previewTileMatrix;
	
	private void Awake()
	{
		Instance = this;
		GameManager.OnGameStateChange += GameStateChanged;
	}

	private void Start()
	{
		_previewList = new List<PreViewGrid>();
		_previewTileMatrix = new Dictionary<Vector2, PreViewGrid>();
	}

	private void OnDestroy()
	{
		GameManager.OnGameStateChange -= GameStateChanged;
	}

	public void SpawnPreviewGrid(int id, Vector2 pos, bool newList)
	{
		if (GetPreview(pos) != null)
		{
			return;
		}
		else if (GridManager.instance.GetGrid(pos) != null) {
			return;
		}
		
		PreViewGrid gridPreView = Instantiate(_preViewGrid, new Vector2(pos.x * GridManager._width + GridManager._width / 2, pos.y * GridManager._width + GridManager._width / 2), Quaternion.identity);
		gridPreView.name = $"Preview Tile {id}";
		gridPreView.Init(pos, id);
		_previewList.Add(gridPreView);
		_previewTileMatrix.Add(pos, gridPreView);
		if (newList) GridManager.instance.AddGridBranch();
	}

	public PreViewGrid GetPreViewGrid(Vector2 pos)
	{
		return _previewTileMatrix.TryGetValue(pos, out PreViewGrid grid) ? grid : null;
	}

	public void MovePreviewTile(int id, Vector2 justSpawndGridsPos, GridType gridTyp)
	{
		_previewTileMatrix.Remove(justSpawndGridsPos);
		List<Vector2> directions = GridTypManager.instance.GetDirectionsOfType(gridTyp);
		Vector2 offset = Vector2.zero;
		Vector2 offsetIfneedToSpawnNew = Vector2.zero;
		foreach (Vector2 direction in directions)
		{
			if (GridManager.instance.GetGrid(justSpawndGridsPos + direction) == null && GetPreViewGrid(justSpawndGridsPos + direction) == null)
			{
				if (offset ==  Vector2.zero)
				{
					offset = direction;
				}
				else
				{
					offsetIfneedToSpawnNew = direction;
				}
			}
		}

		if (offset == Vector2.zero) { _previewList[id].Destroy(); _previewList[id] = null; } else { _previewList[id].Move(offset); }
		if (offsetIfneedToSpawnNew != Vector2.zero) { SpawnPreviewGrid(_previewList.Count, justSpawndGridsPos + offsetIfneedToSpawnNew, true); }	

		_previewTileMatrix[justSpawndGridsPos + offset] = _previewList[id];
	}

	private void GameStateChanged(GameState newState)
	{
		switch (newState)
		{
			case GameState.PlaceNewGrid:
				ShowPreview();
				break;
			default:
				HidePreview();
				break;
		}
	}
	private void HidePreview()
	{
		_previewList.ForEach(delegate (PreViewGrid tile)
		{
			tile.gameObject.SetActive(false);
		});
	}
	private void ShowPreview()
	{
		_previewList.ForEach(delegate (PreViewGrid tile)
		{
			tile.gameObject.SetActive(true);
		});
	}

	private PreViewGrid GetPreview(Vector2 pos)
	{	// For the Futture to check if after moving there would be overlaping Preview Tile so I can hinnder a Loop happening
		return _previewTileMatrix.TryGetValue(pos, out PreViewGrid pre) ? pre : null;
	}
}