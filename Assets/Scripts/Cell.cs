using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cell : MonoBehaviour
{
	private bool immobile, enemy, generated, slider, trash, weak;
	private Vector3 orgpos, pos;
    void Start()
    {
        immobile = false;
		enemy = false;
		slider = false;
		orgpos = transform.position;
		pos = orgpos;
		weak = false;
    }
	
    public void Reset() {
		transform.position = orgpos;
		gameObject.SetActive(true);
    }
	
	void LateUpdate() {
		pos += Vector3.Normalize(transform.position-transform.GetChild(0).position)*Time.deltaTime*5f;
		if((pos-transform.position).magnitude <= 0.1f)
		{
		pos = transform.position;
		}
		transform.GetChild(0).position = pos;
	}
	
    public void Move(Vector3 dir, Cell pusher) {
		if (GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>().inMenu || !GameObject.FindGameObjectWithTag("CellFunctions").GetComponent<CellFunctions>().running) {
			return;
		}
		if(enemy) {
			if(!trash){
				Delete();
			}
			pusher.Delete();
		} else {
			bool canmove = true;
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("cell")) {
				if(g.transform.position == transform.position+dir) {
					if(!g.GetComponent<Cell>().isPushable(dir) || (weak & pusher == this)) {
					canmove = false;
					} else {
						g.GetComponent<Cell>().Move(dir, this);
					}
				}
			}
			if(canmove) {
				transform.position += dir;
				foreach(GameObject g in GameObject.FindGameObjectsWithTag("cell")) {
					if(g.transform.position == transform.position & g != gameObject) {
						Move(-dir, pusher);
					}
				}
			}
		}
    }
	
    public void Delete() {
		gameObject.SetActive(false);
		if(generated) {
			Destroy(gameObject);
		}
    }
	
    public void SetImmobile() {
		immobile = true;
    }
	
    public void SetEnemy() {
		enemy = true;
    }
	
    public void SetGenerated() {
		generated = true;
    }
	
    public void SetRotation(Quaternion dir) {
		transform.rotation = dir;
    }
	public void Rotate(Quaternion dir) {
		transform.rotation *= dir;
    }
	
    public bool isPushable(Vector3 dir) {
		bool pushable = true;
		if(immobile) {
			pushable = false;
		}
		if(slider) {
			if(dir == transform.forward) {
				pushable = true;	
			} else {
				if(dir == -transform.forward) {
					pushable = true;	
				} else {
					pushable = false;
				}
			}
		}
		return pushable;
    }
	
    public void SetSlider() {
		slider = true;
    } 
    
	public void SetTrash() {
		enemy = true;
		trash = true;
    } 
    
	public void SetWeak() {
		weak = true;
    }
}
