using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GoodCustomerManager : MonoBehaviour
{
     [SerializeField] private int customersInPool = 25 ;


    [SerializeField] private GoodCustomer goodCustomerPrefab;
    private GameObject customerSpawnPoint;
    private GameObject goodCustomerRoot;
    private List<GoodCustomer> goodCustomersList = new();
    private List<Table> tablesList = new();
    private GameplayObjectList gameplayObjectList;
    private GameState gameState;
    private GameManager gameManager;

    void Start()
    {
        gameplayObjectList = GameplayObjectList.Instance;
        gameState = GameState.Instance;

        goodCustomerRoot = GameObject.Find("_NPCs/GoodCustomers");
        customerSpawnPoint = gameplayObjectList.customerSpawnPoint.gameObject;
        tablesList = gameplayObjectList.tablesList;

        if (goodCustomerRoot == null)
        {
            Debug.Log("Good Customer Root gameObject doesn't exist!");
            return;
        }

        // Create new customers for pool
        for (int i = 0; i < customersInPool; i++)
        {
            GoodCustomer newCustomer = Instantiate(goodCustomerPrefab);
            newCustomer.transform.parent = GameObject.Find("_NPCs/GoodCustomers").transform;
            newCustomer.Init(this, customerSpawnPoint.transform.position);
            newCustomer.gameObject.SetActive(false);
            goodCustomersList.Add(newCustomer);
        }

        Debug.Log("Start with " + goodCustomersList.Count + " good customers");
    }
    public void StartDay()
    {
        // customer spawn loop
        StartCoroutine(CustomerSpawnLoop());
    }
    public void EndDay()
    {
        StopCoroutine(CustomerSpawnLoop());
        foreach (GoodCustomer currentCustomer in goodCustomersList)
        {
            currentCustomer.ResetStateSelf();
            ReturnToPool(currentCustomer);
        }
    }
    IEnumerator CustomerSpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(5f, 7f);
            yield return new WaitForSeconds(delay);

            TrySpawnCustomer();
        }
    }
    void TrySpawnCustomer()
    {
        if (HasTableAvaliable() == false) return;

        foreach (GoodCustomer currentCustomer in goodCustomersList)
        {
            if (currentCustomer.gameObject.activeSelf == false) // and also need to check if there are a table avaliable at the momeny
            {
                currentCustomer.gameObject.SetActive(true);
                currentCustomer.transform.position = customerSpawnPoint.transform.position + new Vector3(0 , 2, 0);
                currentCustomer.FindTable(tablesList);

                break;
            }
        }
    }
    public void ReturnToPool(GoodCustomer customer)
    {
        if (goodCustomersList.Contains(customer))
        {
            customer.gameObject.transform.position = customerSpawnPoint.transform.position;
            customer.ResetStateSelf();
            customer.gameObject.SetActive(false);
        }
    }
    private bool HasTableAvaliable()
    {
        foreach (Table currentTable in tablesList)
        {
            if (currentTable.occupationState == TableStates.Free)
                return true;
        }
        return false;
    }
    public void RegisterGameManager(GameManager manager)
    {
        gameManager = manager;
    }
    public void TrySpawnRudeCustomer(Vector3 position)
    {
        gameManager.SpawnRudeCustomer(position);
    }
}
