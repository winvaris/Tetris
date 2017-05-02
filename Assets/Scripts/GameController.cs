﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private int[,] blocks;
	private GameObject[,] blocksObj;
	public GameObject blockPrefab;
	public GameObject tetrominoObj;
	private TetrominoController tetromino;
	private bool hasPlaced;
	private bool gameRunning;
	private float dropDelay;
	private float dropDelayCounter;
	private int currentX;
	private int currentY;
	private int[] x;
	private int[] y;
	private float startX;
	private float startY;

	// Use this for initialization
	void Awake () {
		blocks = new int [20, 10];
		tetromino = tetrominoObj.GetComponent<TetrominoController> ();
		hasPlaced = false;
		gameRunning = false;
		dropDelay = 1f;
		dropDelayCounter = dropDelay;
		ResetCurrentPos ();
		x = new int[4];
		y = new int[4];
		startX = -1.88f;
		startY = 4.005f;
		InitializeBlocksObj ();
		UpdateBlocksState ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Y)) {
			gameRunning = true;
			Debug.Log ("Y Pressed");
			SetTetrominoPosArray (tetromino.RandomTetromino ());
			UpdateBlocks ();
		}
		else if (Input.GetKeyDown (KeyCode.T)) {
			DeleteBlocks ();
			SetTetrominoPosArray (tetromino.RotateTetromino ());
			UpdateBlocks ();
		}
		if (!gameRunning) {
			return;
		}

		if (dropDelayCounter > 0) {
			dropDelayCounter -= Time.deltaTime;
		}
		else {
			// If the piece haven't reached the bottom
			if (true) {

			}

			// If the piece have reached the bottom
			else {

			}

			DropTetromino ();
			dropDelayCounter = dropDelay;
		}
	}

	private void DropTetromino () {
		DeleteBlocks ();
		currentY++;
		UpdateBlocks ();
	}

	private void DeleteBlocks () {
		Debug.Log ("DeleteBlocks ()");
		for (int i = 0; i < 4; i++) {
			blocks [y [i] + currentY, x [i] + currentX] = 0;
		}
	}

	private void UpdateBlocks () {
		Debug.Log ("UpdateBlocks ()");
		for (int i = 0; i < 4; i++) {
			blocks [y [i] + currentY, x [i] + currentX] = 1;
		}
		UpdateBlocksState ();
	}

	private void SetTetrominoPosArray (string str) {
		Debug.Log ("SetTetrominoPosArray ()");
		string[] temp = str.Split (' ');
		x [0] = int.Parse (temp [0]);
		y [0] = int.Parse (temp [1]);
		x [1] = int.Parse (temp [2]);
		y [1] = int.Parse (temp [3]);
		x [2] = int.Parse (temp [4]);
		y [2] = int.Parse (temp [5]);
		x [3] = int.Parse (temp [6]);
		y [3] = int.Parse (temp [7]);
	}

	private void UpdateBlocksState () {
		Debug.Log ("UpdateBlocksState ()");
		for (int i = 0; i < blocksObj.GetLength (0); i++) {
			for (int j = 0; j < blocksObj.GetLength (1); j++) {
				blocksObj [i, j].SetActive (blocks [i, j] > 0);
			}
		}
	}

	private void ResetCurrentPos () {
		currentX = 4;
		currentY = 1;
	}

	private void InitializeBlocksObj () {
		blocksObj = new GameObject[blocks.GetLength (0), blocks.GetLength (1)];
		Debug.Log (blocksObj.GetLength (0));
		Debug.Log (blocksObj.GetLength (1));
		for (int i = 0; i < blocksObj.GetLength (0); i++) {
			for (int j = 0; j < blocksObj.GetLength (1); j++) {
				blocksObj [i, j] = Instantiate (blockPrefab);
				blocksObj [i, j].transform.parent = this.transform;
				blocksObj [i, j].transform.localPosition = new Vector3 (startX + (0.42f * j), startY - (0.42f * i), -1f);
				blocksObj [i, j].SetActive (false);
			}
		}
	}
}