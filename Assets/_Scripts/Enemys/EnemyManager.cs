using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
   public static EnemyManager instance;

	public Enemy[] enemies;
	public GameObject monster;
	List<EnemySpawnBiom> spawnBioms;

	private int waveIndex = 0;
	

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
				Prepar();
				break;
			case GameState.Wave:
				spawnBioms = GridManager.Instance.GetEnemySpawns();
				Wave();
				break;
			case GameState.Death:

				break;
		}
	}

	private void Prepar()
	{
		waveIndex++;

	}

	private void Wave()
	{
		spawnBioms[0].SpawnEnemys(monster, 1);
	}
}
