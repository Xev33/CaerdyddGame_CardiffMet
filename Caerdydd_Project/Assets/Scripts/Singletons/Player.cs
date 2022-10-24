using UnityEngine;
using XDScript;

/// <summary>
/// Fake Player fo testing
/// </summary>
public class Player : Singleton<Player>, ISubject
{
    private InputHandler inputHandler;
    private Subject subject = new Subject();
    private Rigidbody body;
    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;
    private float lastDirection = 1f;
    [SerializeField] float jumpVelocity = 10.0f;
    [SerializeField] private float fallingGlidingSpeedDivider;
    [SerializeField] private float hoveringJumpVelocityDivider;
    [SerializeField] private GameObject dragonMesh;
    public float hoveringSpeedDivider;
    private float distToGround = 1.0f;

    public IPlayerState currentState;
    [HideInInspector] public State_Standing standingState;
    [HideInInspector] public State_Jumping jumpState;
    [HideInInspector] public State_Gliding glidingState;
    [HideInInspector] public State_Hovering hoveringState;
    [HideInInspector] public CameraZone cameraZone;

    #region Unity functions
    void Start()
    {
        body = GetComponent<Rigidbody>();
        inputHandler = this.gameObject.AddComponent<InputHandler>();
        standingState = new State_Standing();
        jumpState = new State_Jumping();
        glidingState = new State_Gliding();
        hoveringState = new State_Hovering();
        currentState = standingState;

        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    void Update()
    {
        currentState.HandleInput(this);
        currentState.StateUpdate(this);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (currentState == glidingState)
            currentState = jumpState;
    }

    #endregion

    #region Fake Inpute
    public void BackInMenu()
    {
        Debug.Log("Player move back in menu");
    }

    public void Move()
    {
        float direction = Input.GetAxis("Horizontal");

        if ((direction < -0.3f || direction > 0.3f) && currentState != glidingState)
            lastDirection = direction;
        body.velocity = new Vector2(direction * speed, body.velocity.y);
        RotateMesh();
    }

    public void Jump()
    {
            body.velocity = new Vector2(body.velocity.x, jumpVelocity);
    }

    public void Glide()
    {
        if (lastDirection > 0)
            body.velocity = new Vector2(1.0f * speed, (body.velocity.y / fallingGlidingSpeedDivider));
        else if (lastDirection < 0)
            body.velocity = new Vector2(-1.0f * speed, (body.velocity.y / fallingGlidingSpeedDivider));
        RotateMesh();
    }

    public void Hover()
    {
        body.velocity = new Vector2(body.velocity.x, (jumpVelocity / hoveringJumpVelocityDivider));
    }

    #endregion

    public bool IsGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit, (distToGround + 0.0f)))
        {
            if (hit.collider.isTrigger)
                return false;
            else
                return true;
        }
        else
            return false;
    }

    private void RotateMesh()
    {
        Quaternion toRotation;
        if (lastDirection < 0)
            toRotation = Quaternion.LookRotation(new Vector3(-1, 0f, 0f));
        else
            toRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
        dragonMesh.transform.localRotation = Quaternion.RotateTowards(dragonMesh.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    #region Subject initialization
    // How to use it =======> NotifyObservers(this.gameObject, E_Event.YOUR_EVENT);
    public void NotifyObservers(GameObject entity, E_Event eventToTrigger)
    {
        subject.NotifyObservers(entity, eventToTrigger);
    }

    public void AddObserver(ref AbstractObserver observer)
    {
        subject.AddObserver(ref observer);
    }

    public void RemoveObserver(ref AbstractObserver observer)
    {
        subject.RemoveObserver(ref observer);
    }
    #endregion
}