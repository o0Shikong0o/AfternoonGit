using UnityEngine;
using System.Collections;

public class LineControl : MonoBehaviour {
	//these public properties are set in the inspector

	//reference to the Game Manager object which controls the game, 
	//so we can tell the manager when the player crosses the line
	public GameObject gameManager; 
	public bool finishLine; //is this the finish line or the half-way line?

	//OnTriggerEnter2D is called by the Collider2D component in the following conditions:
	//1) this object's Collider2D is set to 'Trigger' mode
	//2) some GameObject with a Collider2D and a Rigidbody2D touched it
	//The system sends this function a parameter that is a reference to the collider that touched this one
	void OnTriggerEnter2D(Collider2D colliderThatHitMe){
		
		//We will use SendMessage() to tell the Game Manager to call the 'PlayerHitLine' function
		//we pass our 'finishLine' boolean to the game Manager as a parameter to the function,  
		//so it knows if the car passed the finish line or the halfway line
		gameManager.SendMessage ("PlayerHitLine", finishLine); 
	}
}
