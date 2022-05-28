using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int id;
    public string enemyName;
    public string category;
    public string ability;
    public string description;
    public int health;
    public int movement;
    public Enemy(int id, string enemyName, string category, string ability, string description, int health, int movement)
    {
        this.id = id;
        this.enemyName = enemyName;
        this.category = category;
        this.ability = ability;
        this.description = description;
        this.health = health;
        this.movement = movement;
    }
}
