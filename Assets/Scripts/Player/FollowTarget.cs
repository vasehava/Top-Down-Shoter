using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    public float lerpSpeed = 5;
    void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	void Update () 
    {
        if(target != null)
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * lerpSpeed);
	}
}
