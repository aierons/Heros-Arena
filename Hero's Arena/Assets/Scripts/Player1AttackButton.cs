using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1AttackButton : MonoBehaviour {

	public GameObject player1; 
	//public GameObject player2; 
	private Animator anim;

	private void Start() {
		//player2 = GameObject.Find ("Player2");
		GetComponent<Button> ().onClick.AddListener (TriggerPlayer1);
		anim = player1.GetComponent<Animator> ();
	}

	private void TriggerPlayer1() {
		/*
		if ((player2.transform - player1.transform.position).sqrMagnitude < 1 ) {
			anim.SetTrigger ("playerChop");
			player2.hp -= 10; 
		}*/
		anim.SetTrigger ("playerChop");
	}
}
