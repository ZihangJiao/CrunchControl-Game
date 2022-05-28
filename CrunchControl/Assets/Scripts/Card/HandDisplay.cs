using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject Deck;

    CardDictionary cardDictionary;
    List<GameObject> cards = new List<GameObject>();
    public int handLimit = 4;

    // Start is called before the first frame update
    void Start()
    {
        cardDictionary = GetComponent<CardDictionary>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drawHand()
    {
        clearHand();
        for(int i =0;i< handLimit; i++)
        {
            GameObject newCard = GameObject.Instantiate(cardPrefab, Deck.transform);
            newCard.GetComponent<CardDisplay>().card = cardDictionary.RandomCard();
            cards.Add(newCard);
        }
    }

    public void clearHand()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }
}
