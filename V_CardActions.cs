using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
///      CardActions script for "BattleCards: CCG Adventure Template"
/// 
/// "This handles all of THIS card's actions and commands. This script requires
///  the V_Card component in the same GameObject."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>

public class V_CardActions : MonoBehaviour {

	[Header("    Card State:")]
	public bool isSelected = false;
	public bool isUsed = false;

	[Header("    Misc:")]
	public Transform curParent;
	public bool isInGame = false;
	public GameObject gm;

	//private variables:
	private Image Brendrr;
	private Image Srendrr;

	public bool IsCursorInZone(Vector2 cursor, GameObject zone)
	{
		Vector2 localPos = zone.transform.InverseTransformPoint(cursor);
		return ((RectTransform) zone.transform).rect.Contains(localPos);
	}

	void Awake (){
		
	}
	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag ("GameController");
		if (isInGame) {
			curParent = V_GameManager.handZone.transform;
			Brendrr = V_GameManager.battleZone.GetComponent<Image> ();
			Srendrr = V_GameManager.spellZone.GetComponent<Image> ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (isUsed) {
			this.GetComponent<V_Card> ().disabledEffect.SetActive (true);
		} else {
			this.GetComponent<V_Card> ().disabledEffect.SetActive (false);
		}

		// For Player:
		if (gameObject.tag == "PlayerOwned" || gameObject.tag == "InHand") {
			if (gm.GetComponent<V_GameManager> ().curSelected == this.GetComponent<V_Card>()) {
				this.GetComponent<V_Card> ().selectedEffect.SetActive (true);
				isSelected = true;
			} else {
				this.GetComponent<V_Card> ().selectedEffect.SetActive (false);
				isSelected = false;
			}
		}

		// For AI:
		if (gameObject.tag == "AIOwned") {
			if (gm.GetComponent<V_GameManager> ().aiCurSelected == this.GetComponent<V_Card>()) {
				this.GetComponent<V_Card> ().selectedEffect.SetActive (true);
				isSelected = true;
			} else {
				this.GetComponent<V_Card> ().selectedEffect.SetActive (false);
				isSelected = false;
			}
		}
	}
	
