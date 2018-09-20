using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {

    public float timeToDestroy = 1.51f;

	void Start () 
    {
        Destroy(gameObject, timeToDestroy);
	}
	

}
