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

	public int columns = 9; 
	public int rows = 7; 
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count (1, 5); 
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles; 
	public GameObject[] outerWallTiles;
	public int boardAmount = 1;
	public GameObject Tile1;
	public GameObject Tile2;
	public GameObject Tile3;
	public GameObject Tile4;
	public GameObject Tile5;
	public GameObject Tile6;


	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList() {
		gridPositions.Clear ();
		//setup grid for Main Tile1
		for (int a = 0; a < 7; a++) {
			for (int b = 0; b < 9; b++) {
				//setting the grid position for random items/walls to spawn
				gridPositions.Add (new Vector3 (a, b, 0f));
			}
		}

		//setup grid for main tile 2
		for (int a = 7; a < 14; a++) {
			for (int b = 2; b < 11; b++) {
				//setting the grid position for random items/walls to spawn
				gridPositions.Add (new Vector3 (a, b, 0f));
			}
		}
		//setup grid for breakoff tile 1
		for (int a = 0; a < 5; a++) {
			for (int b = 9; b < 14; b++) {
				//setting the grid position for random items/walls to spawn
				gridPositions.Add (new Vector3 (a, b, 0f));
			}
		}
		//setup grid for breakoff tile 2
		for (int a = 5; a < 12; a++) {
			for (int b = 11; b < 15; b++) {
				//setting the grid position for random items/walls to spawn
				gridPositions.Add (new Vector3 (a, b, 0f));
			}
		}
		//setup grid for breakoff tile 3
		for (int a = 9; a < 14; a++) {
			for (int b = -3; b < 2; b++) {
				//setting the grid position for random items/walls to spawn
				gridPositions.Add (new Vector3 (a, b, 0f));
			}
		}
		//setup grid for breakoff tile 4
		for (int a = 2; a < 9; a++) {
			for (int b = -4; b < 0; b++) {
				//setting the grid position for random items/walls to spawn
				gridPositions.Add (new Vector3 (a, b, 0f));
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;
		/*for (int i = 0; i < 6; i++) {
			if (i < 3) {
				
				for (int x = -1; x < columns + 1; x++) {

					for (int y = -1; y < rows + 1; y++) {
						GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
						if (x == -1 ||  y == rows) {
							toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];
						}
						if (x + i == -1 || (x == columns && i == 2) || y == -1) {
							toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
						}
						GameObject instance = Instantiate (toInstantiate, new Vector3 (x + (9 * i), y, 0f), Quaternion.identity) as GameObject;

						instance.transform.SetParent (boardHolder);
					}
				}
			} else {
				for (int x = -1; x < columns + 1; x++) {

					for (int y = -1; y < rows + 1; y++) {
						GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
						if (x == -1) {
							toInstantiate = wallTiles [Random.Range (0, wallTiles.Length)];
						}
						if (x + (i - 3) == -1 || (x == columns && i == 5) || y == rows) {
							toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
						}
						GameObject instance = Instantiate (toInstantiate, new Vector3 (x + (9 * (i - 3)), y + 9, 0f), Quaternion.identity) as GameObject;

						instance.transform.SetParent (boardHolder);
					}
				}
			}

			boardAmount++;
		}*/
		//creat the Main Tile1
		for (int a = 0; a < 7; a++) {
			for (int b = 0; b < 9; b++) {
				//creats the floortile object
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(Tile1.transform);

			}
		}

		for (int b = -1; b < 15; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (-1, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		//Create the main tile 2
		for (int a = 7; a < 14; a++) {
			for (int b = 2; b < 11; b++) {
				//creats the floortile object
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(Tile2.transform);

			}
		}
			
		for (int b = -4; b < 12; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (14, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		//create the breakoff tile 1
		for (int a = 0; a < 5; a++) {
			for (int b = 9; b < 14; b++) {
				//creats the floortile object
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(Tile3.transform);

			}
		}

		for (int b = 2; b < 10; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, -5, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		for (int b = 9; b < 14; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, -4, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		for (int b = -5; b < 0; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (1, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}
		//create the breakoff tile 2
		for (int a = 5; a < 12; a++) {
			for (int b = 11; b < 15; b++) {
				//creats the floortile object
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(Tile4.transform);

			}
		}



		for (int b = 11; b < 16; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (12, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		//creat the breakoff tile 3
		for (int a = 9; a < 14; a++) {
			for (int b = -3; b < 2; b++) {
				//creats the floortile object
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(Tile5.transform);

			}
		}

		for (int a = 7; a < 9; a++) {
			for (int b = 0; b < 2; b++) {
				//creats the walltile object
				GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(boardHolder);

			}
		}

		for (int a = 5; a < 7; a++) {
			for (int b = 9; b < 11; b++) {
				//creats the walltile object
				GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(boardHolder);

			}
		}

		for (int b = 0; b < 5; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 14, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		for (int b = 4; b < 12; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 15, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		//create the breakoff tile 4
		for (int a = 2; a < 9; a++) {
			for (int b = -4; b < 0; b++) {
				//creats the floortile object
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (a, b, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(Tile6.transform);

			}
		}

		for (int b = 11; b < 12; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (13, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent (boardHolder);
		}

		for (int b = -1; b < 0; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (0, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent (boardHolder);
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
