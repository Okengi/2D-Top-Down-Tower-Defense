using System;
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

	private List<EnemySpawnBiom> _enemySpawnBioms;

	// Variable Inputs (Prefaps)
	[Header("INPUT")]
	[SerializeField] private Grid _grassGridePrefap;
	[SerializeField] private CastelGrid _castleGridPrefap;
	[SerializeField] private int gridCost = 12;

	private CastelGrid _castleGrid;

	private void Awake()
	{
		instance = this;
		GameManager.OnGameStateChange += GameStateChanged;
	}
	private void OnDestroy()
	{
		GameManager.OnGameStateChange -= GameStateChanged;
	}
	private void Start()
	{
		_gridsMatrix = new Dictionary<Vector2, Grid>();
		_gridList = new List<List<Grid>>();
		GenerateCastle();
		_enemySpawnBioms = new List<EnemySpawnBiom>();
		EnemySpawnBiom defaultEnemySpawnOne = new EnemySpawnBiom(new Vector2(1000,1000), _width, _height);
		EnemySpawnBiom defaultEnemySpawnTwo = new EnemySpawnBiom(new Vector2(1008,1000), _width, _height);
		_enemySpawnBioms.Add(defaultEnemySpawnOne);
		_enemySpawnBioms.Add(defaultEnemySpawnTwo);
	}



	private void GameStateChanged(GameState newState)
	{
		switch (newState)
		{
			case GameState.PlaceNewGrid:
	
				break;
			case GameState.PlaceUnits:

				break;
			case GameState.WavePreperations:
				int i = 0;
				foreach(PreViewGrid preview in PreviewTilesManager.Instance.GetAllPreviewGrids())
				{
					Debug.Log($"Trying to move a enemyBiom to {preview._gridPos}");
					_enemySpawnBioms[i].MoveTo(preview._gridPos);
					i++;
				}
				break;
			case GameState.Wave:

				break;
			case GameState.PostWave:
				foreach (EnemySpawnBiom enemySpawnBiom in _enemySpawnBioms)
				{
					enemySpawnBiom.Hide();
				}
				break;
			case GameState.Death:

				break;
		}
	}

	// Diffrent GridBranches ----------------------------------------------------
	public Grid LastGrid (int gridBranchID)
	{
		return _gridList[gridBranchID].Last<Grid>();
	}
	public void AddGridBranch()
	{
		_gridList.Add(new List<Grid>());
		_enemySpawnBioms.Add(new EnemySpawnBiom(new Vector2(1000, 1000), _width, _height));
	}
	// --------------------------------------------------------------------------

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

	public void GenerateGrid(Vector2 posInMatrix, int id)
	{	
		// Check if Player has enough Money
		if (GameManager.instance.GetMoney() - gridCost >= 0) { GameManager.instance.SpendMoney(gridCost); }
		else { return; }
		// Spawning + Nameing + Setup
		Vector2 pos = new Vector2(posInMatrix.x * _width, posInMatrix.y * _height);
		Grid spawnedGrid = Instantiate(_grassGridePrefap, pos, Quaternion.identity);

		GridType gridType = GridTypManager.instance.GetRandomPossiebelGridTyp(posInMatrix);

		spawnedGrid.Init(gridType, posInMatrix, id);
		spawnedGrid.name = $"Grid {posInMatrix.x} {posInMatrix.y}";
		spawnedGrid.transform.parent = transform;

		_gridsMatrix[posInMatrix] = spawnedGrid;
		FocusOnGrid(posInMatrix);

		// Assignes spawnedGrid to GridBranch and Generatse new when nessasary
		if (_gridList.Count == id)
		{
			_gridList.Add(new List<Grid>());
		}
		_gridList[id].Add(spawnedGrid);

		PreviewTilesManager.Instance.MovePreviewTile(id, posInMatrix, gridType);
	}

	public void GenerateCastle()
	{
		CastelGrid spawnedGrid = Instantiate(_castleGridPrefap, new Vector2(0,0), Quaternion.identity);

		List<GridType> possibleTypes = new List<GridType>() {GridType.BottomLeft, GridType.TopLeft, GridType.TopRight, GridType.BottomRight, GridType.Vertical, GridType.Horizontal};
		GridType gridTyp = possibleTypes[UnityEngine.Random.Range(0, possibleTypes.Count)];

		spawnedGrid.Init(gridTyp, new Vector2(0, 0), 0);
		spawnedGrid.name = $"Castel 0 0";
		spawnedGrid.transform.parent = transform;
		_gridsMatrix[new Vector2(0, 0)] = spawnedGrid;
		_castleGrid = spawnedGrid;
		_gridList.Add(new List<Grid>());
		_gridList[0].Add(_castleGrid);

		List<Vector2> directions = GridTypManager.instance.GetDirectionsOfType(gridTyp);
		
		PreviewTilesManager.Instance.SpawnPreviewGrid(0, directions[0], false);
		PreviewTilesManager.Instance.SpawnPreviewGrid(1, directions[1], false);

		FocusOnGrid(new Vector2(0, 0));
	}

	public void MergedTwoPreviews()
	{
		_enemySpawnBioms[0].SelfDestroy();
		_enemySpawnBioms.RemoveAt(0);
	}
}

enum GridBiom
{
	Castle,
	Grass,
	EnemyHedgh
}

