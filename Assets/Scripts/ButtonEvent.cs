using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {
	private Image myImage;
	public Sprite normal, hover, clicked;

	// Use this for initialization
	void Start () {
		myImage = gameObject.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ButtonHover() {
		myImage.sprite = hover;
	}

	public void ButtonExit() {
		myImage.sprite = normal;
	}

	public void ButtonDown() {
		myImage.sprite = clicked;
	}

	public void ButtonUp() {
		myImage.sprite = hover;
	}
}
