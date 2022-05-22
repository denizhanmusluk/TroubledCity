using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fireFighter : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform targetGuilty;
    Transform targetpoint;
    [SerializeField] public ParticleSystem gunParticle;

    public enum States { idle, moveTarget, aid, firstMove }
    public States currentSelection;
    Animator anim;
 public   Transform firstPos;
    float gunCounter = 0f;

    void Start()
    {
        targetpoint = targetGuilty.parent;
        anim = GetComponent<Animator>();
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
            case States.aid:
                {
                    gun();
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
        if (targetGuilty != null)
        {

            if (Vector3.Distance(transform.position, targetGuilty.transform.parent.position) < 3f)
            {
                anim.SetBool("aid", true);
                //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
                agent.SetDestination(transform.position);
                currentSelection = States.aid;
                gunParticle.Play();
            }
            else
            {
                agent.SetDestination(targetGuilty.transform.parent.position);
                anim.SetBool("aid", false);
            }
        }
        else
        {
            currentSelection = States.firstMove;
            //gunParticle.Stop();
        }
    }
    void firstMoving()
    {
        if (Vector3.Distance(transform.position, firstPos.position) < 5f)
        {
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            Destroy(gameObject);
        }
        else
        {
            agent.SetDestination(firstPos.position);
            anim.SetBool("aid", false);

        }
    }
    void gun()
    {
        if (targetGuilty == null)
        {
            currentSelection = States.firstMove;
            //gunParticle.Stop();

        }
        else
        {
            gunCounter += Time.deltaTime;
            transform.LookAt(targetGuilty);
            if (gunCounter > 2.5f)
            {
                gunCounter = 0;
                currentSelection = States.firstMove;

                gunParticle.Stop();
                //targetGuilty.GetComponent<guilty>().npcDead((targetGuilty.position - transform.position).normalized);
                //Destroy(targetGuilty.GetComponent<guilty>());



            }
        }
    }
}
