using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTypManager : MonoBehaviour
{
	public static GridTypManager instance;

	public bool canSplit = false;
	private int pathBranchCount = 0;
	private List<GridType> typesWithMultipleBranches = new List<GridType>();
	[SerializeField]
	List<GridType> notAllowed = new List<GridType>();
	[SerializeField] 
	List<GridType> rheinfolge = new List<GridType>();
	[SerializeField]
	public bool rheinfolgeEinhalten = false;



	public void SetPathBranchCount(int pathBranchCount) { this.pathBranchCount = pathBranchCount; }
	public void DecreasBranchCount() { pathBranchCount--; }

	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		typesWithMultipleBranches.Add(GridType.LeftTopBottom);
		typesWithMultipleBranches.Add(GridType.RightTopBottom);
		typesWithMultipleBranches.Add(GridType.BottomLeftRight);
		typesWithMultipleBranches.Add(GridType.TopLeftRight);
	}

	public GridType GetRandomPossiebelGridTyp(Vector2 pointInMatrix, List<GridType> excluding = null)
	{
		if(rheinfolge.Count > 0 && rheinfolgeEinhalten)
		{
			GridType d= rheinfolge[0];
			rheinfolge.RemoveAt(0);
			return d;
		}
		CheckBranchPossibility();
		List<GridType> possibelTypesOfGrid = GetPossiebelTypes(pointInMatrix);
		if (excluding != null)
		{
			possibelTypesOfGrid.RemoveAll(type => excluding.Contains(type));
		}
		GridType randGridTyp = possibelTypesOfGrid[Random.Range(0, possibelTypesOfGrid.Count)];
		if (typesWithMultipleBranches.Contains(randGridTyp)) { pathBranchCount--; }
		return randGridTyp;
	}

	private void CheckBranchPossibility()
	{
		if (pathBranchCount > 0) { 
			canSplit = true;
		}
		else
		{
			canSplit = false;
		}
	}
	
	List<GridType> possibleTypesExcludingSplitingPaths = new List<GridType>() {
			GridType.BottomLeft,
			GridType.TopLeft,
			GridType.TopRight,
			GridType.BottomRight,
			GridType.Vertical,
			GridType.Horizontal,
		};
	
	List<GridType> possibleTypesIncludingSplitingPaths = new List<GridType>() {
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
	
	
	public List<GridType> GetPossiebelTypes(Vector2 pointInGridMatrix)
	{
		List<GridType> notPossibleTypes = new List<GridType>();
		notPossibleTypes = GetNotPossibleTypes(pointInGridMatrix);
		List<GridType> possibleTypes;
		if (canSplit)
		{
			possibleTypes = new List<GridType>(possibleTypesIncludingSplitingPaths);
		}
		else
		{
			possibleTypes = new List<GridType>(possibleTypesExcludingSplitingPaths);
		}
		possibleTypes.RemoveAll(type => notPossibleTypes.Contains(type));
		possibleTypes.RemoveAll(type => notAllowed.Contains(type));
		return possibleTypes;
	}
	

	public List<GridType> GetNotPossibleTypes(Vector2 pointInGridMatrix)
	{
		List<GridType> notPossibleTypes = new List<GridType>();
		Biom leftGrid = GridManager.Instance.GetGrid(pointInGridMatrix + new Vector2(-1, 0));
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
		Biom topGrid = GridManager.Instance.GetGrid(pointInGridMatrix + new Vector2(0, 1));
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
		Biom rightGrid = GridManager.Instance.GetGrid(pointInGridMatrix + new Vector2(1, 0));
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
		Biom bottomGrid = GridManager.Instance.GetGrid(pointInGridMatrix + new Vector2(0, -1));
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
		return notPossibleTypes;
	}
	
	
	public List<Vector2> GetDirectionsOfType(GridType gridType)
	{
		List<Vector2> directions = new List<Vector2>();
		switch (gridType)
		{
			case GridType.TopLeft:
				directions.Add(new Vector2(0, 1));   // Top
				directions.Add(new Vector2(-1, 0));  // Left
				break;
			case GridType.TopRight:
				directions.Add(new Vector2(0, 1));   // Top
				directions.Add(new Vector2(1, 0));   // Right
				break;
			case GridType.BottomLeft:
				directions.Add(new Vector2(0, -1));  // Bottom
				directions.Add(new Vector2(-1, 0));  // Left
				break;
			case GridType.BottomRight:
				directions.Add(new Vector2(0, -1));  // Bottom
				directions.Add(new Vector2(1, 0));   // Right
				break;
			case GridType.Vertical:
				directions.Add(new Vector2(0, 1));   // Up
				directions.Add(new Vector2(0, -1));  // Down
				break;
			case GridType.Horizontal:
				directions.Add(new Vector2(1, 0));   // Right
				directions.Add(new Vector2(-1, 0));  // Left
				break;
			case GridType.BottomLeftRight:
				directions.Add(new Vector2(0, -1));  // Bottom
				directions.Add(new Vector2(-1, 0));  // Left
				directions.Add(new Vector2(1, 0));   // Right
				break;

			case GridType.TopLeftRight:
				directions.Add(new Vector2(0, 1));   // Top
				directions.Add(new Vector2(-1, 0));  // Left
				directions.Add(new Vector2(1, 0));   // Right
				break;

			case GridType.LeftTopBottom:
				directions.Add(new Vector2(-1, 0));  // Left
				directions.Add(new Vector2(0, 1));   // Top
				directions.Add(new Vector2(0, -1));  // Bottom
				break;

			case GridType.RightTopBottom:
				directions.Add(new Vector2(1, 0));   // Right
				directions.Add(new Vector2(0, 1));   // Top
				directions.Add(new Vector2(0, -1));  // Bottom
				break;
		}
		return directions;
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