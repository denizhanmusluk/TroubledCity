using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panelBG;
    [SerializeField] public TextMeshProUGUI costEquipCount_Text;
    [SerializeField] public TextMeshProUGUI costMoveSpeed_Text;
    [SerializeField] public TextMeshProUGUI costHelpSpeed_Text;

    [SerializeField] public TextMeshProUGUI levelEquipCount_Text;
    [SerializeField] public TextMeshProUGUI levelEquipSpeed_Text;
    [SerializeField] public TextMeshProUGUI levelTroubleSolutionSpeed_Text;
    helperCenter helperCenter;


    [SerializeField] Button button1, button2, button3;

    [SerializeField] Material buttonDownMaterial, buttonUpMaterial;

    [SerializeField] RectTransform fullPanel;
    float firstImageScale = 0;
    float lastImageScale = 1f;
    void Start()
    {
        panel.SetActive(false);
        panelBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out hit))
            {
                if (hit.collider.transform.name == "Upgrade")
                {
                    StartCoroutine(buttonMove(hit.collider.transform));
                    helperCenter = hit.collider.transform.parent.GetComponent<helperCenter>();
                    upgradeOpen();
                }  
                //if (hit.collider.GetComponent<helperCenter>() != null)
                //{
                //    helperCenter = hit.collider.GetComponent<helperCenter>();
                //    upgradeOpen();
                //}
            }
        }
    }
    IEnumerator buttonMove(Transform _button)
    {
        _button.GetComponent<MeshRenderer>().material = buttonDownMaterial;

        Vector3 firstPos = _button.position;
        Vector3 targetPos = _button.position + new Vector3(0, -1, 0);
        while (Vector3.Distance(_button.position, targetPos) > 0.1f)
        {
            _button.position = Vector3.MoveTowards(_button.position, targetPos, 5 * Time.deltaTime);
            yield return null;
        }
        _button.position = firstPos;
        _button.GetComponent<MeshRenderer>().material = buttonUpMaterial;

    }
    public void upgradeOpen()
    {
        panel.SetActive(true);
        panelBG.SetActive(true);
        levelEquipCount_Text.text = (helperCenter.levelEquipCount + 1).ToString();
        levelEquipSpeed_Text.text = (helperCenter.levelEquipSpeed + 1).ToString();
        levelTroubleSolutionSpeed_Text.text = (helperCenter.levelTroubleSolutionSpeed + 1).ToString();

        costEquipCount_Text.text = helperCenter.costEquipCountUpgrade[helperCenter.levelEquipCount].ToString();
        costMoveSpeed_Text.text = helperCenter.costEquipSpeedUpgrade[helperCenter.levelEquipSpeed].ToString();
        costHelpSpeed_Text.text = helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed].ToString();
        buttonInteractableSet();

        StartCoroutine(panelScaleSet(fullPanel));
    }

    public void upgradeClose()
    {
        panel.SetActive(false);
        panelBG.SetActive(false);
    }


    public void equipCountUpgradeButton()
    {
        if (Globals.moneyAmount >= helperCenter.costEquipCountUpgrade[helperCenter.levelEquipCount])
        {
            GameManager.Instance.MoneyUpdate(-helperCenter.costEquipCountUpgrade[helperCenter.levelEquipCount]);

            helperCenter.levelEquipCount++;
            levelEquipCount_Text.text = (helperCenter.levelEquipCount + 1).ToString();
            costEquipCount_Text.text = helperCenter.costEquipCountUpgrade[helperCenter.levelEquipCount].ToString();

            helperCenter.equipCountUpgradeSet();
        }
        buttonInteractableSet();
    }
    public void equipSpeedUpgradeButton()
    {
        if (Globals.moneyAmount >= helperCenter.costEquipSpeedUpgrade[helperCenter.levelEquipSpeed])
        {
            GameManager.Instance.MoneyUpdate(-helperCenter.costEquipSpeedUpgrade[helperCenter.levelEquipSpeed]);

            helperCenter.levelEquipSpeed++;
            levelEquipSpeed_Text.text = (helperCenter.levelEquipSpeed + 1).ToString();
            costMoveSpeed_Text.text = helperCenter.costEquipSpeedUpgrade[helperCenter.levelEquipSpeed].ToString();
            helperCenter.equipSpeedUpgradeSet();
        }
        buttonInteractableSet();
    }
    public void troubleSolutionSpeedUpgradeButton()
    {
        if (Globals.moneyAmount >= helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed])
        {
            GameManager.Instance.MoneyUpdate(-helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed]);

            helperCenter.levelTroubleSolutionSpeed++;
            levelTroubleSolutionSpeed_Text.text = (helperCenter.levelTroubleSolutionSpeed + 1).ToString();
            costHelpSpeed_Text.text = helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed].ToString();
            helperCenter.troubleSolutionSpeedUpgradeSet();
        }
        buttonInteractableSet();
    }

    void buttonInteractableSet()
    {
        //////////////////////////////////////////////////
        if (Globals.moneyAmount >= helperCenter.costEquipCountUpgrade[helperCenter.levelEquipCount])
        {
            button1.interactable = true;
        }
        else
        {
            button1.interactable = false;
        }
        //////////////////////////////////////////////////
        if (Globals.moneyAmount >= helperCenter.costEquipSpeedUpgrade[helperCenter.levelEquipSpeed])
        {
            button2.interactable = true;
        }
        else
        {
            button2.interactable = false;
        }
        //////////////////////////////////////////////////
        if (Globals.moneyAmount >= helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed])
        {
            button3.interactable = true;
        }
        else
        {
            button3.interactable = false;
        }
    }

    IEnumerator panelScaleSet(RectTransform image)
    {
        float counter = firstImageScale;
        while (counter < lastImageScale)
        {
            counter += 5 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);
        counter = 0f;
        float scale = 0;
        while (counter < Mathf.PI)
        {
            counter += 10 * Time.deltaTime;
            scale = Mathf.Sin(counter);
            scale *= 0.2f;
            image.localScale = new Vector3(lastImageScale + scale, lastImageScale + scale, lastImageScale + scale);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);

    }
}