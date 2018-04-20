using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxenfree : Hero {
	private string s1name = "Catapult";
	private string s2name = "Mad Rush";
	private string ultName = "Brave Horn";
	private string info = "Catapult: Can throw one ally within 1 space to an open space within 7 spaces. {1BP}\n\t" +
		"Mad Rush: Charge foward up to 8 spaces, each enemy moved through is inflicted with bleed. {2BP}\n\t" +
		"Brave Horn: Next attack inflicts bleed and deals double damage if target is already inflicted with bleed. {4BP}\n\t" +
		"Red Rage: Deal a small amount of extra damage to targets inflicted with bleed.";

	private bool hold = false;
	// Use this for initialization
	void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 18;
		DEF = 14;
		DMG = 80;

		HP = 400;
		maxHP = 400;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 2;
		RNG = 1;
	}
	
	// Update is called once per frame
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [4]";
		}
		base.Update ();
	}

	protected override void AttackCalc ()
	{
		if (ult > 0) {
			if (selectedTarget.effects.Contains (Effects.BLEED)) {
				DMG = 88;
				this.effects.Add (Effects.DOUBLEDMG);
				this.ult = 0;
				int prevHp = selectedTarget.getHP ();
				base.AttackCalc ();
				DMG = 80;
				if (prevHp != selectedTarget.getHP ()) {
					selectedTarget.Bleed ();
					tman.msgText.text += "\n" + selectedTarget.tag + " was inflicted with bleed";
				}
				return;
			} else {
				this.ult = 0;
				int prevHp = selectedTarget.getHP ();
				base.AttackCalc ();
				if (prevHp != selectedTarget.getHP ()) {
					selectedTarget.Bleed ();
					tman.msgText.text += "\n" + selectedTarget.tag + " was inflicted with bleed";
				}
				return;
			}
		} else {
			if (selectedTarget.effects.Contains (Effects.BLEED)) {
				DMG = 88;
				base.AttackCalc ();
				DMG = 80;
				return;
			}
			base.AttackCalc ();
		}
	}

	//Catapult: Can throw one ally within 1 space to an open space within 7 spaces
	public override bool Skill1 ()
	{
		int cost = 1;
		if (!hold && tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag && AllyInRange (1)) {
			makeAllyTarget (1);
			targeting = true;
			targetingType = 1;
			hold = true;
			return true;
		} else if (hold) {
			findTileTargets (this.transform.position.x, this.transform.position.y, 7);
			List<GameObject> tmp = new List<GameObject> ();
			bool good = true;
			foreach (GameObject tile in tileTargets) {
				foreach (Hero ally in tman.getTeam()) {
					if (tile.transform.position.Equals (ally.transform.position)) {
						good = false;
					}
				}
				foreach (Hero enemy in tman.getEnemyTeam()) {
					if (tile.transform.position.Equals (enemy.transform.position)) {
						good = false;
					}
				}
				foreach (GameObject wall in GameObject.FindGameObjectsWithTag("TempWall")) {
					if (tile.transform.position.Equals (wall.transform.position)) {
						good = false;
					}
				}
				if (good) {
					tmp.Add (tile);
				} else {
					good = true;
				}
			}
			tileTargets = tmp;
			if (tileTargets.Count > 0) {
				targeting = true;
				tileTargeting = true;
				hold = false;
				GameObject t = Instantiate (Target);
				t.name = "Target";
				t.transform.position = tileTargets [0].transform.position;
				t.layer = 8;
				return true;
			}
			hold = false;
			return false;
		}
		return false;
	}

	protected override void Skill1Calc ()
	{
		int cost = 1;
		if (hold == true) {
			Destroy (GameObject.Find ("Target"));
			targeting = false;
			Skill1 ();
		} else {
			selectedTarget.transform.position = selectedTile.transform.position;
			targeting = false;
			tileTargeting = false;
			Destroy (GameObject.Find ("Target"));
			tman.BP -= cost;
		}
	}

	//Mad Rush: Charge foward up to 8 spaces, each enemy moved through is inflicted with bleed
	public override bool Skill2 ()
	{
		int cost = 2;
		if (!hold && tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			tileTargets = new List<GameObject> ();
			GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Floor");
			GameObject[] walls = GameObject.FindGameObjectsWithTag ("TempWall");
			GameObject[] outerWalls = GameObject.FindGameObjectsWithTag ("OuterWall");
			List<Hero> allies = tman.getTeam ();
			List<Hero> enemies = tman.getEnemyTeam ();
			bool good = true;
			bool stop = false;
			int tileIndex = 0;
			for (int n = 1; n < 9; ++n) {
				int currentIndex = 0;
				foreach (GameObject tile in tiles) {
					if(tile.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + n, this.transform.position.z))) {
						good = true;
						tileIndex = currentIndex;
					}
					++currentIndex;
				}
				foreach (GameObject outWall in outerWalls) {
					if(outWall.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + n, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (GameObject wall in walls) {
					if(wall.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + n, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (Hero ally in allies) {
					if(ally.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + n, this.transform.position.z))) {
						good = false;
					}
				}
				foreach (Hero enemy in enemies) {
					if(enemy.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + n, this.transform.position.z))) {
						good = false;
					}
				}
				if (stop) {
					stop = false;
					break;
				}
				if (good) {
					tileTargets.Add (tiles [tileIndex]);
				} else {
					good = true;
				}
			}
			for (int e = 1; e < 9; ++e) {
				int currentIndex = 0;
				foreach (GameObject tile in tiles) {
					if(tile.transform.position.Equals(new Vector3(this.transform.position.x + e, this.transform.position.y, this.transform.position.z))) {
						good = true;
						tileIndex = currentIndex;
					}
					++currentIndex;
				}
				foreach (GameObject outWall in outerWalls) {
					if(outWall.transform.position.Equals(new Vector3(this.transform.position.x + e, this.transform.position.y, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (GameObject wall in walls) {
					if(wall.transform.position.Equals(new Vector3(this.transform.position.x + e, this.transform.position.y, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (Hero ally in allies) {
					if(ally.transform.position.Equals(new Vector3(this.transform.position.x + e, this.transform.position.y, this.transform.position.z))) {
						good = false;
					}
				}
				foreach (Hero enemy in enemies) {
					if(enemy.transform.position.Equals(new Vector3(this.transform.position.x + e, this.transform.position.y, this.transform.position.z))) {
						good = false;
					}
				}
				if (stop) {
					stop = false;
					break;
				}
				if (good) {
					tileTargets.Add (tiles[tileIndex]);
				} else {
					good = true;
				}
			}
			for (int s = 1; s < 9; ++s) {
				int currentIndex = 0;
				foreach (GameObject tile in tiles) {
					if(tile.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y - s, this.transform.position.z))) {
						good = true;
						tileIndex = currentIndex;
					}
					++currentIndex;
				}
				foreach (GameObject outWall in outerWalls) {
					if(outWall.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y - s, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (GameObject wall in walls) {
					if(wall.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y - s, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (Hero ally in allies) {
					if(ally.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y - s, this.transform.position.z))) {
						good = false;
					}
				}
				foreach (Hero enemy in enemies) {
					if(enemy.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y - s, this.transform.position.z))) {
						good = false;
					}
				}
				if (stop) {
					stop = false;
					break;
				}
				if (good) {
					tileTargets.Add (tiles[tileIndex]);
				} else {
					good = true;
				}
			}
			for (int w = 1; w < 9; ++w) {
				int currentIndex = 0;
				foreach (GameObject tile in tiles) {
					if(tile.transform.position.Equals(new Vector3(this.transform.position.x - w, this.transform.position.y, this.transform.position.z))) {
						good = true;
						tileIndex = currentIndex;
					}
					++currentIndex;
				}
				foreach (GameObject outWall in outerWalls) {
					if(outWall.transform.position.Equals(new Vector3(this.transform.position.x - w, this.transform.position.y, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (GameObject wall in walls) {
					if(wall.transform.position.Equals(new Vector3(this.transform.position.x - w, this.transform.position.y, this.transform.position.z))) {
						stop = true;
						good = false;
					}
				}
				foreach (Hero ally in allies) {
					if(ally.transform.position.Equals(new Vector3(this.transform.position.x - w, this.transform.position.y, this.transform.position.z))) {
						good = false;
					}
				}
				foreach (Hero enemy in enemies) {
					if(enemy.transform.position.Equals(new Vector3(this.transform.position.x - w, this.transform.position.y, this.transform.position.z))) {
						good = false;
					}
				}
				if (stop) {
					stop = false;
					break;
				}
				if (good) {
					tileTargets.Add (tiles[tileIndex]);
				} else {
					good = true;
				}
			}
			if (tileTargets.Count > 0) {
				targeting = true;
				tileTargeting = true;
				targetingType = 2;
				GameObject t = Instantiate (Target);
				t.name = "Target";
				t.transform.position = tileTargets [0].transform.position;
				t.layer = 8;
				return true;
			}
		}
		return false;
	}

	protected override void Skill2Calc ()
	{
		int cost = 2;
		Destroy (GameObject.Find ("Target"));
		Vector3 prevPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		this.transform.position = selectedTile.transform.position;
		animator.SetTrigger ("ATK");
		if (prevPos.x < this.transform.position.x) {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0 
					&& enemy.transform.position.y == this.transform.position.y
					&& enemy.transform.position.x > prevPos.x 
					&& enemy.transform.position.x < this.transform.position.x) {
					int loss = 0;
					if (isHit (enemy)) {
						if (enemy.effects.Contains (Effects.BLEED)) {
							DMG = 88;
						}
						loss = getDamage (enemy.getDEF ()) / 2;
						DMG = 80;
						enemy.Losehp (loss);
						tman.msgText.text += enemy.tag + " took " + loss + " damage and was inflicted with bleed\n";
						enemy.Bleed ();
					} else {
						tman.msgText.text += this.tag + " missed a hit on " + enemy.tag + "\n";
					}
				}
			}
		} else if (prevPos.x > this.transform.position.x) {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0
					&& enemy.transform.position.y == this.transform.position.y 
					&& enemy.transform.position.x < prevPos.x 
					&& enemy.transform.position.x > this.transform.position.x) {
					int loss = 0;
					if (isHit (enemy)) {
						if (enemy.effects.Contains (Effects.BLEED)) {
							DMG = 88;
						}
						loss = getDamage (enemy.getDEF ()) / 2;
						DMG = 80;
						enemy.Losehp (loss);
						tman.msgText.text += enemy.tag + " took " + loss + " damage and was inflicted with bleed\n";
						enemy.Bleed ();
					} else {
						tman.msgText.text += this.tag + " missed a hit on " + enemy.tag + "\n";
					}
				}
			}
		} else if (prevPos.y < this.transform.position.y) {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0 && enemy.transform.position.x == this.transform.position.x 
					&& enemy.transform.position.y > prevPos.y 
					&& enemy.transform.position.y < this.transform.position.y) {
					int loss = 0;
					if (isHit (enemy)) {
						if (enemy.effects.Contains (Effects.BLEED)) {
							DMG = 88;
						}
						loss = getDamage (enemy.getDEF ()) / 2;
						DMG = 80;
						enemy.Losehp (loss);
						tman.msgText.text += enemy.tag + " took " + loss + " damage and was inflicted with bleed\n";
						enemy.Bleed ();
					} else {
						tman.msgText.text += this.tag + " missed a hit on " + enemy.tag + "\n";
					}
				}
			}
		} else {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0 
					&& enemy.transform.position.x == this.transform.position.x
					&& enemy.transform.position.y < prevPos.y 
					&& enemy.transform.position.y > this.transform.position.y) {
					int loss = 0;
					if (isHit (enemy)) {
						if (enemy.effects.Contains (Effects.BLEED)) {
							DMG = 88;
						}
						loss = getDamage (enemy.getDEF ()) / 2;
						DMG = 80;
						enemy.Losehp (loss);
						tman.msgText.text += enemy.tag + " took " + loss + " damage and was inflicted with bleed\n";
						enemy.Bleed ();
					} else {
						tman.msgText.text += this.tag + " missed a hit on " + enemy.tag + "\n";
					}
				}
			}
		}
		targeting = false;
		tileTargeting = false;
	}

	//Brave Horn: Next attack inflicts bleed and deals double damage if target is already inflicted with bleed
	public override bool Ult ()
	{
		int cost = 4;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			ult = 10;
			tman.BP -= cost;
			tman.msgText.text = "Oxenfree prepares for a devastating attack";
			return true;
		}
		return false;
	}

	//Red Rage: Deal a small amount of extra damage to targets inflicted with bleed
}
