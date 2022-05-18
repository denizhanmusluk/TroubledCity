using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troubleArea : MonoBehaviour, IHelper
{
    [SerializeField] int _helpNo;
    [SerializeField] public int helpNo { get; set; }
    public bool helpDrawActive { get; set; }

    [SerializeField] public GameObject fireParticle;
    public GameObject troubleIcon;
    public GameObject troubleCanvasIcon;
    private void Start()
    {
        helpNo = _helpNo;
        if (helpNo == 0)
        {
            StartCoroutine(fires());
        }
    }
    public void helper(Line currentLine, GameObject troubleArea)
    {

    }
    IEnumerator fires()
    {
        for(int i = 0; i < fireParticle.transform.childCount; i++)
        {
            StartCoroutine(fireUp(fireParticle.transform.GetChild(i).transform));
            yield return new WaitForSeconds(1f);
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
    public void iconSizeDown()
    {
        StartCoroutine(iconScaleDown());
    }
    IEnumerator iconScaleDown( )
    {
        float counter = 1;
        while (counter > 0)
        {
            counter -= 5 * Time.deltaTime;
            troubleIcon.transform.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        troubleIcon.transform.localScale = new Vector3(0, 0, 0);
    }
}
