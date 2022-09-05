using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Trash : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<Cell>().SetTrash();
    }
}
