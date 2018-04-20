using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ferocity : Hero {

	private string s1name = "Explosion";
	private string s2name = "Ignition";
	private string ultname = "Hellfire";
	private string passname = "Immolate";

	private float burn = .65f;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 17;
		DEF = 14;
		DMG = 90;

		HP = 280;
		maxHP = 280;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 2;
		RNG = 3;
	}
	
	// Update is called once per frame
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [3]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultname + " [4]";
		}

		base.Update ();
	}

	public override void EndTurn ()
	{
		base.EndTurn ();
		if (ult == 0) {
			burn = .65f;
			DMG = 90;
		}
	}

	protected override void AttackCalc ()
	{
		int beforeAttack = selectedTarget.getHP ();
		base.AttackCalc ();
		if (beforeAttack != selectedTarget.getHP ()) {
			if (Random.value < burn) {
				selectedTarget.Burn ();
				tman.msgText.text += "\n" + selectedTarget.tag + " was burned!";
			}
		}
	}

	//Explosion: next attack deals splash damage to spaces above and below target.
	public override bool Skill1() {
		int cost = 3;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 1;
			makeTarget (RNG);
			return true;
		}
		return false;
	}

	protected override void Skill1Calc() {
		int cost = 2;
		int loss = 0;
		if (isHit (selectedTarget)) {
			loss = getDamage (selectedTarget.getDEF());
			selectedTarget.Losehp (loss);
			tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage";
			animator.SetTrigger ("ATK");
			if (ult > 0 && Random.value < burn) {
				selectedTarget.Burn ();
				tman.msgText.text += "\n" + selectedTarget.tag + " was burned!";
			}
			foreach (Hero e in tman.getEnemyTeam()) {
				if (e.transform.position.y == selectedTarget.transform.position.y + 1
				   || e.transform.position.y == selectedTarget.transform.position.y - 1) {
					loss = getDamage (e.getDEF ());
					e.Losehp (loss);
					tman.msgText.text += "\n" + e.tag + " took " + loss + " splash damage";
				}
			}
		} else {
			tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag;
		}
		targeting = false;
		tman.BP -= cost;
		Destroy (GameObject.Find ("Target"));
	}

	//Ignition: next attack has increased range and inflicts burn on enemy
	public override bool Skill2() {
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 2;
			makeTarget (RNG + 1);
			return true;
		}
		return false;
	}

	protected override void Skill2Calc() {
		int cost = 2;
		int loss = 0;
		if (isHit (selectedTarget)) {
			loss = getDamage (selectedTarget.getDEF());
			selectedTarget.Losehp (loss);
			tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage";
			animator.SetTrigger ("ATK");
			selectedTarget.Burn ();
			tman.msgText.text += "\n" + this.tag + " was burned!";
		} else {
			tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag;
		}
		targeting = false;
		tman.BP -= cost;
		Destroy (GameObject.Find ("Target"));
	}

	//Hellfire: for the next two turns DMG is increased and all abilities have a large chance of inflicting burn on targets.
	public override bool Ult() {
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			tman.BP -= cost;
			ult = 2;
			burn = .90f;
			DMG = 100;
			return true;
		} else {
			return false;
		}
	}

	//Immolate: all direct attacks (basically the chosen target)
}
