using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTilesManager : MonoBehaviour
{
	public List<PreviewTile> _previewList;
	public Dictionary<Vector2, PreviewTile> _previewTileMatrix;
	[SerializeField] private PreviewTile previouvTile;

	public static PreviewTilesManager Instance;

	private void Awake()
	{
		Instance = this;
		GameManager.OnGameStateChange += GameStateChanged;
	}

	private void Start()
	{
		_previewList = new List<PreviewTile>();
		_previewTileMatrix = new Dictionary<Vector2, PreviewTile>();
	}

	private void OnDestroy()
	{
		GameManager.OnGameStateChange -= GameStateChanged;
	}

	public void SpawnPreviewTile(int id, Vector2 pos, bool newList)
	{
		if (GetPreview(pos) != null)
		{
			Debug.Log($"<color=red>ERROR:Spawn Preview! Preview Tile already at pos{pos}</color>");
			return;
		}
		else if (GridManager.instance.GetGrid(pos) != null) {
			Debug.Log($"<color=red>ERROR:Spawn Preview! Grid already at pos{pos} </color>");
			return;
		}
		
		PreviewTile tile = Instantiate(previouvTile, new Vector2(pos.x * GridManager._width + GridManager._width / 2, pos.y * GridManager._width + GridManager._width / 2), Quaternion.identity);
		tile.name = $"Preview Tile {id}";
		tile.Init(pos, id);
		_previewList.Add(tile);
		_previewTileMatrix.Add(pos, tile);
		if (newList) GridManager.instance.AddGridList();
	}

	public void MovePreviewTile(int id, GridType gridTyp, Vector2 newGridPos, Vector2 previousGridPosition)
	{
		Vector2 relativePreviousGridPosition = previousGridPosition - newGridPos;
		Vector2 offset = Vector2.zero;
		Vector2 newOffset = Vector2.zero;
		Debug.Log($"<color=orange>Id:{id} | GridTyp:{gridTyp} | PreviousGridPos:{previousGridPosition}</color>");
		if (relativePreviousGridPosition == new Vector2(0,1)) {
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
					//SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(1, 0), true);
					break;
				case GridType.LeftTopBottom:
					offset = new Vector2(-1, 0);
					newOffset = new Vector2(0, -1);
					//SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, -1), true);
					break;
				case GridType.RightTopBottom:
					offset = new Vector2(1, 0);
					newOffset = new Vector2(0, -1);
					//SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, -1), true);
					break;
			}
		}
		else if (relativePreviousGridPosition == Vector2.down) {
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
					SpawnPreviewTile(_previewList.Count, newGridPos + Vector2.right, true);
					break;
				case GridType.LeftTopBottom:
					offset = new Vector2(-1, 0);
					SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, 1), true);
					break;
				case GridType.RightTopBottom:
					offset = new Vector2(1, 0);
					SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, 1), true);
					break;
			}
		}
		else if((relativePreviousGridPosition == Vector2.left)) {
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
					SpawnPreviewTile(_previewList.Count, newGridPos + Vector2.down, true);
					break;
				case GridType.TopLeftRight:
					offset = new Vector2(1, 0);
					SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, 1), true);
					break;
				case GridType.LeftTopBottom:
					offset = new Vector2(0, -1);
					SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, 1), true);
					break;
			}
		}
		else if (relativePreviousGridPosition == Vector2.right) {
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
					SpawnPreviewTile(_previewList.Count, newGridPos + Vector2.down, true);
					break;
				case GridType.TopLeftRight:
					offset = new Vector2(-1, 0);
					SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, 1), true);
					break;
				case GridType.RightTopBottom:
					offset = new Vector2(0, -1);
					SpawnPreviewTile(_previewList.Count, newGridPos + new Vector2(0, 1), true);
					break;
			}
		}
		if (GridManager.instance.GetGrid(new Vector2(0, 0)) == null && newOffset != Vector2.zero)
		{
			Debug.Log("Spawned new Preview tile at");
			SpawnPreviewTile(_previewList.Count, newGridPos + newOffset, true);
		}
		else
		{
			Debug.Log("Didn't spawn new PreviewTile");
		}
		//Debug.Log($"Moved by offset: {offset}");
		Debug.Log("---------------------------------");
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
		_previewList.ForEach(delegate (PreviewTile tile)
		{
			tile.gameObject.SetActive(false);
		});
	}
	private void ShowPreview()
	{
		_previewList.ForEach(delegate (PreviewTile tile)
		{
			tile.gameObject.SetActive(true);
		});
	}

	private PreviewTile GetPreview(Vector2 pos)
	{	// For the Futture to check if after moving there would be overlaping Preview Tile so I can hinnder a Loop happening
		return _previewTileMatrix.TryGetValue(pos, out PreviewTile pre) ? pre : null;
	}
}