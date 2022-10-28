using UnityEngine;
using System.Collections;
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
    ANIM_MOVE,
    ANIM_DEATH,
    ANIM_SPIN
}

/// <summary>
/// Fake Player fo testing
/// </summary>
public class Player : Singleton<Player>, ISubject
{
    #region Variables

    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;
    public float hoveringSpeedDivider;
    public GameObject selfCamAnchor;
    public bool isInvicible = false;
    public bool isOnMovingPlatform = false;
    [HideInInspector] public float maxSpeed;
    public float spinSpeed = 1700.0f; // The rotation to add for the spin move jump
    private Subject subject = new Subject();
    private InputHandler inputHandler;
    private Rigidbody body;
    private Animator anim;
    private bool shouldNotMove = false;
    [HideInInspector] public bool isSpinning = false;

    public int hp = 2;
    private int idleState = 0;
    private float lastDirection = 1f;
    private float sleepTimer = 0f;
    private float direction = 0f;
    private float timeBeforIdle = 5f;
    private float distToGround = 1.0f;
    public int collectibleNbr = 0;

    [SerializeField] private GameObject dragonMesh;
    [SerializeField] private PlayerUI canvas;
    [SerializeField] private float jumpVelocity = 10.0f;
    [SerializeField] private float invincibilityTime = 5f;
    [SerializeField] private float fallingGlidingSpeed;
    [SerializeField] private float hoveringJumpVelocityDivider;

    public IPlayerState currentState;
    [HideInInspector] public State_Standing standingState;
    [HideInInspector] public State_Jumping jumpState;
    [HideInInspector] public State_Gliding glidingState;
    [HideInInspector] public State_Hovering hoveringState;
    [HideInInspector] public State_Disable disableState;
    [HideInInspector] public CameraZone cameraZone;

    #endregion

