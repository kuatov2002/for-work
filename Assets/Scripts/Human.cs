using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isWalking = false;

    void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    void Update()
    {
        bool newIsWalking = agent.velocity.magnitude > 0.1f;

        if (newIsWalking != isWalking)
        {
            isWalking = newIsWalking;
            animator.SetBool("IsWalking", isWalking);
        }
    }
}