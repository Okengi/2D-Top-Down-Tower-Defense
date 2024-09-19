using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
   public static EnemyManager instance;

	public Enemy[] enemies;
	private bool Spawnd = false;
	private Enemy debugEnemy = null;

	private void Awake()
	{
		instance = this;
		GameManager.OnGameStateChange += GameStateChanged;
	}
	private void OnDestroy()
	{
		GameManager.OnGameStateChange -= GameStateChanged;
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
				
				break;
			case GameState.Wave:
				if (!Spawnd)
				{
					Grid spawnOn = GridManager.instance.LastGrid(0);
					Debug.Log($"<color=Red>Spawn Enemy at: {spawnOn.name}</color>");
					Spawn(spawnOn);
					Spawnd = true;
				}
				break;
			case GameState.Death:

				break;
		}
	}

	public void Spawn(Grid grid)
	{
		Tile spawnedOn;
		if (grid._pointInMatrix == new Vector2(0,0)) {
			CastelGrid g = GridManager.instance.GetCastle();
			Vector2Int[] re = g.LastRoadTile();
			debugEnemy = Instantiate(enemies[0], g._pointInMatrix * 7 + re[0], Quaternion.identity);
			spawnedOn = TileManager.Instance.GetTile(re[0]);
		}
		else
		{
			Vector2Int pos = new Vector2Int(grid.GetLastRoadTile().GetXPosition(), grid.GetLastRoadTile().GetYPosition());
			spawnedOn = TileManager.Instance.GetTile(pos);
			debugEnemy = Instantiate(enemies[0], grid._pointInMatrix *7 + pos, Quaternion.identity);
		}
		//Debug.Log($"<color=red>{spawnedOn.name}, {spawnedOn._positionInTileMatrix}</color>");
		debugEnemy.Initialize(spawnedOn);
	}
}
