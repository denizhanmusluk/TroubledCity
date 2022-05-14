using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIdirection : MonoBehaviour
{
    public List<GameObject> targetList;
    Vector3 direction;
    Vector3 distance;
    bool followActive = false;
    Transform player;
    int selectionTarget;
    bool troubleActive = false;
    private void Start()
    {

    }
    public void selectTarget(int _selectionTarget, Transform _player)
    {
        GetComponent<Image>().enabled = true;
        player = _player;
        selectionTarget = _selectionTarget;
        followActive = true;
    }

    private void Update()
    {
        if (targetList.Count>0)
        {
            arrowUIPos();
        }
    }
    #region arrow ui position
    public void arrowUIPos()
    {
        direction = (targetList[selectionTarget].transform.position - Camera.main.transform.GetChild(0).transform.position).normalized;
        distance = targetList[selectionTarget].transform.position - Camera.main.transform.GetChild(0).transform.position;
        float distZ = Mathf.Clamp(distance.z, -150, 150);
        float distX = Mathf.Clamp(distance.x, -70, 70);
        //distX = Mathf.Abs(distX);
        //distZ = Mathf.Abs(distZ);
        int magnX;
        int magnZ;
        if (distance.x > 0)
        {
            magnX = -50;
        }
        else
        {
            magnX = 50;
        }

        if (distance.z > 0)
        {
            magnZ = -100;
        }
        else
        {
            magnZ = 100;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector3(direction.x  * Mathf.Abs(distX) * Screen.width / 140 + magnX, direction.z* Mathf.Abs(distZ) * Screen.height / 300 + magnZ , 0);

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
