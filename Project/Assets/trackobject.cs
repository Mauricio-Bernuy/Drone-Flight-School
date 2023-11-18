using UnityEngine;
using System.Collections;
public class trackobject : MonoBehaviour {

	void Update() {     
        this.transform.position =  Camera.main.transform.position + Camera.main.transform.forward * 0.3f;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y -  0.1f, this.transform.position.z);
	}
}