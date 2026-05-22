using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RudeCustomer : MonoBehaviour
{
        [Header("Repath update")]
    // [SerializeField] private float updateRate = 0.25f;
    [Header("Detection")]
    [SerializeField] private float detectionRange = 6f;

    [Header("Roaming")]
    // [SerializeField] private float minRoamDistance = 2f;
    // [SerializeField] private float roamRadius = 6f;
    //[SerializeField] private float minWaitTime = 0.7f;
    //[SerializeField] private float maxWaitTime = 1.8f;
    [SerializeField] private float minNewTargetDistanceFromLast = 1.5f;
    Transform playerTransform;
    [SerializeField] private float targetUpdateTime;
    //private float nextRoamActionTime;
    private RudeCustomerStates currentRudeCustomerState;
    private List<Table> tablesList;
    // private List<FurnitureList> furnitureList; // breakable furnitures dont exist yet

    [Header("Combat")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float fellDownDuration = 5f;
    [SerializeField] private float stunDuration = 0.4f;
    [Header("Throw-out")]
    [SerializeField] private float minLaunchUpSpeed = 5f;
    private Transform exitPoint;
    private Collider exitCollider;
    private float currentHP;
    private bool isStunned;
    private bool isCarried;
    private Coroutine stunRoutine;
    private Collider[] bodyColliders;

    private RudeCustomerMovement rudeCustomerMovement;
    private GameplayObjectList gameplayObjectList;
    private Coroutine stateRoutine;
    private RudeCustomerManager boundManager;

    void Awake()
    {
        rudeCustomerMovement = GetComponent<RudeCustomerMovement>();
        bodyColliders = GetComponentsInChildren<Collider>(true); // all of them, so the throw raycast never hits ourselves
    }

    private void SetCollidersEnabled(bool value)
    {
        foreach (Collider c in bodyColliders)
            if (c != null) c.enabled = value;
    }
    void Start()
    {
        gameplayObjectList = GameplayObjectList.Instance; // import singletons
        playerTransform = PlayerLocator.PlayerTransform;

        tablesList = gameplayObjectList.tablesList;
        if (gameplayObjectList.exitPoint != null)
        {
            exitPoint = gameplayObjectList.exitPoint.transform;
            exitCollider = gameplayObjectList.exitPoint.GetComponentInChildren<Collider>();
        }

        targetUpdateTime = 10f;
        currentHP = maxHP;

        ChangeState(RudeCustomerStates.Roaming);
    }

    void Update()
    {
        if (playerTransform == null) return;
        if (isCarried || currentRudeCustomerState == RudeCustomerStates.FellDown || isStunned) return; // locked: carried, knocked down, or staggered

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        RudeCustomerStates desiredState = distance <= detectionRange ? RudeCustomerStates.Chasing : RudeCustomerStates.Roaming;
        ChangeState(desiredState);
    }
    void ChangeState(RudeCustomerStates newState, bool forceRestart = false)
    {
        if (!forceRestart && currentRudeCustomerState == newState)
            return;

        rudeCustomerMovement.Stop();

        if (stateRoutine != null)
            StopCoroutine(stateRoutine);

        currentRudeCustomerState = newState;

        switch (newState)
        {
            case RudeCustomerStates.Chasing:
                stateRoutine = StartCoroutine(ChaseState());
                break;

            case RudeCustomerStates.Roaming:
                stateRoutine = StartCoroutine(RoamState());
                break;

            case RudeCustomerStates.FellDown:
                stateRoutine = StartCoroutine(FellDownState());
                break;
        }
    }
    private RudeCustomerTarget PickRandomTarget()
    {
        int roamMode = Random.Range(1,3); // 1 = annoy other customers, 2 = destroy furnitures -> this random will choose either 1 or 2 yu la
        roamMode = 1; // no furniture added yet so i'll set to 1 always first

        if (roamMode == 1)
        {
            RudeCustomerTarget returnObject = PickRandomTable();
            if (returnObject != null)
                return returnObject; 
        }
        // else if (roamMode == 2)
        // {
        //     Object returnObject = PickRandomFurniture();
        //     if (returnObject != null)
        //         return returnObject; 
        // }

        return null; // nothing to pick
    }
    IEnumerator ChaseState()
    {
        while (true)
        {
            rudeCustomerMovement.MoveTo(playerTransform.position);
            yield return null;
        }
    }
    IEnumerator RoamState()
    {
        while (true)
        {
            RudeCustomerTarget target = PickRandomTarget();

            if (target is Table table && table.occupier != null)
            {
                yield return StartCoroutine(AnnoyAtTable(table));
            }
            // else if (target is Furniture && furnitureIsNotBrokenOrSmth)
            // {
                
            // }
            else
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            yield return new WaitForSeconds(targetUpdateTime);
        }
    }
    IEnumerator AnnoyAtTable(Table table)
    {
        rudeCustomerMovement.MoveTo(table.accessPointTransform.position);
        yield return new WaitUntil(() => rudeCustomerMovement.HasArrived());

        while (table.occupier != null)
        {
            GoodCustomer target = table.occupier;

            if (target != null)
            {
                if (Random.Range(0, 3) == 0) // 1/3 chance they will change table
                    ChangeState(RudeCustomerStates.Roaming, true);
                else
                    target.ApplyAnnoyance(2);
            }

            yield return new WaitForSeconds(1f);
        }

        // Table is now empty
        ChangeState(RudeCustomerStates.Roaming, true);
    }
    private Table PickRandomTable()
    {
        int tableCount = tablesList.Count;
        Table currentTable = null;

        for (int i = 0; i < 10; i++) // try to find avaliable 10 times, if can't find it'll give up and return null
        {
            int randomTablePick = Random.Range(0, tableCount);
            currentTable = tablesList[randomTablePick];
            
            if (currentTable.occupationState == TableStates.Taken)
                return currentTable;
        }

        return null;
    }

    public void TakeHit(float damage, Vector3 knockbackDir, float knockbackForce)
    {
        if (currentRudeCustomerState == RudeCustomerStates.FellDown) return; // already down, ignore punches

        currentHP -= damage;
        rudeCustomerMovement.Knockback(knockbackDir, knockbackForce);

        if (currentHP <= 0)
        {
            if (stunRoutine != null) StopCoroutine(stunRoutine);
            isStunned = false;
            ChangeState(RudeCustomerStates.FellDown, true);
        }
        else
        {
            if (stunRoutine != null) StopCoroutine(stunRoutine);
            stunRoutine = StartCoroutine(StunBriefly());
        }
    }

    public bool IsDown => currentRudeCustomerState == RudeCustomerStates.FellDown;
    public float HPNormalized => maxHP > 0f ? Mathf.Clamp01(currentHP / maxHP) : 0f;

    private IEnumerator StunBriefly()
    {
        isStunned = true;
        rudeCustomerMovement.Stop();
        if (stateRoutine != null) StopCoroutine(stateRoutine);

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        ChangeState(currentRudeCustomerState, true); // resume whatever state we were in
    }

    private IEnumerator FellDownState()
    {
        rudeCustomerMovement.Stop();

        yield return new WaitForSeconds(fellDownDuration);

        currentHP = maxHP;
        ChangeState(RudeCustomerStates.Roaming, true);
    }

    public bool TryGetCarried()
    {
        if (currentRudeCustomerState != RudeCustomerStates.FellDown || isCarried) return false; // only downed customers can be picked up

        isCarried = true;
        if (stateRoutine != null) StopCoroutine(stateRoutine); // cancel the get-back-up countdown
        rudeCustomerMovement.SetAgentEnabled(false);
        SetCollidersEnabled(false);
        return true;
    }

    public void GetThrownOut(Vector3 direction, float force)
    {
        if (!isCarried) return;

        isCarried = false;
        StartCoroutine(ThrowArc(direction, force));
    }

    private IEnumerator ThrowArc(Vector3 direction, float force)
    {
        Vector3 dir = direction.normalized;
        Vector3 velocity = new Vector3(dir.x, 0f, dir.z) * force                    // horizontal travel from aim
                         + Vector3.up * (Mathf.Max(dir.y * force, 0f) + minLaunchUpSpeed); // always pop up first

        while (true)
        {
            // flew into the exit zone's volume -> kicked out for good
            if (exitCollider != null && exitCollider.bounds.Contains(transform.position))
            {
                KickedOut();
                yield break;
            }

            Vector3 nextPos = transform.position + velocity * Time.deltaTime;

            // sweep to the next position so we collide with the world instead of phasing through it
            if (Physics.Linecast(transform.position, nextPos, out RaycastHit hit, ~0, QueryTriggerInteraction.Ignore))
            {
                if (IsExit(hit.collider))
                {
                    KickedOut();                       // flung straight into the exit -> gone for good
                    yield break;
                }

                if (velocity.y <= 0f && hit.normal.y >= 0.5f) // hit the floor while falling -> land
                {
                    RecoverFromThrow(hit.point);       // hit the ground FIRST -> now they get up and walk
                    yield break;
                }

                // hit a wall -> stop against it and slide/fall instead of going through
                transform.position = hit.point + hit.normal * 0.1f;
                velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
            }
            else
            {
                transform.position = nextPos;
            }

            velocity += Physics.gravity * Time.deltaTime;
            transform.Rotate(Vector3.right * 540f * Time.deltaTime, Space.Self); // tumble through the air

            // guard: fell out of the world -> recover instead of falling forever
            if (transform.position.y < -20f)
            {
                RecoverFromThrow(transform.position);
                yield break;
            }

            yield return null;
        }
    }

    private bool IsExit(Collider col)
    {
        if (exitPoint == null || col == null) return false;
        return col.transform == exitPoint || col.transform.IsChildOf(exitPoint);
    }

    private void KickedOut()
    {
        if (boundManager != null)
            boundManager.ReturnToPool(this); // pooled customer -> back to the pool
        else
            gameObject.SetActive(false); // not pooled (e.g. placed in the scene) -> just remove it
    }

    private void RecoverFromThrow(Vector3 landingPosition)
    {
        SetCollidersEnabled(true);
        rudeCustomerMovement.SetAgentEnabled(true);

        bool onMesh = rudeCustomerMovement.WarpToNearestNavMesh(landingPosition, 10f);
        if (!onMesh && gameplayObjectList != null && gameplayObjectList.customerSpawnPoint != null)
            rudeCustomerMovement.WarpToNearestNavMesh(gameplayObjectList.customerSpawnPoint.transform.position, 10f);

        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f); // stand upright after tumbling

        currentHP = maxHP; // full HP again -> you have to knock them down once more
        isStunned = false;
        isCarried = false;

        ChangeState(RudeCustomerStates.Roaming, true);
    }

    public void ResetStateSelf()
    {
        currentRudeCustomerState = RudeCustomerStates.Chasing;
        currentHP = maxHP;
        isStunned = false;
        isCarried = false;
        rudeCustomerMovement.SetAgentEnabled(true);
        SetCollidersEnabled(true);
    }
    public void Init(RudeCustomerManager manager)
    {
        if (boundManager == null || boundManager != manager)
            boundManager = manager;
        ResetStateSelf();
    }
}
