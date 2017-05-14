using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
///      Card script for "BattleCards: CCG Adventure Template"
/// 
/// "This is the main component for every cards in this game 
///              - the most important actually..."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>



[ExecuteInEditMode]
[DisallowMultipleComponent]
public class V_Card : MonoBehaviour {

	public enum cardType {Close_Rage_Creature, Long_Rage_Creature, Spell, endureSpell };
	public enum cardRank {Common, Uncommon, Unique};
	public enum cardEffect {None, DrawNewCard, AddEnergy, AddHealth, DamagePlayer};
	public enum cardTarget {None, ToUs, ToOpponent};
	public enum usage {All, Close_Rage_CardsOnly, BaseOnly};
	[Header("    Card Type:")]
	public cardType type;
	[Header("    Card Rank:")]
	public cardRank rank;
	[Header("    Name & Description:")]
	public string cardName = "Warrior";
	public string cardDescription = "Active: Deal 1 damage to a card or to the opponent player.";

	[Header("    Attributes:")]
	public int attackDamage = 2;
	public int health = 1;
	public int energyCost = 1;
	[Space]
	[Header("                             -Extra Effects-")]
	public cardEffect extraEffect;
	public cardTarget target;
	public int effectValue = 1;
	[HideInInspector]
	public bool autoUse = false;
	[Space]
	[Header("    Usage:")]
	public usage canBeUsedTo;
	[Space]
	[Header("    Parts:")]
	public Sprite graphicImage;
	public Image graphicHandler;
	public Text cardNameHandler;
	public Text cardDescriptionHandler;
	public Text cardLevelHandler;
	public Text cardDamageHandler;
	public Text cardHealthHandler;
	public Text cardEnergyHandler;
	[Space]
	[Header("    Miscs:")]
	public GameObject selectedEffect;
	public GameObject disabledEffect;
	public GameObject deathEffect;
	[Space]
	public bool placed = false;
	public float delay = 1;
	private float curDelay = 0;

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			health = 0;
		}
		if (type == cardType.Spell && autoUse == false) {
			autoUse = true;
		}
		if (extraEffect == cardEffect.None) {
			target = cardTarget.None;
		}
		// For UI:
		if (graphicImage != null && graphicHandler != null) {
			graphicHandler.sprite = graphicImage;
		}
		if (cardNameHandler != null) {
			cardNameHandler.text = cardName;
		}
		if (cardDescriptionHandler != null) {
			cardDescriptionHandler.text = cardDescription;
		}
		if (cardDamageHandler != null) {
			cardDamageHandler.text = attackDamage.ToString ();
		}
		if (cardHealthHandler != null) {
			cardHealthHandler.text = health.ToString ();
		}
		if (cardEnergyHandler != null) {
			cardEnergyHandler.text = energyCost.ToString ();
		}

		//// IN THE BATTLEFIELD:
		if (placed == true) {
			curDelay += Time.deltaTime;
			if (curDelay >= delay){
				curDelay = 0;
				placed = false;
				DoEffect();
			}
		}
	}

	public void DoEffect(){
		if (this.gameObject.tag == "PlayerOwned") {
			if (extraEffect == cardEffect.AddEnergy) {
				if (target == cardTarget.ToOpponent) {
					V_GameManager.EffectAddEnergy (effectValue);
				} else {
					V_PlayerHandler.AddEnergy (effectValue);
				}
			}
			if (extraEffect == cardEffect.AddHealth) {
				if (target == cardTarget.ToOpponent) {
					V_GameManager.EffectHeal (effectValue);
				} else {
					V_PlayerHandler.EffectHeal (effectValue, V_GameManager.shealEffect.gameObject, V_GameManager.avatarZone.transform);
				}
			}
			if (extraEffect == cardEffect.DamagePlayer) {
				if (target == cardTarget.ToOpponent) {
					V_GameManager.EffectDamage (effectValue);
				} else {
					V_PlayerHandler.EffectDamage (effectValue, V_GameManager.sdamageEffect.gameObject, V_GameManager.avatarZone.transform);
				}
			}
			if (extraEffect == cardEffect.DrawNewCard) {
				if (target == cardTarget.ToOpponent) {
					// nothing here yet...
				} else {
					V_PlayerHandler player = GameObject.FindGameObjectWithTag ("PlayerHandler").GetComponent<V_PlayerHandler> ();
					player.DrawOneCard();
				}
			}
		}
		if (type == cardType.Spell) {
			SpellActivate ();
		}
	}

	public void DoEffectAI(){
		if (this.gameObject.tag == "AIOwned") {
			if (extraEffect == cardEffect.AddEnergy) {
				if (target == cardTarget.ToOpponent) {
					V_PlayerHandler.AddEnergy (effectValue);
				} else {
					V_GameManager.EffectAddEnergy (effectValue);
				}
			}
			if (extraEffect == cardEffect.AddHealth) {
				if (target == cardTarget.ToOpponent) {
					V_PlayerHandler.EffectHeal (effectValue, V_GameManager.shealEffect.gameObject, V_GameManager.avatarZone.transform);
				} else {
					V_GameManager.EffectHeal (effectValue);
				}
			}
			if (extraEffect == cardEffect.DamagePlayer) {
				if (target == cardTarget.ToOpponent) {
					V_PlayerHandler.EffectDamage (effectValue, V_GameManager.sdamageEffect.gameObject, V_GameManager.avatarZone.transform);
				} else {
					V_GameManager.EffectDamage (effectValue);
				}
			}
		}
		if (type == cardType.Spell) {
			SpellActivate ();
		}
	}

	public void SpellActivate(){
		Destroy (gameObject, delay);
		GameObject gm = GameObject.FindGameObjectWithTag("GameController");
		if (gm.GetComponent<V_GameManager> ().curSelected == this) {
			gm.GetComponent<V_GameManager> ().curSelected = null;
		}
	}
}
