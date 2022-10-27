using UnityEngine;

public abstract class AbstractBumper : MonoBehaviour
{
    private Player player;
    [SerializeField] private float bumpProjection;

    private void Start()
    {
        player = Player._instance;    
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (player.currentState != player.standingState && Player._instance.hp > 0)
        {
            player.SpinJump(bumpProjection);
        }
    }
}
