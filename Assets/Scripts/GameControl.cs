using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	//private properties: these are like global variables in P5.play
	float timeLeft;
	float numLaps;
	bool halfwayLineHitLast;
	bool firstGame;
	public GameObject player;
	Vector3 playerStartPoint; 
	Quaternion playerStartRotation; //3D rotations are stored as Quaternions (4D Vectors). Don't worry too much about it for now.

	//public properties: these are typically set in the Inspector, and are accessible by other scripts
	[Header("Store the prefab to create the car")] //I'm just using these to put comments in the inspector
	public GameObject carPrefab; //Reference to the prefab we'll use to make the player car

	[Header("Reference to the score text")] //I'm just using this to put comments in the inspector
	public TextMesh timeText; //Reference to the 3D Text object's TextMesh component, set in the inspector

	[Header("How much time to give on a complete lap")] //I'm just using this to put comments in the inspector
	public float bonusTimePerLap; //This float is set to public to make it easier to tune using the inspector

	// Unity calls Start() on every component or script at the start of the game.
	void Start () {
		
		//we want to store the player's starting position and rotation for when we need to reset them
		//for that we're going to need a reference to the player. I could make it a public variable and set it in the inspector
		//but instead I will use GameObject.Find to find it by name. Beware: Find() is slow! don't do it every frame.
		player = GameObject.Find ("Player");
	
		if (player != null) { //if I accidentally renamed the Player to 'Car' or something, GameObject.Find("Player") would return null
			playerStartPoint = player.transform.position;
			playerStartRotation = player.transform.rotation;
		} else {
			Debug.LogError ("Couldn't find player!");
		}
		firstGame = true;

		//If you have a custom reset function it's always a great idea to call it the first time the game runs,
		//So that you don't wind up with bugs that only happen on subsequent playthroughs
		ResetGame (); 

	}
	
	// Update is called once per frame by the engine
	void Update () {
		timeLeft -= Time.deltaTime; //Time.deltaTime is the time that passed since the last Update() 
		timeText.text = "Time left: " + timeLeft.ToString("#.#") + "\nLaps: " + numLaps; //update the text object with the score
		if (timeLeft <= 0) { //player ran out of time
			ResetGame (); 
		}
	}

	// This custom function resets the score and timer, and puts the player back in the starting point, 
	// and brings the player back to life (if they crashed)
	void ResetGame(){
		Debug.Log("dasfdsfasfsafas");
		timeLeft = bonusTimePerLap;
		numLaps = 0;
		halfwayLineHitLast = false;

		if (firstGame == false) {
			//we want to destroy the player and recreate it if this isn't the first run
			if (player != null) Destroy(player);
			player = Instantiate (carPrefab, playerStartPoint, playerStartRotation) as GameObject;
		}

		firstGame = false;
	}

	//This custom function is set to public so it can be called by the LineControl script, using SendMessage
	//It gets called whenever the player touches the finish line collider or the halfway line collider
	public void PlayerHitLine(bool finishLine){

		//The main scoring rule is implemented here. You can go forwards or backwards around the track,
		//But you have to reach the halfway line before you reach the finish line or it won't count.
		if ((halfwayLineHitLast == true) && (finishLine == true)) {
			//if we're hitting the finish line after touching the halfway line, that counts as a lap
			timeLeft += bonusTimePerLap;
			numLaps+= 1;
			halfwayLineHitLast = false;
		}

		//if the player just hit the halfway line, set that variable so we can score next time we cross the finish line
		if (finishLine == false) { 
			halfwayLineHitLast = true;
		}
	}

	//This custom function is triggered by the CarControl script, using SendMessage. 
	//It gets called after the player hits a wall, and it resets the game after a delay
	public void Crash(){
		//'Invoke' will cause the 'ResetGame' function to be called after 1 second passes.
		Invoke ("ResetGame", 1.0f); 
	}
}
