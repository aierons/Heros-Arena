using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2AttackButton : MonoBehaviour {

	public GameObject player2; 
	//public GameObject player2; 
	private Animator anim;

	private void Start() {
		//player2 = GameObject.Find ("Player2");
		GetComponent<Button> ().onClick.AddListener (TriggerPlayer2);
		anim = player2.GetComponent<Animator> ();
	}

	private void TriggerPlayer2() {
		if(GameManager.instance.playersTurn) return;
		/*
		if ((player2.transform - player1.transform.position).sqrMagnitude < 1 ) {
			anim.SetTrigger ("playerChop");
			player2.hp -= 10; 
		}*/
		anim.SetTrigger ("enemyChop"); // whatever it's called i can't recall 
		GameManager.instance.playersTurn = true;
	}
}
