using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class police : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform targetGuilty;
    [SerializeField] public ParticleSystem gunParticle;

    public enum States { idle, moveTarget, gun, firstMove }
    public States currentSelection;
    Animator anim;
    Vector3 firstPos;
    float gunCounter = 0f;

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
            case States.gun:
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
            if (Vector3.Distance(transform.position, targetGuilty.position) < 10f)
            {
                anim.SetBool("gun", true);
                //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
                agent.SetDestination(transform.position);
                currentSelection = States.gun;
                gunParticle.Play();
            }
            else
            {
                agent.SetDestination(targetGuilty.position);
                anim.SetBool("gun", false);
            }
        }
        else
        {
            currentSelection = States.firstMove;
            gunParticle.Stop();
        }
    }
    void firstMoving()
    {
        if (Vector3.Distance(transform.position, firstPos) < 2)
        {
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            Destroy(gameObject);
        }
        else
        {
            agent.SetDestination(firstPos);
            anim.SetBool("gun", false);
        }
    }
    void gun()
    {
        if (targetGuilty == null)
        {
            currentSelection = States.firstMove;
            gunParticle.Stop();

        }
        else
        {
            gunCounter += Time.deltaTime;
            transform.LookAt(targetGuilty);
            if (gunCounter > 2f)
            {
                gunCounter = 0;
                currentSelection = States.firstMove;

                gunParticle.Stop();
                targetGuilty.GetComponent<guilty>().npcDead((targetGuilty.position - transform.position).normalized);
                Destroy(targetGuilty.GetComponent<guilty>());


                GameObject _npc = targetGuilty.gameObject;
                Destroy(_npc, 2);
            }
        }
    }
}
