using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class guilty : MonoBehaviour
{
    public enum States { idle, moveTarget, gun, dead , firstMove }
    public States currentSelection;
    public NavMeshAgent agent;
    public Transform destination;
    [SerializeField] public ParticleSystem gunParticle;
    Vector3 firstPos;
    public Transform targetNpc;
  [SerializeField]  List<Transform> npcList;
  [SerializeField]  bool targetSelectActive = true;
    Animator anim;
    float gunCounter = 0f;
    void Start()
    {
        firstPos = transform.position;
        anim = GetComponent<Animator>();
        gunParticle.Play();

        currentSelection = States.idle;

        StartCoroutine(targetSelection());
    }
    IEnumerator targetSelection()
    {
        yield return null;

        foreach (var npc in SpawnManager.Instance.transform.GetComponentsInChildren<NPC>())
        {
            npcList.Add(npc.transform);
        }

        for (int i = 0; i < npcList.Count; i++)
        {
            if (Vector3.Distance(transform.position, npcList[i].position) < 50 && targetSelectActive)
            {
                targetNpc = npcList[i];
                targetSelectActive = false;
                npcList.Clear();
                currentSelection = States.moveTarget;
                gunParticle.Stop();
                break;
            }
        }
        yield return null;

        if (targetSelectActive)
        {
            npcList.Clear();
            StartCoroutine(targetSelection());
        }
    }
    // Update is called once per frame
    void Update()
    {

        switch (currentSelection)
        {
            case States.idle:
                {
                    anim.SetBool("gun", true);
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
            case States.dead:
                {

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
        if (Vector3.Distance(transform.position, targetNpc.position) < 20)
        {
            anim.SetBool("gun", true);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            currentSelection = States.gun;
            gunParticle.Play();
        }
        else
        {
            agent.SetDestination(targetNpc.position);
            anim.SetBool("gun", false);
        }
    }
    void firstMoving()
    {
        if (Vector3.Distance(transform.position, firstPos) < 8)
        {
            anim.SetBool("gun", true);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            currentSelection = States.idle;
            gunParticle.Play();
            StartCoroutine(targetSelection());
        }
        else
        {
            agent.SetDestination(firstPos);
            anim.SetBool("gun", false);
        }
    }
    void gun()
    {
        gunCounter += Time.deltaTime;
        transform.LookAt(targetNpc);
        if(gunCounter> 2f)
        {
            gunCounter = 0;
            targetSelectActive = true;
            currentSelection = States.firstMove;

            gunParticle.Stop();
            targetNpc.GetComponent<NPC>().npcDead((targetNpc.position - transform.position).normalized);
            Destroy(targetNpc.GetComponent<NPC>());


            GameObject _npc = targetNpc.gameObject;
            Destroy(_npc, 2);
        }
    }
    public void npcDead(Vector3 forceDirection)
    {
        currentSelection = States.dead;
        GetComponent<Ragdoll>().RagdollActivateWithForce(true, forceDirection);

        Destroy(gameObject, 2);
    }
}
