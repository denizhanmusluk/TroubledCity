using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class tutorial : MonoBehaviour,IStartGameObserver
{
    [SerializeField] GameObject gameCamera, tutorialCamera;
    [SerializeField] CinemachineVirtualCamera cam1, cam2, cam3;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject trail;
    void Start()
    {
        hand.SetActive(false);
        GameManager.Instance.Add_StartObserver(this);
    }
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("tutoriallevel") == 1)
        {

        }
        else
        {
            tutorialStart();
            TroubleManager.Instance.tutorialTrouble();
        }
    }

   void tutorialStart()
    {
        FindObjectOfType<LineDraw>().cam = tutorialCamera.GetComponent<Camera>();
        gameCamera.SetActive(false);
        tutorialCamera.SetActive(true);
        cam1.Priority = 0;
        cam2.Priority = 10;
        StartCoroutine(camSet1());
    }
    IEnumerator camSet1()
    {

        Globals.tutorialActive = true;
        yield return new WaitForSeconds(4f);
        cam1.Priority = 10;
        cam2.Priority = 0;
        yield return new WaitForSeconds(2f);
        cam1.Priority = 0;
        cam2.Priority = 0;
        cam3.Priority = 10;
        StartCoroutine(handActive());
        while (!Globals.tutorialFireCam)
        {

            yield return null;
        }
        hand.SetActive(false);
        trail.SetActive(false);

        cam1.Priority = 0;
        cam2.Priority = 10;
        cam3.Priority = 0;

        while (Globals.tutorialFireCam)
        {
            yield return null;
        }

        cam1.Priority = 10;
        cam2.Priority = 0;
        cam3.Priority = 0;
        yield return new WaitForSeconds(2f);
        tutorialCamera.SetActive(false);
        gameCamera.SetActive(true);
        Globals.tutorialActive = false;
        FindObjectOfType<LineDraw>().cam = gameCamera.GetComponent<Camera>();
        PlayerPrefs.SetInt("tutoriallevel", 1);

    }
    IEnumerator handActive()
    {
        yield return new WaitForSeconds(2f);
        hand.SetActive(true);
        trail.SetActive(true);
        trail.GetComponent<TrailRenderer>().enabled = false;
        hand.GetComponent<HandTutorial>().StartSwipe();
        yield return new WaitForSeconds(1f);
        trail.GetComponent<TrailRenderer>().enabled = true;
    }
}
