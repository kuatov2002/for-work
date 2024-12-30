using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Kassa : MonoBehaviour
{
    public Player player;
    public int clientCount = 0;
    public Transform Playzone;

    private float interactionDistance = 3f;
    private bool isPlayerNear = false;

    private bool isPlaying = false;
    private float currentPlayingTime = 0f;
    public float playingTime = 3f;
    public float exitDistance = 3f;
    public Transform exitPoint;
    public Image loadingIndicatorUI;
    public Image loadingIndicatorBack;

    public GameObject humanPrefab;
    public Transform spawnPoint;
    public Transform kassaPoint;
    public Transform tablePoint;
    public float spawnIntervalMin = 10f;
    public float spawnIntervalMax = 15f;
    public float offsetDistance = 1.5f;

    private List<GameObject> humansAtKassa = new List<GameObject>();
    public List<GameObject> humansAtTable = new List<GameObject>();
    private float nextSpawnTime = 0f;
    private List<bool> humansCounted = new List<bool>();

    // Делегаты и события для обслуживания
    public delegate void ServiceEventHandler();
    public static event ServiceEventHandler OnServiceStarted;
    public static event ServiceEventHandler OnServiceFinished;
    public static event ServiceEventHandler OnServiceReset;

    void Start()
    {
        loadingIndicatorUI.fillAmount = 0f;
        loadingIndicatorBack.fillAmount = 0f;

        nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    void Update()
    {
        float distance = Vector3.Distance(Playzone.position, player.transform.position);
        isPlayerNear = (distance <= interactionDistance);

        if (isPlayerNear && !isPlaying && clientCount > 0)
        {
            StartPlay();
            Debug.Log("Игрок начал заказ");
        }
        else if (!isPlayerNear && isPlaying)
        {
            ResetPlaying();
        }

        if (isPlaying)
        {
            currentPlayingTime += Time.deltaTime;

            loadingIndicatorUI.fillAmount = currentPlayingTime / playingTime;

            if (currentPlayingTime >= playingTime)
            {
                FinishPlaying();
            }
        }

        if (Time.time >= nextSpawnTime && humansAtKassa.Count < 6)
        {
            SpawnHuman();
            nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
        }
        CheckHumanArrivedAtKassa();
    }

    void CheckHumanArrivedAtKassa()
    {
        if (humansAtKassa.Count > 0)
        {
            GameObject firstHuman = humansAtKassa[0];
            float distanceToKassa = Vector3.Distance(firstHuman.transform.position, kassaPoint.position);

            if (distanceToKassa < 4f && !humansCounted[0])
            {
                clientCount++;
                humansCounted[0] = true;
                Debug.Log($"Клиент прибыл к кассе. Общее количество клиентов: {clientCount}");
            }
        }
    }

    void SpawnHuman()
    {
        GameObject newHuman = Instantiate(humanPrefab, spawnPoint.position + Vector3.left, Quaternion.identity);
        newHuman.SetActive(true);
        humansAtKassa.Add(newHuman);
        humansCounted.Add(false);

        Vector3 destination = kassaPoint.position + (humansAtKassa.Count) * offsetDistance * Vector3.forward + 0.3f * Vector3.right;

        NavMeshAgent agent = newHuman.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(destination);
        }
    }

    private void StartPlay()
    {
        isPlaying = true;
        currentPlayingTime = 0f;
        loadingIndicatorBack.fillAmount = 1f;

        OnServiceStarted?.Invoke();
    }

    private void ResetPlaying()
    {
        Debug.Log("Игрок прервал заказ");
        isPlaying = false;
        currentPlayingTime = 0f;

        if (loadingIndicatorUI != null)
        {
            loadingIndicatorUI.fillAmount = 0f;
            loadingIndicatorBack.fillAmount = 0f;
        }

        OnServiceReset?.Invoke();
    }

    private void FinishPlaying()
    {
        clientCount--;
        Debug.Log("Игрок закончил заказ");
        isPlaying = false;

        loadingIndicatorUI.fillAmount = 0f;
        loadingIndicatorBack.fillAmount = 0f;

        if (humansAtKassa.Count > 0)
        {
            MoveFirstHumanToTable();
        }

        OnServiceFinished?.Invoke();
    }

    void MoveFirstHumanToTable()
    {
        player.AddMoney(2.5f);
        GameObject firstHuman = humansAtKassa[0];

        humansAtKassa.RemoveAt(0);
        humansCounted.RemoveAt(0);
        humansAtTable.Add(firstHuman);

        NavMeshAgent agent = firstHuman.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            Vector3 destination = tablePoint.position + (humansAtTable.Count) * offsetDistance * Vector3.forward;
            agent.SetDestination(destination);
        }

        MoveAllHumansForwardAtKassa();
    }

    void MoveAllHumansForwardAtKassa()
    {
        for (int i = 0; i < humansAtKassa.Count; i++)
        {
            GameObject human = humansAtKassa[i];
            Vector3 newDestination = kassaPoint.position + (i+1) * offsetDistance * Vector3.forward + 0.3f * Vector3.right;
            NavMeshAgent agent = human.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(newDestination);
            }
        }
    }

    void MoveAllHumansForwardAtTable()
    {
        for (int i = 0; i < humansAtTable.Count; i++)
        {
            GameObject human = humansAtTable[i];
            Vector3 newDestination = tablePoint.position + i * offsetDistance * Vector3.forward;
            NavMeshAgent agent = human.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(newDestination);
            }
        }
    }

    public void CustomerLeave()
    {

            GameObject leavingHuman = humansAtTable[0];
            humansAtTable.RemoveAt(0);

            NavMeshAgent agent = leavingHuman.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(exitPoint.position + Vector3.right);
            }

            StartCoroutine(DestroyHumanWhenFarEnough(leavingHuman));

            MoveAllHumansForwardAtTable();
        
    }

    private System.Collections.IEnumerator DestroyHumanWhenFarEnough(GameObject human)
    {
        while (human != null && Vector3.Distance(human.transform.position, tablePoint.position) < exitDistance)
        {
            yield return null;
        }

        if (human != null)
        {
            Destroy(human);
        }
    }
}
