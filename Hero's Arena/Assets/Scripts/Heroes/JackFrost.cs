using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackFrost : Hero {
	private string s1name = "Arctic Cliff";
	private string s2name = "Frostbite";
	private string ultname = "Zeroth Hour";
	private string passname = "Permafrost";

	private float freeze = .65f;

	public GameObject jackWall;
	private int jackWallTime = 0;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 16;
		DEF = 15;
		DMG = 85;

		HP = 295;
		maxHP = 295;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 1;
		RNG = 2;
	}

	// Update is called once per frame
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [3]";
			if (ult > 0){
				tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [0]";
			} else {
				tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			}
			tman.ultButton.GetComponentInChildren<Text> ().text = ultname + " [4]";
		}
		base.Update ();
	}

	//Arctic Cliff: create a wall covering 3 spaces within 5 spaces (lasts 2 turns or till canceled).
	public override bool Skill1() {
		int cost = 0;
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
				GameObject t = Instantiate (Target);
				t.name = "Target";
				t.transform.position = tileTargets [0].transform.position;
				t.layer = 8;
			} else {
				targeting = false;
			}

		}
		return true;
	}

	protected override void Skill1Calc() {
		int cost = 3;
		jackWallTime = 3;
		GameObject old1 = GameObject.Find ("j1");
		if (old1 != null) {
			Destroy (old1);
		}
		GameObject old2 = GameObject.Find ("j2");
		if (old2 != null) {
			Destroy (old2);
		}
		GameObject old3 = GameObject.Find ("j3");
		if (old3 != null) {
			Destroy (old3);
		}

		GameObject j1 = Instantiate (jackWall);
		j1.name = "j1";
		j1.transform.position = selectedTile.transform.position;
		j1.layer = 8;

		Vector3 pos;
		Vector3 pos2;
		if (Mathf.Abs (this.transform.position.x - selectedTile.transform.position.x) > Mathf.Abs (this.transform.position.y - selectedTile.transform.position.y)) {
			pos = new Vector3 (selectedTile.transform.position.x, selectedTile.transform.position.y + 1, selectedTile.transform.position.z);
			pos2 = new Vector3 (selectedTile.transform.position.x, selectedTile.transform.position.y - 1, selectedTile.transform.position.z);
		} else if (Mathf.Abs (this.transform.position.x - selectedTile.transform.position.x) < Mathf.Abs (this.transform.position.y - selectedTile.transform.position.y)) {
			pos = new Vector3 (selectedTile.transform.position.x + 1, selectedTile.transform.position.y, selectedTile.transform.position.z);
			pos2 = new Vector3 (selectedTile.transform.position.x - 1, selectedTile.transform.position.y, selectedTile.transform.position.z);
		} else {
			if ((selectedTile.transform.position.x > this.transform.position.x && selectedTile.transform.position.y > this.transform.position.y)
				|| (selectedTile.transform.position.x < this.transform.position.x && selectedTile.transform.position.y < this.transform.position.y)) {
				pos = new Vector3 (selectedTile.transform.position.x - 1, selectedTile.transform.position.y + 1, selectedTile.transform.position.z);
				pos2 = new Vector3 (selectedTile.transform.position.x + 1, selectedTile.transform.position.y - 1, selectedTile.transform.position.z);
			} else {
				pos = new Vector3 (selectedTile.transform.position.x + 1, selectedTile.transform.position.y + 1, selectedTile.transform.position.z);
				pos2 = new Vector3 (selectedTile.transform.position.x - 1, selectedTile.transform.position.y - 1, selectedTile.transform.position.z);
			}
		}
		GameObject j2 = Instantiate (jackWall);
		j2.name = "j2";
		j2.transform.position = pos;

		GameObject j3 = Instantiate(jackWall);
		j3.name = "j3";
		j3.transform.position = pos2;

		foreach (Hero h in tman.getTeam()) {
			if (j2 != null && j2.transform.position.Equals (h.transform.position)) {
				Destroy (j2);
			}
			if (j3 != null && j3.transform.position.Equals (h.transform.position)) {
				Destroy (j3);
			}
		}

		foreach (Hero e in tman.getEnemyTeam()) {
			if (j2 != null && j2.transform.position.Equals (e.transform.position)) {
				Destroy (j2);
			}
			if (j3 != null && j3.transform.position.Equals (e.transform.position)) {
				Destroy (j3);
			}
		}
		targeting = false;
		tileTargeting = false;
		tman.BP -= cost;
		Destroy (GameObject.Find ("Target"));
	}

	//Frostbite: next attack has a chance of freezing the target
	public override bool Skill2() {
		int cost = 2;
		if(ult > 0) {
			cost = 0;
		}
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 2;
			makeTarget (RNG);
			return true;
		}
		return false;
	}


	protected override void Skill2Calc() {
		//attack calcs
		int cost = 2;
		if(ult > 0) {
			cost = 0;
		}
		int loss = 0;
		if (isHit (selectedTarget)) {
			loss = getDamage (selectedTarget.getDEF());
			selectedTarget.Losehp (loss);
			tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage";
			animator.SetTrigger ("ATK");
			if(Random.value < freeze) {
				selectedTarget.Freeze();
				tman.msgText.text += "\n" + selectedTarget.tag + " was frozen!";
			}
		} else {
			tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag;
		}
		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//Zeroth hour: for the next 2 turns frostbite costs 0BP and Permafrost has chance to freeze targets in range.
	public override bool Ult() {
		int cost = 4;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			tman.BP -= cost;
			ult = 2;
			return true;
		} else {
			return false;
		}
	}

	//Permafrost: when target ends turn within 1 space their speed is reduced on their next turn

	public override void EndTurn ()
	{
		if (jackWallTime > 0) {
			--jackWallTime;
			if (jackWallTime == 0) {
				GameObject old1 = GameObject.Find ("j1");
				if (old1 != null) {
					Destroy (old1);
				}
				GameObject old2 = GameObject.Find ("j2");
				if (old2 != null) {
					Destroy (old2);
				}
				GameObject old3 = GameObject.Find ("j3");
				if (old3 != null) {
					Destroy (old3);
				}
			}
		}
		base.EndTurn ();
	}
}
