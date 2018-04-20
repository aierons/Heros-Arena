﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panther : Hero {

	private string s1name = "Agile";
	private string s2name = "Scratch Dash";
	private string ultName = "Catastrophe";
	private string info = "Panther: \nAgile: Next time an enemy attacks, increase chance attack will miss{1BP}\n" +
		"Scratch Dash: Move up to 4 spaces, enemies in path take small amount of damage and have a chance to be inflicted with bleed [costs 4 movement] {2BP}\n" +
		"Catastrophe: choose target space up to 4 spaces and deal damage to all enemies within 2 spaces of target spot.{5BP}\n";

	// Use this for initialization
	public override void StartGame () {
		base.StartGame ();

		EV = 1f;
		ACCb = .91f;
		ACC = 1f;

		ATK = 16;
		DEF = 15;
		DMG = 80;

		HP = 245;
		maxHP = 245;
		SPEED = 10;
		maxSPEED = 10;
		wallDMG = 2;
		RNG = 1;
	}

	// Update is called once per frame
	public override void Update () {
		if (tman != null && GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}
		base.Update ();
	}

	//Agile: Next time an enemy attacks, increase chance attack will miss (Quick) {1BP}
	override public bool Skill1() {
		int cost = 1;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && !effects.Contains (Effects.ADV)) {
			Quick ();
			tman.msgText.text = this.tag + " has taken an evasive position.";
			tman.BP -= cost;
			return true;
		}
		return false;
	}

	//Scratch Dash: Move up to 4 spaces, enemies in path take small amount of damage and have a chance to be inflicted with bleed [costs movement] {2BP}
	public override bool Skill2 ()
	{
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && SPEED >= 4) {
			SPEED -= 4;

			tileTargets = new List<GameObject> ();

			Skill2NS (1);
			Skill2NS (-1);
			Skill2WE (1);
			Skill2WE (-1);

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

	private void Skill2NS(int g) {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Floor");
		GameObject[] walls = GameObject.FindGameObjectsWithTag ("TempWall");
		GameObject[] outerWalls = GameObject.FindGameObjectsWithTag ("OuterWall");
		List<Hero> allies = tman.getTeam ();
		List<Hero> enemies = tman.getEnemyTeam ();
		bool good = true;
		bool stop = false;
		int tileIndex = 0;

		for (int i = 1; i < 5; ++i) {
			int currentIndex = 0;
			foreach (GameObject tile in tiles) {
				if(tile.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + i*g, this.transform.position.z))) {
					good = true;
					tileIndex = currentIndex;
				}
				++currentIndex;
			}
			foreach (GameObject outWall in outerWalls) {
				if(outWall.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + i*g, this.transform.position.z))) {
					stop = true;
					good = false;
				}
			}
			foreach (GameObject wall in walls) {
				if(wall.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + i*g, this.transform.position.z))) {
					stop = true;
					good = false;
				}
			}
			foreach (Hero ally in allies) {
				if(ally.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + i*g, this.transform.position.z))) {
					good = false;
				}
			}
			foreach (Hero enemy in enemies) {
				if(enemy.transform.position.Equals(new Vector3(this.transform.position.x, this.transform.position.y + i*g, this.transform.position.z))) {
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
	}

	private void Skill2WE(int g) {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Floor");
		GameObject[] walls = GameObject.FindGameObjectsWithTag ("TempWall");
		GameObject[] outerWalls = GameObject.FindGameObjectsWithTag ("OuterWall");
		List<Hero> allies = tman.getTeam ();
		List<Hero> enemies = tman.getEnemyTeam ();
		bool good = true;
		bool stop = false;
		int tileIndex = 0;

		for (int i = 1; i < 5; ++i) {
			int currentIndex = 0;
			foreach (GameObject tile in tiles) {
				if(tile.transform.position.Equals(new Vector3(this.transform.position.x + i*g, this.transform.position.y, this.transform.position.z))) {
					good = true;
					tileIndex = currentIndex;
				}
				++currentIndex;
			}
			foreach (GameObject outWall in outerWalls) {
				if(outWall.transform.position.Equals(new Vector3(this.transform.position.x + i*g, this.transform.position.y, this.transform.position.z))) {
					stop = true;
					good = false;
				}
			}
			foreach (GameObject wall in walls) {
				if(wall.transform.position.Equals(new Vector3(this.transform.position.x + i*g, this.transform.position.y, this.transform.position.z))) {
					stop = true;
					good = false;
				}
			}
			foreach (Hero ally in allies) {
				if(ally.transform.position.Equals(new Vector3(this.transform.position.x + i*g, this.transform.position.y, this.transform.position.z))) {
					good = false;
				}
			}
			foreach (Hero enemy in enemies) {
				if(enemy.transform.position.Equals(new Vector3(this.transform.position.x + + i*g, this.transform.position.y, this.transform.position.z))) {
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
	}

	override protected void Skill2Calc() {
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
					Skill2CalcHelper (enemy);
				}
			}
		} else if (prevPos.x > this.transform.position.x) {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0
					&& enemy.transform.position.y == this.transform.position.y 
					&& enemy.transform.position.x < prevPos.x 
					&& enemy.transform.position.x > this.transform.position.x) {
					Skill2CalcHelper (enemy);
				}
			}
		} else if (prevPos.y < this.transform.position.y) {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0 && enemy.transform.position.x == this.transform.position.x 
					&& enemy.transform.position.y > prevPos.y 
					&& enemy.transform.position.y < this.transform.position.y) {
					Skill2CalcHelper (enemy);
				}
			}
		} else {
			foreach (Hero enemy in tman.getEnemyTeam()) {
				if (enemy.getHP() > 0 
					&& enemy.transform.position.x == this.transform.position.x
					&& enemy.transform.position.y < prevPos.y 
					&& enemy.transform.position.y > this.transform.position.y) {
					Skill2CalcHelper (enemy);
				}
			}
		}
		targeting = false;
		tileTargeting = false;
		tman.BP -= cost;
	}

	private void Skill2CalcHelper(Hero enemy) {
		int loss = 0;
		float r = Random.value;
		if (isHit (enemy)) {
			loss = getDamage (enemy.getDEF ());
			enemy.Losehp (loss);
			tman.msgText.text += enemy.tag + " took " + loss + " damage";
			if (r <= .70) {
				enemy.Bleed ();
				tman.msgText.text += " and was inflicted with bleed\n";
			}else {
				tman.msgText.text += this.tag + " missed a hit on " + enemy.tag + "\n";
			}
		}
	}

	//Catastrophe: choose target space up to 4 spaces and deal damage to all enemies within 2 spaces of target spot.{5BP}
	override public bool Ult() {
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			tileTargeting = true;
			targetingType = 1;
			findTileTargets (this.transform.position.x, this.transform.position.y, 5);
			bool good = true;
			List<GameObject> tmp = new List<GameObject> ();
			foreach (GameObject tile in tileTargets) {
				foreach (Hero e in tman.getEnemyTeam()) {
					if (tile.transform.position.Equals (e.transform.position)) {
						good = false;
					}
				}
				foreach (Hero h in tman.getTeam()) {
					if (tile.transform.position.Equals (h.transform.position)) {
						good = false;
					}
				}
				if (good == true) {
					tmp.Add (tile);
				} else {
					good = true;
				}
			}
			tileTargets = tmp;
			if (tileTargets.Count > 0) {
				targeting = true;
				tileTargeting = true;
				targetingType = 3;
				GameObject t = Instantiate (Target);
				t.name = "Target";
				t.transform.position = tileTargets [0].transform.position;
				t.layer = 8;
			} else {
				targeting = false;
				tileTargeting = false;
			}

		}
		return false;
	}

	protected override void UltCalc() {
		int cost = 5;
		Destroy (GameObject.Find ("Target"));
		this.transform.position = selectedTile.transform.position;
		List<Hero> trgts = getEnemyTrgts ();
		tman.msgText.text = this.tag + " has used Catastrophe";
		foreach (Hero enemy in trgts) {
			int l = getDamage (enemy.getDEF ());
			enemy.Losehp (l);
			tman.msgText.text += enemy.tag + " took " + l + " damage";
		}
		targeting = false;
		tileTargeting = false;
		tman.BP -= cost;
	}

	private List<Hero> getEnemyTrgts() {
		List<Hero> enemies = tman.getEnemyTeam ();

		Vector3 pos = this.transform.position;

		List<Hero> temp = new List<Hero> ();

		foreach (Hero enemy in enemies) {
			if (Mathf.Abs (pos.x - enemy.transform.position.x)
				+ Mathf.Abs (pos.y - enemy.transform.position.y) <= 2
				&& enemy.getHP () > 0) {
				temp.Add (enemy);
			}
		}
		return temp;
	}

	public override string Info()
	{
		return info;
	}
}