using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medico : Hero
{

	private string s1name = "Injection";
	private string s2name = "Patch Up";
	private string ultname = "Elixir";
	private string passname = "This Might Sting A Bit";
	private string info = "Medico: \nInjection: Heals 3 hp to target ally in range or inflicts poison on target enemy [Poison deals damage at end of units turn lasting 1-4 turns]. Can also target self {2BP}\n" +
	                      "Patch: Remove all effects from a target including ADV and DISADV {1BP}\n" +
	                      "Elixir: Choose target in range, for next 2 turns target has increased DMG, DEF, SPEED. Once over they are unable to move for 1 turn. {4BP}\n" +
	                      "This Might Sting a Bit: Can attack his allies which heals a small amount of health for them";

	private float poison = .65f;


	// Use this for initialization
	public override void StartGame ()
	{
		base.StartGame ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 15;
		DEF = 13;
		DMG = 75;

		HP = 250;
		maxHP = 250;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 1;
		RNG = 1;
		
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		if (tman != null && GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [3]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultname + " [5]";

			if (!tman.attackButton.interactable) {
				tman.skill1Button.interactable = false;
			}
		}

		base.Update ();
		
	}

	//Injection: make attack on target, if target is enemy inflict poison, if ally heal a medium amount of hp (can target self)
	override public bool Skill1 ()
	{
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			if (!tman.attackButton.interactable) {
				tman.msgText.text = "can not currently use this ability";
				return false;
			}
			targeting = true;
			targetingType = 1;
			makeEATarget (RNG);
			return true;
		}
		return false;
	}

	protected override void Skill1Calc ()
	{
		int cost = 2;
		List<Hero> allies = tman.getTeam ();
		List<Hero> enemies = tman.getEnemyTeam ();
		if (allies.Contains (selectedTarget)) {
			int gain = (int)(selectedTarget.getMaxHP () * .1); 
			selectedTarget.Heal (gain);
			tman.msgText.text = selectedTarget.tag + " was healed " + gain + " hp.";
		}
		if (enemies.Contains (selectedTarget)) {
			if (isHit (selectedTarget)) {
				int loss = getDamage (selectedTarget.getDEF ());
				selectedTarget.Losehp (loss);

				tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage\n";
				float r = Random.value;
				if (r <= .65) {
					selectedTarget.Poison ();
					tman.msgText.text += selectedTarget.tag + " is now poisoned.";
				} else {
					tman.msgText.text += "the poison did not take.";
				}
				animator.SetTrigger ("ATK");
			} else {
				tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag + "\n";
			}
		}
		tman.BP -= cost;
		tman.attackButton.interactable = false;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//Patch: remove all effects from target (good and bad)
	override public bool Skill2 ()
	{
		int cost = 3;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 2;
			makeEATarget (RNG);
			return true;
		}
		return false;
	}

	override protected void Skill2Calc ()
	{
		int cost = 3;
		selectedTarget.effects = new List<Effects> ();
		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//Elixir: for a few turns target ally gets large buff to all stats.
	public override bool Ult ()
	{
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 3;
			makeAllyTarget (RNG);
			return true;
		}
		return false;
	}

	protected override void UltCalc ()
	{
		int cost = 5;
		selectedTarget.Elixir ();
		tman.msgText.text = this.tag + " injected " + selectedTarget.tag + " with Elixir.";
		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	public override bool Attack ()
	{
		if (GameManager.instance.turn == team.tag && tman.getCurrentHero ().tag == this.tag && TargetInRange()) {
			targeting = true;
			targetingType = 0;
			makeEATarget (RNG);
			return true;
		}
		return false;
	}

	protected virtual bool TargetInRange() {
		List<Hero> allies = tman.getTeam ();
		List<Hero> targets = tman.getEnemyTeam ();
		foreach (Hero a in allies) {
			targets.Add (a);
		}

		foreach (Hero trgt in targets) {
			if (Mathf.Abs (this.transform.position.x - trgt.transform.position.x)
				+ Mathf.Abs (this.transform.position.y - trgt.transform.position.y) <= RNG
				&& trgt.getHP () > 0) {
				return true;
			}
		}
		return false;
	}

	//This might sting a bit :  can target allies with attack to heal a small amount
	protected override void AttackCalc ()
	{
		//attack calcs
		int loss = 0;
		List<Hero> allies = tman.getTeam ();
		List<Hero> enemies = tman.getEnemyTeam ();

		if (allies.Contains (selectedTarget)) {
			int gain = (int)(selectedTarget.getMaxHP () * .05); 
			selectedTarget.Heal (gain);
			tman.msgText.text = selectedTarget.tag + " was healed " + gain + " hp.";
		}

		if (enemies.Contains (selectedTarget)) {
			if (isHit (selectedTarget)) {
				loss = getDamage (selectedTarget.getDEF ());
				selectedTarget.Losehp (loss);

				tman.msgText.text = this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss + " damage\n";
				animator.SetTrigger ("ATK");
			} else {
				tman.msgText.text = this.tag + " missed a hit on " + selectedTarget.tag + "\n";
			}
		}

		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	public override string Info() {
		return info;
	}
}
