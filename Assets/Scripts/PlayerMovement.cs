using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	
	public float speed = 12f;
	public float gravity = -9.81f;
	public float groundDistance = 0.2f;
	public float jumpHeight = 3f;
	
	public LayerMask groundMask;
	
	public Transform groundCheck;
	
	Vector3 velocity;
	Vector3 startingPos = new Vector3(0,-3,0);
	bool isGrounded;
	bool inMenu;
    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		inMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>().inMenu;
		if(!inMenu) {
			isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
			
			if (isGrounded && velocity.y < 0) {
				velocity.y = -2f;
			}
			
			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");
			
			Vector3 move = transform.right * x + transform.forward * z;
			
			controller.Move(move * speed * Time.deltaTime);
			
			if(Input.GetButtonDown("Jump") && isGrounded){
				velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
			}
			
			velocity.y += gravity * Time.deltaTime;
			
			controller.Move(velocity * Time.deltaTime);
			
			if (this.transform.position.y <= -1000) {
				print("die");
				velocity.y = -2f;
				this.transform.position = startingPos;
			}
		}
    }
}
