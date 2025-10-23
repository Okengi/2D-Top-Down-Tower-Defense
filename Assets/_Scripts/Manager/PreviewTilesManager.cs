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

	public List<PreViewGrid> GetAllPreviewGrids()
	{
		return _previewList;
	}

	public void SpawnPreviewGrid(int id, Vector2 pos, bool newList)
	{
		if (GetPreview(pos) != null)
		{
			return;
		}
		else if (GridManager.Instance.GetGrid(pos) != null) {
			return;
		}
		
		PreViewGrid gridPreView = Instantiate(_preViewGrid, new Vector2(pos.x * GridManager._width + GridManager._width / 2, pos.y * GridManager._width + GridManager._width / 2), Quaternion.identity);
		gridPreView.name = $"Preview Tile {id}";
		gridPreView.Init(pos, id);
		_previewList.Add(gridPreView);
		_previewTileMatrix.Add(pos, gridPreView);
		if (newList) GridManager.Instance.AddGridBranch();
	}

	public void MovePreviewGrid(int id, Vector2 justSpawnedGridPos, List<Vector2> directions)
	{
		_previewTileMatrix.Remove(justSpawnedGridPos);

		List<int> previewDirs = new List<int>();
		List<Vector2> emptyDirs = new List<Vector2>();
		Vector2 occupiedDir = Vector2.zero;

		foreach (Vector2 direction in directions)
		{
			Vector2 gridPosToCheck = justSpawnedGridPos + direction;

			if (GridManager.Instance.GetGrid(gridPosToCheck) == null && !IsPreview(gridPosToCheck))
			{
				emptyDirs.Add(direction);
			}
			else if (IsPreview(gridPosToCheck))
			{
				PreViewGrid pre = GetPreview(gridPosToCheck);
				previewDirs.Add(pre.id);
			}
			else
			{
				occupiedDir = direction;
			}
		}

		if (directions.Count == 2)
		{
			if(emptyDirs.Count == 1)
			{
				Move(id, emptyDirs[0]);
			}
			else if(previewDirs.Count == 1)
			{
				Merge(id, previewDirs[0]);
			}
		}
		else if(directions.Count == 3)
		{
			if(emptyDirs.Count == 2)
			{
				Move(id, emptyDirs[0]);
				CreateNew(justSpawnedGridPos + emptyDirs[1]);
			}
			else if(previewDirs.Count == 2)
			{
				Merge(id, previewDirs[0]);
				Merge(previewDirs[1]-2);
			}
			else if(emptyDirs.Count == 1 && previewDirs.Count == 1)
			{
				Move(id, emptyDirs[0]);
				Merge(previewDirs[0]);
			}
		}
	}

	private void Merge(int ID1, int ID2)
	{
		Vector2 gr = _previewList[ID1]._gridPos;
		Vector2 gridPosition = _previewList[ID2]._gridPos;
		_previewList[ID1].Destroy();
		_previewList[ID2].Destroy();
		_previewList.RemoveAt(ID1);
		_previewList.RemoveAt(ID2);
		GridManager.Instance.RemoveEnemyBiom();
		GridManager.Instance.ConvertToStaticEnemyBiom(gridPosition);
		_previewTileMatrix.Remove(gridPosition);
		_previewTileMatrix.Remove(gr);
	}
	private void Merge(int ID1)
	{
		Vector2 gridPosition = _previewList[ID1]._gridPos;
		_previewList[ID1].Destroy();
		_previewList.RemoveAt(ID1 );
		GridManager.Instance.ConvertToStaticEnemyBiom(gridPosition);
		_previewTileMatrix.Remove(gridPosition);
	}

	private void CreateNew(Vector2 pos)
	{
		SpawnPreviewGrid(_previewList.Count, pos, true);
	}

	private void Move(int id, Vector2 direction)
	{
		_previewList[id].Move(direction);
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
	{	
		return _previewTileMatrix.TryGetValue(pos, out PreViewGrid pre) ? pre : null;
	}

	public bool IsPreview(Vector2 pos)
	{
		return _previewTileMatrix.TryGetValue(pos, out PreViewGrid pre);
		
	}
}