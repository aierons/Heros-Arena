using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MovingObject {

	//Number of points to add to player hp points when picking up a hp object.
	public int pointsPerFood = 1;
	//Number of points to add to player hp points when picking up a soda object.
	public int pointsPerSoda = 2;

	public enum Effects {ADV, DADV, STUN, DOUBLEDMG, BOOST, VBOOST};

	public List<Effects> effects;

	protected int HP = 8;
	protected int maxHP = 8;
	protected int DEF = 13;
	protected int SPEED = 5;
	protected int maxSPEED = 5;
	protected int ATK = 2;
	protected int DMG = 3;
	protected int wallDMG = 1;
	protected int RNG = 1;

	protected int Direction;

	public AudioClip moveSound1;
	//2 of 2 Audio clips to play when player moves.
	public AudioClip moveSound2;
	//1 of 2 Audio clips to play when player collects a hp object.
	public AudioClip eatSound1;
	//2 of 2 Audio clips to play when player collects a hp object.
	public AudioClip eatSound2;
	//1 of 2 Audio clips to play when player collects a soda object.
	public AudioClip drinkSound1;
	//2 of 2 Audio clips to play when player collects a soda object.
	public AudioClip drinkSound2;

	//Audio clip to play when player dies.
	public AudioClip gameOverSound;

	protected Animator animator;

	public GameObject team;
	protected TeamManager tman;

	//Getter and Setters
	public virtual string getHeroText() {
		return tag + " : " + HP + "/" + maxHP + " SPEED:" + SPEED;
	}


	public int getHP() {
		return HP;
	}

	public int getMaxHP() {
		return maxHP;
	}

	public int getDEF() {
		return DEF;
	}

	public int getSPEED() {
		return SPEED;
	}

	//Start overrides the Start function of MovingObject
    public override void Start ()
	{
		effects = new List<Effects> ();

		tman = team.GetComponent<TeamManager> ();

		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator> ();

		Direction = 0;

		//Call the Start function of the MovingObject base class.
		base.Start ();
	}
	
	virtual public void Update ()
	{
		CheckIfDead ();
		//If it's not the player's turn, exit the function.
		//print(this.tag + ":" +team.tag + ":" + (GameManager.instance.turn == team.tag).ToString() + " , " + (tman.turn == this.tag).ToString());
		if (!(GameManager.instance.turn == team.tag && tman.turn == this.tag)) {
			//print (this.tag + ":" +team.tag + ":" + "stuck");
			return;
		}

		if ((
			((Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D)) && team.tag == "Team1")
			|| ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) 
				&& team.tag == "Team2"))
			)  {
			//print (this.tag + ":" +team.tag + ":" + "reach");
			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;		//Used to store the vertical move direction.

			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int)(Input.GetAxisRaw ("Horizontal"));

			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int)(Input.GetAxisRaw ("Vertical"));

			//Check if moving horizontally, if so set vertical to zero.
			if (horizontal != 0) {
				vertical = 0;
			}

			if (vertical > 0)
			{
				animator.SetInteger("Direction", 2);
				Direction = 2;
			}
			else if (vertical < 0)
			{
				animator.SetInteger("Direction", 0);
				Direction = 0;
			}
			else if (horizontal > 0)
			{
				animator.SetInteger("Direction", 3);
				Direction = 3;
			}
			else if (horizontal < 0)
			{
				animator.SetInteger("Direction", 1);
				Direction = 1;
			}

			if (SPEED > 0) {
			//Check if we have a non-zero value for horizontal or vertical
			if (horizontal != 0 || vertical != 0) {
				//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
				//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
				AttemptMove<Wall> (horizontal, vertical);
			}
			}
		}
	}

	//AttemptMove overrides the AttemptMove function in the base class MovingObject
	//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
	protected override void AttemptMove <T> (int xDir, int yDir)
	{

		//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
		base.AttemptMove <T> (xDir, yDir);

		//Hit allows us to reference the result of the Linecast done in Move.
		RaycastHit2D hit;

		//If Move returns true, meaning Player was able to move into an empty space.
		if (Move (xDir, yDir, out hit)) {
			//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
			SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			SPEED--;
		}

		//Since the player has moved and lost hp points, check if the game has ended.
		CheckIfDead ();
	}

	//OnCantMove overrides the abstract function OnCantMove in MovingObject.
	//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
	protected override void OnCantMove <T> (T component)
	{
		//Set hitWall to equal the component passed in as a parameter.
		Wall hitWall = component as Wall;

		//Call the DamageWall function of the Wall we are hitting.
		if (hitWall.hp > wallDMG) {
			SPEED--;
		}
		hitWall.DamageWall (wallDMG);
		//Set the attack trigger of the player's animation controller in order to play the player's attack animation.
		animator.SetTrigger ("ATK");
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (HP == maxHP) {

		}
		//Check if the tag of the trigger collided with is hp.
		else if (other.tag == "Food") {
			//Add pointsPerhp to the players current hp total.
			if (HP + pointsPerFood > maxHP) {
				HP = maxHP;
			} else {
				HP += pointsPerFood;
			}
			tman.msgText.text = this.tag + " ate the food"; 

			//Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
			SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);

			//Disable the hp object the player collided with.
			other.gameObject.SetActive (false);
		}

		//Check if the tag of the trigger collided with is Soda.
		else if (other.tag == "Soda") {
			//Add pointsPerSoda to players hp points total
			if (HP + pointsPerSoda > maxHP) {
				HP = maxHP;
			} else {
				HP += pointsPerSoda;
			}

			tman.msgText.text = this.tag + " drank the potion"; 

			//Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);

			//Disable the soda object the player collided with.
			other.gameObject.SetActive (false);
		}
	}

	//Losehp is called when an enemy attacks the player.
	//It takes a parameter loss which specifies how many points to lose.
	public virtual void Losehp (int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger ("HIT");

		//Subtract lost hp points from the players total.
		HP -= loss;

		//Check to see if game has ended.
		CheckIfDead ();
	}

	public void Heal(int gain) {
		if (HP == maxHP) {
		} else if (HP + gain > maxHP) {
			HP = maxHP;
		} else {
			HP += pointsPerFood;
		}
	}


	private void CheckIfDead ()
	{
		//Check if hp point total is less than or equal to zero.
		if (HP <= 0) {
			//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
			SoundManager.instance.PlaySingle (gameOverSound);
			tman.CheckIfGameOver ();
			this.gameObject.SetActive (false);
		}
	}
	/*
	public virtual bool Attack() {
		if (GameManager.instance.turn == team.tag && tman.getCurrentHero ().tag == this.tag) {


		} else {
			return false;
		}
	}
*/
	public virtual bool Attack() {
		if (GameManager.instance.turn == team.tag && tman.getCurrentHero ().tag == this.tag) {
			GameObject t1 = tman.etman.captain;
			GameObject t2 = tman.etman.member1;
			GameObject t3 = tman.etman.member2;
			Hero e1 = t1.GetComponent<Hero>();
			Hero e2 = t2.GetComponent<Hero>();
			Hero e3 = t3.GetComponent<Hero>();

			if (Mathf.Abs (transform.position.x - t1.transform.position.x)
				+ Mathf.Abs (transform.position.y - t1.transform.position.y) <= RNG) {
				if (isHit (e1)) {
					if (effects.Contains(Effects.DOUBLEDMG)) {
						e1.Losehp (DMG * 2);
						effects.Remove (Effects.DOUBLEDMG);
					} else {
						e1.Losehp (DMG);
					}
					tman.msgText.text = this.tag + " landed a hit on " + e1.tag;
					animator.SetTrigger ("ATK");
				} else {
					tman.msgText.text = this.tag + " missed a hit on " + e1.tag;
				}
				return true;
			} 
			if (Mathf.Abs (transform.position.x - t2.transform.position.x)
				+ Mathf.Abs (transform.position.y - t2.transform.position.y) <= RNG) {
				if (isHit (e2)) {
					if (effects.Contains(Effects.DOUBLEDMG)) {
						e2.Losehp (DMG * 2);
						effects.Remove (Effects.DOUBLEDMG);
					} else {
						e2.Losehp (DMG);
					}
					tman.msgText.text = this.tag + " landed a hit on " + e2.tag;
					animator.SetTrigger ("ATK");
				} else {
					tman.msgText.text = this.tag + " missed a hit on " + e2.tag;
				}
				return true;
			} 
			if (Mathf.Abs (transform.position.x - t3.transform.position.x)
				+ Mathf.Abs (transform.position.y - t3.transform.position.y) <= RNG) {
				if (isHit (e3)) {
					if (effects.Contains(Effects.DOUBLEDMG)) {
						e3.Losehp (DMG * 2);
						effects.Remove (Effects.DOUBLEDMG);
					} else {
						e3.Losehp (DMG);
					}
					tman.msgText.text = this.tag + " landed a hit on " + e3.tag;
					animator.SetTrigger ("ATK");
				} else {
					tman.msgText.text = this.tag + " missed a hit on " + e3.tag;
				}
				return true;
			}

		}
		return false;
	}

	public virtual void EndTurn() {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			SPEED = maxSPEED;
			if (effects.Contains(Effects.STUN)) {
				effects.Remove(Effects.STUN);
			}
			if (effects.Contains (Effects.BOOST)) {
				effects.Remove (Effects.BOOST);
				ATK -= 1;
				DMG -= 1;
			}
			if (effects.Contains (Effects.VBOOST)) {
				effects.Remove (Effects.VBOOST);
			}
		}
	}

	virtual public bool Skill1 () {
		return false;
	}

	virtual public bool Skill2() {
		return false;
	}

	virtual public bool Ult() {
		return false;
	}


	//Buffs

	public bool isHit(Hero trgt) {
		int r = 10;
		if (effects.Contains (Effects.ADV)) {
			int rand1 = Random.Range (1, 21);
			int rand2 = Random.Range (1, 21);
			r = Mathf.Max (rand1, rand2);
			effects.Remove (Effects.ADV);
		} else if (effects.Contains (Effects.DADV)) {
			int rand1 = Random.Range (1, 21);
			int rand2 = Random.Range (1, 21);
			r = Mathf.Min (rand1, rand2);
			effects.Remove (Effects.DADV);
		} else {
			r = Random.Range (1, 21);
		}

		if (ATK + r > trgt.DEF) {
			return true;
		} else {
			return false;
		}
	}

	public void Boost() {
		effects.Add (Effects.BOOST);
		ATK += 1;
		DMG += 1;
	}

	public void ValorBoost() {
		effects.Add (Effects.VBOOST);
		ATK += 3;
		DMG += 2;
	}
	//Debuffs

	public void Stun() {
		SPEED /= 2;
		effects.Add (Effects.STUN);
	}

}