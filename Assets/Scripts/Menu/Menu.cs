using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
	public int MenuType = 0;
	
	public GameObject menu1;
	public GameObject menu2;
	public GameObject menu3;
	public GameObject menu4;
	
	List<GameObject> Objects = new List<GameObject>();
	
	public Slider slider;
	public GameObject cam;
	
	void Start() {
		menu1.SetActive(false);
		menu2.SetActive(false);
		menu3.SetActive(false);
		menu4.SetActive(false);
	}
	
    void Update()
    {
        if(Input.GetKeyDown("p") || Input.GetKeyDown("escape")) {
			if (MenuType == 0) {MenuType = 1;}
			else {MenuType = 0;}
		}
		if(MenuType != 0) {
			switch (MenuType) {
				case 1:
					menu1.SetActive(true);
					menu2.SetActive(false);
					menu3.SetActive(false);
					menu4.SetActive(false);
					break;
				case 2:
					menu1.SetActive(false);
					menu2.SetActive(true);
					menu3.SetActive(false);
					menu4.SetActive(false);
					break;
				case 3:
					menu1.SetActive(false);
					menu2.SetActive(false);
					menu3.SetActive(true);
					menu4.SetActive(false);
					break;
				case 4:
					menu1.SetActive(false);
					menu2.SetActive(false);
					menu3.SetActive(false);
					menu4.SetActive(true);
					break;
			}
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else {
			menu1.SetActive(false);
			menu2.SetActive(false);
			menu3.SetActive(false);
			menu4.SetActive(false);
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
    }
	public void Quit() {
		Application.Quit();
	}
	public void Sensitivity() {
		cam.GetComponent<Mouselook>().mouseSensitivity = slider.value;
	}
	public void Reload() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void Back() {
		SceneManager.LoadScene(0);
	}
	public void noMenu() {
		MenuType = 0;
	}
}
