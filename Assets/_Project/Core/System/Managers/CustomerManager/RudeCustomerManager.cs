using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class RudeCustomerManager : MonoBehaviour
{
    [SerializeField] private int customersInPool = 25 ;


    [SerializeField] private RudeCustomer RudeCustomerPrefab;
    private GameObject customerSpawnPoint;
    private GameObject rudeCustomerRoot;
    private List<RudeCustomer> rudeCustomersList = new();
    private List<Table> tablesList = new();
    private GameplayObjectList gameplayObjectList;
    void Start()
    {
        gameplayObjectList = GameplayObjectList.Instance;

        rudeCustomerRoot = GameObject.Find("_NPCs/RudeCustomers");
        customerSpawnPoint = gameplayObjectList.customerSpawnPoint.gameObject;
        tablesList = gameplayObjectList.tablesList;

        if (rudeCustomerRoot == null)
        {
            Debug.Log("Rude Customer Root gameObject doesn't exist!");
            return;
        }

        // Create new rude customers for pool
        for (int i = 0; i < customersInPool; i++)
        {
            RudeCustomer newCustomer = Instantiate(RudeCustomerPrefab);
            newCustomer.transform.parent = GameObject.Find("_NPCs/RudeCustomers").transform;
            newCustomer.Init(this);
            newCustomer.gameObject.SetActive(false);
            rudeCustomersList.Add(newCustomer);
        }

        tablesList.AddRange(GameObject.Find("_GameplayObjects/Tables").GetComponentsInChildren<Table>(includeInactive: true));

        Debug.Log("Start with " + rudeCustomersList.Count + " rude customers");
    }
    public void StartDay()
    {
        StartCoroutine(CustomerSpawnLoop());
    }
    public void EndDay()
    {
        StopCoroutine(CustomerSpawnLoop());
    }
    IEnumerator CustomerSpawnLoop()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(10f, 16f);
            yield return new WaitForSeconds(delay);

            TrySpawnCustomer();
        }
    }
    public void TrySpawnCustomer(Vector3 spawnPosition)
    {
        foreach (RudeCustomer currentCustomer in rudeCustomersList)
        {
            if (currentCustomer.gameObject.activeSelf == false) // and also need to check if there are a table avaliable at the momeny
            {
                currentCustomer.gameObject.SetActive(true);
                currentCustomer.transform.position =spawnPosition + new Vector3(0 , 2, 0);

                break;
            }
        }
    }
    public void TrySpawnCustomer()
    {
        TrySpawnCustomer(customerSpawnPoint.transform.position);
    }
    public void ReturnToPool(RudeCustomer customer)
    {
        if (rudeCustomersList.Contains(customer))
        {
            customer.gameObject.transform.position = customerSpawnPoint.transform.position;
            customer.ResetStateSelf();
            customer.gameObject.SetActive(false);
        }
    }
}
