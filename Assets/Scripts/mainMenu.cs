using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void Play() {
		SceneManager.LoadScene(2);
	}
	public void Back() {
		SceneManager.LoadScene(0);
	}
	public void Credits() {
		SceneManager.LoadScene(1);
	}
}
