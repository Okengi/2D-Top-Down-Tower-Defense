using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
   public static EnemyManager instance;

	public Enemy[] enemies;
	private bool Spawnd = false;
	private Enemy eny = null;

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
			case GameState.Wave:
				if (!Spawnd) {
					Grid spawnOn = GridManager.instance.LastGrid(0);
					Debug.Log($"<color=Color.Red>Spaen Enemy at: {spawnOn._pointInMatrix}");
					Spawn(spawnOn);
					Spawnd = true;
				}
				else
				{
					eny.MoveOneTileForward();
				}
				break;
			case GameState.Death:

				break;
		}
	}

	public void Spawn(Grid grid)
	{
		if (grid._pointInMatrix == new Vector2(0,0)) {
			CastelGrid g = GridManager.instance.GetCastle();
			Vector2[] re = g.LastRoadTile();
			eny = Instantiate(enemies[0], g._pointInMatrix * 7 + re[0], Quaternion.identity);
		}
		else
		{
			Vector2 pos = grid.GetLastRoadTile()._position;
			eny = Instantiate(enemies[0], grid._pointInMatrix *7 + pos, Quaternion.identity);
		}
		eny.Initialize(grid);
	}
}
