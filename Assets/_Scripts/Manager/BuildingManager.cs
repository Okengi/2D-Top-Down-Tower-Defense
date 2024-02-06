using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	public static BuildingManager instance;
	private Building buildingToPlace;
	private Building follow;
	[SerializeField] private Building firstBuilding;

	private void Awake()
	{
		instance = this;
	}

	public void Build(Vector2 _gridPos, Vector2 _tilePos)
	{
		if(GameManager.instance.GetMoney()-buildingToPlace.cost >= 0)
		{
			GameManager.instance.SpendMoney(buildingToPlace.cost);
			var spawnedBuilding = Instantiate(buildingToPlace, _gridPos * 7 + _tilePos, Quaternion.identity);
		}
	}

	public void SelectFirst()
	{
		buildingToPlace = firstBuilding;
		follow = Instantiate(buildingToPlace, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
		follow.gameObject.GetComponent<BoxCollider2D>().enabled = false;
	}

	private void Update()
	{
		//if (Input.GetMouseButtonDown(0)) Debug.Log("Pressed left click.");
		if (Input.GetMouseButtonDown(1)) { 
			Deselect();
		}
		//if (Input.GetMouseButtonDown(2)) Debug.Log("Pressed middle click.");
		if (follow != null) { 
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0f;
			follow.gameObject.transform.position = pos;
		}
	}

	public void Deselect()
	{
		buildingToPlace = null;
		if(follow != null) Destroy(follow.gameObject);
		follow = null;
	}
}
