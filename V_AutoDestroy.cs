﻿using UnityEngine;
using System.Collections;

/// <summary>
///      AutoDestroy script for "BattleCards: CCG Adventure Template"
/// 
/// "A very simple script for destroying objects with delay..."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>

public class V_AutoDestroy : MonoBehaviour {

	[Header("    Delay (in ms)")]
	public float delay = 1f;

	// Use this for initialization
	void Start () {
		// Destroy this GameObject after the delay:
		Destroy (gameObject, delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
