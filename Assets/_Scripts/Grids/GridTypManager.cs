using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTypManager : MonoBehaviour
{
	public static GridTypManager instance;

	public bool canSplit;

	private void Awake()
	{
		instance = this;
	}

	public GridType GetRandomPossiebelGridTyp(Vector2 pointInMatrix)
	{
		List<GridType> possibelTypesOfGrid = GetPossiebelTypes(pointInMatrix);
		return possibelTypesOfGrid[Random.Range(0, possibelTypesOfGrid.Count)];
	}

	public List<GridType> GetPossiebelTypes(Vector2 pointInGridMatrix)
	{
		if (canSplit)
		{
			return IncludingSplitingPathsGetPossibleTypes(pointInGridMatrix);
		}
		else
		{
			return ExcludingSplitingPathsGetPossibleTypes(pointInGridMatrix);
		}
	}
	public List<GridType> IncludingSplitingPathsGetPossibleTypes(Vector2 pointInGridMatrix)
	{
		List<GridType> notPossibleTypes = new List<GridType>();
		Grid leftGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(-1, 0));
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
		Grid topGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(0, 1));
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
		Grid rightGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(1, 0));
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
		Grid bottomGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(0, -1));
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
		List<GridType> possibleTypes = new List<GridType>() {
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
		possibleTypes.RemoveAll(type => notPossibleTypes.Contains(type));
		return possibleTypes;
	}
	public List<GridType> ExcludingSplitingPathsGetPossibleTypes(Vector2 pointInGridMatrix)
	{
		List<GridType> notPossibleTypes = new List<GridType>();
		Grid leftGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(-1, 0));
		if (leftGrid != null)
		{
			if (leftGrid.HasConnectionToRight())
			{
				notPossibleTypes.Add(GridType.Vertical);
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.BottomRight);
			}
			else
			{
				notPossibleTypes.Add(GridType.Horizontal);
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.BottomLeft);
			}
		}
		
		Grid topGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(0, 1));
		
		if (topGrid != null)
		{
			if (topGrid.HasConnectionToBottom())
			{
				notPossibleTypes.Add(GridType.BottomRight);
				notPossibleTypes.Add(GridType.BottomLeft);
				notPossibleTypes.Add(GridType.Horizontal);
			}
			else
			{
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.Vertical);
			}
		}
		Grid rightGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(1, 0));
		
		if (rightGrid != null)
		{
			if (rightGrid.HasConnectionToLeft())
			{
				notPossibleTypes.Add(GridType.BottomLeft);
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.Vertical);
			}
			else
			{
				notPossibleTypes.Add(GridType.BottomRight);
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.Horizontal);
			}
		}
		Grid bottomGrid = GridManager.instance.GetGrid(pointInGridMatrix + new Vector2(0, -1));
		
		if (bottomGrid != null)
		{
			if (bottomGrid.HasConnectionToTop())
			{
				notPossibleTypes.Add(GridType.TopLeft);
				notPossibleTypes.Add(GridType.TopRight);
				notPossibleTypes.Add(GridType.Horizontal);
			}
			else
			{
				notPossibleTypes.Add(GridType.BottomLeft);
				notPossibleTypes.Add(GridType.BottomRight);
				notPossibleTypes.Add(GridType.Vertical);
			}
		}
		List<GridType> possibleTypes = new List<GridType>() {
			GridType.BottomLeft,
			GridType.TopLeft,
			GridType.TopRight,
			GridType.BottomRight,
			GridType.Vertical,
			GridType.Horizontal,
		};
		possibleTypes.RemoveAll(type => notPossibleTypes.Contains(type));
		
		return possibleTypes;
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