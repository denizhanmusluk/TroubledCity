using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleManager : MonoBehaviour
{
    public static TroubleManager Instance;

    [SerializeField] Transform[] fireTroublePoints;
    [SerializeField] Transform[] ambulanceTroublePoints;
    [SerializeField] Transform[] policeTroublePoints;
    [SerializeField] GameObject[] troubles;
    bool gameActive = true;
    public UIdirection uiDir;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
     void Start()
    {
        fireTroubleStart();
    }
    public void fireTroubleStart()
    {
        StartCoroutine(fireTroubleSpawn());
    }
    IEnumerator fireTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(5f);
            int selectionPoint = Random.Range(0, fireTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (fireTroublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[0], fireTroublePoints[selectionPoint].position, fireTroublePoints[selectionPoint].rotation, fireTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
            }
        }
    }
    public void TroubleStart(int troubleNum)
    {
        if (troubleNum == 1)
        {
            StartCoroutine(ambulanceTroubleSpawn());
        }   if (troubleNum == 2)
        {
            StartCoroutine(policeTroubleSpawn());
        }
    }
    IEnumerator ambulanceTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(5f);
            int selectionPoint = Random.Range(0, ambulanceTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (ambulanceTroublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[1], ambulanceTroublePoints[selectionPoint].position, ambulanceTroublePoints[selectionPoint].rotation, ambulanceTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
            }
        }
    }
    IEnumerator policeTroubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(5f);
            int selectionPoint = Random.Range(0, policeTroublePoints.Length);
            //int selectionTrouble = Random.Range(0, troubles.Length);
            if (policeTroublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[2], policeTroublePoints[selectionPoint].position, policeTroublePoints[selectionPoint].rotation, policeTroublePoints[selectionPoint]);
                uiDir.targetList.Add(toruble.gameObject);
            }
        }
    }
}
