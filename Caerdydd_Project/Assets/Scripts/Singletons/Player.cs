using UnityEngine;
using XDScript;

public enum AnimationToLaunch
{
    ANIM_IDLE,
    ANIM_JUMP,
    ANIM_GLIDE,
    ANIM_HOVER,
    ANIM_TAKE_HIT,
    ANIM_WALK,
    ANIM_GROUNDED,
    ANIM_MOVE
}

/// <summary>
/// Fake Player fo testing
/// </summary>
public class Player : Singleton<Player>, ISubject
{
    #region Variables

    private InputHandler inputHandler;
    private Subject subject = new Subject();
    private Rigidbody body;
    [SerializeField] private Animator anim;
    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;
    private float lastDirection = 1f;
    private float sleepTimer = 0f;
    private float direction = 0f;
    private int idleState = 0;
    private float timeBeforIdle = 5f;
    [SerializeField] private GameObject dragonMesh;
    [SerializeField] float jumpVelocity = 10.0f;
    [SerializeField] private float fallingGlidingSpeedDivider;
    [SerializeField] private float hoveringJumpVelocityDivider;
    public float hoveringSpeedDivider;
    private float distToGround = 1.0f;

    public IPlayerState currentState;
    [HideInInspector] public State_Standing standingState;
    [HideInInspector] public State_Jumping jumpState;
    [HideInInspector] public State_Gliding glidingState;
    [HideInInspector] public State_Hovering hoveringState;
    [HideInInspector] public CameraZone cameraZone;

    #endregion

    #region Unity functions
    void Start()
    {
        body = GetComponent<Rigidbody>();
        anim = dragonMesh.gameObject.GetComponent<Animator>();
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

        //Part that trigger the sleeping animations
        sleepTimer += Time.deltaTime;
        if (sleepTimer >= timeBeforIdle && idleState == 0)
        {
            idleState = 1;
            LaunchGivenAnimation(AnimationToLaunch.ANIM_IDLE);
        } else if (sleepTimer >= (timeBeforIdle * 2) && idleState == 1)
        {
            idleState = 2;
            LaunchGivenAnimation(AnimationToLaunch.ANIM_IDLE);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (currentState == glidingState)
        {
            currentState = jumpState;
            LaunchGivenAnimation(AnimationToLaunch.ANIM_HOVER);
        }
    }

    #endregion

    #region Input

    public void Move()
    {
        direction = Input.GetAxis("Horizontal");

        if ((direction < -0.3f || direction > 0.3f) && currentState != glidingState)
        {
            LaunchGivenAnimation(AnimationToLaunch.ANIM_IDLE);
            idleState = 0;
            sleepTimer = 0f;
            lastDirection = direction;
        }
        body.velocity = new Vector2(direction * speed, body.velocity.y);
        if (currentState == standingState)
        {
            LaunchGivenAnimation(AnimationToLaunch.ANIM_IDLE);
            LaunchGivenAnimation(AnimationToLaunch.ANIM_GROUNDED);
            LaunchGivenAnimation(AnimationToLaunch.ANIM_WALK);
        }
        else if (currentState == jumpState)
            LaunchGivenAnimation(AnimationToLaunch.ANIM_JUMP);
        RotateMesh();
    }

    public void Jump()
    {
        LaunchGivenAnimation(AnimationToLaunch.ANIM_JUMP);
        body.velocity = new Vector2(body.velocity.x, jumpVelocity);
        sleepTimer = 0f;
        idleState = 0;
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
        LaunchGivenAnimation(AnimationToLaunch.ANIM_HOVER);
        body.velocity = new Vector2(body.velocity.x, (jumpVelocity / hoveringJumpVelocityDivider));
    }

    #endregion

    #region Utils

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

    public void LaunchGivenAnimation(AnimationToLaunch animToLaunch)
    {
        switch (animToLaunch)
        {
            case AnimationToLaunch.ANIM_IDLE:
                anim.SetInteger("Idle", idleState);
                break;
            case AnimationToLaunch.ANIM_JUMP:
                anim.SetTrigger("JumpTrigger");
                break;
            case AnimationToLaunch.ANIM_GLIDE:
                anim.SetTrigger("GlideTrigger");
                break;
            case AnimationToLaunch.ANIM_HOVER:
                anim.SetTrigger("LandTrigger");
                break;
            case AnimationToLaunch.ANIM_TAKE_HIT:
                Debug.Log("YOU TAKE A HIT! ");
                break;
            case AnimationToLaunch.ANIM_WALK:
                anim.SetFloat("Move", direction);
                break;
            case AnimationToLaunch.ANIM_GROUNDED:
                anim.SetBool("IsGrounded", IsGrounded());
                break;
            case AnimationToLaunch.ANIM_MOVE:
                anim.SetFloat("Move", direction);
                break;
            default:
                break;
        }
    }

    #endregion

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