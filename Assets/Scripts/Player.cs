using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed = 720f;
    public Transform cameraTransform;
    public FixedJoystick joystick;

    private Vector3 movement;
    private Animator animator;
    private Rigidbody rb;
    private bool isWalking = false;

    public TextMeshProUGUI moneyTextMesh; // Переменная для хранения ссылки на TextMesh
    public float moneyAmount = 0; // Количество денег

    public GameObject equippedBurger;
    public bool canEquipBurger = false;
    public Transform burgerHoldPoint;

    public GameObject burgerPrefab;
    public delegate void MoneyChangedEventHandler(float newAmount);
    public static event MoneyChangedEventHandler OnMoneyChanged;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player!");
            this.enabled = false;  // Отключаем скрипт, если нет аниматора
            return;
        }
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody component not found on the player!");
        }

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
                Debug.Log("Main camera automatically assigned to player.");
            }
            else
            {
                Debug.LogError("No camera found in the scene. Please assign a camera to the player or add a camera tagged as 'MainCamera' to the scene.");
            }
        }

        // Подписка на события готовки
        Stove.OnCookingStarted += OnCookingStarted;
        Stove.OnCookingFinished += OnCookingFinished;
        Stove.OnCookingReset += OnCookingReset;

        // Подписка на события обслуживания
        Kassa.OnServiceStarted += OnServiceStarted;
        Kassa.OnServiceFinished += OnServiceFinished;
        Kassa.OnServiceReset += OnServiceReset;
    }
    
    void Update()
    {
        if (cameraTransform == null || animator == null)
        {
            return;
        }

        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        movement = (forward * moveVertical + right * moveHorizontal).normalized;

        bool newIsWalking = movement.magnitude > 0;

        if (newIsWalking != isWalking)
        {
            isWalking = newIsWalking;
            animator.SetBool("IsWalking", isWalking);
        }
    }

    void FixedUpdate()
    {
        if (cameraTransform == null || rb == null) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (movement != Vector3.zero)
        {
            Vector3 adjustedDirection = Quaternion.Euler(0, -65, 0) * movement;

            Quaternion toRotation = Quaternion.LookRotation(adjustedDirection, Vector3.up);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void EquipBurger(GameObject burger)
    {
        if (!canEquipBurger)
        {
            return;
        }

        Debug.Log("Одеваем бургер");

        if (equippedBurger != null)
        {
            Destroy(equippedBurger);
        }

        equippedBurger = burger;

        if (burgerHoldPoint != null)
        {
            burger.transform.SetParent(burgerHoldPoint);
            burger.transform.localPosition = Vector3.zero;
            burger.transform.localRotation = Quaternion.Euler(-70,0,0);
        }
        else
        {
            Debug.LogWarning("Burger hold point is not set on the player.");
        }

        canEquipBurger = false;
        animator.SetBool("IsTakingBurger", true);
    }

    public void SetCanEquipBurger(bool canEquip)
    {
        canEquipBurger = canEquip;
    }

    public void DropBurger(Table table)
    {
        if (equippedBurger != null && table != null)
        {
            equippedBurger.transform.SetParent(null);
            equippedBurger.transform.rotation = Quaternion.identity;
            equippedBurger.transform.position = table.transform.position + (1 + 0.4f * table.burgerCount) * Vector3.up;
            table.burgerCount++;
            table.burgers.Add(equippedBurger);
            equippedBurger = null;
            Debug.Log($"Количество бургеров: {table.burgerCount}");
            animator.SetBool("IsTakingBurger", false);
        }
    }

    public void AddMoney(float amount)
    {
        moneyAmount += amount;
        moneyTextMesh.text = $"{moneyAmount.ToString("F2")}$";
        OnMoneyChanged?.Invoke(moneyAmount);
    }

    // Методы для обработки событий готовки
    private void OnCookingStarted()
    {
        Debug.Log("Готовка началась");
    }

    private void OnCookingFinished()
    {
        GameObject burger = Instantiate(burgerPrefab, transform.position, Quaternion.identity);
        Debug.Log("Готовка завершена");
        SetCanEquipBurger(true);
        EquipBurger(burger);
    }

    private void OnCookingReset()
    {
        Debug.Log("Готовка сброшена");
    }

    // Методы для обработки событий обслуживания
    private void OnServiceStarted()
    {
        Debug.Log("Обслуживание началось");
    }

    private void OnServiceFinished()
    {
        Debug.Log("Обслуживание завершено");
    }

    private void OnServiceReset()
    {
        Debug.Log("Обслуживание сброшено");
    }
}
