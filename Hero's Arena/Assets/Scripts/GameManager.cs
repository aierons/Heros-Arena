using UnityEngine;
using System.Collections;


using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.

public class GameManager : MonoBehaviour
{
	public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
	public float turnDelay = 0.1f;                          //Delay between each Player turn.
	public int playerhp = 100;                      //Starting value for Player food points.
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	[HideInInspector] public bool playersTurn = true;       //Boolean to check if it's players turn, hidden in inspector but public.


	private Text levelText;                                 //Text to display current level number.
	private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	private int level = 1;                                  //Current level number, expressed in game as "Day 1".
	//private List<Enemy> enemies;                          //List of all Enemy units, used to issue them move commands.
	private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.

	//Awake is always called before any Start functions
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

	//This is called each time a scene is loaded.
	void OnLevelWasLoaded(int index)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
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

	//Update is called every frame.
	void Update()
	{
		//Check that playersTurn or doingSetup are not currently true.
		if(playersTurn  || doingSetup)

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