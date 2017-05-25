using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

	private int[,] blocks;
	private GameObject[,] blocksObj;
	public GameObject blockPrefab;
	public Sprite[] sprites;
	public GameObject tetrominoObj;
	private TetrominoController tetromino;
	public GameObject queueObj;
	private NextTetrominos queue;
	public GameObject holdObj;
	private HoldTetromino hold;
	private bool justSpanwed;
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
	private int[] randomedTetrominos;
	private int holdTetromino;
	private bool holdUsed;
	private bool pushUsed;
	public GameObject downSound;
	private AudioSource audioPlayer;
	[SyncVar] public string networkBlocks;

	// Use this for initialization
	void Awake () {
		blocks = new int [20, 10];
		tetromino = tetrominoObj.GetComponent<TetrominoController> ();
		queue = queueObj.GetComponent<NextTetrominos> ();
		hold = holdObj.GetComponent<HoldTetromino> ();
		networkBlocks = "";
		justSpanwed = true;
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
		randomedTetrominos = new int[4];
		RandomTetrominos ();
		holdUsed = false;
		pushUsed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Y) && !gameRunning) {
			Debug.Log ("Y Pressed");
			StartGame ();
		}
		else if (Input.GetKeyDown (KeyCode.R)) {
			ResetBlocks ();
		}
		if (!gameRunning) {
			return;
		}
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			RotateTetromino ();
		}
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			MoveLeft ();
		}
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			MoveRight ();
		}
		else if (Input.GetKey (KeyCode.DownArrow)) {
			dropDelayCounter -= Time.deltaTime * 18;
		}
		else if (Input.GetKeyDown (KeyCode.Space)) {
			PushToBottom ();
		}
		else if (Input.GetKeyDown (KeyCode.LeftShift)) {
			HoldTetromino ();
		}
		else if (Input.GetKeyDown (KeyCode.N)) {
			PrintArray ();
		}
		else if (Input.GetKeyDown (KeyCode.M)) {
			Debug.Log ("Randomed Tetrominos");
			for (int i = 0; i < 4; i++) {
				Debug.Log (randomedTetrominos [i]);
			}
		}

		// Time counter for tetromino to drop down
		if (dropDelayCounter > 0) {
			dropDelayCounter -= Time.deltaTime;
		}
		else {
			DropTetromino ();
		}
	}

	// Start the game
	public void StartGame () {
		gameRunning = true;
		SetTetrominoPosArray (tetromino.RandomTetromino (randomedTetrominos [0]));
		RandomNextTetromino ();
		UpdateBlocks ();
		queue.SetQueueTetrominos (randomedTetrominos);
	}

	// Set hold tetromino
	private void HoldTetromino () {
		if (!holdUsed) {
			int temp = hold.GetHoldTetromino ();
			holdTetromino = tetromino.GetTetrominoID ();
			hold.SetHoldTetromino (holdTetromino);
			DeleteBlocks ();
			if (temp < 0) {
				SpawnTetromino ();
			}
			else {
				SetTetrominoPosArray (tetromino.RandomTetromino (temp));
				ResetCurrentPos ();
				dropDelayCounter = 0f;
				justSpanwed = true;
			}
			holdUsed = true;
		}
	}

	private void SetSyncStr () {
		networkBlocks = "";
		for (int i = 0; i < blocks.GetLength (0); i++) {
			for (int j = 0; j < blocks.GetLength (1); j++) {
				networkBlocks += blocks [i, j] + " ";
			}
		}
	}

	private void GetSyncStr () {
		string[] syncArray = networkBlocks.Split (' ');
		for (int i = 0; i < blocks.GetLength (0); i++) {
			for (int j = 0; j < blocks.GetLength (1); j++) {
				if (syncArray.Length > 1) {
					blocks [i, j] = int.Parse (syncArray [(i * 10) + j]);
				}
				else {
					blocks [i, j] = 0;
				}
			}
		}
	}

	// Initialize random tetrominoes
	private void RandomTetrominos () {
		randomedTetrominos [0] = Random.Range (0, 7);
		for (int i = 1; i < randomedTetrominos.GetLength (0); i++) {
			randomedTetrominos [i] = Random.Range (0, 7);
			while (randomedTetrominos [i] == randomedTetrominos [i - 1]) {
				randomedTetrominos [i] = Random.Range (0, 7);
			}
		}
	}

	// Random next tetromino
	private void RandomNextTetromino () {
		int temp = Random.Range (0, 7);
		while (temp == randomedTetrominos [3]) {
			temp = Random.Range (0, 7);
		}
		randomedTetrominos [0] = randomedTetrominos [1];
		randomedTetrominos [1] = randomedTetrominos [2];
		randomedTetrominos [2] = randomedTetrominos [3];
		randomedTetrominos [3] = temp;
	}

	// Print out the array
	private void PrintArray () {
		for (int i = 0; i < 20; i++) {
			Debug.Log (blocks [i, 0] + " " + blocks [i, 1] + " " + blocks [i, 2] + " " + blocks [i, 3] + " " + blocks [i, 4] + " " + blocks [i, 5] + " " + blocks [i, 6] + " " + blocks [i, 7] + " " + blocks [i, 8] + " " + blocks [i, 9]);
		}
	}

	// Push the tetromino directly to the buttom
	private void PushToBottom () {
		if (!pushUsed) {
			while (!ReachedBottom ()) {
				DropTetromino ();
				dropDelayCounter = 0;
			}
			dropDelayCounter = 0f;
			pushUsed = true;
		}
		audioPlayer = downSound.GetComponent<AudioSource> ();
		audioPlayer.Play ();

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
			if (justSpanwed) {
				currentY--;
				justSpanwed = false;
			}
			UpdateBlocks ();
			dropDelayCounter = dropDelay;
		}
		else {
			CheckFullRows ();
			SpawnTetromino ();
			holdUsed = false;
		}
		pushUsed = false;
	}

	// Spawn tetromino
	private void SpawnTetromino () {
		SetTetrominoPosArray (tetromino.RandomTetromino (randomedTetrominos [0]));
		RandomNextTetromino ();
		queue.SetQueueTetrominos (randomedTetrominos);
		ResetCurrentPos ();
		if (GameOver ()) {
			Debug.Log ("Game Over");
			gameRunning = false;
			BlackOut ();
		}
		dropDelayCounter = 0f;
		justSpanwed = true;
	}

	// Rotate the tetromino clockwise
	private void RotateTetromino () {
//		if (tetromino.GetCurrentRotation () % 2 == 1) {
//			if (tetromino.GetTetrominoID () != 0) {
//				for (int i = 0; i < 4; i++) {
//					if (x [i] + currentX <= 0 || x [i] + currentX > 8) {
//						return;
//					}
//				}
//			}
//			else {
//				for (int i = 0; i < 4; i++) {
//					if (x [i] + currentX <= 0 || x [i] + currentX > 7) {
//						return;
//					}
//				}
//			}
//		}
		if (CanRotate ()) {
			DeleteBlocks ();
			SetTetrominoPosArray (tetromino.RotateTetrominoClockwise ());
			UpdateBlocks ();
		}
	}

	// Check whether the tetromino can rotate or not
	private bool CanRotate () {
		string tempStr = tetromino.RotateTetrominoClockwise ();
		tetromino.RotateTetrominoCounterClockwise ();
		string[] arrStr = tempStr.Split (' ');
		int[] tempPosX = new int [4];
		int[] tempPosY = new int [4];
		tempPosX [0] = int.Parse (arrStr [0]);
		tempPosY [0] = int.Parse (arrStr [1]);
		tempPosX [1] = int.Parse (arrStr [2]);
		tempPosY [1] = int.Parse (arrStr [3]);
		tempPosX [2] = int.Parse (arrStr [4]);
		tempPosY [2] = int.Parse (arrStr [5]);
		tempPosX [3] = int.Parse (arrStr [6]);
		tempPosY [3] = int.Parse (arrStr [7]);
		for (int i = 0; i < 4; i++) {
			if (tempPosX [i] + currentX < 0 || tempPosX [i] + currentX > 9 || (blocks [tempPosY [i] + currentY, tempPosX [i] + currentX] != 0 && !IsMe (tempPosX [i] + currentX, tempPosY [i] + currentY))) {
				return false;
			}
		}
		return true;
	}

	// Check whether the game is over or not
	private bool GameOver () {
		for (int i = 0; i < 4; i++) {
			if (blocks [y [i] + currentY, x [i] + currentX] != 0) {
				return true;
			}
		}
		return false;
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

	// Check and clear the rows that are complete
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

	// Clear the row being passed in as a parameter
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
				blocksObj [i, j].GetComponent<SpriteRenderer> ().sprite = sprites [blocks [i, j]];
//				blocksObj [i, j].GetComponent<BlockController> ().ChooseColor (blocks [i, j]);
			}
		}
	}

	// Reset the position for the next tetromino
	private void ResetCurrentPos () {
		currentX = 4;
		currentY = 0;
	}

	// Black out all the blocks for game over
	private void BlackOut () {
		for (int i = 0; i < blocksObj.GetLength (0); i++) {
			for (int j = 0; j < blocksObj.GetLength (1); j++) {
				blocksObj [i, j].SetActive (true);
				blocksObj [i, j].GetComponent<BlockController> ().ChooseColor (0);
			}
		}
	}

	// Reset all the blocks to 0
	private void ResetBlocks () {
		for (int i = 0; i < blocksObj.GetLength (0); i++) {
			for (int j = 0; j < blocksObj.GetLength (1); j++) {
				blocksObj [i, j].SetActive (false);
				blocks [i, j] = 0;
			}
		}
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