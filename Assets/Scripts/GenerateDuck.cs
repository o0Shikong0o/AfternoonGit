using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDuck : MonoBehaviour {

	public Transform leftFoot;
	public Transform rightFoot;

	public Transform duckBaby;
	public float timer = 2;

	public List<Duckie> duckbabies = new List<Duckie>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		timer -= Time.deltaTime;

		if (timer <= 0) {
			Instantiate (duckBaby, new Vector2 (Random.Range (1, 5), Random.Range (1, 5)), Quaternion.identity);
			timer = 2;
		}

		/*if (duckbabies.Count > 0) {
			for(int i = 0; i < duckbabies.Count; i++){
				Vector2.Lerp(duckbabies[i].transform.position, duckbabies[i - 1].transform.position, 0.2f);
			}
		}*/

	}
}
