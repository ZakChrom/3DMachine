using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Slide : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<Cell>().SetSlider();
    }
}
