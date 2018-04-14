using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pendragon : Hero {
	private string s1name = "Flash Strike";
	private string s2name = "Shield Bash";
	private string ultName = "Bolster The Army";
	//passive : Challenger deal 1 extra damage on hit if target has more hp than her.

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 18;
		DEF = 16;
		DMG = 88;

		HP = 300;
		maxHP = 300;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 2;
		RNG = 1;
	}
	
	// Update is called once per frame
	public override void Update () {
		/*
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			passive.interactable = true;
		} else {
			passive.interactable = false;
		}
		*/

		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [1]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}
		base.Update ();
	}

	//True Strike : grant next attack advantage
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
			GameObject t1 = tman.etman.captain;
			GameObject t2 = tman.etman.member1;
			GameObject t3 = tman.etman.member2;
			Hero e1 = t1.GetComponent<Hero>();
			Hero e2 = t2.GetComponent<Hero>();
			Hero e3 = t3.GetComponent<Hero>();

			if (Mathf.Abs (transform.position.x - t1.transform.position.x)
			    + Mathf.Abs (transform.position.y - t1.transform.position.y) <= RNG
			    && !e1.effects.Contains (Effects.STUN)) {
				e1.Stun ();
				tman.msgText.text = this.tag + " has inflicted stun on " + e1.tag;
				tman.BP -= cost;
				return true;
			} else if (Mathf.Abs (transform.position.x - t2.transform.position.x)
			         + Mathf.Abs (transform.position.y - t2.transform.position.y) <= RNG
			         && !e2.effects.Contains (Effects.STUN)) {
				e2.Stun ();
				tman.msgText.text = this.tag + " has inflicted stun on " + e2.tag;
				tman.BP -= cost;
				return true;
			} else if (Mathf.Abs (transform.position.x - t3.transform.position.x)
			         + Mathf.Abs (transform.position.y - t3.transform.position.y) <= RNG
			         && !e3.effects.Contains (Effects.STUN)) {
				e3.Stun ();
				tman.msgText.text = this.tag + " has inflicted stun on " + e3.tag;
				tman.BP -= cost;
				return true;
			} else {
			}

		}
		return false;
	}

	//Bolster the Army : grat all allies advantage and all enemies disadvantage
	public override bool Ult() {
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && tman.BP >= cost) {
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

	/*
	private bool alliesInRange() {
		List<Vector3> p = tman.getAllyPoses (this);
		Vector3 pos = this.transform.position;

		for (int i = 0; i < p.Count; i++) {
			if ((p [i].x >= pos.x - 3 || p [i].x <= pos.x + 3) &&
			   (p [i].y >= pos.y - 3 || p [i].y <= pos.x + 3)) {
				target.effects.Add (Effects.ADV);
				return true;
			}
		}
		return false;
	}

	*/

	public override bool Attack() {
		if (GameManager.instance.turn == team.tag && tman.getCurrentHero ().tag == this.tag) {
			GameObject t1 = tman.etman.captain;
			GameObject t2 = tman.etman.member1;
			GameObject t3 = tman.etman.member2;
			Hero e1 = t1.GetComponent<Hero>();
			Hero e2 = t2.GetComponent<Hero>();
			Hero e3 = t3.GetComponent<Hero>();
			int loss = 0;
			bool challenger = false;

			if (Mathf.Abs (transform.position.x - t1.transform.position.x)
				+ Mathf.Abs (transform.position.y - t1.transform.position.y) <= RNG) {
				if (isHit (e1)) {
					if (e1.getHP() > this.HP){
						e1.Losehp (15);
						challenger = true;
					}
					loss = getDamage (e1.getDEF());
					e1.Losehp (loss);
					tman.msgText.text = this.tag + " landed a hit on " + e1.tag + " dealt " + loss + " damage";
					if (challenger) {
						tman.msgText.text += this.tag + " passive (Challenger) activated ";
						challenger = false;
					}
					animator.SetTrigger ("ATK");
				} else {
					tman.msgText.text = this.tag + " missed a hit on " + e1.tag;
				}
				return true;
			} 
			if (Mathf.Abs (transform.position.x - t2.transform.position.x)
				+ Mathf.Abs (transform.position.y - t2.transform.position.y) <= RNG) {
				if (isHit (e2)) {
					if (e1.getHP() > this.HP){
						e1.Losehp (15);
						challenger = true;
					}
					loss = getDamage (e2.getDEF());
					e2.Losehp (loss);
					tman.msgText.text = this.tag + " landed a hit on " + e2.tag + " dealt " + loss + " damage";
					if (challenger) {
						tman.msgText.text += this.tag + " passive (Challenger) activated ";
						challenger = false;
					}
					animator.SetTrigger ("ATK");
				} else {
					tman.msgText.text = this.tag + " missed a hit on " + e2.tag;
				}
				return true;
			} 
			if (Mathf.Abs (transform.position.x - t3.transform.position.x)
				+ Mathf.Abs (transform.position.y - t3.transform.position.y) <= RNG) {
				if (isHit (e3)) {
					if (e1.getHP() > this.HP){
						e1.Losehp (15);
						challenger = true;
					}
					loss = getDamage (e3.getDEF());
					e3.Losehp (loss);
					tman.msgText.text = this.tag + " landed a hit on " + e3.tag + " dealt " + loss + " damage";
					if (challenger) {
						tman.msgText.text += this.tag + " passive (Challenger) activated ";
						challenger = false;
					}
					animator.SetTrigger ("ATK");
				} else {
					tman.msgText.text = this.tag + " missed a hit on " + e3.tag;
				}
				return true;
			}

		}
		return false;
	}
		
}
