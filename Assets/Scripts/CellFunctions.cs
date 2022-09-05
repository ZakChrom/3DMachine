using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFunctions : MonoBehaviour
{
	public bool running = false;
	void Update() {
		if (Input.GetKeyDown("r")) {
			running = !running;
		}
	}
}
