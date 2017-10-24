using UnityEngine;
using System.Collections;

public class CarControl : MonoBehaviour {
	//private properties (like P5's global variables) - but inaccessible by other scripts)
	Rigidbody2D rb;
	AudioSource audio;
	GameObject gameManager;
	bool crashed;

	//public properties - these are typically set in the inspector and are accessible by other scripts
	[Header("Plays when the car crashes")] //I'm just using this to put comments in the inspector
	public AudioClip crashSound;
	[Header("Tuning variable for car acceleration")]
	public float acceleration;
	[Header("Tuning variable for car turning")]
	public float turnSpeed;
	[Header("Tuning variable for braking")]
	public float brakeAmount;


	// Use this for initialization
	void Start () {
		//we need a reference to the game manager so we can tell it if the car crashed, so we search for it by name
		//Beware: Find() is slow! don't do it every frame.
		gameManager = GameObject.Find ("GameManager");
		rb = GetComponent<Rigidbody2D> (); //we need an easy way to access the Rigidbody2D component on this GameObject
		audio = GetComponent<AudioSource> (); //and an easy way to access the AudioSource component
		crashed = false;
	}
	
	// FixedUpdate is called before the physics system updates
	void FixedUpdate () {

		//We want to use that value to change the player's velocity in the forward direction only (so it drives like a car)
		//To make a small change in the velocity we first copy it into a temporary Vector2
		//transform.InverseTransformDirection takes a velocity in world coordinates and converts it to local coordinates
		//What is a local coordinate? Well, in this case, it means that the right vector (x=1,y=0) always points to the right of the car,
		//even if the car is rotated. Don't worry too much if this is confusing at first.
		Vector2 speedInLocalCoordinates = transform.InverseTransformDirection (rb.velocity);
		speedInLocalCoordinates.x *= 0.9f; //if the car is sliding sideways we slow that down a lot

		if (crashed == false) { //only accept input if the car hasn't crashed
			//We're going to use the vertical input axis for accelerating and braking
			//The axis ranges from -1.0 to 1.0, but I want braking to be weaker so I clamp it to 0 to 1.0 so it only goes forward
			//Then I multiply it by the acceleration global variable to get a number to accelerate by
			float changeInForwardSpeed = Mathf.Clamp(Input.GetAxis ("Vertical"),0.0f,1.0f) * acceleration;

			//The Y coordinate of this vector always points towards the front of the car, so we use that for accelerating
			speedInLocalCoordinates.y += changeInForwardSpeed * Time.fixedDeltaTime; //Time.fixedDeltaTime is the number of seconds that will pass before the next fixedupdate
			//We'll use the fire button (A or Space) as a brake
			if (Input.GetButton ("Jump"))
				speedInLocalCoordinates.y *= brakeAmount;

			//Same thing for turning - first we store the current rotation of the Rigidbody2D in a variable (it's in degrees)
			float newAngle = rb.rotation;
			//Then we update that value using the horizontal input axis, the turn speed variable, and the current forward speed
			//(we don't want the car to be able to turn when stopped)
			newAngle -= Input.GetAxis ("Horizontal") * turnSpeed * Time.fixedDeltaTime*Mathf.Clamp(speedInLocalCoordinates.y*0.1f,0.0f,1.0f);
			//the MoveRotation() and MovePosition() methods are safe ways to directly move Rigidbody2Ds, respecting collisions
			rb.MoveRotation (newAngle);

			//Using the forward speed and input axis to set the volume and pitch of the engine noise
			audio.volume = speedInLocalCoordinates.y;
			audio.pitch = (Input.GetAxis ("Vertical") + 1.0f) * 2.0f;
		} 

		//Having updated the velocity, we just do the reverse operation, converting back into World coordinates
		//and copying it to the velocity property of the rigidBody2D
		rb.velocity = transform.TransformDirection(speedInLocalCoordinates);
	}

	//OnCollisionEnter2D() is called by the Unity engine under the following conditions:
	//1) this object has a Collider2D and a Rigidbody2D
	//2) it touched another object with (at least) a Collider2D
	//The system passes it a special Collision2D object with information about the collision 
	//(including which object we collided with)
	void OnCollisionEnter2D(Collision2D thisCollision){
		if (crashed == true) {
			
			//abort the function! we only want to crash once
			return;
		}
		//Debug.Log works like Javascript's console.log
		Debug.Log ("Player crashed into " + thisCollision.collider.name);

		GetComponent<SpriteRenderer> ().color = Color.red; //Color.red is a nice shorthand for 'new Color(1.0f,0.0f,0.0f,1.0f)'
		audio.Stop (); //stop the engine sound
		audio.volume = 1.0f; 
		audio.pitch = 1.0f; //When .pitch is 1.0 it plays the sound at normal pitch
		audio.PlayOneShot (crashSound); //PlayOneShot lets you play an AudioClip that isn't currently set as the 'clip' property on this AudioSource
		gameManager.SendMessage ("ResetGame");
		crashed = true; //set this to make sure this function doesn't get called again

	}

}
