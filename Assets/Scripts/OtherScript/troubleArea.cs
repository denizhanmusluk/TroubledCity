using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troubleArea : MonoBehaviour, IHelper
{
    [SerializeField] int _helpNo;
    [SerializeField] public int helpNo { get; set; }
    public bool helpDrawActive { get; set; }

    [SerializeField] public GameObject fireParticle;
    private void Start()
    {
        helpNo = _helpNo;
        StartCoroutine(fires());
    }
    public void helper(Line currentLine, GameObject troubleArea)
    {

    }
    IEnumerator fires()
    {
        for(int i = 0; i < fireParticle.transform.childCount; i++)
        {
            StartCoroutine(fireUp(fireParticle.transform.GetChild(i).transform));
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator fireUp(Transform fire)
    {
        float counter = 0f;
        while (counter < 1f)
        {
            counter += 0.2f * Time.deltaTime;
            fire.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        fire.localScale = new Vector3(1, 1, 1);
    }
}
