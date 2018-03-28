using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MatchManager : MonoBehaviour {

	public float levelStartDelay = 2f;                      
	public float turnDelay = 0.1f;                          
	//public int Team1HP = 3;
	//public int Team2HP = 3;
	public static MatchManager instance = null;   


	[HideInInspector] public string turn = "Team1";

	private int level = 1; 

	private BoardManager boardScript;
	private bool doingSetup = true;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Assign enemies to a new List of Enemy objects.
		//enemies = new List<Enemy>();

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	//Initializes the game for each level.
	void InitGame()
	{
		//While doingSetup is true the player can't move, prevent player from moving while title card is up.
		doingSetup = true;

		//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
		Invoke("HideLevelImage", levelStartDelay);

		//Clear any Enemy objects in our List to prepare for next level.
		//enemies.Clear();

		//Call the SetupScene function of the BoardManager script, pass it current level number.
		boardScript.SetupScene(level);

	}

	//Hides black image used between levels
	void HideLevelImage()
	{

		//Set doingSetup to false allowing player to move again.
		doingSetup = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Check that playersTurn or doingSetup are not currently true.
		if(turn == "Team1"  || doingSetup)

			//If any of these are true, return and do not start MoveEnemies.
			return;
	}


	//GameOver is called when the player reaches 0 hp points
	public void GameOver()
	{
		//Disable this GameManager.
		enabled = false;
	}
}
