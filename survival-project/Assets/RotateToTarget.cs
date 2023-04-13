using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Vector 2 direction = target.position - enemy.position
    //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg
    //Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward)
    //enemy.transform.rotation = rotation
}
