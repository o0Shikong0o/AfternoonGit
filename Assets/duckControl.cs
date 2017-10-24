using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class duckControl : MonoBehaviour {

	public Transform leftFoot;
	public Transform rightFoot;

	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.A )) {
		Debug.Log("Test");
			float testForce = 8f;
			rb.AddForceAtPosition(transform.up*testForce, leftFoot.position, ForceMode2D.Impulse);
		} else if (Input.GetKeyDown (KeyCode.D )) {
		Debug.Log("Test");
			float testForce = 8f;
			rb.AddForceAtPosition(transform.up*testForce, rightFoot.position, ForceMode2D.Impulse);
		}
	}
}
