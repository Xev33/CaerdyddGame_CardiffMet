using System.Collections;
using UnityEngine;

public class FlyingEnemy : AbstractEnemy
{
    [SerializeField] private float speed;
    [SerializeField] private float waitingTime;
    [SerializeField] private float distanceMinToChange;
    [SerializeField] private GameObject[] waypoints;
    private Vector3[] transPoint;
    private Vector3 velocity;
    private int currentPoint;

    protected override void Awake()
    {
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
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            if (player.currentState != player.standingState && Player._instance.hp > 0 && player.isInvicible == false)
            {
                KillEnemy();
            }
        }
        else if (other.tag == "KillZone")
        {
            KillEnemy();
        }
    }

    protected override void KillEnemy()
    {
        Destroy(damageZone);
        base.KillEnemy();
    }

    private IEnumerator WaitBeforeMove()
    {
        isTrigger = false;
        yield return new WaitForSeconds(waitingTime);
        isTrigger = true;
    }
}
