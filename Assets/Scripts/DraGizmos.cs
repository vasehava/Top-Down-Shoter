using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraGizmos : MonoBehaviour
{

    public Color c;
    void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(base.transform.position, Quaternion.identity, new Vector3(1f, 0.1f, 1f));
        Gizmos.color = c;
        Gizmos.DrawWireSphere(Vector3.zero, 3f);
        Gizmos.color = new Color(c.r, c.g, c.b, 0.4f);
        Gizmos.DrawSphere(Vector3.zero, 3f);
        Gizmos.matrix = Matrix4x4.identity;
    }
}