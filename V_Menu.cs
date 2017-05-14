using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
///      Menu script for "BattleCards: CCG Adventure Template"
/// 
/// "Just a simple menu script for demo..."
/// 
/// To clarify some comments, here are the reference for the TERMS:
/// 
///    Our    - Refers to this instance/GameObject (not the main player).
///    Player - Refers to the player.
///    
///            Jared Polidario | Visyde Interactives 2016
/// </summary>

public class V_Menu : MonoBehaviour {

	void Awake(){
		// Set the player to Menu Mode;
		V_PlayerHandler.isInGame = false;

		DontDestroyOnLoad (gameObject);
	}
		
	public void GoTo(string sceneName){
		SceneManager.LoadScene (sceneName);
	}
}
