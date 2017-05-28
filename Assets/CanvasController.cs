using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	
	public Text myScore, enemyScore, result;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void hide(){
		gameObject.SetActive (false);
	}

	public void show(string myscore, string enemyscore){
		myScore.text = myscore;
		enemyScore.text = enemyscore;

		if (int.Parse(myscore) > int.Parse(enemyscore)) {
			result.text = "You win!";
		} else if (int.Parse (myscore) < int.Parse(enemyscore)) {
			result.text = "You Lose!";
		} else {
			result.text = "equal";
		}

		gameObject.SetActive (true);
	}
}
