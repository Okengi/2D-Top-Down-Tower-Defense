using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GridManager : MonoBehaviour
{
	public static GridManager Instance;
	public static int _width = 7, _height = 7;

	// Biom Management
	public Dictionary<Vector2, Biom> _gridsMatrix;
	[SerializeField]
	public List<List<Biom>> _gridList;

	[SerializeField]
	private List<EnemySpawnBiom> _enemySpawnBioms;
	[SerializeField]
	private List<EnemySpawnBiom> _staticEnemyBioms;

	// Variable Inputs (Prefaps)
	[Header("INPUT")]
	[SerializeField] private int gridCost = 12;

	private CastelBiom _castleGrid;


	private void Awake()
	{
		Instance = this;
		GameManager.OnGameStateChange += GameStateChanged;
	}
	private void OnDestroy()
	{
		GameManager.OnGameStateChange -= GameStateChanged;
	}
	private void Start()
	{
		_gridsMatrix = new Dictionary<Vector2, Biom>();
		_gridList = new List<List<Biom>>();
		GenerateCastle();
		_enemySpawnBioms = new List<EnemySpawnBiom>();
		_staticEnemyBioms = new List<EnemySpawnBiom>();
		EnemySpawnBiom defaultEnemySpawnOne = new EnemySpawnBiom(-1, new Vector2(500,0), _width, _height, GridType.Horizontal);
		EnemySpawnBiom defaultEnemySpawnTwo = new EnemySpawnBiom(-2, new Vector2(-500,0), _width, _height, GridType.Horizontal);
		_enemySpawnBioms.Add(defaultEnemySpawnOne);
		_enemySpawnBioms.Add(defaultEnemySpawnTwo);
	}

	public List<EnemySpawnBiom> GetEnemySpawns()
	{
		List<EnemySpawnBiom> enemySpawnBioms = new List<EnemySpawnBiom>();
		enemySpawnBioms.AddRange(_staticEnemyBioms);
		enemySpawnBioms.AddRange(_enemySpawnBioms);
		return enemySpawnBioms;
	}

	public Vector2 CalcGridPos(Vector2 tilePos)
	{
		int x = (int)Math.Floor(tilePos.x / _width);
		int y = (int)Math.Floor(tilePos.y / _height);
		return new Vector2(x, y);
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
					//Debug.Log($"Trying to move a enemyBiom to {preview._gridPos}");
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

	public void RemoveEnemyBiom()
	{
		int index = _enemySpawnBioms.Count;
		_enemySpawnBioms[index - 1].SelfDestroy();
		_enemySpawnBioms.RemoveAt(index - 1);
	}

	public void ConvertToStaticEnemyBiom(Vector2 gridPos)
	{
		int index = _enemySpawnBioms.Count;
		EnemySpawnBiom biom = _enemySpawnBioms[index - 1];
		_enemySpawnBioms.RemoveAt(index - 1);

		_staticEnemyBioms.Add(biom);
		biom.MoveTo(gridPos);
		_gridsMatrix.Add(gridPos, biom);
	}

	// Diffrent GridBranches ----------------------------------------------------
	public Biom LastGrid (int gridBranchID)
	{
		return _gridList[gridBranchID].Last<Biom>();
	}
	public void AddGridBranch()
	{
		_gridList.Add(new List<Biom>());
		_enemySpawnBioms.Add(new EnemySpawnBiom((_enemySpawnBioms.Count + 1)* -1, new Vector2(1000, 1000), _width, _height, GridType.Horizontal));
	}
	// --------------------------------------------------------------------------

	public Biom GetGrid(Vector2 pointInGridMatrix)
	{
		return _gridsMatrix.TryGetValue(pointInGridMatrix, out Biom grid) ? grid : null;
	}
	public void FocusOnGrid(Vector2 pointInGridMatrix)
	{
		Biom grid = GetGrid(pointInGridMatrix);
		if (grid != null)
		{
			CameraManger.instance.MoveTo(pointInGridMatrix);
		}
	}
	public CastelBiom GetCastleGrid()
	{   
		return _castleGrid;
	}

	public void GenerateGrassBiom(Vector2 posInMatrix, int id)
	{	
		// Check if Player has enough Money
		if (GameManager.instance.GetMoney() - gridCost >= 0) { GameManager.instance.SpendMoney(gridCost); }
		else { return; }

		GridType gridType = GridTypManager.instance.GetRandomPossiebelGridTyp(posInMatrix);

		Biom biom = new GrassGrid(id, posInMatrix, _width, _height, gridType);
		_gridsMatrix.Add(posInMatrix, biom);
		FocusOnGrid(posInMatrix);

		// Assignes spawnedGrid to GridBranch and Generatse new when nessasary
		if (_gridList.Count == id)
		{
			_gridList.Add(new List<Biom>());
		}
		_gridList[id].Add(biom);

		PreviewTilesManager.Instance.MovePreviewGrid(id, posInMatrix, GridTypManager.instance.GetDirectionsOfType(gridType));
	}

	public void GenerateCastle()
	{
		List<GridType> SplitingPaths = new List<GridType>() {
			GridType.BottomLeftRight,
			GridType.TopLeftRight,
			GridType.RightTopBottom,
			GridType.LeftTopBottom,
		};
		GridType gridType = GridTypManager.instance.GetRandomPossiebelGridTyp(new Vector2(0, 0), SplitingPaths);
		Biom castleBiom = new CastelBiom(id: 0, new Vector2(0, 0), _width, _height, gridType);

		_gridsMatrix[new Vector2(0, 0)] = castleBiom;
		_castleGrid = (CastelBiom)castleBiom;
		_gridList.Add(new List<Biom>());
		_gridList[0].Add(_castleGrid);

		List<Vector2> directions = GridTypManager.instance.GetDirectionsOfType(gridType);
		
		PreviewTilesManager.Instance.SpawnPreviewGrid(0, directions[0], false);
		PreviewTilesManager.Instance.SpawnPreviewGrid(1, directions[1], false);

		FocusOnGrid(new Vector2(0, 0));
	}

}

enum GridBiom
{
	Castle,
	Grass,
	Enemy
}

