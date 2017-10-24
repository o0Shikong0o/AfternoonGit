using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duckie : MonoBehaviour {

	GameControl gameControl; 
	public float speed;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameControl.player != null) MoveTowardsPlayer();
	}

	void MoveTowardsPlayer ()
	{
		Vector3 playerPos = gameControl.player.transform.position;
		Vector3 direction = (playerPos - transform.position).normalized;
		Debug.Log(direction);
		rb.velocity = speed * direction;
	}
}
