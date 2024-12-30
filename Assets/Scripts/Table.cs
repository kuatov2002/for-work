using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Player player;
    public int burgerCount = 0;
    public Transform Givezone;

    public Kassa Kassa;
    public float interactionDistance = 3f;
    private bool isPlayerNear = false;

    public List<GameObject> burgers = new List<GameObject>();

    void Update()
    {
        float distance = Vector3.Distance(Givezone.position, player.transform.position);
        isPlayerNear = (distance <= interactionDistance);

        if (isPlayerNear)
        {
            player.DropBurger(this);
        }

        if (Kassa.humansAtTable.Count>0&&Vector3.Distance(Kassa.humansAtTable[0].transform.position, transform.position) <= interactionDistance && burgers.Count > 0)
        {
            GameObject lastBurger = burgers[burgers.Count - 1];
            Destroy(lastBurger);
            burgers.RemoveAt(burgers.Count - 1);

            burgerCount--;

            Kassa.CustomerLeave();
        }
    }
}