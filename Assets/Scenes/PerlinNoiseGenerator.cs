using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{

		public int mapWidth;
		public int mapHeight;
		public float noiseScale;

		void Start()
		{
			GenerateMap();
		}

		void GenerateMap()
		{
			float[,] noiseMap = new float[mapWidth, mapHeight];

			// Loop through map dimensions to generate Perlin noise values
			for (int x = 0; x < mapWidth; x++)
			{
				for (int y = 0; y < mapHeight; y++)
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
		for (int x = 0; x < mapWidth; x++)
		{
			for (int y = 0; y < mapHeight; y++)
			{
				float currentNoise = noiseMap[x, y];

				if (currentNoise > borderThreshold)
				{
					// Place border or mountain tiles
					// Implement logic to set borders/mountains in your grid system
				}
				else if (currentNoise > roadThreshold)
				{
					// Place road tiles
					// Implement logic to set roads in your grid system
				}
				else
				{
					// Place grass tiles
					// Implement logic to set grass tiles in your grid system
				}
			}
		}
	}
	}

