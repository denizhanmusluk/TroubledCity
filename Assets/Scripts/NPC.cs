﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour, ITroubleFix
{
    public NavMeshAgent agent;
    [SerializeField] public Transform destination;
    public Animator anim;
    public enum States { moveTarget, troubleHospital, troublePolice, troubleFarm, troubleUniversity,dead,stopMove }
    public States currentSelection;
    int deadSelecting;
    [SerializeField] GameObject[] deadSprite;
    [SerializeField] Transform pankartPoint;
    [SerializeField] GameObject[] pancards;
    [SerializeField] GameObject[] emojies;
    [SerializeField] Transform emojiePoint;

    [SerializeField] GameObject guiltyPrefab;
    GameObject guilty;
    bool trigger = false;

    GameObject pancard;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = transform.GetComponent<Animator>();
        anim.SetBool("walk", true);
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
            case States.troubleHospital:
                {
                    troubleMoveHospital();
                }
                break;
            case States.troublePolice:
                {
                    troubleMovePolice();
                }
                break;
            case States.troubleFarm:
                {
                    troubleMoveFarm();
                }
                break;
            case States.troubleUniversity:
                {
                    troubleMoveUniversity();
                }
                break;
            case States.dead:
                {
                    agent.SetDestination(transform.position);
                }
                break;    
            case States.stopMove:
                {
                    stopping();
                }
                break;
        }
    }
    public void npcDead(Vector3 forceDirection)
    {
        currentSelection = States.dead;
        GetComponent<Ragdoll>().RagdollActivateWithForce(true, forceDirection);
        StartCoroutine(npcStandUp());
    }
    IEnumerator npcStandUp()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<Ragdoll>().RagdollActivate(false);
        currentSelection = States.moveTarget;

    }
    public void stopping()
    {
        agent.SetDestination(transform.position);

    }

    public void moveTarget()
    {
        if(agent.speed > 3)
        {
            agent.speed -= Time.deltaTime;
            anim.speed -= Time.deltaTime / 6;
        }
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
    public void troubleMoveHospital()
    {
        if (Vector3.Distance(transform.position, destination.position) < 7 && trigger)
        {
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            currentSelection = States.stopMove;
            GetComponent<CapsuleCollider>().radius *= 2;

        }else if (Vector3.Distance(transform.position, destination.position) < 1)
        {
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            currentSelection = States.stopMove;
            GetComponent<CapsuleCollider>().radius *= 2;

        }
        else
        {
            agent.SetDestination(destination.position);
            anim.SetBool("walk", true);
        }
    }
    public void troubleMovePolice()
    {
        agent.SetDestination(destination.position);

        if (Vector3.Distance(transform.position, destination.position) < 1)
        {
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
        }
        else
        {
            anim.SetBool("walk", true);
        }
    }
    public void troubleMoveFarm()
    {
        if (Vector3.Distance(transform.position, destination.position) < 8 && trigger)
        {
            currentSelection = States.stopMove;
            StartCoroutine(troubleFarm());
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            GetComponent<CapsuleCollider>().radius *= 2;

        }


        if (Vector3.Distance(transform.position, destination.position) < 5)
        {
            currentSelection = States.stopMove;
            StartCoroutine(troubleFarm());
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            GetComponent<CapsuleCollider>().radius *= 2;

        }
        else
        {
            agent.SetDestination(destination.position);

            anim.SetBool("walk", true);
        }
    }
    IEnumerator troubleFarm()
    {
        int selection = Random.Range(0, pancards.Length);
        int openPancard = Random.Range(0, 3);
        if (openPancard == 0)
        {
            pancard = Instantiate(pancards[selection], pankartPoint.position, pankartPoint.rotation, pankartPoint);
        }
        anim.SetBool("pancard", true);

        float counter = 0f;
        while (counter < 1f)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180, 0), 250 * Time.deltaTime);
            yield return null;
        }
    }
    public void troubleMoveUniversity()
    {
        if (Vector3.Distance(transform.position, destination.position) < 7 && trigger)
        {
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            currentSelection = States.stopMove;
            GetComponent<CapsuleCollider>().radius *= 2;

        }
        else if (Vector3.Distance(transform.position, destination.position) < 5)
        {
            anim.SetBool("walk", false);
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            agent.SetDestination(transform.position);
            currentSelection = States.stopMove;
            GetComponent<CapsuleCollider>().radius *= 2;
        }
        else
        {
            agent.SetDestination(destination.position);
            anim.SetBool("walk", true);
        }
    }
    public void _randomDead()
    {
        StartCoroutine(randomDead());
    }
    IEnumerator randomDead()
    {

        while (true)
        {
            yield return new WaitForSeconds(5f);
            deadSelecting = Random.Range(0, 3);
            if(deadSelecting == 0)
            {
                //TroubleManager.Instance.Remove_TroubleFixObserver(this);

                guilty = Instantiate(guiltyPrefab, transform.position + new Vector3(10, 0, 0), Quaternion.identity, this.transform.parent);
                guilty.GetComponent<guilty>().destination = transform;
                //guilty.GetComponent<Animator>().SetBool("walk", true);
                //GetComponent<Ragdoll>().RagdollActivate(true);
                //currentSelection = States.dead;
                StartCoroutine(guiltyPosCheck());
                break;
                //Destroy(this.gameObject ,5);
            }
        }
        yield return new WaitForSeconds(3f);
        //int selecting = Random.Range(0, deadSprite.Length);
        //Instantiate(deadSprite[selecting], transform.position, Quaternion.Euler(90, 0, 0),this.transform);
        //Destroy(this.gameObject);
    }
    IEnumerator guiltyPosCheck()
    {
        while(Vector3.Distance(transform.position, guilty.transform.position) > 3)
        {

            yield return null;
        }
        guilty.GetComponent<guilty>().gunParticle.Play();
        guilty.GetComponent<guilty>().gunParticle.GetComponent<AudioSource>().Play();

        float counter = 0f;
        while(counter < 2f)
        {
            counter += Time.deltaTime;
            guilty.GetComponent<guilty>().destination = guilty.transform;
            guilty.transform.LookAt(transform);
            guilty.GetComponent<Animator>().SetBool("gun", true);
            yield return null;
        }
        guilty.GetComponent<guilty>().gunParticle.Stop();
        guilty.GetComponent<guilty>().gunParticle.GetComponent<AudioSource>().Stop();

        guilty.GetComponent<Animator>().SetBool("gun", false);

        guilty.GetComponent<guilty>().destination = FindObjectOfType<SpawnManager>().transform.GetChild(Random.Range(0, FindObjectOfType<SpawnManager>().transform.childCount)).transform;

        GetComponent<Ragdoll>().RagdollActivate(true);
        currentSelection = States.dead;
        int selecting = Random.Range(0, deadSprite.Length);
        Instantiate(deadSprite[selecting], transform.position, Quaternion.Euler(90, 0, 0), this.transform);
        int i = 5;
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
        yield return new WaitForSeconds(10f);
        Destroy(guilty);
    }
    public void _randomEmoji()
    {
        StartCoroutine(randomEmoji());
    }
    IEnumerator randomEmoji()
    {
        while (true)
        {
            int emojiSelecting = Random.Range(0, emojies.Length);
            yield return new WaitForSeconds(emojiSelecting);
            Instantiate(emojies[emojiSelecting], emojiePoint.position, Quaternion.Euler(0, 0, 0),this.transform);                    
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NPC>() != null)
        {
            trigger = true;

        }
        if(other.GetComponent<Vehicle>() != null)
        {
            agent.speed = 9;
            anim.speed = 2;
        }
    }
    public void torubleFix()
    {
        //TroubleManager.Instance.Remove_TroubleFixObserver(this);
        currentSelection = States.moveTarget;
        if(pancard != null)
        {
            Destroy(pancard);
        }
    }
}
