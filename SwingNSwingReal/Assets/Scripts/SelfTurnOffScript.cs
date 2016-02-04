﻿using UnityEngine;
using System.Collections;

public class SelfTurnOffScript : MonoBehaviour {
	public float delay;
	public float randomInterval;
	public bool random;
	public bool shrink;
	public bool beginCountOnEnable = true;
	public bool IsChainLink = false;
	// only works with polygon colliders
	public bool sinkIntoGround;
	Rigidbody2D RB;
	PolygonCollider2D PC;
	Vector3 scale;
	// Use this for initialization
	void Start () {
		if (sinkIntoGround) {
			RB = GetComponent<Rigidbody2D> ();
			PC = GetComponent<PolygonCollider2D> ();
		}
		if (shrink) {
			scale = transform.localScale;
		}
	}
	void OnEnable(){
		if (beginCountOnEnable) {
			StartCountdown ();
		}
	}

	public void StartCountdown(float newDelay = -1){
		if (newDelay == -1){
			newDelay = delay;
		}
		if (shrink && scale.x != 0) {
			transform.localScale = scale;
		}
		if (sinkIntoGround) {
			if (RB == null) {
				RB = GetComponent<Rigidbody2D> ();
			}
			RB.gravityScale = 1;
			if (PC == null) {
				PC = GetComponent<PolygonCollider2D> ();
			}
			PC.enabled = true;
		}
		if (random) {
			float x = Random.Range (-randomInterval, randomInterval);
			StartCoroutine(TurnOff(newDelay + x));
		} else {

			StartCoroutine(TurnOff(newDelay));
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator TurnOff(float newDelay){
		if (IsChainLink) {
			yield return new WaitForSeconds (newDelay -1);
			GetComponent<ChainLinkScript> ().DelinkChain ();
			yield return new WaitForSeconds (1);
		} else {
			yield return new WaitForSeconds (newDelay);
		}

		if (shrink) {
			for (int x = 0; x < 50; x++) {
				if (transform.localScale.x > 0 && transform.localScale.y > 0 && transform.localScale.z > 0) {
					transform.localScale -= new Vector3 (.01f, .01f, .01f);
				}
				yield return null;
			}
		} else if (sinkIntoGround) {
			PC.enabled = false;
			RB.gravityScale = .05f;
			yield return new WaitForSeconds (1f);
		}

		gameObject.SetActive (false);
	}

}
