using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GoodCustomerManager : MonoBehaviour
{
     [SerializeField] private int customersInPool = 25 ;


    [SerializeField] private GoodCustomer goodCustomerPrefab;
    private GameObject customerSpawnPoint;
    private List<GoodCustomer> goodCustomersList = new();
    private List<Table> tablesList = new();

    void Start()
    {
        GameObject goodCustomerRoot = GameObject.Find("_NPCs/GoodCustomers");
        GameObject tableRoot = GameObject.Find("_GameplayObjects/Tables");
        GameObject customerSpawnPoint = GameObject.Find("_EventLocations/CustomerSpawnPoint");

        if (goodCustomerRoot == null || tableRoot == null)
        {
            Debug.Log("No root for GoodCustomer or TableRoot found!");
            return;
        }
        if (customerSpawnPoint == null)
        {
            Debug.Log("No customer spawn point found!");
            return;
        }

        // Create new customers for pool
        for (int i = 0; i < customersInPool; i++)
        {
            GoodCustomer newCustomer = Instantiate(goodCustomerPrefab);
            newCustomer.transform.parent = GameObject.Find("_NPCs/GoodCustomers").transform;
            newCustomer.gameObject.SetActive(false);
            newCustomer.RegisterManager(this);
            goodCustomersList.Add(newCustomer);
        }

        tablesList.AddRange(GameObject.Find("_GameplayObjects/Tables").GetComponentsInChildren<Table>(includeInactive: true));

        Debug.Log("Start with\n" + goodCustomersList.Count + " customers and " + tablesList.Count + " tables");
    }
    public void StartDay()
    {
        // customer spawn loop
        StartCoroutine(CustomerSpawnLoop());
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
    IEnumerator CustomerSpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(5f, 7f);
            yield return new WaitForSeconds(delay);

            TrySpawnCustomer();
        }
    }
}
