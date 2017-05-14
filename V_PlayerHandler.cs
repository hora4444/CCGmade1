using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

/// <summary>
///      PlayerHandler script for "BattleCards: CCG Adventure Template"
/// 
/// "This is the main script for the player controller object. Only one instance of
/// this component per scene is allowed."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>

public class V_PlayerHandler : MonoBehaviour {

	[Header("Put some cards here!")]
	public V_Card[] myDeck;

	public static int[] deckData;
	// The following variables are to be managed by the V_GameManager script...
	public static int health = 0;
	public static int energy = 0;
	public static bool isInGame = false;
	//private variables:
	private GameObject gm;

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			health = 0;
		}
		if (!isInGame) {
			V_CardCollections dtbase = GameObject.FindGameObjectWithTag ("CardDatabase").GetComponent<V_CardCollections> ();
			if (deckData != null) {
				for (int i=0;i<deckData.Length; i++) {
					myDeck [i] = dtbase.gameCards [deckData [i]];
				}
			}
		}
	}

	public void EndTurn(){
		V_GameManager.ChangeTurn (V_GameManager.playerTypes.Us);
		gm = GameObject.FindGameObjectWithTag ("GameController");
		gm.GetComponent<V_GameManager> ().curSelected = null;
		gm.GetComponent<V_GameManager> ().aiCurSelected = null;
	}

	public void ReDraw(){
		gm = GameObject.FindGameObjectWithTag ("GameController");
		if (energy >= gm.GetComponent<V_GameManager> ().drawCost && V_GameManager.playerTurn == V_GameManager.playerTypes.Us) {
			V_Card[] picks = new V_Card[] {
				myDeck [Random.Range (0, myDeck.Length)],
				myDeck [Random.Range (0, myDeck.Length)],
				myDeck [Random.Range (0, myDeck.Length)],
				myDeck [Random.Range (0, myDeck.Length)]
			};
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			gc.GetComponent<V_GameManager> ().Redraw (picks);
			energy -= gm.GetComponent<V_GameManager> ().drawCost;
		} else {
			// Display error:
			gm.GetComponent<V_GameManager> ().DisplayError (gm.GetComponent<V_GameManager> ().noEnergy);
		}
	}

	public void StartDraw(){
		if (V_GameManager.playerTurn == V_GameManager.playerTypes.Us) {
			V_Card[] picks = new V_Card[] {
				myDeck [Random.Range (0, myDeck.Length)],
				myDeck [Random.Range (0, myDeck.Length)],
				myDeck [Random.Range (0, myDeck.Length)],
				myDeck [Random.Range (0, myDeck.Length)]
			};
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			gc.GetComponent<V_GameManager> ().Redraw (picks);
		}
	}

	public void DrawOneCard(){
		V_Card[] picks = new V_Card[] { myDeck [Random.Range (0, myDeck.Length)] };
		GameObject gc = GameObject.FindGameObjectWithTag ("GameController");
		gc.GetComponent<V_GameManager> ().DrawACard (picks);
	}

	// RECIEVE EFFECTS (called by other scripts):
	public static void AddEnergy(int value){
        energy = 0;
		energy += value;
	}

	public static void EffectDamage (int value, GameObject effect, Transform parentZone){
		health -= value;
		GameObject obj = Instantiate (effect, parentZone) as GameObject;
		obj.GetComponent<Text>().text = "-" + value.ToString();
	}
	public static void EffectHeal (int value, GameObject effect, Transform parentZone){
		health += value;
		GameObject obj = Instantiate (effect, parentZone) as GameObject;
		obj.GetComponent<Text>().text = "+" + value.ToString();
	}
		
}