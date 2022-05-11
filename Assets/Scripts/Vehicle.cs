using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] public Transform destination;
    public enum States { moveTarget, troubleHospital, troublePolice, troubleFarm, troubleUniversity, dead, stopMove }
    public States currentSelection;
    [SerializeField] GameObject[] deadSprite;

    float stopCounter = 0f;

    Vector3 forceDirection;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(destination.position);
        //currentSelection = States.moveTarget;
        //TroubleManager.Instance.Add_TroubleFixObserver(this);
    }
    void Update()
    {
        switch (currentSelection)
        {
            case States.moveTarget:
                {
                    moveTarget();
                }
                break;

            case States.stopMove:
                {
                    stopping();
                }
                break;
        }
    }

    public void stopping()
    {
        agent.enabled = false;
        stopCounter += Time.deltaTime;
        if (stopCounter > 4f)
        {
            agent.enabled = true;
            currentSelection = States.moveTarget;
            agent.speed = 30;
        }
    }

    public void moveTarget()
    {
        if(agent.speed < 15)
        {
            agent.speed += 0.5f * Time.deltaTime;
        }
        agent.SetDestination(destination.position);

        if (Vector3.Distance(transform.position, destination.position) < 5)
        {
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            Destroy(this.gameObject, 0.3f);
        }
        else
        {
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "light")
        {
            if (collision.transform.GetComponent<Rigidbody>() == null)
            {
                collision.gameObject.AddComponent<Rigidbody>();
            }
            forceDirection = (transform.position - collision.transform.position).normalized;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.up * 15 - forceDirection * 7);
            collision.transform.GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);

        }
        if (collision.transform.GetComponent<Vehicle>() != null)
        {
            if(transform.position.x > collision.transform.position.x)
            {
                agent.speed = 20;
            }
            else
            {
                agent.speed = 10;
            }
        }

    }

}
