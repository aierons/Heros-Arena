using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1AttackButton : MonoBehaviour {

	public GameObject player1; 
	private Animator anim;

	private void Start() {
		GetComponent<Button> ().onClick.AddListener (TriggerPlayer1);
		anim = player1.GetComponent<Animator> ();
	}

	private void TriggerPlayer1() {
		anim.SetTrigger ("playerChop");

	}
}
	
/*
	public GameObject player;
	public Button Player1Attack;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = player.GetComponent<Animator> ();
		Button btn = Player1Attack.GetComponent<Button> ();
		btn.onClick.AddListener (Attack);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Attack() {
		anim.SetTrigger ("playerChop");
	}

	public void Button_Click() {
		anim.SetTrigger ("playerChop");
	}
}
*/