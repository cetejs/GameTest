using System;
using System.Collections;
using UnityEngine;

namespace Escape
{
    public class ScreenFadeInOut : MonoBehaviour
    {
        [SerializeField]
        private Material blackScreenMat;
        [SerializeField]
        private float fadeSpeed = 3f;
        [SerializeField]
        private float waitTime = 0.5f;

        private WaitForSeconds waitForSeconds;
        private readonly int RadiusProperty = Shader.PropertyToID("_Radius");

        private void Start()
        {
            if (!blackScreenMat)
            {
                blackScreenMat = new Material(Shader.Find("Escape/BlackScreen"));
            }

            waitForSeconds = new WaitForSeconds(waitTime);
        }

        private void OnDisable()
        {
            blackScreenMat.SetFloat(RadiusProperty, 1.5f);
        }

        public void StartFadeScreen(Action fadeInCallback = null, Action fadeOutCallback = null)
        {
            StopAllCoroutines();
            StartCoroutine(FadeScreen(fadeInCallback, fadeOutCallback));
        }

        private IEnumerator FadeScreen(Action fadeIn, Action fadeOut)
        {
            yield return StartCoroutine(FadeInScreen());
            fadeIn?.Invoke();
            yield return waitForSeconds;
            yield return StartCoroutine(FadeOutScreen());
            fadeOut?.Invoke();
        }

        private IEnumerator FadeInScreen()
        {
            float radius = blackScreenMat.GetFloat(RadiusProperty);
            while (radius >= 0)
            {
                radius -= fadeSpeed * Time.deltaTime;
                blackScreenMat.SetFloat(RadiusProperty, radius);
                yield return null;
            }
        }

        private IEnumerator FadeOutScreen()
        {
            float radius = blackScreenMat.GetFloat(RadiusProperty);
            while (radius <= 1.5f)
            {
                radius += fadeSpeed * Time.deltaTime;
                blackScreenMat.SetFloat(RadiusProperty, radius);
                yield return null;
            }
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, blackScreenMat);
        }
    }
}