using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Fall : MonoBehaviour
{
private float cooldown;
private Quaternion orgrot;
    void Awake()
    {
        cooldown = 0.2f;
	orgrot = transform.rotation;
    }
    void Update()
    {
	gameObject.GetComponent<Cell>().SetWeak();
	gameObject.GetComponent<Cell>().SetRotation(orgrot);
        cooldown += -Time.deltaTime;
if(cooldown <= 0)
{
        cooldown = 0.2f;
gameObject.GetComponent<Cell>().Move(transform.forward, gameObject.GetComponent<Cell>());
}
    }
}
