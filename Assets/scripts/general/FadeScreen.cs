using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public PlayerMovement _playerMovement;
    public bool isFadingIn = false;
    public bool isFadingOut = false;

    public void StartFadeInScreen(float fadeDuration)
    {
        if (isFadingIn)
        {
            return;
        }
        StartCoroutine(FadeInScreenCoroutine(fadeDuration));
    }

    public void StartFadeOutScreen(float fadeDuration)
    {
        if (isFadingOut)
        {
            return;
        }
        StartCoroutine(FadeOutScreenCoroutine(fadeDuration));
    }

    public IEnumerator FadeInScreenCoroutine(float fadeDuration)
    {
        isFadingIn = true;
        _playerMovement.DisableMovement();
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, elapsedTime/fadeDuration);
            yield return null;
        }

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator FadeOutScreenCoroutine(float fadeDuration)
    {
        isFadingOut = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, (1 - elapsedTime/fadeDuration));
            yield return null;
        }

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
        _playerMovement.EnableMovement();
    }
}
