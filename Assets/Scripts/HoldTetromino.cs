using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HoldTetromino : MonoBehaviour {

	private int holdTetromino;
	private GameObject tetrominosObj;
	public GameObject[] tetrominoPrefab;

	// Use this for initialization
	void Awake () {
		holdTetromino = -1;
	}

	// Get the hold tetromino
	public int GetHoldTetromino () {
		return holdTetromino;
	}

	// Set the hold tetromino
	public int SetHoldTetromino (int tetromino) {
		int temp = holdTetromino;
		holdTetromino = tetromino;
		UpdateQueue ();
		return temp;
	}

	// Update the hold tetromino object
	private void UpdateQueue () {
		if (tetrominosObj == null) {
			tetrominosObj = new GameObject ();
		}
		else {
			Destroy (tetrominosObj);
		}
		tetrominosObj = Instantiate (tetrominoPrefab [holdTetromino]);
		tetrominosObj.transform.parent = this.transform;
		tetrominosObj.transform.localScale = new Vector3 (1.3f, 1.3f, 1f);
		tetrominosObj.transform.localPosition = new Vector3 (-3.049f, 2.75f, -1f);
	}
}
