using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GridManager : MonoBehaviour
{
	public static GridManager instance;
	public static int _width = 7, _height = 7;

	// Grid Management
	public Dictionary<Vector2, Grid> _gridsMatrix;
	public List<List<Grid>> _gridList;

	// Variable Inputs (Prefaps)
	[Header("INPUT")]
	[SerializeField] private Grid _grassGridePrefap;
	[SerializeField] private CastelGrid _castleGridPrefap;
	[SerializeField] private int gridCost = 12;

	private CastelGrid _castleGrid;

	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		_gridsMatrix = new Dictionary<Vector2, Grid>();
		_gridList = new List<List<Grid>>();
		GenerateCastle();
	}

	public Grid LastGrid (int id)
	{
		return _gridList[id].Last<Grid>();
	}

	public void AddGridList()
	{
		_gridList.Add(new List<Grid>());
	}

	public void GenerateGrid(Vector2 posInMatrix, int id)
	{	// Spawning + Nameing + Setup
		if (GameManager.instance.GetMoney() - gridCost >= 0)
		{
			GameManager.instance.SpendMoney(gridCost);
		}
		else { return; }
		Vector2 pos = new Vector2(posInMatrix.x * _width, posInMatrix.y * _height);
		Grid spawnedGrid = Instantiate(_grassGridePrefap, pos, Quaternion.identity);
		
		List<GridType> possibelTypesOfGrid = GetPossiebelTypes(posInMatrix);
		GridType gridTyp = possibelTypesOfGrid[Random.Range(0, possibelTypesOfGrid.Count)];
		
		spawnedGrid.Init(gridTyp, posInMatrix, id);
		spawnedGrid.name = $"Grid {posInMatrix.x} {posInMatrix.y}";
		spawnedGrid.transform.parent = transform;
		_gridsMatrix[posInMatrix] = spawnedGrid;
		FocusOnGrid(posInMatrix);

		// Assignes spawnedGrid to List and Generatse new when nessasary
		if (_gridList.Count == id)
		{
			_gridList.Add(new List<Grid>());
		}
		_gridList[id].Add(spawnedGrid);

		int previousGridCount = _gridList[id].Count - 2;
		Grid prevGrid = null;
		if (previousGridCount >= 0)
		{
			Debug.Log($"<color=green>Grids Count({previousGridCount}) >= 0</color>");
			prevGrid = _gridList[id][previousGridCount];
		}
		else
		{
			Debug.Log($"<color=red>Grids Count({previousGridCount}) < 0</color>");
			if (_gridList[id -1].Count != 0)
			{
				prevGrid = _gridList[id - 1].Last();
			}
			else
			{
				prevGrid = _castleGrid;
			}
		}
		
		Vector2 previousGridPosition = prevGrid._pointInMatrix;

		PreviewTilesManager.Instance.MovePreviewTile(id,  gridTyp, posInMatrix, previousGridPosition);
	}

	public void GenerateCastle()
	{
		CastelGrid spawnedGrid = Instantiate(_castleGridPrefap, new Vector2(0,0), Quaternion.identity);
		List<GridType> possibleTypes = new List<GridType>() {GridType.BottomLeft, GridType.TopLeft, GridType.TopRight, GridType.BottomRight, GridType.Vertical, GridType.Horizontal};
		GridType gridTyp = possibleTypes[Random.Range(0, possibleTypes.Count)];
		spawnedGrid.Init(gridTyp, new Vector2(0, 0), 0);
		spawnedGrid.name = $"Castel 0 0";
		spawnedGrid.transform.parent = transform;
		_gridsMatrix[new Vector2(0, 0)] = spawnedGrid;

		_gridList.Add(new List<Grid>());
		_gridList[0].Add(spawnedGrid);

		Vector2 posOne = Vector2.zero;
		Vector2 posTwo = Vector2.zero;
		switch (gridTyp)
		{
			case GridType.Horizontal:
				posOne = new Vector2(-1, 0);
				posTwo = new Vector2(1, 0);
				break;
			case GridType.Vertical:
				posOne = new Vector2(0, 1);
				posTwo = new Vector2(0, -1);
				break;
			case GridType.TopLeft:
				posOne = new Vector2(0, 1);
				posTwo = new Vector2(-1, 0);
				break;
			case GridType.TopRight:
				posOne = new Vector2(0, 1);
				posTwo = new Vector2(1, 0);
				break;
			case GridType.BottomLeft:
				posOne = new Vector2(0, -1);
				posTwo = new Vector2(-1, 0);
				break;
			case GridType.BottomRight:
				posOne = new Vector2(0, -1);
				posTwo = new Vector2(1, 0);
				break;
		}
		PreviewTilesManager.Instance.SpawnPreviewTile(0, posOne, false);
		PreviewTilesManager.Instance.SpawnPreviewTile(1, posTwo, false);
		FocusOnGrid(new Vector2(0, 0));
		_castleGrid = spawnedGrid;
	}
	private List<GridType> GetPossiebelTypes(Vector2 pointInGridMatrix)
	{
		List<GridType> notPossibleTypes = new List<GridType>();

		Grid leftGrid = GetGrid(pointInGridMatrix + new Vector2(-1, 0));
		if (leftGrid != null)
		{
			if (leftGrid.HasConnectionToRight())
			{
				notPossibleTypes.Add(GridType.Vertical);
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.BottomRight);

				notPossibleTypes.Add(GridType.RightTopBottom);
			}
			else
			{
				notPossibleTypes.Add(GridType.Horizontal);
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.BottomLeft);

				notPossibleTypes.Add(GridType.LeftTopBottom);
				notPossibleTypes.Add(GridType.TopLeftRight);
				notPossibleTypes.Add(GridType.BottomLeftRight);
			}
		}
		Grid topGrid = GetGrid(pointInGridMatrix + new Vector2(0, 1));
		if (topGrid != null)
		{
			if (topGrid.HasConnectionToBottom())
			{
				notPossibleTypes.Add(GridType.BottomRight);
				notPossibleTypes.Add(GridType.BottomLeft);
				notPossibleTypes.Add(GridType.Horizontal);

				notPossibleTypes.Add(GridType.BottomLeftRight);
			}
			else
			{
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.Vertical);

				notPossibleTypes.Add(GridType.TopLeftRight);
				notPossibleTypes.Add(GridType.LeftTopBottom);
				notPossibleTypes.Add(GridType.RightTopBottom);
			}
		}
		Grid rightGrid = GetGrid(pointInGridMatrix + new Vector2(1, 0));
		if (rightGrid != null)
		{
			if (rightGrid.HasConnectionToLeft())
			{
				notPossibleTypes.Add(GridType.BottomLeft);
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.Vertical);

				notPossibleTypes.Add(GridType.LeftTopBottom);
			}
			else
			{
				notPossibleTypes.Add(GridType.BottomRight);
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.Horizontal);

				notPossibleTypes.Add(GridType.RightTopBottom);
				notPossibleTypes.Add(GridType.TopLeftRight);
				notPossibleTypes.Add(GridType.BottomLeftRight);
			}
		}
		Grid bottomGrid = GetGrid(pointInGridMatrix + new Vector2(0, -1));
		if (bottomGrid != null)
		{
			if (bottomGrid.HasConnectionToTop())
			{
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.Horizontal);

				notPossibleTypes.Add(GridType.TopLeftRight);
			}
			else
			{
				notPossibleTypes.Add(GridType.BottomLeft);
				notPossibleTypes.Add(GridType.BottomRight);
				notPossibleTypes.Add(GridType.Vertical);

				notPossibleTypes.Add(GridType.BottomLeftRight);
				notPossibleTypes.Add(GridType.LeftTopBottom);
				notPossibleTypes.Add(GridType.RightTopBottom);
			}
		}
		List<GridType> possibleTypes = new List<GridType>() {
			GridType.BottomLeft,
			GridType.TopLeft,
			GridType.TopRight,
			GridType.BottomRight,
			GridType.Vertical,
			GridType.Horizontal,
			GridType.BottomLeftRight,
			GridType.TopLeftRight,
			GridType.RightTopBottom,
			GridType.LeftTopBottom,
		};
		possibleTypes.RemoveAll(type => notPossibleTypes.Contains(type));
		// if (Random.Range(0, 100) > 75) possibleTypes.Remove(GridType.BottomLeftRight);
		// if (Random.Range(0, 100) > 75) possibleTypes.Remove(GridType.TopLeftRight);
		// if (Random.Range(0, 100) > 75) possibleTypes.Remove(GridType.LeftTopBottom);
		// if (Random.Range(0, 100) > 75) possibleTypes.Remove(GridType.RightTopBottom);
		return possibleTypes;
	}
	public Grid GetGrid(Vector2 pointInGridMatrix)
	{
		return _gridsMatrix.TryGetValue(pointInGridMatrix, out Grid grid) ? grid : null;
	}
	public void FocusOnGrid(Vector2 pointInGridMatrix)
	{
		Grid grid = GetGrid(pointInGridMatrix);
		if (grid != null)
		{
			CameraManger.instance.MoveTo(pointInGridMatrix);
		}
	}
	public CastelGrid GetCastle()
	{   
		return _castleGrid;
	}
}

public enum GridType
{
	Horizontal = 0,
	Vertical = 1,
	TopLeft = 2,
	TopRight = 3,
	BottomLeft = 4, 
	BottomRight = 5,

	BottomLeftRight = 6,
	TopLeftRight = 7,
	LeftTopBottom = 8,
	RightTopBottom = 9,
}