using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private int[,] blocks;
	private GameObject tetrominoObj;
	private TetrominoController tetromino;
	private bool hasPlaced;
	private bool gameRunning;
	public float dropDelay;
	private float dropDelayCounter;
	private int currentX;
	private int currentY;

	// Use this for initialization
	void Awake () {
		blocks = new int [20, 10];
		hasPlaced = false;
		gameRunning = false;
		dropDelay = 1f;
		dropDelayCounter = dropDelay;
		ResetCurrentPos ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameRunning) {
			return;
		}

		if (dropDelayCounter > 0) {
			dropDelayCounter -= Time.deltaTime;
		}
	}

	private void ResetCurrentPos () {
		currentX = 4;
		currentY = 1;
	}
}