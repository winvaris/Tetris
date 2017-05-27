using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NextTetrominos : MonoBehaviour {

	private int[] queueTetrominos;
	private GameObject[] tetrominosObj;
	public GameObject[] tetrominosPrefabs;

	// Use this for initialization
	void Awake () {
		queueTetrominos = new int[4];
	}

	// Set the array of the queue
	public void SetQueueTetrominos (int[] tetrominos) {
		for (int i = 0; i < queueTetrominos.GetLength (0); i++) {
			queueTetrominos [i] = tetrominos [i];
		}
		UpdateQueue ();
	}

	// Update the queue object tetrominos
	private void UpdateQueue () {
		if (tetrominosObj == null) {
			tetrominosObj = new GameObject[queueTetrominos.GetLength (0)];
		}
		else {
			for (int i = 0; i < tetrominosObj.GetLength (0); i++) {
				Destroy (tetrominosObj [i]);
			}
		}
		for (int i = 0; i < tetrominosObj.GetLength (0); i++) {
			tetrominosObj [i] = Instantiate (tetrominosPrefabs [queueTetrominos [i]]);
			tetrominosObj [i].transform.parent = this.transform;
		}
		tetrominosObj [0].transform.localScale = new Vector3 (1.3f, 1.3f, 1f);
		tetrominosObj [0].transform.localPosition = new Vector3 (3.075f, 2.75f, -1f);
		tetrominosObj [1].transform.localPosition = new Vector3 (3.075f, 1.3f, -1f);
		tetrominosObj [2].transform.localPosition = new Vector3 (3.075f, -0.02f, -1f);
		tetrominosObj [3].transform.localPosition = new Vector3 (3.075f, -1.34f, -1f);
	}
}
