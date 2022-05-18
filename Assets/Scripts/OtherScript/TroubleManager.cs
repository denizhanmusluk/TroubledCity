using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleManager : MonoBehaviour,IStartGameObserver
{
    public static TroubleManager Instance;

    [SerializeField] Transform[] fireTroublePoints;
    [SerializeField] Transform[] ambulanceTroublePoints;
    [SerializeField] Transform[] policeTroublePoints;
    [SerializeField] Transform[] repairTroublePoints;
    [SerializeField] GameObject[] troubles;
    bool gameActive = true;
    public UIdirectionManager uiDir;
    [SerializeField] float troubleSpawnPeriod;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
    }
    public void StartGame()
    {
        fireTroubleStart();

    }
    public void tutorialTrouble()
    {
        var toruble = Instantiate(troubles[0], fireTroublePoints[0].position, fireTroublePoints[0].rotation, fireTroublePoints[0]);

    }
    public void fireTroubleStart()
    {
        //////////////////
        //uiDir.targetList.Add(toruble.gameObject);
        //uiDir.instIcon(toruble.gameObject, 0);
        //////////////////
        StartCoroutine(fireTroubleSpawn());
    }
    IEnumerator fireTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(troubleSpawnPeriod);
            int selectionPoint = Random.Range(0, fireTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (fireTroublePoints[selectionPoint].childCount == 0 && !Globals.tutorialActive)
            {
                var toruble = Instantiate(troubles[0], fireTroublePoints[selectionPoint].position, fireTroublePoints[selectionPoint].rotation, fireTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
                uiDir.instIcon(toruble.gameObject,0);
            }
        }
    }
    public void TroubleStart(int troubleNum)
    {
        if (troubleNum == 1)
        {
            StartCoroutine(ambulanceTroubleSpawn());
            troubleSpawnPeriod *= 0.9f;
        }   
        if (troubleNum == 2)
        {
            StartCoroutine(policeTroubleSpawn());
            troubleSpawnPeriod *= 0.85f;
        }
        if (troubleNum == 3)
        {
            StartCoroutine(repairTroubleSpawn());
            troubleSpawnPeriod *= 0.8f;
        }
    }
    IEnumerator ambulanceTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(troubleSpawnPeriod);
            int selectionPoint = Random.Range(0, ambulanceTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (ambulanceTroublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[1], ambulanceTroublePoints[selectionPoint].position, ambulanceTroublePoints[selectionPoint].rotation, ambulanceTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
                uiDir.instIcon(toruble.gameObject,1);
            }
        }
    }
    IEnumerator policeTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(troubleSpawnPeriod);
            int selectionPoint = Random.Range(0, policeTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (policeTroublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[2], policeTroublePoints[selectionPoint].position, policeTroublePoints[selectionPoint].rotation, policeTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
                uiDir.instIcon(toruble.gameObject,2);
            }
        }
    }

    IEnumerator repairTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(troubleSpawnPeriod);
            int selectionPoint = Random.Range(0, repairTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (repairTroublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[3], repairTroublePoints[selectionPoint].position, repairTroublePoints[selectionPoint].rotation, repairTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
                uiDir.instIcon(toruble.gameObject, 3);
            }
        }
    }
}
