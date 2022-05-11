using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroubleManager : MonoBehaviour
{
    [SerializeField] Transform[] troublePoints;
    [SerializeField] GameObject[] allTroublesPrefab;
    [SerializeField] GameObject[] troubles;
    bool gameActive = true;

    void Start()
    {
        StartCoroutine(troubleSpawn());
    }
    IEnumerator troubleSpawn()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(5f);
            int selectionPoint = Random.Range(0, troublePoints.Length);
            int selectionTrouble = Random.Range(0, troubles.Length);
            if (troublePoints[selectionPoint].childCount == 0)
            {
                var toruble = Instantiate(troubles[selectionTrouble], troublePoints[selectionPoint].position, troublePoints[selectionPoint].rotation, troublePoints[selectionPoint]);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
