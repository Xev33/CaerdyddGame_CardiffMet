using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MovingPlateform : MonoBehaviour, ITriggerActor
{
    [SerializeField] private bool shouldLerp = false;
    [SerializeField] private float speed;
    [SerializeField] private float waitingTime;
    [SerializeField] private float distanceMinToChange;
    [SerializeField] private float waitingTimeToTrigger;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float timeToMove = 1f;
    private Vector3[] transPoint;
    private Vector3 velocity = Vector3.zero;
    private int currentPoint;
    private int lastPoint;
    private bool isTrigger = false;

    // Start is called before the first frame update
    void Awake()
    {
        transPoint = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            transPoint[i] = new Vector3(waypoints[i].transform.position.x, waypoints[i].transform.position.y, waypoints[i].transform.position.z);
        }
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger == false)
            return;
        if (Vector3.Distance(transPoint[currentPoint], this.transform.position) < distanceMinToChange)
        {
            currentPoint++;
            if (currentPoint >= transPoint.Length)
                currentPoint = 0;
            StartCoroutine(WaitBeforeMove());
        }
        if (shouldLerp == true)
        {
            transform.position = Vector3.SmoothDamp(this.transform.position, transPoint[currentPoint], ref velocity, timeToMove);
            //transform.position = Vector3.SmoothDamp(this.transform.position, transPoint[currentPoint], ref velocity, speed * Time.deltaTime);
        }
        else
            transform.position = Vector3.MoveTowards(this.transform.position, transPoint[currentPoint], speed * Time.deltaTime);
    }

    public void TriggerActor()
    {
        if (waitingTimeToTrigger == 0)
            isTrigger = true;
        else
            StartCoroutine(WaitToTrigger());
    }

    private IEnumerator WaitBeforeMove()
    {
        isTrigger = false;
        yield return new WaitForSeconds(waitingTime);
        isTrigger = true;
    }

    private IEnumerator WaitToTrigger()
    {
        yield return new WaitForSeconds(waitingTimeToTrigger);
        isTrigger = true;
    }

    void OnCollisionEnter(Collision c)
    {
        c.transform.parent = this.transform;
        if (c.gameObject.tag == "Player")
            Player._instance.isOnMovingPlatform = true;
    }
    void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "Player")
            Player._instance.isOnMovingPlatform = false;
        StartCoroutine(WaitToUnparent(c));
    }

    private IEnumerator WaitToUnparent(Collision c)
    {
        float timer = 0.0f;
        lastPoint = currentPoint;
        float duration = 1.1f;

        if (isTrigger == false)
            duration = 0.0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (currentPoint != lastPoint && Player._instance.isOnMovingPlatform == false || isTrigger == false)
                timer = duration + 1.0f;
            yield return null;
        }
        Player._instance.transform.parent = null;
    }
}
