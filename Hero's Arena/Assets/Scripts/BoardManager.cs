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

		//all of the next for loops are used to set up the outer walls of the areana
		for (int b = -1; b < 10; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (-1, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		for (int b = 0; b < 12; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (14, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(boardHolder);

		}

		for (int b = 0; b < 5; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 14, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile3.transform);

		}

		for (int b = 10; b < 15; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (-1, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile3.transform);

		}

		for (int b = 11; b < 16; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (12, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile4.transform);

		}

		for (int b = 4; b < 12; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 15, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile4.transform);

		}

		for (int b = -4; b < 0; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (14, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile5.transform);

		}

		for (int b = 9; b < 14; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, -4, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile5.transform);

		}

		for (int b = 2; b < 10; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (b, -5, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile6.transform);

		}

		for (int b = -5; b < 0; b++) {
			//creats the walltile object
			GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
			//puts the object in its place
			GameObject instance = Instantiate (toInstantiate, new Vector3 (1, b, 0f), Quaternion.identity) as GameObject;
			//sets the parent as an object
			instance.transform.SetParent(Tile6.transform);

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

	//lays out random objects in the map based on input(used for random breakable walls and food pickupsw)
	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max) {
		int objectCount = Random.Range (min, max + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			GameObject instance = Instantiate (tileChoice, randomPosition, Quaternion.identity) as GameObject;
			if((0f <= randomPosition.x && randomPosition.x <= 5f) && (9f <= randomPosition.y && randomPosition.y <= 14f)) {
				instance.transform.SetParent (Tile3.transform);
			}
			if((5f <= randomPosition.x && randomPosition.x <= 12f) && (11f <= randomPosition.y && randomPosition.y <= 15f)) {
				instance.transform.SetParent (Tile4.transform);
			}
			if((2f <= randomPosition.x && randomPosition.x <= 9f) && (-4f <= randomPosition.y && randomPosition.y <= 0f)) {
				instance.transform.SetParent (Tile6.transform);
			}
			if((9f <= randomPosition.x && randomPosition.x <= 14f) && (-3f <= randomPosition.y && randomPosition.y <= 2f)) {
				instance.transform.SetParent (Tile5.transform);
			}
		}
	}

	public void SetupScene(int level) {
		BoardSetup();
		InitializeList ();
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
	}

	//used to remove one of the tiles when a charecter dies
	public void removeTile() {
		GameObject[] Team1 = GameObject.FindGameObjectsWithTag ("Team1");
		GameObject[] Team2 = GameObject.FindGameObjectsWithTag ("Team2");
		List<GameObject> heros = new List<GameObject> ();
		heros.Add (Team1[0].GetComponent<TeamManager>().captain);
		heros.Add (Team2[0].GetComponent<TeamManager>().captain);
		heros.Add (Team1[0].GetComponent<TeamManager>().member1);
		heros.Add (Team1[0].GetComponent<TeamManager>().member2);
		heros.Add (Team2[0].GetComponent<TeamManager>().member1);
		heros.Add (Team2[0].GetComponent<TeamManager>().member2);
		//randomly decides which tile to remove
		int randTile = Random.Range (0, 3);
		switch (randTile) {
		case 0:
			Tile4.SetActive (false);
			//sets the hero back to spawn location if on the tile being removed
			foreach (GameObject a in heros) {
				if ((5f <= a.transform.position.x && a.transform.position.x <= 12f)
				   && (11f <= a.transform.position.y && a.transform.position.y <= 15f)) {
					a.transform.position = a.GetComponent<Hero> ().getStartPosition ();
				}
			}
			//creates a new outerwall after the tile is removed
			for (int b = 5; b < 13; b++) {
				//creats the walltile object
				GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 11, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent (boardHolder);
			}
			//if the breakoff tile adjacent to this one has not been removed add an outer wall to it
			if (Tile3.activeSelf) {
				for (int b = 11; b < 15; b++) {
					//creats the walltile object
					GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					//puts the object in its place
					GameObject instance = Instantiate (toInstantiate, new Vector3 (5, b, 0f), Quaternion.identity) as GameObject;
					//sets the parent as an object
					instance.transform.SetParent(Tile3.transform);

				}
			}
			break;
		case 1:
			Tile5.SetActive (false);
			//sets the hero back to spawn location if on the tile being removed
			foreach(GameObject a in heros) {
				if((9f <= a.transform.position.x && a.transform.position.x <= 14f)
					&& (-3f <= a.transform.position.y && a.transform.position.y <= 2f)) {
					a.transform.position = a.GetComponent<Hero> ().getStartPosition ();
				}
			}
			//creates a new outerwall after the tile is removed
			for (int b = 9; b < 14; b++) {
				//creats the walltile object
				GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 1, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent(boardHolder);

			}
			//if the breakoff tile adjacent to this one has not been removed add an outer wall to it
			if (Tile6.activeSelf) {
				for (int b = -4; b < 0; b++) {
					//creats the walltile object
					GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					//puts the object in its place
					GameObject instance = Instantiate (toInstantiate, new Vector3 (9, b, 0f), Quaternion.identity) as GameObject;
					//sets the parent as an object
					instance.transform.SetParent(Tile6.transform);

				}
			}
			break;
		case 2:
			Tile6.SetActive (false);
			//sets the hero back to spawn location if on the tile being removed
			foreach (GameObject a in heros) {
				if ((2f <= a.transform.position.x && a.transform.position.x <= 9f)
				    && (-4f <= a.transform.position.y && a.transform.position.y <= 0f)) {
					a.transform.position = a.GetComponent<Hero> ().getStartPosition ();
				}
			}
			//creates a new outerwall after the tile is removed
			for (int b = 2; b < 10; b++) {
				//creats the walltile object
				GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (b, -1, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent (boardHolder);

			}
			//if the breakoff tile adjacent to this one has not been removed add an outer wall to it
			if (Tile5.activeSelf) {
				for (int b = -5; b < 0; b++) {
					//creats the walltile object
					GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					//puts the object in its place
					GameObject instance = Instantiate (toInstantiate, new Vector3 (8, b, 0f), Quaternion.identity) as GameObject;
					//sets the parent as an object
					instance.transform.SetParent(Tile5.transform);

				}
			}
			break;
		case 3:
			Tile3.SetActive (false);
			//sets the hero back to spawn location if on the tile being removed
			foreach (GameObject a in heros) {
				if ((0f <= a.transform.position.x && a.transform.position.x <= 5f)
				   && (9f <= a.transform.position.y && a.transform.position.y <= 14f)) {
					a.transform.position = a.GetComponent<Hero> ().getStartPosition ();
				}
			}
			//creates a new outerwall after the tile is removed
			for (int b = 0; b < 5; b++) {
				//creats the walltile object
				GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				//puts the object in its place
				GameObject instance = Instantiate (toInstantiate, new Vector3 (b, 10, 0f), Quaternion.identity) as GameObject;
				//sets the parent as an object
				instance.transform.SetParent (boardHolder);

			}
			//if the breakoff tile adjacent to this one has not been removed add an outer wall to it
			if (Tile4.activeSelf) {
				for (int b = 10; b < 15; b++) {
					//creats the walltile object
					GameObject toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					//puts the object in its place
					GameObject instance = Instantiate (toInstantiate, new Vector3 (4, b, 0f), Quaternion.identity) as GameObject;
					//sets the parent as an object
					instance.transform.SetParent(Tile4.transform);

				}
			}
			break;
		}
	}
}
