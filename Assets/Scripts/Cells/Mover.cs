using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mover : MonoBehaviour
{
	private float cooldown;
    void Awake() {
        cooldown = 0.2f;
    }
    void Update() {
        cooldown += -Time.deltaTime;
		if(cooldown <= 0) {
			cooldown = 0.2f;
			gameObject.GetComponent<Cell>().Move(transform.forward, gameObject.GetComponent<Cell>());
		}
	}
}
