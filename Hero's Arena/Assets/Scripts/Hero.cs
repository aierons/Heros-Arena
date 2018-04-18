using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MovingObject {

	//Number of points to add to player hp points when picking up a hp object.
	public int pointsPerFood = 1;
	//Number of points to add to player hp points when picking up a soda object.
	public int pointsPerSoda = 2;

	public int ult = 0;

	public enum Effects {ADV, DADV, STUN, DOUBLEDMG, BOOST, VBOOST, RNGUP, PSN, FROZEN, BURN, STATIC, ELIXIR};

	public List<Effects> effects;
	/*
	protected int HP = 8;
	protected int maxHP = 8;
	protected int DEF = 13;
	protected int SPEED = 5;
	protected int maxSPEED = 5;
	protected int ATK = 2;
	protected int DMG = 3;
	protected int wallDMG = 1;
	protected int RNG = 1;
*/
	protected float EV = 1f; // (.7 <STUN- 1 -QUICK> 1.1)
	protected float ACCb = .90f; // unique for each character
	protected float ACC = 1f; // (.7 <DISADV- 1 -ADV> 1.1)

	protected int ATK = 18;
	protected int DEF = 16;
	protected int DMG = 80;

	protected int HP = 300;
	protected int maxHP = 300;
	protected int SPEED = 15;
	protected int maxSPEED = 15;
	protected int wallDMG = 2;
	protected int RNG = 1;

	protected int psn_count  = 0;
	protected int elixir_count = 0;
	protected int brn_count = 0;
	protected int static_count = 0;

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

	public GameObject Target;
	protected bool targeting;
	protected bool tileTargeting;
	//0 = Attack, 1 = Skill 1, 2 = Skill 2, 3 = Ultimate
	protected int targetingType;
	protected List<Hero> targets;
	protected List<GameObject> tileTargets;
	protected int currentTarget = 0;
	protected Hero selectedTarget;
	protected GameObject selectedTile;

	//protected GameObject selectedTiletarget;

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

		checkStatic ();

		if ((
			((Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D)) && team.tag == "Team1")
			|| ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) 
				&& team.tag == "Team2")) && !targeting
			)  {
			if (effects.Contains (Effects.FROZEN)) {
				tman.msgText.text = this.tag + " is frozen and unable to move";
				//EndTurn ();
				SPEED = 0;
			} else {
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

				if (vertical > 0) {
					animator.SetInteger ("Direction", 2);
					Direction = 2;
				} else if (vertical < 0) {
					animator.SetInteger ("Direction", 0);
					Direction = 0;
				} else if (horizontal > 0) {
					animator.SetInteger ("Direction", 3);
					Direction = 3;
				} else if (horizontal < 0) {
					animator.SetInteger ("Direction", 1);
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
		if (targeting) {
			int lastTarget = currentTarget;
			if ((Input.GetKeyDown (KeyCode.A) && team.tag == "Team1") ||
				(Input.GetKeyDown (KeyCode.LeftArrow) && team.tag == "Team2")) {
				--currentTarget;
			}
			if ((Input.GetKeyDown (KeyCode.D) && team.tag == "Team1") ||
				(Input.GetKeyDown (KeyCode.RightArrow) && team.tag == "Team2")) {
				++currentTarget;
			}
			if (tileTargeting) {
				currentTarget = (currentTarget % tileTargets.Count + tileTargets.Count) % tileTargets.Count;
				if (lastTarget != currentTarget) {
					GameObject.Find ("Target").transform.position = tileTargets [currentTarget].transform.position;
					if (tileTargets [currentTarget].transform.position.y > transform.position.y) {
						animator.SetInteger ("Direction", 2);
						Direction = 2;
					} else if (tileTargets [currentTarget].transform.position.y < transform.position.y) {
						animator.SetInteger ("Direction", 0);
						Direction = 0;
					} else if (tileTargets [currentTarget].transform.position.x < transform.position.x) {
						animator.SetInteger ("Direction", 1);
						Direction = 1;
					} else if (tileTargets [currentTarget].transform.position.x > transform.position.x) {
						animator.SetInteger ("Direction", 3);
						Direction = 3;
					}
				}
			} else {
				currentTarget = (currentTarget % targets.Count + targets.Count) % targets.Count;
				if (lastTarget != currentTarget) {
					GameObject.Find ("Target").transform.position = targets [currentTarget].transform.position;
					if (targets [currentTarget].transform.position.y > transform.position.y) {
						animator.SetInteger ("Direction", 2);
						Direction = 2;
					} else if (targets [currentTarget].transform.position.y < transform.position.y) {
						animator.SetInteger ("Direction", 0);
						Direction = 0;
					} else if (targets [currentTarget].transform.position.x < transform.position.x) {
						animator.SetInteger ("Direction", 1);
						Direction = 1;
					} else if (targets [currentTarget].transform.position.x > transform.position.x) {
						animator.SetInteger ("Direction", 3);
						Direction = 3;
					}
				}
			}
			if (Input.GetKeyDown (KeyCode.Space)) {

				if (tileTargeting) {
					selectedTile = tileTargets [currentTarget];
				} else {
					selectedTarget = targets [currentTarget];
				}
				if (targetingType == 0) {
					AttackCalc ();
				}
				if (targetingType == 1) {
					Skill1Calc ();
				}
				if (targetingType == 2) {
					Skill2Calc ();
				}
				if (targetingType == 3) {
					UltCalc ();
				}
			}
		}
	}

	protected virtual void Skill1Calc () {
	}

	protected virtual void Skill2Calc() {
	}

	protected virtual void UltCalc() {
	}

	protected virtual void AttackCalc() {
		//attack calcs
		int loss = 0;
		if (isHit (selectedTarget)) {
			loss = getDamage (selectedTarget.getDEF());
			selectedTarget.Losehp (loss);
			tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage";
				animator.SetTrigger ("ATK");
			} else {
			tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag;
			}
		targeting = false;
		Destroy (GameObject.Find ("Target"));
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

	protected void makeTarget(int r) {
		Vector3 pos = transform.position;
		findPlayerTargets (pos.x, pos.y, r);
		if (targets.Count > 0) {
			GameObject t = Instantiate (Target);
			t.name = "Target";
			t.transform.position = targets [0].transform.position;
			t.layer = 8;
		} else {
			targeting = false;
		}
	}
	protected void makeAllyTarget(int r) {
		Vector3 pos = transform.position;
		findAllyTargets (pos.x, pos.y, r);
		if (targets.Count > 0) {
			GameObject t = Instantiate (Target);
			t.name = "Target";
			t.transform.position = targets [0].transform.position;
			t.layer = 8;
		} else {
			targeting = false;
		}
	}

	protected void makeEATarget(int r) {
		Vector3 pos = transform.position;
		findEATargets (pos.x, pos.y, r);
		if (targets.Count > 0) {
			GameObject t = Instantiate (Target);
			t.name = "Target";
			t.transform.position = targets [0].transform.position;
			t.layer = 8;
		} else {
			targeting = false;
		}
	}

	public virtual bool Attack() {
		if (GameManager.instance.turn == team.tag && tman.getCurrentHero ().tag == this.tag && TargetInRange()) {
			targeting = true;
			targetingType = 0;
			makeTarget (RNG);
			return true;
		}
		return false;
	}

	//**********************************************************************************************************************
	public bool TargetInRange() {
		return true;
	}

	public void findPlayerTargets(float centerX, float centerY, int range) {
		List<Hero> enemies = tman.getEnemyTeam ();

		targets = new List<Hero> ();
		foreach (Hero enemy in enemies) {
			if (Mathf.Abs (centerX - enemy.transform.position.x)
			   + Mathf.Abs (centerY - enemy.transform.position.y) <= range
			   && enemy.getHP () > 0) {
				targets.Add (enemy);

			}
		}
	}

	public void findAllyTargets(float centerX, float centerY, int range) {
		List<Hero> allies = tman.getTeam ();

		targets = new List<Hero> ();
		foreach (Hero ally in allies) {
			if (Mathf.Abs (centerX - ally.transform.position.x)
				+ Mathf.Abs (centerY - ally.transform.position.y) <= range
				&& ally.getHP () > 0) {
				targets.Add (ally);
			}
		}
	}

	public void findEATargets(float centerX, float centerY, int range) {
		List<Hero> targs = tman.getEnemyTeam ();
		List<Hero> allies = tman.getTeam ();
		foreach (Hero a in allies) {
			targs.Add (a);
		}
		targets = new List<Hero> ();
		foreach (Hero t in targs) {
			if (Mathf.Abs (centerX - t.transform.position.x)
				+ Mathf.Abs (centerY - t.transform.position.y) <= range
				&& t.getHP () > 0) {
				targets.Add (t);
			}
		}
	}

	public void findTileTargets(float centerX, float centerY, int range) {
		tileTargets = new List<GameObject>();
		foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Floor")) {
			if (Mathf.Abs (centerX - tile.transform.position.x)
				+ Mathf.Abs (centerY - tile.transform.position.y) <= range) {
				tileTargets.Add (tile);
			}
		}
	}

	public int getDamage(int eDEF) {
		float r = Random.Range (2.85f, 4.85f);

		if (effects.Contains(Effects.DOUBLEDMG)) {
			effects.Remove (Effects.DOUBLEDMG);
			return Mathf.RoundToInt( ((6 * DMG * ((float)ATK / (float)eDEF)) / 50 + 2) * r) * 2;
		} else {
			return Mathf.RoundToInt( ((6 * DMG * ((float)ATK / (float)eDEF)) / 50 + 2) * r);
		}
	}

	public virtual void EndTurn() {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			if (ult > 0) {
				--ult;
			}
			SPEED = maxSPEED;
			if (effects.Contains(Effects.STUN)) {
				effects.Remove(Effects.STUN);
				tman.msgText.text += "\n" + this.tag + "'s stun has worn off";
			}
			if (effects.Contains (Effects.BOOST)) {
				effects.Remove (Effects.BOOST);
				ATK -= 1;
				DMG -= 1;
				tman.msgText.text += this.tag + "'s boost has worn off";
			}
			if (effects.Contains (Effects.VBOOST)) {
				effects.Remove (Effects.VBOOST);
				ATK -= 3;
				DMG -= 2;
				tman.msgText.text += this.tag + "'s victory boost has worn off";
			}

			if (effects.Contains (Effects.PSN)) {
				if (psn_count > 0) {
					int dmg = (int)(maxHP * .05);
					HP -= dmg;
					tman.msgText.text = this.tag + " has taken damage " + dmg + " from poison";
				} else {
					psn_count--;
					if (psn_count == 0) {
						effects.Remove (Effects.PSN);
						tman.msgText.text += this.tag + "'s poison has worn off";
					}
				}
			}

			if(effects.Contains(Effects.FROZEN)) {
				effects.Remove(Effects.FROZEN);
				tman.msgText.text += "\n" + this.tag + " thawed out";
			}

			if (effects.Contains (Effects.BURN)) {
				if (brn_count > 0) {
					tman.msgText.text += this.tag + " was burned for 15 damage";
					HP -= 15;
					--brn_count;
					if(brn_count == 0) {
						tman.msgText.text += this.tag + "'s burn has worn off";
					}
				}
			}

			foreach(Hero enemy in tman.getEnemyTeam()) {
				if (Mathf.Abs (this.transform.position.x - enemy.transform.position.x)
					+ Mathf.Abs (this.transform.position.y - enemy.transform.position.y) <= 1) {
					if(enemy.tag == "JackFrost") {
						SPEED /= 2;
						if(enemy.ult > 0) {
							if(Random.value < 0.65f) {
								this.Freeze();
							}
						}
					}
				}
			}

			if (effects.Contains (Effects.ELIXIR)) {
				elixir_count--;
				if (elixir_count == 0) {
					effects.Remove (Effects.ELIXIR);
					ATK -= 3;
					DEF -= 3;
					SPEED -= 4;
					maxSPEED -= 4;
					DMG -= 15;
				}
			}

			if (effects.Contains (Effects.STATIC)) {
				effects.Remove (Effects.STATIC);
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
			
	public bool isHit(Hero trgt) {
		float r = Random.value;
		if (effects.Contains(Effects.ADV) && !(effects.Contains(Effects.DADV))) {
			RemoveHitEffects ();
			return r < ACCb * (ACC + .3 / trgt.EV);

		} else if (effects.Contains(Effects.DADV) && !(effects.Contains(Effects.ADV))) {
			RemoveHitEffects ();
			return r < ACCb * (ACC - .3 / trgt.EV);
		} else {
			RemoveHitEffects ();
			return r < ACCb * (ACC / trgt.EV);
		}
	}

	//Removes effects which end once an attack lands 
	public void RemoveHitEffects () {

		if (effects.Contains (Effects.RNGUP)) {
			effects.Remove (Effects.RNGUP);
			RNG -= 1;
		}

		if (effects.Contains (Effects.ADV)) {
			effects.Remove (Effects.ADV);
		}
		if (effects.Contains (Effects.DADV)) {
			effects.Remove (Effects.DADV);
		}
		
	}

	public void Boost() {
		effects.Add (Effects.BOOST);
		ATK += 1;
		DMG += 1;
	}


	public void RangeUp() {
		effects.Add (Effects.RNGUP);
		RNG += 1;
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

	public void Poison() {
		if (!effects.Contains (Effects.PSN)) {
			effects.Add (Effects.PSN);
			psn_count = 3;
		}
	}

	public void Elixir() {
		if (!selectedTarget.effects.Contains (Effects.ELIXIR)) {
			effects.Add (Effects.ELIXIR);
			elixir_count = 2;
			ATK += 3;
			DEF += 3;
			SPEED += 4;
			maxSPEED += 4;
			DMG += 15;
		}
	}

	public void checkStatic() {
		if (effects.Contains(Effects.STATIC)) {
			if (static_count == 0) {
				tman.skill1Button.interactable = false;
				tman.skill1Button.GetComponentInChildren<Text> ().text = "STATIC";
			} else if (static_count == 1) {
				tman.skill2Button.interactable = false;
				tman.skill2Button.GetComponentInChildren<Text> ().text = "STATIC";
			} else if (static_count == 2) {
				tman.ultButton.interactable = false;
				tman.ultButton.GetComponentInChildren<Text> ().text = "STATIC";
			}
		}
}
	public void Static() {
		effects.Add (Effects.STATIC);
		static_count = Random.Range (0, 3);
	}

	public void Freeze() {
		effects.Add(Effects.FROZEN);
	}

	public void Burn() {
		if (!effects.Contains (Effects.BURN)) {
			effects.Add (Effects.BURN);
		}
		brn_count = 2;
	}

}