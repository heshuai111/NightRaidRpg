using UnityEngine;
using System.Collections;

public class DestroyForTime1 : MonoBehaviour {

	public float time;

	void Start () {
		Destroy(this.gameObject,time);
	}

}
