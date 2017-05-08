using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalCheckController : NetworkBehaviour {

	public GameObject sideBars;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			sideBars.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
