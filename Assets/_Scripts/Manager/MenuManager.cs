using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager instance;

	public TextMeshProUGUI stateText;
	public TextMeshProUGUI moneyText;

	public GameObject buildingsPanel;
	public GameObject enemyPanel;

	private void Awake()
	{
		instance = this;
		GameManager.OnGameStateChange += UpdateGameState;
	}

	private void Start()
	{
		UpdateStats();
	}

	public void UpdateStats()
	{
		stateText.text = GameManager.instance.state.ToString();
		moneyText.text = GameManager.instance.GetMoney().ToString();
	}

	private void UpdateGameState(GameState newState)
	{
		switch (newState)
		{
			case GameState.PlaceNewGrid:
				stateText.text = "PlaceNewGrid";
				buildingsPanel.SetActive(false);
				enemyPanel.SetActive(false);
				break;
			case GameState.PlaceUnits:
				stateText.text = "PlaceUnits";
				buildingsPanel.SetActive(true);
				enemyPanel.SetActive(false);
				break;
			case GameState.WavePreperations:
				stateText.text = "EnemysMakePreperations";
				buildingsPanel.SetActive(false);
				enemyPanel.SetActive(true);
				break;
			case GameState.Wave:
				stateText.text = "Wave";
				buildingsPanel.SetActive(false);
				enemyPanel.SetActive(true);
				break;
			case GameState.PostWave:
				stateText.text = "Fight Aftermath";
				buildingsPanel.SetActive(false);
				enemyPanel.SetActive(true);
				break;
			case GameState.Death:
				stateText.text = "Death";
				buildingsPanel.SetActive(false);
				enemyPanel.SetActive(false);
				break;
		}
	}
}
