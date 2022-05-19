using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class engineer : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform targetGuilty;

    public enum States { idle, moveTarget, repairing, firstMove }
    public States currentSelection;
    Animator anim;
    Vector3 firstPos;
    float repairCounter = 0f;
    public Transform point1, point2, point3, point4;
    void Start()
    {
        anim = GetComponent<Animator>();
        firstPos = transform.position;
        currentSelection = States.moveTarget;


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
            case States.repairing:
                {
                    repair();
                }
                break;

            case States.firstMove:
                {
                    firstMoving();
                }
                break;
        }
    }
    void moveTarget()
    {
        if (Vector3.Distance(transform.position, targetGuilty.position) < 4f)
        {
            //anim.SetBool("gun", true);
            agent.SetDestination(transform.position);
            currentSelection = States.repairing;
        }
        else
        {
            agent.SetDestination(targetGuilty.position);
            //anim.SetBool("gun", false);
        }
    }
    void firstMoving()
    {
        if (Vector3.Distance(transform.position, firstPos) < 2)
        {
            agent.SetDestination(transform.position);
            Destroy(gameObject);
        }
        else
        {
            agent.SetDestination(firstPos);
            //anim.SetBool("gun", false);
        }
    }
    void repair()
    {

        repairCounter += Time.deltaTime;
        transform.LookAt(targetGuilty);
        if (repairCounter > 2f)
        {
            repairCounter = 0;
            currentSelection = States.firstMove;
            targetGuilty.GetComponent<carCrash>().carRepairing(point1, point2, point3,point4);

        }
    }
}