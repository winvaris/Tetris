using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private int[,] blocks;
	private GameObject[,] blocksObj;
	public GameObject blockPrefab;
	public Sprite[] sprites;
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
		if (!gameRunning) {
			return;
		}
		else if (Input.GetKeyDown (KeyCode.W)) {
			RotateTetromino ();
		}
		else if (Input.GetKeyDown (KeyCode.A)) {
			MoveLeft ();
		}
		else if (Input.GetKeyDown (KeyCode.D)) {
			MoveRight ();
		}
		else if (Input.GetKeyDown (KeyCode.S)) {
			DropTetromino ();
		}
		else if (Input.GetKeyDown (KeyCode.N)) {
			PrintArray ();
		}

		if (dropDelayCounter > 0) {
			dropDelayCounter -= Time.deltaTime;
		}
		else {
			DropTetromino ();
		}
	}

	private void PrintArray () {
		for (int i = 0; i < 20; i++) {
			Debug.Log (blocks [i, 0] + " " + blocks [i, 1] + " " + blocks [i, 2] + " " + blocks [i, 3] + " " + blocks [i, 4] + " " + blocks [i, 5] + " " + blocks [i, 6] + " " + blocks [i, 7] + " " + blocks [i, 8] + " " + blocks [i, 9]);
		}
	}

	// Move the tetromino to the left
	private void MoveLeft () {
		if (!ReachedLeft ()) {
			DeleteBlocks ();
			currentX--;
			UpdateBlocks ();
		}
	}

	// Move the tetromino to the right
	private void MoveRight () {
		if (!ReachedRight ()) {
			DeleteBlocks ();
			currentX++;
			UpdateBlocks ();
		}
	}

	// Drop the tetromino by 1 level
	private void DropTetromino () {
		if (!ReachedBottom ()) {
			DeleteBlocks ();
			currentY++;
			UpdateBlocks ();
		}
		else {
			CheckFullRows ();
			SetTetrominoPosArray (tetromino.RandomTetromino ());
			ResetCurrentPos ();
		}
		dropDelayCounter = dropDelay;
	}

	// Rotate the tetromino clockwise
	private void RotateTetromino () {
		if (tetromino.GetCurrentRotation () % 2 == 1) {
			if (tetromino.GetTetrominoID () != 0) {
				for (int i = 0; i < 4; i++) {
					if (x [i] + currentX <= 0 || x [i] + currentX > 8) {
						return;
					}
				}
			}
			else {
				for (int i = 0; i < 4; i++) {
					if (x [i] + currentX <= 0 || x [i] + currentX > 7) {
						return;
					}
				}
			}
		}
		DeleteBlocks ();
		SetTetrominoPosArray (tetromino.RotateTetrominoClockwise ());
		UpdateBlocks ();
	}

	// Check whether the tetromino has reached the left border
	private bool ReachedLeft () {
		for (int i = 0; i < 4; i++) {
			if (x [i] + currentX - 1 < 0 || (blocks [y [i] + currentY, x [i] + currentX - 1] != 0 && !IsMe (x [i] + currentX - 1, y [i] + currentY))) {
				return true;
			}
		}
		return false;
	}

	// Check whether the tetromino has reached the right border
	private bool ReachedRight () {
		for (int i = 0; i < 4; i++) {
			if (x [i] + currentX + 1 >= 10 || (blocks [y [i] + currentY, x [i] + currentX + 1] != 0 && !IsMe (x [i] + currentX + 1, y [i] + currentY))) {
				return true;
			}
		}
		return false;
	}

	// Check whether the tetromino has reached the bottom border or other tetromino
	private bool ReachedBottom () {
		for (int i = 0; i < 4; i++) {
			if (y [i] + currentY + 1 < 20) {
				if (y [i] + currentY >= 20 || (blocks [y [i] + currentY + 1, x [i] + currentX] != 0 && !IsMe (x [i] + currentX, y [i] + currentY + 1))) {
					return true;
				}
			}
			else {
				return true;
			}
		}
		return false;
	}

	// Check for collision between tetrominoes
	private bool IsMe (int checkX, int checkY) {
		for (int i = 0; i < 4; i++) {
			if (checkY == y [i] + currentY && checkX == x [i] + currentX) {
				return true;
			}
		}
		return false;
	}

	private void CheckFullRows () {
		for (int i = blocks.GetLength (0) - 1; i >= 0; i--) {
			int count = 0;
			for (int j = 0; j < blocks.GetLength (1); j++) {
				Debug.Log ("i: " + i + ", j: " + j);
				if (blocks [i, j] > 0) {
					count++;
				}
			}
			if (count >= 10) {
				ClearRow (i);
				i++;
			}
		}
		UpdateBlocksState ();
	}

	private void ClearRow (int n) {
		Debug.Log (n);
		for (int i = n; i > 0; i--) {
			for (int j = 0; j < blocks.GetLength (1); j++) {
				blocks [i, j] = blocks [i - 1, j];
			}
		}
	}

	// Delete the previous tetromino before moving
	private void DeleteBlocks () {
		Debug.Log ("DeleteBlocks ()");
		for (int i = 0; i < 4; i++) {
			blocks [y [i] + currentY, x [i] + currentX] = 0;
		}
	}

	// Create the next tetromino after moving
	private void UpdateBlocks () {
		Debug.Log ("UpdateBlocks ()");
		for (int i = 0; i < 4; i++) {
			Debug.Log ("X: " + (x [i] + currentX));
			blocks [y [i] + currentY, x [i] + currentX] = tetromino.GetTetrominoID () + 1;
		}
		UpdateBlocksState ();
	}

	// Set the positions of the tetromino blocks
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

	// Set the UI of the blocks in the game
	private void UpdateBlocksState () {
		Debug.Log ("UpdateBlocksState ()");
		for (int i = 0; i < blocksObj.GetLength (0); i++) {
			for (int j = 0; j < blocksObj.GetLength (1); j++) {
				blocksObj [i, j].SetActive (blocks [i, j] > 0);
				blocksObj [i, j].GetComponent<SpriteRenderer>().sprite = sprites [blocks [i, j]];
			}
		}
	}

	// Reset the position for the next tetromino
	private void ResetCurrentPos () {
		currentX = 5;
		currentY = 1;
	}

	// Initialize all the blocks in the game and deactivate it
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