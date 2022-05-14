using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingIcon : MonoBehaviour
{
    [SerializeField] GameObject Icon;
    [SerializeField] GameObject IconCanvas;
    bool iconActive = true;
    bool iconDeActive = true;

    float firstImageScale = 0;
    float lastImageScale = 1f;
    void Start()
    {

    }

    private void Update()
    {
        //if(iconActive)
        if ((Camera.main.WorldToScreenPoint(IconCanvas.transform.position).x < Screen.width - 50 && Camera.main.WorldToScreenPoint(IconCanvas.transform.position).x > 50) && (Camera.main.WorldToScreenPoint(IconCanvas.transform.position).y < Screen.height - 50 && Camera.main.WorldToScreenPoint(IconCanvas.transform.position).y > 50))
        {
            if (iconActive)
            {
                Icon.SetActive(true);
                iconActive = false;
                iconDeActive = true;
                StartCoroutine(iconScaleUp(Icon.GetComponent<RectTransform>()));
            }
            Icon.transform.LookAt(Camera.main.transform);

        }
        else
        {
            if (iconDeActive)
            {
                //Icon.SetActive(false);
                StartCoroutine(iconScaleDown(Icon.GetComponent<RectTransform>()));
                iconActive = true;
                iconDeActive = false;
            }

        }
    }

    IEnumerator iconScaleUp(RectTransform image)
    {
        float counter = firstImageScale;
        while (counter < lastImageScale)
        {
            counter += 5 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);
        counter = 0f;
        float scale = 0;
        while (counter < Mathf.PI)
        {
            counter += 10 * Time.deltaTime;
            scale = Mathf.Sin(counter);
            scale *= 0.4f;
            image.localScale = new Vector3(lastImageScale + scale, lastImageScale + scale, lastImageScale + scale);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);

    }

    IEnumerator iconScaleDown(RectTransform image)
    {
        float counter = lastImageScale;
        while (counter > firstImageScale)
        {
            counter -= 5 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(firstImageScale, firstImageScale, firstImageScale);
        Icon.SetActive(false);
    }
}