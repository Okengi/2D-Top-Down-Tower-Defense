using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
   public static EnemyManager instance;

	public Enemy[] enemies;

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
				Spawn(GridManager.instance.LastGrid(0));
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
			Instantiate(enemies[0], g._pointInMatrix * 7 + re[0], Quaternion.identity);
		}
		else
		{
			Vector2 pos = grid.GetLastRoadTile()._position;
			Instantiate(enemies[0], grid._pointInMatrix *7 + pos, Quaternion.identity);
		}
		
	}
}
