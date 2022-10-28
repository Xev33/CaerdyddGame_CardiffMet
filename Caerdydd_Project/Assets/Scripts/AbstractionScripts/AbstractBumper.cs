using UnityEngine;

public abstract class AbstractBumper : MonoBehaviour
{
    protected Player player;
    [SerializeField] private float bumpProjection;

    private void Start()
    {
        player = Player._instance;    
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player.currentState != player.standingState && Player._instance.hp > 0 && player.isInvicible == false)
            {
                player.SpinJump(bumpProjection);
            }
        }
    }
}
