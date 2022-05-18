using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIdirection : MonoBehaviour
{
    Vector3 direction;
    Vector3 distance;
    bool followActive = false;
    Transform player;
   public Transform Target;
    bool troubleActive = false;
    bool iconActive = true;
    bool iconDeActive = true;
    float firstImageScale = 0;
    float lastImageScale = 1f;
    private void Start()
    {

    }
    //public void selectTarget(int _selectionTarget, Transform _player)
    //{
    //    GetComponent<Image>().enabled = true;
    //    player = _player;
    //    selectionTarget = _selectionTarget;
    //    followActive = true;
    //}

    private void Update()
    {
        if (Target != null)
        {
            arrowUIPos2();
            targetPositionCheck();
            iconPosCheck();
        }
    }
    void targetPositionCheck()
    {

    }
    void iconPosCheck()
    {
        if ((Camera.main.WorldToScreenPoint(Target.position).x < Screen.width - 50 && Camera.main.WorldToScreenPoint(Target.position).x > 50) && (Camera.main.WorldToScreenPoint(Target.position).y < Screen.height - 50 && Camera.main.WorldToScreenPoint(Target.position).y > 50))
        {
            if (iconDeActive)
            {
                //Icon.SetActive(false);
                StartCoroutine(iconScaleUp(Target.GetComponent<troubleArea>().troubleIcon.GetComponent<RectTransform>()));

                StartCoroutine(iconScaleDown(GetComponent<RectTransform>()));
                iconActive = true;
                iconDeActive = false;
            }
        }
        else
        {
            if (iconActive)
            {
                //Icon.SetActive(true);
                iconActive = false;
                iconDeActive = true;
                StartCoroutine(iconScaleDown(Target.GetComponent<troubleArea>().troubleIcon.GetComponent<RectTransform>()));

                StartCoroutine(iconScaleUp(GetComponent<RectTransform>()));
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
    }
    #region arrow ui position
    public void arrowUIPos2()
    {
        direction = (Target.position - Camera.main.transform.GetChild(0).transform.position).normalized;

        float dirX = Camera.main.WorldToScreenPoint(Target.position).x;
        dirX -= Screen.width / 2;
        dirX = Mathf.Clamp(dirX, -Screen.width / 2 + 60, Screen.width / 2 - 60);

        float dirZ = Camera.main.WorldToScreenPoint(Target.position).y;
        dirZ -= Screen.height / 2;
        dirZ = Mathf.Clamp(dirZ, -Screen.height / 2 + 60, Screen.height / 2 - 60);

        GetComponent<RectTransform>().anchoredPosition = new Vector3(dirX, dirZ, 0);
        float angle;
        if (direction == Vector3.zero)
        {
            angle = 0;
        }
        else
        {
            angle = Mathf.Atan(direction.x / direction.z);
        }
        angle = angle * 180 / 3.14f;
        if (direction.z < 0)
        {
            angle += 180;
        }
        transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180-angle);
    }
    public void arrowUIPos()
    {
        direction = (Target.position - Camera.main.transform.GetChild(0).transform.position).normalized;
        //distance = Target.position - Camera.main.transform.GetChild(0).transform.position;

        if(direction.x > 0)
        {
            if(Mathf.Abs(direction.z)< Mathf.Abs(direction.x))
            {
                direction.x = 1;
            }
        }
        else
        {
            if (Mathf.Abs(direction.z) < Mathf.Abs(direction.x))
            {
                direction.x = -1;
            }
        }


        if (direction.z > 0)
        {
            if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
            {
                direction.z = 1;
            }
        }
        else
        {
            if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
            {
                direction.z = -1;
            }
        }
        float dirX = direction.x * Screen.width / 2;
        dirX = Mathf.Clamp(dirX, -Screen.width / 2 + 50, Screen.width / 2 - 50);   
        
        float dirZ = direction.z * Screen.height / 2;
        dirZ = Mathf.Clamp(dirZ, -Screen.height / 2 + 50, Screen.height / 2 - 50);
        //distX = Mathf.Abs(distX);
        //distZ = Mathf.Abs(distZ);
        int magnX;
        int magnZ;
        //if (distance.x > 0)
        //{
        //    magnX = -50;
        //}
        //else
        //{
        //    magnX = 50;
        //}

        //if (distance.z > 0)
        //{
        //    magnZ = -100;
        //}
        //else
        //{
        //    magnZ = 100;
        //}
       GetComponent<RectTransform>().anchoredPosition = new Vector3(dirX, dirZ, 0);

        float angle;
        if (direction == Vector3.zero)
        {
            angle = 0;
        }
        else
        {
            angle = Mathf.Atan(direction.x / direction.z);
        }
        angle = angle * 180 / 3.14f;
        if (direction.z < 0)
        {
            angle += 180;
        }
       GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0, -angle);
    }
    #endregion
    //public void isTrouble()
    //{
    //    troubleActive = true;
    //    this.StartCoroutine(loopColorScaleSet());
    //    this.StartCoroutine(scaleSet());
    //}
    //public void torubleFix()
    //{
    //    troubleActive = false;
    //}
    IEnumerator loopColorScaleSet()
    {
        float counter1 = 0f;
        float scaleValue1 = 0f;
        while (troubleActive)
        {
            counter1 += 5 * Time.deltaTime;
            scaleValue1 = Mathf.Abs(Mathf.Cos(counter1));
            GetComponent<Image>().color = new Color(1, 1 - scaleValue1, 1 - scaleValue1);
            yield return null;
        }
        GetComponent<Image>().color = new Color(1, 1, 1);
    }
    IEnumerator scaleSet()
    {
        float counter1 = 0f;
        float scaleValue1 = 0f;
        while (troubleActive)
        {
            counter1 += 20 * Time.deltaTime;
            scaleValue1 = Mathf.Abs(Mathf.Cos(counter1));
            transform.localScale = Vector3.one + new Vector3(scaleValue1 / 5f, scaleValue1 / 5f, scaleValue1 / 5f);

            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}
