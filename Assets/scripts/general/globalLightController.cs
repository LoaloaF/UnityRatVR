using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalLightController : MonoBehaviour
{
    void Start()
    {
        switchSceneColor(new Color(1, 1, 1, 1));
    }

    public void switchSceneColor(Color sceneColor)
    {
        foreach (Transform child in transform)
        {
            Light light = child.GetComponent<Light>();
            if (light != null)
            {
                light.color = sceneColor; // replace r, g, b, a with the color values you want to use
            }
        }
    }
}
