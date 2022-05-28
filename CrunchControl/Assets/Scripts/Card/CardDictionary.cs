using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDictionary : MonoBehaviour
{
    public TextAsset cardData;
    public List<Card> cardList = new List<Card>();
    // Start is called before the first frame update
    void Start()
    {
        LoadCardData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCardData()
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "id")
            {
                continue;
            }
            else if(rowArray[0] != "")
            {
                int id = int.Parse(rowArray[0]);
                string cardName = rowArray[1];
                int cost = int.Parse(rowArray[2]);
                string description = rowArray[3];
                int damage = int.Parse(rowArray[4]);
                int knockback = int.Parse(rowArray[5]);
                int type = int.Parse(rowArray[6]);
                Card card = new Card(id, cardName, cost, description, damage, knockback, type);
                cardList.Add(card);
            }
        }
    }

    public Card RandomCard()
    {
        Card card = cardList[Random.Range(0, cardList.Count)];
        return card;
    }
}
