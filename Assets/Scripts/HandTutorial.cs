using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 firstPos = new Vector3(6 * Screen.width / 8, 5 * Screen.height / 10, 0);
    Vector3 lastPos = new Vector3(3 * Screen.width / 8, 8 * Screen.height / 10, 0);
    bool swipeActive = true;
   public void StartSwipe()
    {
        //transform.DOMoveX(Screen.width / 3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        //transform.DOScale(Vector3.one * 1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        GetComponent<RectTransform>().position = firstPos;
        StartCoroutine(Ymove());
    }

  IEnumerator Ymove()
    {
        float cntr = 0f;
        float counter1 = 0f;
        float counter2 = 0f;
        float Ymove = 0f;
        float Xmove = 0f;
        while (swipeActive)
        {
            counter1 += 0.6f * Time.deltaTime;
            counter2 += 3f * Time.deltaTime;
            Ymove = 0.7f *  Mathf.Abs(Mathf.Sin(counter1));
            Xmove = 0.8f * Mathf.Abs(Mathf.Cos(counter2));
            Debug.Log("ymove" + Ymove);
            GetComponent<RectTransform>().position = new Vector3((3 * Screen.width / 8) * (1 + Xmove), (5 * Screen.height / 10) *(1+ Ymove), 0);
            if (counter1 > 1f)
            {
                counter1 = 0f;
                counter2 = 0f;
                cntr = 0f;
                while (cntr <1f)
                {
                    cntr += Time.deltaTime;
                    GetComponent<RectTransform>().position = Vector3.MoveTowards(GetComponent<RectTransform>().position, firstPos, 1000 * Time.deltaTime);
                    yield return null;
                }
            }
            yield return null;
        }
    }
}
