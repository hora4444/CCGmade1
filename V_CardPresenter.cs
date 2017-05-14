using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
///      CardPresenter script for "BattleCards: CCG Adventure Template"
/// 
/// "This script is used to present a card for deck customization or cards
/// viewing..."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>


public class V_CardPresenter : MonoBehaviour {

	[HideInInspector]
	public int index;
	[Header(" Mode:")]
	public bool OnDeck = false;     // if this presenter will be used in presenting deck cards
	public bool OnCards = true;     // if this presenter will be used in presenting card selectors

	private GameObject toolTip;

	//Miscs:
	public bool IsCursorInZone(Vector2 cursor, GameObject zone)
	{
		Vector2 localPos = zone.transform.InverseTransformPoint(cursor);
		return ((RectTransform) zone.transform).rect.Contains(localPos);
	}

	// Use this for initialization
	void Start () {
		toolTip = GameObject.FindGameObjectWithTag ("ToolTip");

	}
	
	// Update is called once per frame
	void Update () {
		Text txt = transform.GetChild (0).GetComponent<Text> ();
		txt.text = V_CardCollections.cards [index].cardName;

		if (OnDeck) {
			OnCards = false;
		} else {
			OnCards = true;
		}
		// Let's do some tooltips!
		if (EventSystem.current.IsPointerOverGameObject ()){
			if (IsCursorInZone (Input.mousePosition, gameObject)) {
				toolTip.SetActive (true);
				toolTip.transform.position = new Vector3 (Input.mousePosition.x +  toolTip.GetComponent<RectTransform>().rect.width, Input.mousePosition.y - toolTip.GetComponent<RectTransform>().rect.height, 0f);
				toolTip.transform.GetChild (0).GetComponent<Text> ().text = V_CardCollections.cards [index].cardDescription;
			}
		} else {
			toolTip.SetActive (false);
		}

		if (V_DeckEditor.selectedCard == this) {
			this.GetComponent<Button> ().image.color = Color.cyan;
		} else {
			this.GetComponent<Button> ().image.color = Color.white;
		}
	}

	public void Click(){
		V_DeckEditor de = GameObject.FindGameObjectWithTag ("DeckEditor").GetComponent<V_DeckEditor> ();
		if (OnDeck) {
			if (V_DeckEditor.selectedCard == this) {
				de.Deselect ();
			} else {
				de.Select (this);
			}
		} else {
			de.SetCard (index);
		}
	}
}
