using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterReward : MonoBehaviour
{
    public Collider playerCollider;
    public Collider waterCollider;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(360 * 0 * Time.deltaTime, 360 * 1 * Time.deltaTime, 360 * 0 * Time.deltaTime);

        
    }
}
