using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyCanvas : MonoBehaviour
{
    public static moneyCanvas Instance;

    [SerializeField] RectTransform target;
    [SerializeField] GameObject panelPrefab;
    [SerializeField] Image money;
    [SerializeField] public int moneyAmount = 0;
    int moneyAmountPre;
    Vector3 screenPos;
    bool panelPosActive = false;
    Vector3 _troublePos;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    //public void moneySpawn(Vector3 troublePos, int _moneyAmount)
    //{
    //    panel.SetActive(true);
    //    moneyAmount += _moneyAmount;
    //    screenPos = Camera.main.WorldToScreenPoint(troublePos);
    //    for (int i = 0; i< _moneyAmount; i++)
    //    {
    //        Instantiate(money, new Vector3(screenPos.x, screenPos.y, 0), Quaternion.identity, this.transform);
    //    }

    //    for (int i = moneyAmount - _moneyAmount; i < moneyAmount; i++)
    //    {
    //        StartCoroutine(moneyScattering(transform.GetChild(i + 1).GetComponent<RectTransform>()));
    //    }
    //}
    public void moneySpawn(Vector3 troublePos, int _moneyAmount)
    {
        GameObject pnl = Instantiate(panelPrefab, this.transform);
        //panel.SetActive(true);
        pnl.GetComponent<moneyPanel>().troublePos = troublePos;
        pnl.GetComponent<moneyPanel>().moneyAmount = _moneyAmount;
        pnl.GetComponent<moneyPanel>().target = target;


        moneyAmountPre = _moneyAmount;
        moneyAmount += _moneyAmount;
        _troublePos = troublePos;
    }
    //IEnumerator panelPos(Vector3 troublePos)
    //{
    //    panelPosActive = false;
    //    yield return null;
    //    panelPosActive = true;

    //    while (panelPosActive)
    //    {
    //        panel.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(troublePos);
    //        yield return null;
    //    }
    //}
    //public void button()
    //{
    //    screenPos = Camera.main.WorldToScreenPoint(_troublePos);
    //    for (int i = 0; i < moneyAmount; i++)
    //    {
    //        Instantiate(money, new Vector3(screenPos.x, screenPos.y, 0), Quaternion.identity, this.transform);
    //    }

    //    for (int i = moneyAmount - moneyAmountPre; i < moneyAmount; i++)
    //    {
    //        StartCoroutine(moneyScattering(transform.GetChild(i + 1).GetComponent<RectTransform>()));
    //    }
    //    StartCoroutine(collecting());
    //    panelPosActive = false;

    //    panel.SetActive(false);
    //}
    //public void button()
    //{
    //    for (int i = 0; i < moneyAmount; i++)
    //    {
    //        StartCoroutine(moneyCollect(transform.GetChild(i + 1).GetComponent<RectTransform>()));
    //    }

    //    panelPosActive = false;

    //    panel.SetActive(false);
    //}
    IEnumerator moneyScattering(RectTransform mny)
    {
        float speed = 1000;
        Vector3 targetScat = new Vector3(Random.Range(-Screen.width / 8, Screen.width / 8) + Screen.width / 2, Random.Range(-Screen.height / 8, Screen.height / 8) + Screen.height / 2, 0);
        int screenPosDistance = Random.Range(50, 600);
        while (Vector3.Distance(targetScat, mny.position) > 10f && Vector3.Distance(screenPos, mny.position) < screenPosDistance)
        {
            speed -= 200 * Time.deltaTime;
            mny.position = Vector3.MoveTowards(mny.position, targetScat, speed * Time.deltaTime);
            yield return null;
        }
        //mny.position = targetScat;
    }
    IEnumerator collecting()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < moneyAmount; i++)
        {
            StartCoroutine(moneyCollect(transform.GetChild(i + 1).GetComponent<RectTransform>()));
        }
    }
    IEnumerator moneyCollect(RectTransform mny)
    {
        Debug.Log(mny.position);
        while (Vector3.Distance(target.position, mny.position) > 10f)
        {
      

            mny.position = Vector3.MoveTowards(mny.position, target.position, 1000 * Time.deltaTime);
            yield return null;
        }
        mny.position = target.position;
        yield return null;
        Destroy(mny.gameObject);
        GameManager.Instance.MoneyUpdate(200);
        moneyAmount = 0;
    }
}
