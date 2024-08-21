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

	public void MovePreviewTile(int id, GridType gridTyp, Vector2 newGridPos, Vector2 previousGridPosition)
	{
		Vector2 relativePreviousGridPosition = previousGridPosition - newGridPos;
		Vector2 offset = Vector2.zero;
		Vector2 newOffset = Vector2.zero;
		Debug.Log($"<color=orange>Id:{id}|GridTyp:{gridTyp}|Previous:{previousGridPosition}|newGridPos:{newGridPos}|relativGridPos:{relativePreviousGridPosition}</color>");
		if (relativePreviousGridPosition == Vector2.up)
		{
			switch (gridTyp)
			{
				case GridType.Vertical:
					offset = new Vector2(0, -1);
					break;
				case GridType.TopLeft:
					offset = new Vector2(-1, 0);
					break;
				case GridType.TopRight:
					offset = new Vector2(1, 0);
					break;
				case GridType.TopLeftRight:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(1, 0);
					break;
				case GridType.LeftTopBottom:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(0, -1);
					break;
				case GridType.RightTopBottom:
					offset = new Vector2(1, 0);
					newOffset = new Vector2(0, -1);
					break;
			}
		}
		else if (relativePreviousGridPosition == Vector2.down)
		{
			switch (gridTyp)
			{
				case GridType.Vertical:
					offset = new Vector2(0, 1);
					break;
				case GridType.BottomLeft:
					offset = new Vector2(-1, 0);
					break;
				case GridType.BottomRight:
					offset = new Vector2(1, 0);
					break;
				case GridType.BottomLeftRight:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(1, 0);
					break;
				case GridType.LeftTopBottom:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(0, 1);
					break;
				case GridType.RightTopBottom:
					offset = new Vector2(1, 0);
					newOffset = new Vector2(0, 1);
					break;
			}
		}
		else if (relativePreviousGridPosition == Vector2.left)
		{
			switch (gridTyp)
			{
				case GridType.Horizontal:
					offset = new Vector2(1, 0);
					break;
				case GridType.TopLeft:
					offset = new Vector2(0, 1);
					break;
				case GridType.BottomLeft:
					offset = new Vector2(0, -1);
					break;
				case GridType.BottomLeftRight:
					offset = new Vector2(1, 0);
					newOffset = new Vector2(0, -1);
					break;
				case GridType.TopLeftRight:
					offset = new Vector2(1, 0);
					newOffset = new Vector2(0, 1);
					break;
				case GridType.LeftTopBottom:
					offset = new Vector2(0, -1);
					newOffset = new Vector2(0, 1);
					break;
			}
		}
		else if (relativePreviousGridPosition == Vector2.right)
		{
			switch (gridTyp)
			{
				case GridType.Horizontal:
					offset = new Vector2(-1, 0);
					break;
				case GridType.TopRight:
					offset = new Vector2(0, 1);
					break;
				case GridType.BottomRight:
					offset = new Vector2(0, -1);
					break;
				case GridType.BottomLeftRight:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(0, -1);
					break;
				case GridType.TopLeftRight:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(0, 1);
					break;
				case GridType.RightTopBottom:
					offset = new Vector2(0, -1);
					newOffset = new Vector2(0, 1);
					break;
			}
		}

		if (newOffset != Vector2.zero)
		{
			
			SpawnPreviewGrid(_previewList.Count, newGridPos + newOffset, true);
			
		}
		_previewList[id].Move(offset);


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