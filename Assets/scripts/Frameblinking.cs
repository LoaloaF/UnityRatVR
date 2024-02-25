using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frameblinking : MonoBehaviour
{
    public MeshRenderer blinker;
    public int framecounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (framecounter %2 == 1)
        {
            blinker.material.color = Color.white;
        } else
        {
            blinker.material.color = Color.black;
        }
        framecounter++;
    }
}
