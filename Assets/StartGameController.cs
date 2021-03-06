﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class StartGameController : MonoBehaviour {
	
	private Image myImage;
	public GameObject mainBoard;
	public GameObject canvas;
	public Text input;
	public string enemy;
	private bool started = false;
	public Sprite waiting;
	private DatabaseReference reference;

	// Use this for initialization
	void Start () {

		myImage = gameObject.GetComponent<Image> ();
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://tetris-a8118.firebaseio.com/");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;
		reference.ValueChanged += (object sender, ValueChangedEventArgs args) => {
			if (args.DatabaseError != null) {
				Debug.LogError(args.DatabaseError.Message);
				return;
			}
			if(args.Snapshot.HasChild(enemy)){
				if(!started && args.Snapshot.Child(enemy).Child("ready").Value.ToString() == "1" && args.Snapshot.Child(input.text.ToString()).Child("ready").Value.ToString()== "1"){
					mainBoard.gameObject.GetComponent<GameController> ().SetEnemy(enemy);
					canvas.SetActive(false);
					started = true;
				}
			}
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Ready(){
		myImage.sprite (waiting);
		mainBoard.gameObject.GetComponent<GameController> ().SetName(input.text.ToString());
	}
}
