using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class carCrash : MonoBehaviour
{
    public enum States { idle, moveTarget }
    public States currentSelection;
    public NavMeshAgent agent;
    public Transform destination;
    Animator anim;
    [SerializeField] GameObject smokeParticle;
    void Start()
    {
        destination = SpawnManager.Instance.transform.GetChild(0).transform;

        anim = GetComponent<Animator>();
        currentSelection = States.moveTarget;
        StartCoroutine(startDelay());
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(1f);
        Vector3 forceDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));


        destination = transform;
        currentSelection = States.idle;
        StartCoroutine(fireUp(smokeParticle.transform));
        yield return null;
        GetComponent<Rigidbody>().AddForce(transform.up * 800 + transform.forward * 400);
        yield return new WaitForSeconds(4f);
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;
        //GetComponent<Rigidbody>().AddForce(transform.up * 15 - forceDirection * 7);
        //GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentSelection)
        {
            case States.idle:
                {
                    agent.enabled = false;
                }
                break;
            case States.moveTarget:
                {
                    moveTarget();
                }
                break;
        }
    }


    public void moveTarget()
    {

        agent.SetDestination(destination.position);

        if (Vector3.Distance(transform.position, destination.position) < 1)
        {
            //destination.transform.parent.GetComponent<Build>().customerList.Remove(this.gameObject);
            Destroy(this.gameObject, 0.3f);
        }
        else
        {
        }
    }
    IEnumerator fireUp(Transform fire)
    {
        float counter = 0f;
        while (counter < 1f)
        {
            counter += 0.1f * Time.deltaTime;
            fire.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        fire.localScale = new Vector3(1, 1, 1);
    }
    public void carRepairing(Transform pos1, Transform pos2, Transform pos3, Transform pos4)
    {
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;

        StartCoroutine(_carRepairing(pos1, pos2,pos3,pos4));
        transform.parent = pos1.parent;
    }
    IEnumerator _carRepairing(Transform pos1, Transform pos2, Transform pos3, Transform pos4)
    {
        while (Vector3.Distance(transform.position,pos1.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos1.position, 20 * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot(pos1), 500 * Time.deltaTime);
            yield return null;
        }
        while (Vector3.Distance(transform.position, pos2.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2.position, 20 * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot(pos2), 500 * Time.deltaTime);

            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, transform.localEulerAngles.y, transform.localEulerAngles.z), 100 * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, pos3.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos3.position, 20 * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot(pos3), 500 * Time.deltaTime);

            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, transform.localEulerAngles.y, transform.localEulerAngles.z), 100 * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, pos4.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos4.position, 20 * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot(pos4), 500 * Time.deltaTime);

            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, transform.localEulerAngles.y, transform.localEulerAngles.z), 100 * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(carRemove(pos4, pos3, pos2, pos1));

    }
    IEnumerator carRemove(Transform pos1, Transform pos2, Transform pos3, Transform pos4)
    {
        while (!transform.parent.GetComponent<HelperTeam>().helpDrawActive)
        {
            yield return null;
        }
        while (Vector3.Distance(transform.position, pos1.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos1.position, 20 * Time.deltaTime);
            yield return null;
        }
        while (Vector3.Distance(transform.position, pos2.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2.position, 20 * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, pos3.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos3.position, 20 * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, pos4.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos4.position, 20 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    Quaternion targetRot(Transform targetPos)
    {
        Vector3 dir = (targetPos.position - transform.position);

        dir = (targetPos.position - transform.position);
        dir.Normalize();

        Vector3 direction = new Vector3(dir.x, 0f, dir.z);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newRot = Quaternion.Euler(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
        return newRot;
    }
}