	public void OnDrag(){
		if (isInGame && V_GameManager.playerTurn == V_GameManager.playerTypes.Us && gameObject.tag != "PlayerOwned") {
			//Debug.Log ("Dragging Card...");
			transform.position = Input.mousePosition;
			transform.SetParent (V_GameManager.gameArea.transform);

			// Let's have the zone flashing effect when this card is dragged over a certain zone.

			//       If this card is a CREATURE card and this card is over the Battle Zone:
			if (IsCursorInZone (Input.mousePosition, V_GameManager.battleZone) && (this.GetComponent<V_Card> ().type == V_Card.cardType.Close_Rage_Creature || this.GetComponent<V_Card>().type == V_Card.cardType.Long_Rage_Creature)) {
				Brendrr.color = Color.cyan;
			}
			//       But if not, then return the Battle Zone's color to default:
			else 
			{
				Brendrr.color = Color.white;
			}

			//       If this card is a SPELL card and this card is over the Spell Zone:
			if (IsCursorInZone (Input.mousePosition, V_GameManager.spellZone) && this.GetComponent<V_Card> ().type == V_Card.cardType.Spell) {
				Srendrr.color = Color.cyan;
			}
			//       But if not, then return the Spell Zone's color to default:
			else 
			{
				Srendrr.color = Color.white;
			}
		}
	}
	public void OnDrop(){
		//On drop, reset the color of the zones:
		Brendrr.color = Color.white;
		Srendrr.color = Color.white;

		//Conditions for card depoyment:
		if (isInGame && V_GameManager.playerTurn == V_GameManager.playerTypes.Us && gameObject.tag != "PlayerOwned") {

			// If this card is a CREATURE card, then do:
			if (this.GetComponent<V_Card> ().type == V_Card.cardType.Close_Rage_Creature) {
                if (this.GetComponent<V_Card>().type == V_Card.cardType.Close_Rage_Creature){
                    if (curParent == V_GameManager.handZone.transform) {
					if (IsCursorInZone (Input.mousePosition, V_GameManager.battleZone)) {
						Debug.Log ("It's a Close_Rage_Creature!");
						if (this.GetComponent<V_Card> ().energyCost <= V_PlayerHandler.energy) {
							if (gm.GetComponent<V_GameManager> ().battleZoneHandler.transform.childCount < 6) {
								V_PlayerHandler.energy -= this.GetComponent<V_Card> ().energyCost;
								transform.SetParent (V_GameManager.battleZone.transform);
								curParent = V_GameManager.battleZone.transform;
								gameObject.tag = "PlayerOwned";
							} else {
								// Dispay error and revert move:
								gm.GetComponent<V_GameManager> ().DisplayError (gm.GetComponent<V_GameManager> ().battleZoneIsFull);
								transform.SetParent (V_GameManager.handZone.transform);
								curParent = V_GameManager.handZone.transform;
							}
						} else {
							GameObject error = Instantiate (gm.GetComponent<V_GameManager> ().errorText, V_GameManager.gameArea.transform) as GameObject;
							error.GetComponent<Text> ().text = gm.GetComponent<V_GameManager> ().noEnergy;
							transform.SetParent (V_GameManager.handZone.transform);
							curParent = V_GameManager.handZone.transform;
						}
					} else {
						if (IsCursorInZone (Input.mousePosition, V_GameManager.spellZone)) {
							// Display error:
							gm.GetComponent<V_GameManager> ().DisplayError (gm.GetComponent<V_GameManager> ().cantPlaceThere);
						}
						transform.SetParent (V_GameManager.handZone.transform);
						curParent = V_GameManager.handZone.transform;
					}
				} else {
					transform.SetParent (V_GameManager.handZone.transform);
					curParent = V_GameManager.handZone.transform;
				}
		    // But if this card is a SPELL card, then do the spell card specific
		    //    actions when dropped:
			} else {
				if (curParent == V_GameManager.handZone.transform) {
					if (IsCursorInZone (Input.mousePosition, V_GameManager.spellZone)) {
						if (this.GetComponent<V_Card> ().energyCost <= V_PlayerHandler.energy) {
							V_PlayerHandler.energy -= this.GetComponent<V_Card> ().energyCost;
							transform.SetParent (V_GameManager.spellZone.transform);
							curParent = V_GameManager.spellZone.transform;
							gameObject.tag = "PlayerOwned";
							// Do the SPELL effect:
							this.GetComponent<V_Card> ().DoEffect ();
						} else {
							GameObject error = Instantiate (gm.GetComponent<V_GameManager> ().errorText, V_GameManager.gameArea.transform) as GameObject;
							error.GetComponent<Text> ().text = gm.GetComponent<V_GameManager> ().noEnergy;
							transform.SetParent (V_GameManager.handZone.transform);
							curParent = V_GameManager.handZone.transform;
						}
					} else {
						if (IsCursorInZone (Input.mousePosition, V_GameManager.battleZone)) {
							// Display error:
							gm.GetComponent<V_GameManager> ().DisplayError (gm.GetComponent<V_GameManager> ().cantPlaceThere);
						}
						transform.SetParent (V_GameManager.handZone.transform);
						curParent = V_GameManager.handZone.transform;
					}
				} else {
					transform.SetParent (V_GameManager.handZone.transform);
					curParent = V_GameManager.handZone.transform;
				}
			}
		}
	}

	// Player card selecting actions:
	public void Select(){
		if (!isUsed && gameObject.tag == "PlayerOwned" && V_GameManager.playerTurn == V_GameManager.playerTypes.Us) {
			if (isSelected) {
				gm.GetComponent<V_GameManager> ().curSelected = null;
				isSelected = false;
			} else {
				isSelected = true;
				gm.GetComponent<V_GameManager> ().curSelected = gameObject.GetComponent<V_Card> ();
			}
		}

		if (gameObject.tag == "AIOwned" && gm.GetComponent<V_GameManager>().curSelected != null) {
			gm.GetComponent<V_GameManager> ().curSelected.GetComponent<V_CardActions> ().Use (this.GetComponent<V_Card> ());
			gm.GetComponent<V_GameManager> ().curSelected.GetComponent<V_CardActions> ().isUsed = true;
			gm.GetComponent<V_GameManager> ().curSelected = null;
		}

	}
	/*public void AISelect(){
		if (!isUsed && gameObject.tag == "AIOwned") {
			if (isSelected) {
				gm.GetComponent<V_GameManager> ().aiCurSelected = null;
				isSelected = false;
			} else {
				isSelected = true;
				gm.GetComponent<V_GameManager> ().aiCurSelected = gameObject.GetComponent<V_Card> ();

				Debug.Log ("AI has selected a card!");
			}
		}

	}*/

	public void Use(V_Card target){
		V_Card thisCard = this.GetComponent<V_Card> ();
		Debug.Log ("AI attacked!");
		if (target.gameObject.tag == "AIOwned" && this.tag == "PlayerOwned") {
			if (thisCard.type == V_Card.cardType.Close_Rage_Creature) {
				// Enemy damaged effect:
				Text enemy = Instantiate (V_GameManager.sdamageEffect, target.transform) as Text;
				enemy.text = "-" + thisCard.attackDamage;
				// We damaged effect:
				Text us = Instantiate (V_GameManager.sdamageEffect, thisCard.transform) as Text;
				us.text = "-" + target.attackDamage;
				//
				target.health -= thisCard.attackDamage;
				thisCard.health -= target.attackDamage;
				thisCard.DoEffect ();
				DestroyThisCard ();
				target.GetComponent<V_CardActions> ().DestroyThisCard ();
				isUsed = true;
			}
		}
		if (target.gameObject.tag == "PlayerOwned" && this.tag == "AIOwned") {
			if (thisCard.type == V_Card.cardType.Close_Rage_Creature) {
				Debug.Log ("AI attacked!");
				// Enemy damaged effect:
				Text enemy = Instantiate (V_GameManager.sdamageEffect, target.transform) as Text;
				enemy.text = "-" + thisCard.attackDamage;
				// We damaged effect:
				Text us = Instantiate (V_GameManager.sdamageEffect, thisCard.transform) as Text;
				us.text = "-" + target.attackDamage;
				//
				target.health -= thisCard.attackDamage;
				thisCard.health -= target.attackDamage;
				thisCard.DoEffect ();
				DestroyThisCard ();
				target.GetComponent<V_CardActions> ().DestroyThisCard ();
				isUsed = true;
			}
		}
	}

	public void UseToPlayer(GameObject target){
		V_Card thisCard = this.GetComponent<V_Card> ();
		Debug.Log ("AI attacked!");
		if (target.tag == "Player") {
			if (thisCard.type == V_Card.cardType.Close_Rage_Creature) {
				// Enemy damaged effect:
				Text enemy = Instantiate (V_GameManager.sdamageEffect, target.transform) as Text;
				enemy.text = "-" + thisCard.attackDamage;
				//
				V_PlayerHandler.health -= thisCard.attackDamage;
				isUsed = true;
			}

            else if (thisCard.type == V_Card.cardType.Long_Rage_Creature)
            {
                // Enemy damaged effect:
                Text enemy = Instantiate(V_GameManager.sdamageEffect, target.transform) as Text;
                enemy.text = "-" + thisCard.attackDamage;
                //
                V_PlayerHandler.health -= thisCard.attackDamage;
                isUsed = true;
            }
        }
		if (target.tag == "AIPlayer") {
			if (thisCard.type == V_Card.cardType.Close_Rage_Creature) {
				// Enemy damaged effect:
				Text enemy = Instantiate (V_GameManager.sdamageEffect, target.transform) as Text;
				enemy.text = "-" + thisCard.attackDamage;
				//
				V_AI.health -= thisCard.attackDamage;
				isUsed = true;
			}

            if (thisCard.type == V_Card.cardType.Long_Rage_Creature)
            {
                // Enemy damaged effect:
                Text enemy = Instantiate(V_GameManager.sdamageEffect, target.transform) as Text;
                enemy.text = "-" + thisCard.attackDamage;
                //
                V_AI.health -= thisCard.attackDamage;
                isUsed = true;
            }
        }
	}

	public void DestroyThisCard(){
		V_Card thisCard = this.GetComponent<V_Card> ();
		if (thisCard.health <= 0) {
			Instantiate (thisCard.deathEffect, thisCard.transform);
			Destroy (gameObject, 1f);
		}
	}
}

