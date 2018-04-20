using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pendragon : Hero {
	private string s1name = "Flash Strike";
	private string s2name = "Shield Bash";
	private string ultName = "Bolster The Army";
	private string info = "Pendragon: \n" +
	                      "Shield Bash: Inflict stun on target within 1 space [target's speed is reduced by half on their next turn] {1BP}\n" +
	                      "Flash Strike: Grants self ADV on next attack and gain 1 SPEED {1BP}\n" +
	                      "Bolster the Army: Grants entire team ADV and enemy team DISADV {4BP}\n" +
	                      "Challenger: If target of standard attack has more health, Pendragon deals additional damage";
	//passive : Challenger deal 1 extra damage on hit if target has more hp than her.

	// Use this for initialization
	public override void StartGame () {
		base.StartGame ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 18;
		DEF = 16;
		DMG = 88;

		HP = 300;
		maxHP = 300;
		SPEED = 7;
		maxSPEED = 7;
		wallDMG = 2;
		RNG = 1;
	}
	
	// Update is called once per frame
	public override void Update () {
		if (tman != null && GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [1]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}
		base.Update ();
	}

	//Flash Strike : grant next attack advantage
	override public bool Skill1() {
		int cost = 1;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && !effects.Contains(Effects.ADV)) {
			effects.Add (Effects.ADV);
			SPEED++;
			tman.msgText.text = this.tag + " has gained advantage on their next attack, SPEED has increased by 1";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}

	}

	//Shield Bash : Stun enemy
	override public bool Skill2() {
		int cost = 1;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && tman.BP >= cost) {
			targeting = true;
			targetingType = 2;
			makeTarget (RNG);
			return true;
		}
		return false;
	}

	protected override void Skill2Calc() {
		int cost = 1;
		selectedTarget.Stun ();
		tman.msgText.text = this.tag + " used Shield Bash, " + selectedTarget.tag + " was stunned";
		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//Bolster the Army : grant all allies advantage and all enemies disadvantage
	public override bool Ult() {
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			List<Hero> t = tman.getTeam ();
			List<Hero> e = tman.getEnemyTeam ();

			foreach (Hero h in t) {
				h.effects.Add (Effects.ADV);
			}
			foreach (Hero h in e) {
				h.effects.Add (Effects.DADV);
			}
			tman.BP -= cost;
			tman.msgText.text = this.tag + " gave advantage to " + tman.tag + " and gave disadvantage to " + tman.enemyTeam.tag;
			return true;
		}
		return false;
	}

	public override string Info() {
		return info;
	}

	protected override void AttackCalc() {
		int loss = 0;
		bool challenger = false;
		if (isHit (selectedTarget)) {
			loss = getDamage (selectedTarget.getDEF());
			if (selectedTarget.getHP() > this.HP){
				challenger = true;
			}
			selectedTarget.Losehp (loss);
			tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage";
			if (challenger) {
				int dmg = (int)(selectedTarget.getMaxHP() * .1);
				selectedTarget.Losehp (dmg);
				tman.msgText.text += "\n" + this.tag + " dealt " + dmg + " extra damage to " + selectedTarget.tag + " because of Challenger";
				challenger = false;
			}
			animator.SetTrigger ("ATK");
		} else {
			tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag;
		}
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}
		
}