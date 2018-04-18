using System.Collections;using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IronRebel : Hero {

	private string s1name = "Armor Spike";
	private string s2name = "Iron Hide";
	private string ultName = "Panzer Smash";
	private string info = "Thorny Devil: next time a target deals damage to him they take a small amount of damage as well {2BP}\n\t" +
	                      "Iron HideHide: grant self a small amount of armor [temporary health, that can go beyond max health, does not stack] {2BP}\n\t" +
	                      "Panzer Smash: next attack deals damage in a 3x4 rectangle space infront of him, goes through walls and destroys walls, all enemies hit are stunned {4BP}\n\t" +
	                      "________: ";

	private bool spiked;

	private int armor;

	public override string getHeroText ()
	{
		return tag + " : " + HP + "+" + armor + "/" + maxHP + " SPEED:" + SPEED;
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
	
		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 15;
		DEF = 18;
		DMG = 80;

		HP = 400;
		maxHP = 400;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 3;
		RNG = 1;

		spiked = false;
		armor = 0;
	}
	
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}

		base.Update ();
	}

	//Armor Spike : next time an enemy deals damage to you they take 2 damage
	override public bool Skill1() {
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && !spiked) {
			spiked = true;
			tman.msgText.text = this.tag + " has activated Armor Spike";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

	public override void Losehp (int loss) {
		if (spiked) {
			int spike = (int)(loss * .50);
			tman.etman.getCurrentHero ().Losehp (spike);
			tman.msgText.text = tman.etman.getCurrentHero ().tag + " took " + spike + " damage from Armor Spike";
			spiked = false;
		}

		//Armor
		if (armor > 0 && armor >= loss) {
			armor -= loss;
		} else if (armor > 0 && armor < loss) {
			loss -= armor;
			armor = 0;
			base.Losehp(loss);
		} else {
		base.Losehp (loss);
		}
	}

	//Iron Hide : grant self some temp HP
	override public bool Skill2() {
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			armor = 50;
			tman.msgText.text = this.tag + " has activated Iron Hide, gained 50 armor.";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

	//Panzer Smash: next attack deals damage in a 3x4 rectangle space infront of him, goes through walls and destroys walls, all enemies hit are stunned {4BP}
	public override bool Ult() {
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			List<Hero> targets = getEnemyTrgts ();
			foreach (Hero e in targets) {
				e.Losehp (getDamage (e.getDEF ()));
			}
			tman.msgText.text = this.tag + " has used Panzer Smash";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

<<<<<<< HEAD
	public override string Info()  {
		return info;
	}
=======
	private List<Hero> getEnemyTrgts() {
		List<Hero> enemies = tman.getEnemyTeam ();
		List<Hero> targets = new List<Hero> ();

		Vector3 pos = this.transform.position;

		int xlow = 0;
		int xhi = 0;
		int ylow = 0;
		int yhi = 0;

		if (Direction == 0) {
			xlow = -1;
			xhi = 1;
			ylow = -4;
			yhi = -1;
		} else if (Direction == 1) {
			xlow = -4;
			xhi = 1;
			ylow = -1;
			yhi = 1;
		} else if (Direction == 2) {
			xlow = -1;
			xhi = 1;
			ylow = 1;
			yhi = 4;
		} else if (Direction == 3) {
			xlow = 1;
			xhi = 4;
			ylow = -1;
			yhi = 1;
		}

		foreach (Hero e in enemies) {
			Vector3 epos = e.transform.position;
			if (pos.x + xlow <= epos.x && epos.x <= pos.x + xhi &&
			    pos.y + ylow <= epos.y && epos.y <= pos.y + yhi) {
				targets.Add (e);
			}
		}

		BoardManager bm = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<BoardManager>();
		GameObject[] wallList = bm.wallTiles;
		List<Wall> walls = new List<Wall>();
		foreach (GameObject w in wallList) {
			walls.Add (w.GetComponent<Wall> ());
		}
		foreach (GameObject w in GameObject.FindGameObjectsWithTag("TempWall")) {
			Vector3 wpos = w.transform.position;
			if (pos.x + xlow <= wpos.x && wpos.x <= pos.x + xhi &&
				pos.y + ylow <= wpos.y && wpos.y <= pos.y + yhi) {
				w.SetActive (false);
			}
		}
		return targets;
	}


>>>>>>> 82843d53d216502db8c2950e57408c302caba98d
}
