using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class helpProgress : MonoBehaviour
{
    [SerializeField] Image progressBar;
     Transform progressCanvas;
    void Start()
    {
        progressCanvas = progressBar.transform.parent;
        progressBar.fillAmount = 0f;
        progressBar.enabled = false;
    }
    public void progressing(float progressTime)
    {
        progressBar.enabled = true;
        StartCoroutine(_progressing(progressTime));
    }
    IEnumerator _progressing(float progressTime)
    {
        float counter = 0f;
        while (progressTime > counter)
        {
            counter += Time.deltaTime;
            progressBar.fillAmount = counter / progressTime;

            yield return null;
        }
        progressBar.fillAmount = 1;
        progressBar.enabled = false;

    }
    private void Update()
    {
        progressCanvas.transform.LookAt(Camera.main.transform);
    }
}
