using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
///      GameManager script for "BattleCards: CCG Adventure Template"
/// 
/// "This component stores all the cards of the demo."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>

public class V_CardCollections : MonoBehaviour {

	[Header("All cards of the game:")]
	public V_Card[] gameCards;

	[Header("UI:")]
	public GameObject cardsListContent;
	public GameObject cardPresenter;

	public static V_Card[] cards;
	// Use this for initialization
	void Start () {
		
		cards = gameCards;

		foreach (V_Card card in gameCards) {
			GameObject prsntr = Instantiate (cardPresenter, cardsListContent.transform) as GameObject;
			prsntr.transform.GetChild (0).GetComponent<Text> ().text = card.cardName;
			prsntr.GetComponent<V_CardPresenter> ().index = System.Array.IndexOf(gameCards, card);
			prsntr.GetComponent<V_CardPresenter> ().index = cardsListContent.transform.childCount - 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
