using UnityEngine;

public class WindZone : MonoBehaviour
{
    private enum WindDirection
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    [SerializeField] private WindDirection windDir = WindDirection.UP;
    [SerializeField] private float windPower = 1.0f;
    private Rigidbody body;
    private Vector2 projectionDir;

    private void Start()
    {
        body = Player._instance.GetComponent<Rigidbody>();

        switch (windDir)
        {
            case WindDirection.UP:
                projectionDir = new Vector2(body.velocity.x, body.velocity.y + windPower);
                break;
            case WindDirection.DOWN:
                projectionDir = new Vector2(body.velocity.x, body.velocity.y - windPower);
                break;
            case WindDirection.RIGHT:
                projectionDir = new Vector2(body.velocity.x + windPower, body.velocity.y);
                break;
            case WindDirection.LEFT:
                projectionDir = new Vector2(body.velocity.x - windPower, body.velocity.y);
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            if (Player._instance.currentState != Player._instance.glidingState)
                body.velocity = projectionDir;
            if (Player._instance.currentState == Player._instance.hoveringState)
                Player._instance.hoveringState.StopHovering(ref Player._instance);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Player._instance.glidingFallingSpeed = windPower;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Player._instance.glidingFallingSpeed = -1;
        }
    }
}
