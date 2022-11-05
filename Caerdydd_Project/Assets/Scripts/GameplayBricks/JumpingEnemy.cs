using UnityEngine;

public class JumpingEnemy : AbstractEnemy
{
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float jumpCooldown;
    private float timer;

    protected override void Awake()
    {
        base.Awake();
        timer = jumpCooldown;
    }

    void Update()
    {
        if (isTrigger == false)
            return;
        timer += Time.deltaTime;
        if (timer >= jumpCooldown)
        {
            body.velocity = new Vector2(body.velocity.y, jumpVelocity);
            anim.SetTrigger("Jump");
            timer = 0.0f;
        }
        if (player.transform.position.x > this.transform.position.x)
            lastDirection = 1;
        else
            lastDirection = -1;
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
            } else
                Player._instance.TakeHit(1);

        }
        else if (other.tag == "KillZone")
        {
            KillEnemy();
        }
    }
}
