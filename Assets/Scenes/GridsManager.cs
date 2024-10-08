using UnityEngine;

public class GridsManager : MonoBehaviour
{
	public int gridSizeX; // Number of cells in the x-axis
	public int gridSizeY; // Number of cells in the y-axis
	public float noiseScale;
	public TileTypeForTheExperimentelNoise[,] grid; // The 2D array to hold tile types

	public GameObject grassPrefab;
	public GameObject roadPrefab;
	public GameObject borderPrefab;
	void Start()
	{
		GenerateMap();
		SetCastleTiles(0,0);
		InstantiateTiles();
		// Additional setup or initialization
	}

	void GenerateMap()
	{
		float[,] noiseMap = new float[gridSizeX, gridSizeY];

		// Loop through map dimensions to generate Perlin noise values
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				float sampleX = x / noiseScale;
				float sampleY = y / noiseScale;

				noiseMap[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
			}
		}

		// Use the noise map to determine tile placement
		// (Continue to the next step)
		float borderThreshold = 0.5f;
		float roadThreshold = 0.3f;

		// Loop through the noise map to decide tile types
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				float currentNoise = noiseMap[x, y];

				if (currentNoise > borderThreshold)
				{
					grid[x, y] = TileTypeForTheExperimentelNoise.Border;
				}
				else if (currentNoise > roadThreshold)
				{
					grid[x,y] = TileTypeForTheExperimentelNoise.Road;
				}
				else
				{
					grid[x, y] = TileTypeForTheExperimentelNoise.Grass;
				}
			}
		}
	}

	void InitializeGrid()
	{
		grid = new TileTypeForTheExperimentelNoise[gridSizeX, gridSizeY];
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				grid[x, y] = TileTypeForTheExperimentelNoise.Grass;
			}
		}
		SetCastleTiles(2,2);

		InstantiateTiles();

	}

	void SetCastleTiles(int startX, int startY)
	{
		for (int x = startX; x < startX + 3; x++)
		{
			for (int y = startY; y < startY + 3; y++)
			{
				grid[x, y] = TileTypeForTheExperimentelNoise.Castle;
			}
		}
	}

	

	void InstantiateTiles()
	{
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 tilePosition = new Vector3(x,y,0); // Adjust as needed
				TileTypeForTheExperimentelNoise tileType = grid[x, y];

				GameObject tilePrefab;
				switch (tileType)
				{
					case TileTypeForTheExperimentelNoise.Road:
						tilePrefab = roadPrefab;
						break;
					case TileTypeForTheExperimentelNoise.Border:
						tilePrefab = borderPrefab;
						break;
					case TileTypeForTheExperimentelNoise.Castle:
						tilePrefab = borderPrefab;
						break;
					default:
						tilePrefab = grassPrefab;
						break;
				}

				GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
				// Further adjustments (scale, rotation, etc.) can be made here
			}
		}
	}

}
public enum TileTypeForTheExperimentelNoise
{
	Road,
	Border,
	Grass,
	Castle
}