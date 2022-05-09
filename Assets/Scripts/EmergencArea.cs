using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencArea : MonoBehaviour, IHelper
{
    [SerializeField] int _helpNo;
    [SerializeField] public int helpNo { get; set; }
    [SerializeField] public GameObject fireParticle;
    private void Start()
    {
        helpNo = _helpNo;
    }
    public void helper(Line currentLine, GameObject troubleArea)
    {

    }
}
