using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 
public class SpeedUpgrade : MonoBehaviour, IPointerClickHandler
{
    private int[] speed = { 3, 5, 7 };
    private int level = 1;
    public TextMeshProUGUI levelText; 
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        player.moveSpeed = speed[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Объект был кликнут!");
        if (player.moneyAmount>=10&&level<speed.Length)
        {
            player.AddMoney(-10);
        

        level++;
        if (level==speed.Length)
        {
            levelText.text = "MAX";
        }
        else
        {
            levelText.text = level+"";
        }

        player.moveSpeed = speed[level - 1];
        }
    }
}
