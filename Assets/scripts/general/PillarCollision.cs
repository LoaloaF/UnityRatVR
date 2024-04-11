using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarCollision : MonoBehaviour
{
    public bool PlayerDetected;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player detected in pillar collision script");
            //return true;
        }
        else
        {
            //return false;
        }
        
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player is detected in pillar collision script");
            // Debug.Log(transform.position);
            PlayerDetected = true;
        }
        else
        {
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player exits collider");
            // Debug.Log(transform.position);
            PlayerDetected = false;
        }
    }

}
