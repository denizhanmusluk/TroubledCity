using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class faintHuman : MonoBehaviour
{
    public enum States { idle, moveTarget}
    public States currentSelection;
    public NavMeshAgent agent;
    public Transform destination;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentSelection = States.idle;
        StartCoroutine(startDelay());
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(8f);
        GetComponent<Ragdoll>().RagdollActivate(true);
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentSelection)
        {
            case States.idle:
                {

                }
                break;
            case States.moveTarget:
                {
                    moveTarget();
                }
                break;
        }
    }
    public void standUp()
    {
        transform.parent = null;

        StartCoroutine(npcStandUp());
    }
    IEnumerator npcStandUp()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Ragdoll>().RagdollActivate(false);
        destination = SpawnManager.Instance.transform.GetChild(0).transform;
        currentSelection = States.moveTarget;

    }

    public void moveTarget()
    {
     
        agent.SetDestination(destination.position);

        if (Vector3.Distance(transform.position, destination.position) < 1)
        {
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            Destroy(this.gameObject, 0.3f);
        }
        else
        {
            anim.SetBool("walk", true);
        }
    }
}
