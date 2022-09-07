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
	public void Reset() {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("cell")) {
			g.GetComponent<Cell>().Reset();
		}
	}
	public void Save() {
		GUIUtility.systemCopyBuffer = Worlds.Save();
	}
	public void Load() {
		Worlds.Load();
	}
}
