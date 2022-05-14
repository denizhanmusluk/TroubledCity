using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpIcon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loopColorScaleSet());
    }

    IEnumerator loopColorScaleSet()
    {
        float counter1 = 0f;
        float scaleValue1 = 0f;
        while (true)
        {
            counter1 += 5 * Time.deltaTime;
            scaleValue1 = Mathf.Abs(Mathf.Cos(counter1));
            transform.localScale = new Vector3(1, 1f, 1f) + new Vector3(scaleValue1 / 5f, scaleValue1 / 5f, 0f);
            yield return null;
        }
        transform.localScale = new Vector3(1, 1f, 1f);
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
