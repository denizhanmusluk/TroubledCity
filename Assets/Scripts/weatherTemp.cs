﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weatherTemp : MonoBehaviour
{
    bool weatherActive = false;
    public int troubleLevel = 0;
    [SerializeField] public GameObject directionLight;
    [SerializeField] float firstLight = 1.1f, rainLight = 0.6f;
    bool rainCheckActive = true;
    [SerializeField] ParticleSystem rainParticle;
    [SerializeField] ParticleSystem stormParticle;
    [SerializeField] ParticleSystem springParticle;
    ParticleSystem.EmissionModule rainEmission;
    float rainMaxEmission = 300f;
    float firstFogStart = 43f;
    float firstFogEnd = 60;

    float lastFogStart = 25f;
    float lastFogEnd = 50f;
    Color32 fogFirstColor = new Color32(0, 198, 255, 255);
    void Start()
    {
        RenderSettings.fogStartDistance = firstFogStart;
        RenderSettings.fogEndDistance = firstFogEnd;
        RenderSettings.fogColor = new Color32(0, 198, 255, 255);

        rainEmission = rainParticle.emission;
        directionLight = GameObject.Find("DirectionalLight");
        directionLight.GetComponent<Light>().intensity = firstLight;

    }

    IEnumerator springParticleInst()
    {
        var particle1 = Instantiate(springParticle, transform.position, Quaternion.Euler(90, 0, 0), transform.parent.parent);
        particle1.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        var particle2 = Instantiate(springParticle, transform.position, Quaternion.Euler(90, 0, 0), transform.parent.parent);
        particle2.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        var particle3 = Instantiate(springParticle, transform.position, Quaternion.Euler(90, 0, 0), transform.parent.parent);
        particle3.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        var particle4 = Instantiate(springParticle, transform.position, Quaternion.Euler(90, 0, 0), transform.parent.parent);
        particle4.gameObject.SetActive(true);
    }


    IEnumerator rainCheck()
    {
        while (rainCheckActive)
        {
            if (weatherActive)
            {
                rainCheckActive = false;
                if (troubleLevel == 2)
                {
                    rainMaxEmission = 1000f;
                    stormParticle.gameObject.SetActive(true);
                }

                StartCoroutine(rainlySun());
                StartCoroutine(fogDark());
                StartCoroutine(fogColorDark());
                rainParticle.gameObject.SetActive(true);
                rainEmission.rateOverTime = 0f;
            }
            yield return null;
        }
    }

    IEnumerator rainlySun()
    {
        ///// sunset
        yield return new WaitForSeconds(1f);
        float cntr = firstLight;
        while (cntr > rainLight)
        {
            cntr -= 0.2f * Time.deltaTime;
            directionLight.GetComponent<Light>().intensity = cntr;


            yield return null;
        }
        directionLight.GetComponent<Light>().intensity = rainLight;
        StartCoroutine(rainStart());

        while (troubleLevel > 0)
        {
            int flashCount = Random.Range(1, 4);
            for (int i = 0; i < flashCount; i++)
            {
                float counter = 0;
                float lightDelta;
                directionLight.GetComponent<Light>().intensity = rainLight;
                ////ligthning
                while (counter < Mathf.PI)
                {
                    counter += 10 * Time.deltaTime;
                    lightDelta = 1 - Mathf.Abs(Mathf.Cos(counter));
                    //lightDelta *= firstLight - rainLight;
                    directionLight.GetComponent<Light>().intensity = rainLight + lightDelta;
                    if (troubleLevel == 0)
                    {
                        StartCoroutine(fogBright());
                        StartCoroutine(fogColorBright());
                        break;
                    }
                    yield return null;
                }
                yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
            }
            float waitCounter = 0f;
            float waitTime = Random.Range(3f, 6f);
            while (waitTime > waitCounter)
            {
                waitCounter += Time.deltaTime;
                if (troubleLevel == 0)
                {
                    StartCoroutine(fogBright());
                    StartCoroutine(fogColorBright());

                    break;
                }
                yield return null;
            }
        }
        StartCoroutine(rainStop());
        while (cntr < firstLight)
        {
            cntr += Time.deltaTime;
            directionLight.GetComponent<Light>().intensity = cntr;


            yield return null;
        }
        directionLight.GetComponent<Light>().intensity = firstLight;

    }
    IEnumerator rainStart()
    {
        float counter = 0f;
        while (counter < rainMaxEmission)
        {
            counter += 50 * Time.deltaTime;
            rainEmission.rateOverTime = counter;

            yield return null;
        }
        rainEmission.rateOverTime = rainMaxEmission;
    }
    IEnumerator rainStop()
    {
        float counter = rainMaxEmission;
        while (counter > 0)
        {
            counter -= 300 * Time.deltaTime;
            rainEmission.rateOverTime = counter;

            yield return null;
        }
        rainEmission.rateOverTime = 0;
        rainParticle.gameObject.SetActive(false);
        stormParticle.gameObject.SetActive(false);


    }



    IEnumerator fogDark()
    {
        float counter = firstFogStart;
        while (counter > lastFogStart)
        {
            counter -= 3 * Time.deltaTime;
            RenderSettings.fogStartDistance = counter;
            RenderSettings.fogEndDistance = counter + 35;

            yield return null;
        }
        RenderSettings.fogStartDistance = lastFogStart;
        RenderSettings.fogEndDistance = lastFogEnd;

    }
    IEnumerator fogBright()
    {
        float counter = lastFogStart;
        while (counter < firstFogStart)
        {
            counter += 10 * Time.deltaTime;
            RenderSettings.fogStartDistance = counter;
            RenderSettings.fogEndDistance = counter + 35;


            yield return null;
        }
        RenderSettings.fogStartDistance = firstFogStart;
        RenderSettings.fogEndDistance = firstFogEnd;

    }
    IEnumerator fogColorDark()
    {
        byte counter = 198;
        byte counter2 = 255;
        while (counter > 62)
        {
            counter -= 3;
            counter2 -= 3;

            RenderSettings.fogColor = new Color32(0, counter, counter2, 255);
            yield return null;
        }
        RenderSettings.fogColor = new Color32(0, 62, 77, 255);


    }

    IEnumerator fogColorBright()
    {
        byte counter = 62;
        byte counter2 = 77;
        while (counter < 198)
        {
            counter += 10;
            counter2 += 10;

            RenderSettings.fogColor = new Color32(0, counter, counter2, 255);
            yield return null;
        }
        RenderSettings.fogColor = new Color32(0, 198, 255, 255);


    }
}
