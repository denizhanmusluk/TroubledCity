using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trail : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] Camera cam;
    private void Start()
    {
        transform.position = cam.ScreenToWorldPoint(new Vector3(hand.transform.position.x, hand.transform.position.y, transform.position.y));
    }
    void Update()
    {
        transform.position = cam.ScreenToWorldPoint(new Vector3(hand.transform.position.x, hand.transform.position.y, transform.position.y));
    }
}
