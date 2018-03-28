using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]

	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8; 
	public int rows = 8; 
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count (1, 5); 
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles; 
	public GameObject[] outerWallTiles;
	public int boardAmount = 1;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList() {
		gridPositions.Clear ();

		for (int i = 0; i < 6; i++) {
			if (i < 3) {
				for (int x = 1; x < columns - 1; x++) {

					for (int y = 1; y < rows - 1; y++) {
						gridPositions.Add (new Vector3 (x + (8 * i), y, 0f));
					}
				}
			} else {
				for (int x = 1; x < columns - 1; x++) {

					for (int y = 1; y < rows - 1; y++) {
						gridPositions.Add (new Vector3 (x + (8 * (i - 3)), y + 9, 0f));
					}
				}
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
		for (int i = 0; i < 6; i++) {
			if (i < 3) {
				
				for (int x = -1; x < columns + 1; x++) {

					for (int y = -1; y < rows + 1; y++) {
						GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
						/*if (x == -1 || x == columns || y == -1 || y == rows) {
							toInstantiate = outerWallTiles [Random.Range (0, wallTiles.Length)];
						}*/
						if (x + i == -1 || x == columns || y == -1) {
							toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
						}
						GameObject instance = Instantiate (toInstantiate, new Vector3 (x + (8 * i), y, 0f), Quaternion.identity) as GameObject;

						instance.transform.SetParent (boardHolder);
					}
				}
			} else {
				for (int x = -1; x < columns + 1; x++) {

					for (int y = -1; y < rows + 1; y++) {
						GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
						if (x == -1 || x == columns || y == -1 || y == rows) {
							toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];
						}
						if (x + (i - 3) == -1 || x == columns || y == rows) {
							toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
						}
						GameObject instance = Instantiate (toInstantiate, new Vector3 (x + (8 * (i - 3)), y + 9, 0f), Quaternion.identity) as GameObject;

						instance.transform.SetParent (boardHolder);
					}
				}
			}
			boardAmount++;
		}
	}

	Vector3 RandomPosition() {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max) {
		int objectCount = Random.Range (min, max + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {
		BoardSetup();
		InitializeList ();
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
	}
}
