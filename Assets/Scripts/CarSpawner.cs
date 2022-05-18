using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour, IStartGameObserver
{

    public GameObject[] carPrefab;
    public float spawnTime;
    [SerializeField] public List<Transform> target;
    public bool spawnActive = true;
    //GameObject player;
    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
    }
    private void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            target.Add(transform.parent.GetChild(i).transform);
        }
    }
    public void StartGame()
    {

        StartCoroutine(SpawnCustomer());
        GameManager.Instance.Remove_StartObserver(this);
        Globals.population = 20;
    }
    private void OnEnable()
    {

    }
    IEnumerator SpawnCustomer()
    {
        yield return new WaitForSeconds(10f);

        //player = GameObject.Find("Player");
        while (spawnActive)
        {
            //Debug.Log("spawn active");

            //if (Vector3.Distance(player.transform.position,transform.position) < 15f)
            //{
            //    spawnActive = false;
            //    StartCoroutine(playerDistanceCheck());
            //    break;
            //}

            //if (Vector3.Distance(player.transform.position, transform.position) < 15f)
            //{
            //    spawnActive = false;
            //    StartCoroutine(playerDistanceCheck());
            //    break;
            //}
            GameObject car = Instantiate(carPrefab[Random.Range(0, carPrefab.Length)], transform.position, transform.rotation, this.transform);
            int selectTarget = Random.Range(0, target.Count);
            car.GetComponent<Vehicle>().destination = target[selectTarget];
            if (Globals.troubleActive)
            {

                int i = 10;
                while (i > 1)
                {
                    i--;

                    if (Globals.population > 20)
                    {
                        Globals.population -= 1;
                        GameManager.Instance.populationUpdate();
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else
            {
                car.GetComponent<Vehicle>().currentSelection = Vehicle.States.moveTarget;
            }
            Globals.population++;
            GameManager.Instance.populationUpdate();
            yield return new WaitForSeconds(spawnTime);

        }
    }
}
