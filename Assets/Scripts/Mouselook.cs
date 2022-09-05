using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouselook : MonoBehaviour
{
	public float mouseSensitivity = 100f;
	float xRotation = 0f;
	
	public Transform playerBody;
	
	bool inMenu;

    // Update is called once per frame
    void Update()
    {
		inMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>().inMenu;
		if(!inMenu) {
			float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
			
			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			
			transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			
			playerBody.Rotate(Vector3.up * mouseX);
		}
    }
}
