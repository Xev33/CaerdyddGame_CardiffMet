using System.Collections;
using UnityEngine;

public class FlyingEnemy : AbstractEnemy
{
    [SerializeField] private float waitingTime;
    [SerializeField] private float distanceMinToChange;
    [SerializeField] private GameObject[] waypoints;
    private Vector3[] transPoint;
    private Vector3 velocity;
    private int currentPoint;

    protected override void Awake()
    {
        base.Awake();
        transPoint = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            transPoint[i] = new Vector3(waypoints[i].transform.position.x, waypoints[i].transform.position.y, waypoints[i].transform.position.z);
        }
        currentPoint = 0;
    }

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
        transform.position = Vector3.SmoothDamp(this.transform.position, transPoint[currentPoint], ref velocity, speed * Time.deltaTime);

        if (transPoint[currentPoint].x < this.transform.position.x)
            lastDirection = -1;
        else
            lastDirection = 1;

        RotateMesh();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            if (other.gameObject.transform.position.y >= (this.transform.position.y + 1))
            {
                if (player.currentState != player.standingState && Player._instance.hp > 0)
                {
                    KillEnemy();
                }
            }
        }
        else if (other.tag == "KillZone")
        {
            KillEnemy();
        }
    }

    protected override void KillEnemy()
    {
        base.KillEnemy();
    }

    private IEnumerator WaitBeforeMove()
    {
        isTrigger = false;
        yield return new WaitForSeconds(waitingTime);
        isTrigger = true;
    }
}
