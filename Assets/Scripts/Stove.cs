using UnityEngine;
using UnityEngine.UI;

public class Stove : MonoBehaviour
{
    public float cookingTime;
    private float interactionDistance = 2.5f;
    public GameObject burgerPrefab;
    public Transform burgerSpawnPoint;
    public Player player;
    public Image loadingIndicatorUI;
    public Image loadingIndicatorBack;
    private bool isPlayerNear = false;
    private bool isCooking = false;
    private float currentCookingTime = 0f;
    private bool canStartCooking = true;

    public Transform CookingZone;

    // Делегаты и события для готовки
    public delegate void CookingEventHandler();
    public static event CookingEventHandler OnCookingStarted;
    public static event CookingEventHandler OnCookingFinished;
    public static event CookingEventHandler OnCookingReset;

    private void Start()
    {
        if (loadingIndicatorUI != null)
        {
            loadingIndicatorUI.fillAmount = 0f;
            loadingIndicatorBack.fillAmount = 0f;
        }
    }

    private void Update()
    {
        if (isCooking && player.equippedBurger == null)
        {
            currentCookingTime += Time.deltaTime;

            if (loadingIndicatorUI != null)
            {
                loadingIndicatorUI.fillAmount = currentCookingTime / cookingTime;
            }

            if (currentCookingTime >= cookingTime)
            {
                FinishCooking();
            }
        }

        float distance = Vector3.Distance(CookingZone.position, player.transform.position);
        bool wasPlayerNear = isPlayerNear;
        isPlayerNear = (distance <= interactionDistance);

        if (isPlayerNear && !wasPlayerNear)
        {
            OnPlayerEnter();
        }
        else if (!isPlayerNear && wasPlayerNear)
        {
            OnPlayerExit();
        }
    }

    private void OnPlayerEnter()
    {
        if (canStartCooking && !isCooking && !player.equippedBurger)
        {
            StartCooking();
            Debug.Log("Игрок начал готовить");
        }
    }

    private void OnPlayerExit()
    {
        if (isCooking)
        {
            ResetCooking();
        }
    }

    private void StartCooking()
    {
        isCooking = true;
        canStartCooking = false;
        currentCookingTime = 0f;
        loadingIndicatorBack.fillAmount = 1f;

        OnCookingStarted?.Invoke();
    }

    private void ResetCooking()
    {
        isCooking = false;
        currentCookingTime = 0f;
        loadingIndicatorUI.fillAmount = 0f;
        loadingIndicatorBack.fillAmount = 0f;
        canStartCooking = true;

        OnCookingReset?.Invoke();
    }

    private void FinishCooking()
    {
        isCooking = false;
        Debug.Log("Игрок приготовил");

        loadingIndicatorUI.fillAmount = 0f;
        loadingIndicatorBack.fillAmount = 0f;

        OnCookingFinished?.Invoke();

        Invoke("AllowCookingStart", 0.5f);
    }

    private void AllowCookingStart()
    {
        canStartCooking = true;
    }
}
