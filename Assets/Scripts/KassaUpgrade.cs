using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 

public class KassaUpgrade : MonoBehaviour, IPointerClickHandler
{
    public Player player;
    private int[] time = { 3, 2, 1 };
    private int level = 1;
    public Kassa kassa;
    public TextMeshProUGUI levelText; 
    // Start is called before the first frame update
    void Start()
    {
        kassa.playingTime= time[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (player.moneyAmount>=10&&level<time.Length)
        {
            player.AddMoney(-10);
        

            level++;
            if (level==time.Length)
            {
                levelText.text = "MAX";
            }
            else
            {
                levelText.text = level+"";
            }

            kassa.playingTime = time[level - 1];
        
        }
    }
}
