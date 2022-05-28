using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    //basic information of cards
    public int id;
    public string cardName;
    public int cost;
    public string description;
    public int damage;
    public int knockback;
    public int type;// 0 represent direct attack card, 1 represents trap card, 2 represent buff card.

    public Card(int id, string cardName, int cost, string description, int damage, int knockback, int type)
    {
        this.id = id;
        this.cardName = cardName;
        this.cost = cost;
        this.description = description;
        this.damage = damage;
        this.knockback = knockback;
        this.type = type;
    }
}

