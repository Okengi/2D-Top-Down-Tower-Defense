using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//-------  INSTANCE  -------
    public static GameManager instance;
	public GameState state;
	public static event Action<GameState> OnGameStateChange;
	//-------  TOWER  --------
	[SerializeField]
	private int money;

	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		UpdateGameState(GameState.PlaceNewGrid);
	}

	public void UpdateGameState(GameState newState)
	{
		state = newState;
		switch (newState)
		{
			case GameState.PlaceNewGrid:
				break;
			case GameState.PlaceUnits:
				break;
			case GameState.Wave:
				BuildingManager.instance.Deselect();
				break;
			case GameState.Death: 
				break;
		}
		OnGameStateChange?.Invoke(state);
	}

	public void NextState()
	{
		switch (state)
		{
			case GameState.PlaceNewGrid:
				UpdateGameState(GameState.PlaceUnits);
				break;
			case GameState.PlaceUnits:
				UpdateGameState(GameState.WavePreperations);
				break;
			case GameState.WavePreperations: 
				UpdateGameState(GameState.Wave); break;
			case GameState.Wave:
				UpdateGameState(GameState.PostWave);
				break;
			case GameState.PostWave:
				UpdateGameState(GameState.PlaceNewGrid); break;
			case GameState.Death:
				break;
				
		}
	}

	public void SpendMoney(int amount)
	{
		money -= amount;
		MenuManager.instance.UpdateStats();
	}
	public void EarnMoney(int amount)
	{
		money += amount;
		MenuManager.instance.UpdateStats();
	}
	public int GetMoney() { return money; }
}

public enum GameState
{
	PlaceNewGrid = 0,
	PlaceUnits = 1,
	WavePreperations = 2,
	Wave = 3,
	PostWave = 4,
	Death = 5
}