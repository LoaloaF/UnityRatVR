using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarCollision : MonoBehaviour
{
    public bool PlayerDetected;
    public Vector3 detectionBoxSize;
    private bool playerDetected;
    // Start is called before the first frame update
    void Start()
    {
        PlayerDetected = false;
        Transform ColliderTransform = transform.Find("Collider");
        detectionBoxSize = new Vector3(ColliderTransform.localScale.x / 1.5f, transform.localPosition.y*2, ColliderTransform.localScale.z / 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        playerDetected = false;
        Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionBoxSize / 2);

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                PlayerDetected = true;
                playerDetected = true;
                break;
            }
        }
        if (!playerDetected)
        {
            PlayerDetected = false;
        }
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

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("player is detected in pillar collision script");
    //         // Debug.Log(transform.position);
    //         PlayerDetected = true;
    //     }

    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("player exits collider");
    //         // Debug.Log(transform.position);
    //         PlayerDetected = false;
    //     }
    // }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("player is detected in pillar collision script");
    //         // Debug.Log(transform.position);
    //         PlayerDetected = true;
    //     }

    // }

}
