using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class EnemyController : MonoBehaviour {
	
	private int[,] blocks;
	private GameObject[,] blocksObj;
	public GameObject blockPrefab;
	public Sprite[] sprites;
	private float startX;
	private float startY;
	private DatabaseReference reference;
	public string networkBlocks;
	private string myName = "";

	public GameObject mainBoard;


	// Use this for initialization
	void Awake () {
		blocks = new int [20, 10];
		networkBlocks = "";
		startX = -1.88f;
		startY = 4.005f;
		InitializeBlocksObj ();
		UpdateBlocksState ();

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://tetris-a8118.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	// Update is called once per frame
	void Update () {
		if (myName != "") {
			GetSyncStr ();
		}
	}

	public void SetName(string name){
		myName = name;
	}

	private void GetSyncStr () {
		Debug.Log (myName);

		reference.Child(myName).Child("block").GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted) {
				// Handle the error...
			}
			else if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				networkBlocks = snapshot.Value.ToString();
			}
		});	

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
		UpdateBlocksState ();
	}

	// Print out the array
	private void PrintArray () {
		for (int i = 0; i < 20; i++) {
			Debug.Log (blocks [i, 0] + " " + blocks [i, 1] + " " + blocks [i, 2] + " " + blocks [i, 3] + " " + blocks [i, 4] + " " + blocks [i, 5] + " " + blocks [i, 6] + " " + blocks [i, 7] + " " + blocks [i, 8] + " " + blocks [i, 9]);
		}
	}

//	// Check whether the game is over or not
//	private bool GameOver () {
//		for (int i = 0; i < 4; i++) {
//			if (blocks [y [i] + currentY, x [i] + currentX] != 0) {
//				return true;
//			}
//		}
//		return false;
//	}

//	// Create the next tetromino after moving
//	private void UpdateBlocks () {
//		Debug.Log ("UpdateBlocks ()");
//		for (int i = 0; i < 4; i++) {
//			blocks [y [i] + currentY, x [i] + currentX] = tetromino.GetTetrominoID () + 1;
//		}
//
//		SetSyncStr ();
//		reference.Child (myName).Child("block").SetValueAsync (networkBlocks);
//		UpdateBlocksState ();
//	}


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
