using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CWRotator : MonoBehaviour
{
    void Update()
    {
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("cell")) {
			if((g.transform.position-transform.position).magnitude <= 1.15f) {
				g.GetComponent<Cell>().Rotate(transform.rotation);
			}
		}
	}
}
