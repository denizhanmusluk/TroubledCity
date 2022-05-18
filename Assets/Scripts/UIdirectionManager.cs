using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIdirectionManager : MonoBehaviour
{
    public List<GameObject> targetList;
    public GameObject[] iconPrefab;
    void Start()
    {
        
    }
    public void instIcon(GameObject target, int helpNo)
    {
      var icon=  Instantiate(iconPrefab[helpNo], this.transform);
        icon.GetComponent<UIdirection>().Target = target.transform;
        target.GetComponent<troubleArea>().troubleCanvasIcon = icon;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
