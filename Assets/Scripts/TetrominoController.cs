using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour {

	private int tetromino;
	private int currentRotation;
	private string posStr;
	private int x1, y1, x2, y2, x3, y3, x4, y4;

	// Initialize Tetromino
	private void InitializeTetromino () {

		// I Piece
		if (tetromino == 0) {
			Debug.Log ("Initializing I");
			RotateI ();
		}

		// J Piece
		else if (tetromino == 1) {
			Debug.Log ("Initializing J");
			RotateJ ();
		}

		// L Piece
		else if (tetromino == 2) {
			Debug.Log ("Initializing L");
			RotateL ();
		}

		// O Piece
		else if (tetromino == 3) {
			Debug.Log ("Initializing O");
			RotateO ();
		}

		// S Piece
		else if (tetromino == 4) {
			Debug.Log ("Initializing S");
			RotateS ();
		}

		// T Piece
		else if (tetromino == 5) {
			Debug.Log ("Initializing T");
			RotateT ();
		}

		// Z Piece
		else if (tetromino == 6) {
			Debug.Log ("Initializing Z");
			RotateZ ();
		}
		SetPosStr ();
	}

	public int GetCurrentRotation () {
		return currentRotation;
	}

	public int GetTetrominoID () {
		return tetromino;
	}

	public string RandomTetromino (int n) {
		tetromino = n;
		Debug.Log ("Tetromino: " + tetromino);
		currentRotation = 1;
		InitializeTetromino ();
		return posStr;
	}

	public string RotateTetrominoCounterClockwise () {
		
		// O Piece Rotation
		if (tetromino == 3) {
			return posStr;
		}

		// I, S, Z Pieces Rotation
		else if (tetromino == 0 || tetromino == 4 || tetromino == 6) {
			currentRotation--;
			if (currentRotation <= 0) {
				currentRotation = 2;
			}
			if (tetromino == 0) {
				RotateI ();
			}
			else if (tetromino == 4) {
				RotateS ();
			}
			else if (tetromino == 6) {
				RotateZ ();
			}
		}

		// J, L T Pieces Rotation
		else if (tetromino == 1 || tetromino == 2 || tetromino == 5) {
			currentRotation--;
			if (currentRotation <= 0) {
				currentRotation = 4;
			}
			if (tetromino == 1) {
				RotateJ ();
			}
			else if (tetromino == 2) {
				RotateL ();
			}
			else if (tetromino == 5) {
				RotateT ();
			}
		}
		SetPosStr ();
		Debug.Log (posStr);
		return posStr;
	}

	// Rotate the pieces
	public string RotateTetrominoClockwise () {

		// O Piece Rotation
		if (tetromino == 3) {
			return posStr;
		}

		// I, S, Z Pieces Rotation
		else if (tetromino == 0 || tetromino == 4 || tetromino == 6) {
			currentRotation++;
			if (currentRotation >= 3) {
				currentRotation = 1;
			}
			if (tetromino == 0) {
				RotateI ();
			}
			else if (tetromino == 4) {
				RotateS ();
			}
			else if (tetromino == 6) {
				RotateZ ();
			}
		}

		// J, L T Pieces Rotation
		else if (tetromino == 1 || tetromino == 2 || tetromino == 5) {
			currentRotation++;
			if (currentRotation >= 5) {
				currentRotation = 1;
			}
			if (tetromino == 1) {
				RotateJ ();
			}
			else if (tetromino == 2) {
				RotateL ();
			}
			else if (tetromino == 5) {
				RotateT ();
			}
		}
		SetPosStr ();
		Debug.Log (posStr);
		return posStr;
	}

	private void SetPosStr () {
		posStr = x1 + " " + y1 + " " + x2 + " " + y2 + " " + x3 + " " + y3 + " " + x4 + " " + y4;
	}

	/// <Rotation Algorithms> ------------------------------------------------------------------------------------------------------

	// Do I rotation
	private void RotateI () {
		if (currentRotation == 1) {
			x1 = 0;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 0;
			y3 = 2;
			x4 = 0;
			y4 = 3;
		}
		else if (currentRotation == 2) {
			x1 = -1;
			y1 = 1;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 2;
			y4 = 1;
		}
	}

	// Do J rotation
	private void RotateJ () {
		if (currentRotation == 1) {
			x1 = 1;
			y1 = 0;
			x2 = 1;
			y2 = 1;
			x3 = 0;
			y3 = 2;
			x4 = 1;
			y4 = 2;
		}
		else if (currentRotation == 2) {
			x1 = 0;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 2;
			y4 = 1;
		}
		else if (currentRotation == 3) {
			x1 = 0;
			y1 = 0;
			x2 = 1;
			y2 = 0;
			x3 = 0;
			y3 = 1;
			x4 = 0;
			y4 = 2;
		}
		else if (currentRotation == 4) {
			x1 = 0;
			y1 = 0;
			x2 = 1;
			y2 = 0;
			x3 = 2;
			y3 = 0;
			x4 = 2;
			y4 = 1;
		}
	}

	// Do L rotation
	private void RotateL () {
		if (currentRotation == 1) {
			x1 = 0;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 0;
			y3 = 2;
			x4 = 1;
			y4 = 2;
		}
		else if (currentRotation == 2) {
			x1 = 0;
			y1 = 0;
			x2 = 1;
			y2 = 0;
			x3 = 2;
			y3 = 0;
			x4 = 0;
			y4 = 1;
		}
		else if (currentRotation == 3) {
			x1 = 0;
			y1 = 0;
			x2 = 1;
			y2 = 0;
			x3 = 1;
			y3 = 1;
			x4 = 1;
			y4 = 2;
		}
		else if (currentRotation == 4) {
			x1 = 2;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 2;
			y4 = 1;
		}
	}

	// Do O rotation (for what?)
	private void RotateO () {
		x1 = 0;
		y1 = 0;
		x2 = 1;
		y2 = 0;
		x3 = 0;
		y3 = 1;
		x4 = 1;
		y4 = 1;
	}

	// Do S rotation
	private void RotateS () {
		if (currentRotation == 1) {
			x1 = 0;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 1;
			y4 = 2;
		}
		else if (currentRotation == 2) {
			x1 = 1;
			y1 = 0;
			x2 = 2;
			y2 = 0;
			x3 = 0;
			y3 = 1;
			x4 = 1;
			y4 = 1;
		}
	}

	// Do T rotation
	private void RotateT () {
		if (currentRotation == 1) {
			x1 = 1;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 1;
			y4 = 2;
		}
		else if (currentRotation == 2) {
			x1 = 1;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 2;
			y4 = 1;
		}
		else if (currentRotation == 3) {
			x1 = 0;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 0;
			y4 = 2;
		}
		else if (currentRotation == 4) {
			x1 = 0;
			y1 = 0;
			x2 = 1;
			y2 = 0;
			x3 = 2;
			y3 = 0;
			x4 = 1;
			y4 = 1;
		}
	}

	// Do Z rotation
	private void RotateZ () {
		if (currentRotation == 1) {
			x1 = 1;
			y1 = 0;
			x2 = 0;
			y2 = 1;
			x3 = 1;
			y3 = 1;
			x4 = 0;
			y4 = 2;
		}
		else if (currentRotation == 2) {
			x1 = 0;
			y1 = 0;
			x2 = 1;
			y2 = 0;
			x3 = 1;
			y3 = 1;
			x4 = 2;
			y4 = 1;
		}
	}

	/// <Rotation Algorithms> ------------------------------------------------------------------------------------------------------
}
