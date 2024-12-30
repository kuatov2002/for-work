using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Ссылка на трансформ игрока
    public float smoothSpeed = 0.125f;  // Скорость сглаживания движения камеры
    public Vector3 offset;  // Смещение камеры относительно игрока

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}