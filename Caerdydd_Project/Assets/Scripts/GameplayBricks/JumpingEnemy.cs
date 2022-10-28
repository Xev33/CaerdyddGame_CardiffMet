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
            timer = 0.0f;
        }
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
}
