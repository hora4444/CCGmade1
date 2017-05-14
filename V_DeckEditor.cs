using UnityEngine;
using System.Collections;

/// <summary>
///      DeckEditor script for "BattleCards: CCG Adventure Template"
/// 
/// "This script allows the user to customize the deck..."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>


public class V_DeckEditor : MonoBehaviour {

	[Header("    Cards on deck:")]
	public V_CardPresenter[] myCards;
	[Header("    Other Elements:")]
	public GameObject cardCollectionList;
	public int[] myCardsIndex = new int[30] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};

	public static V_CardPresenter selectedCard;

	// Use this for initialization
	void Start () {
		// load saved decks if there is, else, make a random one....
		if (PlayerPrefs.HasKey ("DECKCARDS0")) {
			LoadCards ();
		} else {
			SaveDeck ();
		}
	}
	
	// Update is called once per frame
	public void UpdateCardsInDeck () {
		// update all the cards in the deck:
		foreach (V_CardPresenter card in myCards) {
			card.index = myCardsIndex[System.Array.IndexOf (myCards, card)];
		}
	}

	public void SaveDeck(){
		// Save current deck:
		for(int i=0;i<myCardsIndex.Length;i++){
			PlayerPrefs.SetInt("DECKCARDS"+i,myCardsIndex[i]);
		}
		LoadCards ();
		UpdateToPlayer ();

	}

	void LoadCards(){
		for(int i=0;i<myCardsIndex.Length;i++){
			myCardsIndex[i] = PlayerPrefs.GetInt("DECKCARDS"+i);
			UpdateCardsInDeck ();
			UpdateToPlayer ();
		}

	}

	public void SetCard(int newCardIndex){
		myCardsIndex[selectedCard.transform.GetSiblingIndex ()] = newCardIndex;
		cardCollectionList.SetActive (false);
		selectedCard = null;
		UpdateCardsInDeck ();
		UpdateToPlayer ();
	}

	public void Select(V_CardPresenter card){
		selectedCard = null;
		selectedCard = card;
		cardCollectionList.SetActive (true);
	}
	public void Deselect(){
		selectedCard = null;
		cardCollectionList.SetActive (false);
	}

	public void UpdateToPlayer(){
		V_PlayerHandler.deckData = myCardsIndex;
	}
}
