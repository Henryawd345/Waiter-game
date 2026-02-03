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

    private RudeCustomerMovement rudeCustomerMovement;
    private GameplayObjectList gameplayObjectList;
    private Coroutine stateRoutine;
    private RudeCustomerManager boundManager;

    void Awake()
    {
        rudeCustomerMovement = GetComponent<RudeCustomerMovement>();
    }
    void Start()
    {
        gameplayObjectList = GameplayObjectList.Instance; // import singletons
        playerTransform = PlayerLocator.PlayerTransform;

        tablesList = gameplayObjectList.tablesList;

        targetUpdateTime = 10f;

        ChangeState(RudeCustomerStates.Roaming);
    }

    void Update()
    {
        if (playerTransform == null) return;

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

    public void ResetStateSelf()
    {
        currentRudeCustomerState = RudeCustomerStates.Chasing;
    }
    public void Init(RudeCustomerManager manager)
    {
        if (boundManager == null || boundManager != manager)
            boundManager = manager;
        ResetStateSelf();
    }
}
