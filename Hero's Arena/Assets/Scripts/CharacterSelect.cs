using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelect : MonoBehaviour {

	private List<GameObject> characters;
	private int characterIndex = 0;
	private int current = 0; 

	/*
	 * current starts from characters1.1
	 * -> characters2.1
	 * 1.2
	 * 2.2
	 * 1.3
	 * 2.3
	 */

	// buttons
	public Button nextButton;
	public Button readyButton;
	public Button selectPendragon;
	public Button selectIronRebel;
	public Button selectSongbird;
	/*
	 * 0 = pendragon
	 * 1 = iron rebel
	 * 2 = songbird
	 * 
	 */

	private UnityAction select0;
	private UnityAction select1;
	private UnityAction select2;


	void Awake () {
		characters = new List<GameObject> ();	
		foreach (Transform t in transform) {
			characters.Add (t.gameObject);
			t.gameObject.SetActive (false);
		}
		characters [characterIndex].SetActive (true);

		select0 = new UnityAction (delegate {
			Select (0);
		});
		select1 = new UnityAction (delegate {
			Select (1);
		});
		select2 = new UnityAction (delegate {
			Select (2);
		});
	}

	void Start () {
		nextButton.onClick.AddListener (NextCharacter);
		selectPendragon.onClick.AddListener (select0);
		selectIronRebel.onClick.AddListener (select1);
		selectSongbird.onClick.AddListener (select2);
	}

	public void Select(int index) {
		if (index == characterIndex || index < 0 || index >= characters.Count) {
			return; 
		}

		characters [characterIndex].SetActive (false);
		characterIndex = index;
		characters [characterIndex].SetActive (true);
	}

	public void NextCharacter() {
		GameObject characters1_1 = GameObject.FindGameObjectWithTag ("Characters1.1");
		GameObject characters1_2 = GameObject.FindGameObjectWithTag ("Characters1.2");
		GameObject characters1_3 = GameObject.FindGameObjectWithTag ("Characters1.3");
		GameObject characters2_1 = GameObject.FindGameObjectWithTag ("Characters2.1");
		GameObject characters2_2 = GameObject.FindGameObjectWithTag ("Characters2.2");
		GameObject characters2_3 = GameObject.FindGameObjectWithTag ("Characters2.3");
		//GameObject tm = GameObject.FindGameObjectWithTag ("Team1");

		switch (current) {
		case 0: //1.1
			characters1_1.GetComponent<CharacterSelect> ().enabled = false;
			selectPendragon.onClick.RemoveListener (select0);
			selectIronRebel.onClick.RemoveListener (select1);
			selectSongbird.onClick.RemoveListener (select2);

			characters2_1.GetComponent<CharacterSelect> ().enabled = true;
			current++;
			break;
		//	tm.GetComponent<TeamManager> ().SetCaptain(characters [characterIndex].GetComponent<Hero> ());
		case 1: //2.1
			characters2_1.GetComponent<CharacterSelect> ().enabled = false;
			selectPendragon.onClick.RemoveListener (select0);
			selectIronRebel.onClick.RemoveListener (select1);
			selectSongbird.onClick.RemoveListener (select2);

			characters1_2.GetComponent<CharacterSelect> ().enabled = true;
			current++;
			break;
		case 2: //1.2
			characters1_2.GetComponent<CharacterSelect> ().enabled = false;
			selectPendragon.onClick.RemoveListener (select0);
			selectIronRebel.onClick.RemoveListener (select1);
			selectSongbird.onClick.RemoveListener (select2);

			characters2_2.GetComponent<CharacterSelect> ().enabled = true;
			current++;
			break;
		case 3: //2.2
			characters2_2.GetComponent<CharacterSelect> ().enabled = false;
			selectPendragon.onClick.RemoveListener (select0);
			selectIronRebel.onClick.RemoveListener (select1);
			selectSongbird.onClick.RemoveListener (select2);

			characters1_3.GetComponent<CharacterSelect> ().enabled = true;
			current++;
			break;
		case 4: //1.3
			characters1_3.GetComponent<CharacterSelect> ().enabled = false;
			selectPendragon.onClick.RemoveListener (select0);
			selectIronRebel.onClick.RemoveListener (select1);
			selectSongbird.onClick.RemoveListener (select2);

			characters2_3.GetComponent<CharacterSelect> ().enabled = true;
			current++;
			break;
		default:
			return;
		}
	}

	/*
	public void PrevCharacter() {
		
	}


	public void Done() {
		gameObject.find ("characters...").GetComponent<CharacterSelect>().characterIndex;
		//record that
	}

	//positions for characters 
	*/
}
