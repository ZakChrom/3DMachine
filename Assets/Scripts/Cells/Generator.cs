using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Generator : MonoBehaviour
{
private float cooldown = 0.1f;
    void Update()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Cell"))
{
if(g.transform.position == transform.position-transform.forward & cooldown <= 0)
{
cooldown = 0.25f;
        foreach(GameObject g2 in GameObject.FindGameObjectsWithTag("Cell"))
{
if(g2.transform.position == transform.position+transform.forward)
{
g2.GetComponent<Cell>().Move(transform.forward, gameObject.GetComponent<Cell>());
}
}
Instantiate(g, transform.position+transform.forward, g.transform.rotation).GetComponent<Cell>().SetGenerated();
}
}
cooldown += -Time.deltaTime;
    }
}
