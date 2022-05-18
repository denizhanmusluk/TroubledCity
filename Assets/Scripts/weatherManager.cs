using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weatherManager : MonoBehaviour
{
    [SerializeField] public GameObject directionLight;
    [SerializeField] float firstLight = 1.1f, rainLight = 0.6f;
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

    bool weatherActive = false;
    bool dayCycleActive = true;

    bool rainActive = false;
   [SerializeField] bool sunActive = true;
    private void Start()
    {
        //RenderSettings.fogStartDistance = firstFogStart;
        //RenderSettings.fogEndDistance = firstFogEnd;
        //RenderSettings.fogColor = new Color32(0, 198, 255, 255);

        rainEmission = rainParticle.emission;
        directionLight = GameObject.Find("DirectionalLight");
        directionLight.GetComponent<Light>().intensity = firstLight;
        StartCoroutine(dayCycling());
        StartCoroutine(weatherConditionCheck());
    }
    IEnumerator dayCycling()
    {
        while (dayCycleActive)
        {
            yield return new WaitForSeconds(10f);
            if (rainActive)
            {
                break;
            }
            sunActive = sunActive ? false : true;
            if (sunActive)
            {
                StartCoroutine(sunSet());
                rainActive = true;
                StartCoroutine(lightning());
            }
            else
            {
                StartCoroutine(sunRise());
            }
        }
    }

    IEnumerator weatherConditionCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            
            rainActive = false;


            weatherSelection();
        }
    }

    IEnumerator sunSet()
    {
        float cntr = firstLight;
        while (cntr > rainLight)
        {
            cntr -= 0.2f * Time.deltaTime;
            directionLight.GetComponent<Light>().intensity = cntr;
            yield return null;
        }
        directionLight.GetComponent<Light>().intensity = rainLight;
    }

    IEnumerator sunRise()
    {
        float cntr = rainLight;
        while (cntr < firstLight)
        {
            cntr += Time.deltaTime;
            directionLight.GetComponent<Light>().intensity = cntr;


            yield return null;
        }
        directionLight.GetComponent<Light>().intensity = firstLight;
    }

    void weatherSelection()
    {
        int selection = Random.Range(0, 3);
        if(selection == 0)
        {
            rainParticle.gameObject.SetActive(true);
            rainActive = true;
            StartCoroutine(rainStart());
            StartCoroutine(lightning());
            StartCoroutine(sunSet());
        }
        else
        {
            StartCoroutine(rainStop());
            StartCoroutine(sunRise());
            StartCoroutine(dayCycling());

        }

        if (selection == 1)
        {

        }
        if(selection == 2)
        {

        }
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
            counter -= 50 * Time.deltaTime;
            rainEmission.rateOverTime = counter;

            yield return null;
        }
        rainEmission.rateOverTime = 0;
        rainParticle.gameObject.SetActive(false);
        stormParticle.gameObject.SetActive(false);
    }
    
    IEnumerator lightning()
    {
        yield return new WaitForSeconds(2f);
        while (rainActive)
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
                    counter += 20 * Time.deltaTime;
                    lightDelta = 1 - Mathf.Abs(Mathf.Cos(counter));
                    //lightDelta *= firstLight - rainLight;
                    directionLight.GetComponent<Light>().intensity = rainLight + lightDelta;
                    if (!rainActive)
                    {
                 
                        break;
                    }
                    yield return null;
                }
                yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
            }
            float waitCounter = 0f;
            float waitTime = Random.Range(3f, 6f);
            while (waitTime > waitCounter)
            {
                waitCounter += Time.deltaTime;
                if (!rainActive)
                {

                    break;
                }
                yield return null;
            }
        }
    }

}