    #region Unity functions
    void Start()
    {
        maxSpeed = speed;
        body = GetComponent<Rigidbody>();
        anim = dragonMesh.gameObject.GetComponent<Animator>();
        inputHandler = this.gameObject.AddComponent<InputHandler>();
        standingState = new State_Standing();
        jumpState = new State_Jumping();
        glidingState = new State_Gliding();
        hoveringState = new State_Hovering();
        disableState = new State_Disable();
        currentState = standingState;
        collectibleNbr = PlayerPrefs.GetInt("collectibleNumber", 0);

        StartCoroutine(OpenUI());

        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    void Update()
    {
        if (hp <= 0)
            return;
        if (Input.GetKeyDown(KeyCode.H))
            TakeHit(1);
        if (Input.GetKeyDown(KeyCode.K))
            Heal();
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
        if (currentState == jumpState)
            anim.SetTrigger("JumpTrigger");
        else if (currentState == glidingState)
            anim.SetTrigger("GlideTrigger");
        else if (currentState == hoveringState)
            anim.SetTrigger("LandTrigger");



        //transform.rotation = Quaternion.FromToRotation(hit.normal, Vector3.down);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (currentState == glidingState)
        {
            currentState = jumpState;
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
        if (shouldNotMove)
            body.velocity = new Vector2(direction, 0f);
        else
            body.velocity = new Vector2(direction * speed, body.velocity.y);

        if (currentState == standingState)
        {
            LaunchGivenAnimation(AnimationToLaunch.ANIM_IDLE);
            LaunchGivenAnimation(AnimationToLaunch.ANIM_GROUNDED);
            LaunchGivenAnimation(AnimationToLaunch.ANIM_WALK);
        }
        RotateMesh();
    }

    public void Jump()
    {
        if (isSpinning == true)
            return;
        jumpState.timer = 0.0f;
        jumpState.canGlide = false;
        currentState = jumpState;
        body.velocity = new Vector2(body.velocity.x, jumpVelocity);
        sleepTimer = 0f;
        idleState = 0;
    }

    public void SpinJump(float bumpVelocity)
    {
        LaunchGivenAnimation(AnimationToLaunch.ANIM_SPIN);
        speed = maxSpeed;
        jumpState.timer = 0.0f;
        jumpState.canGlide = false;
        currentState = jumpState;
        body.velocity = new Vector2(body.velocity.x, bumpVelocity);
        sleepTimer = 0f;
        idleState = 0;
        if (isSpinning == true)
            StopCoroutine(SpinPlayer());
        StartCoroutine(SpinPlayer());
    }

    public void Glide()
    {
        if (isSpinning == true)
            return;
        if (lastDirection > 0)
            body.velocity = new Vector2(1.0f * speed, ((-1) * (fallingGlidingSpeed / 10)));
        else if (lastDirection < 0)
            body.velocity = new Vector2(-1.0f * speed, ((-1) * (fallingGlidingSpeed / 10)));
        RotateMesh();
    }

    public void Hover()
    {
        body.velocity = new Vector2(body.velocity.x, (jumpVelocity / hoveringJumpVelocityDivider));
    }

    #endregion

    #region Gameplay

    public void TakeHit(int damage)
    {
        if ((isInvicible == true && damage == 1) || hp <= 0)
            return;
        if (damage == 1 && isSpinning == true)
            return;
        Camera.main.gameObject.GetComponent<CameraShake>().TriggerShake(0.2f);
        isInvicible = true;
        if (hp == 2 && damage == 2)
        {
            hp -= damage;
            canvas.TakeDamage(1);
            canvas.TakeDamage(hp);

        } else
        {
            hp -= damage;
            canvas.TakeDamage(hp);
        }

        if (hp <= 0)
        {
            currentState = disableState;
            StartCoroutine(PlayerDies());
        }
        else
            StartCoroutine(GetInvicibleFrame());
    }

    IEnumerator PlayerDies()
    {
        body.velocity = new Vector2(0, body.velocity.y);
        selfCamAnchor.transform.parent = null;
        currentState = disableState;
        LaunchGivenAnimation(AnimationToLaunch.ANIM_DEATH);
        yield return new WaitForSeconds(2f);
        canvas.CloseBlackGround();
        yield return new WaitForSeconds(1f);
        NotifyObservers(this.gameObject, E_Event.PLAYER_DIES);
    }

    public void Heal()
    {
        if (hp == 1)
        {
            hp++;
            canvas.GetHealed(hp);
        }
    }

    public void CollectPetal()
    {
        collectibleNbr++;
        canvas.CollectPetal();
    }

    #endregion

    #region Utils

    IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(1f);
        canvas.GetHealed(1);
        canvas.GetHealed(2);
        canvas.UpdateUI();
    }

    IEnumerator GetInvicibleFrame()
    {
        float timer = 0f;
        float timerNotMove = 0f;
        float lastTime = 0.2f;
        SkinnedMeshRenderer mesh = dragonMesh.GetComponentInChildren<SkinnedMeshRenderer>();

        shouldNotMove = true;
        while (timer < invincibilityTime)
        {
            timer += Time.deltaTime;
            timerNotMove += Time.deltaTime;
            if (timerNotMove >= 0.5)
                shouldNotMove = false;

            if (timer > lastTime)
            {
                lastTime += 0.2f;
                mesh.enabled = !mesh.enabled;
            }
            yield return null;
        }
        mesh.enabled = true;
        isInvicible = false;
        yield return null;
    }

    public bool IsGrounded()
    {
        if (isOnMovingPlatform == true)
            return true;
        RaycastHit hit;
        Vector3 pos = this.gameObject.transform.position;

        if (Physics.Raycast(pos, Vector3.down, out hit, (distToGround)))
            if (hit.collider.isTrigger == false)
                return true;
        return false;
    }

    private void RotateMesh()
    {
        Quaternion toRotation;
        if (lastDirection < 0)
            toRotation = Quaternion.LookRotation(new Vector3(-0.5f, 0f, 0f));
        else
            toRotation = Quaternion.LookRotation(new Vector3(0.5f, 0f, 0f));
        dragonMesh.transform.localRotation = Quaternion.RotateTowards(dragonMesh.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator SpinPlayer()
    {
        isSpinning = true;
        float timer = 0.0f;
        float duration = 0.5f;
        float normalizedValue = 0.0f;
        float normalizedSpinSpeed = spinSpeed;
        Vector3 v1 = new Vector3(0, 0, 0);
        Vector3 v2 = new Vector3(0, 0, spinSpeed);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / duration; // We normalize our time for the lerp
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue); // Calcul for a smooth lerp

            dragonMesh.transform.Rotate(new Vector3(0, 0, normalizedSpinSpeed * Time.deltaTime));

            yield return null;
        }
        dragonMesh.transform.Rotate(v1);
        isSpinning = false;
        yield return null;
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
            case AnimationToLaunch.ANIM_DEATH:
                anim.SetTrigger("DeathTrigger");
                break;
            case AnimationToLaunch.ANIM_SPIN:
                anim.SetTrigger("SpinJump");
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