using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BlockController : NetworkBehaviour {

	[SyncVar (hook = "SetColor")] public Color color;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChooseColor (int n) {
		if (n == 0) {
			this.color = new Color (129f, 128f, 129f);
		}
		else if (n == 1) {
			this.color = new Color (90f, 162f, 96f);
		}
		else if (n == 2) {
			this.color = new Color (255f, 141f, 0f);
		}
		else if (n == 3) {
			this.color = new Color (255f, 77f, 145f);
		}
		else if (n == 4) {
			this.color = new Color (123f, 84f, 145f);
		}
		else if (n == 5) {
			this.color = new Color (176f, 33f, 33f);
		}
		else if (n == 6) {
			this.color = new Color (233f, 215f, 35f);
		}
		else if (n == 7) {
			this.color = new Color (0f, 133f, 214f);
		}
	}

	public void SetColor (Color color) {
		this.transform.gameObject.GetComponent<SpriteRenderer> ().color = this.color;
	}
}
