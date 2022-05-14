using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helperCenter : MonoBehaviour
{
    [SerializeField] int buildingID;
    [SerializeField] public int[] equipCount;
    [SerializeField] public int[] equipSpeed;
    [SerializeField] public float[] troubleSolutionSpeed;

    [SerializeField] public int[] costEquipCountUpgrade;
    [SerializeField] public int[] costEquipSpeedUpgrade;
    [SerializeField] public int[] costTroubleSolutionSpeedUpgrade;

    [SerializeField] public int levelEquipCount;
    [SerializeField] public int levelEquipSpeed;
    [SerializeField] public int levelTroubleSolutionSpeed;


    int currentEquipCount;
    int currentEquipSpeed;
    float currentTroubleSolutionSpeed;

    [SerializeField] public GameObject[] equip;

    void Start()
    {
        equipCountUpgradeSet();
        equipSpeedUpgradeSet();
        troubleSolutionSpeedUpgradeSet();
    }

    // Update is called once per frame
    public void equipCountUpgradeSet()
    {
        currentEquipCount = equipCount[levelEquipCount];
        for(int i = 0;i< currentEquipCount; i++)
        {
            equip[i].SetActive(true);
        }

        equipSpeedUpgradeSet();
        troubleSolutionSpeedUpgradeSet();
    } 
    public void equipSpeedUpgradeSet()
    {
        currentEquipSpeed = equipSpeed[levelEquipSpeed];
        foreach (var helperTeam in GetComponentsInChildren<HelperTeam>())
        {
            helperTeam.equipSpeed = currentEquipSpeed;
        }

    }
    public void troubleSolutionSpeedUpgradeSet()
    {
        currentTroubleSolutionSpeed = troubleSolutionSpeed[levelTroubleSolutionSpeed];
        foreach (var helperTeam in GetComponentsInChildren<HelperTeam>())
        {
            helperTeam.troubleSolutionSpeed = currentTroubleSolutionSpeed;
        }
    }
}
