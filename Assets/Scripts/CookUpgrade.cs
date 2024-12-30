using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 
public class CookUpgrade : MonoBehaviour, IPointerClickHandler
{
    public Player player;
    private int[] time = { 3, 2, 1 };
    private int level = 1;
    public Stove stove;
    public TextMeshProUGUI levelText; 
    // Start is called before the first frame update
    void Start()
    {
        stove.cookingTime = time[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Объект был кликнут!");
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

        stove.cookingTime = time[level - 1];
        
        }
    }
}
