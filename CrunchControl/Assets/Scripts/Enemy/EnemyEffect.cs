using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyEffect : MonoSingleton<EnemyEffect>
{

    public bool DiscardHand = false;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitialiseEffect(GameObject enemy)
    {
        string name = enemy.GetComponent<EnemyManager>().enemy.enemyName;
        // GameStart: All Low Level Cleaners gain an additional 2 health
        if (name == "Experienced Cleaner")
        {
            foreach (GameObject the_enemy in CardBattle.Instance.EnemyObjList)
            {
                if (the_enemy.GetComponent<EnemyManager>().enemy.enemyName == "Low Level Cleaner")
                {
                    the_enemy.GetComponent<EnemyManager>().enemy.health += 2;
                }
            }
        }
        else if (name == "Rusty Bucket")
        {
            InitialiseTrapEnemy(enemy);
        }
    }

    public void InitialiseTrapEnemy(GameObject enemy)
    {
        GameObject trap = Instantiate(CardBattle.Instance.trapPrefab, CardBattle.Instance.EnemyPath[enemy.GetComponent<EnemyManager>().current_num].transform.position, Quaternion.identity, CardBattle.Instance.canvas);// initialise a new enemy kind of trap.
        trap.SetActive(false);
        trap.GetComponent<TrapDisplay>().trap =
            new Trap(enemy.GetComponent<EnemyManager>().enemy.enemyName, 0, -4, enemy.GetComponent<EnemyManager>().current_num);
        trap.GetComponent<TrapDisplay>().EnemyId = 1;
        CardBattle.Instance.trap_list.Add(trap);// Initialise the enemy trap to trap list
    }


    public void UpdateTrapEnemy(GameObject enemy)
    {
        for (int i = 0; i < CardBattle.Instance.trap_list.Count; i++)
        {
            if (CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().EnemyId == enemy.GetComponent<EnemyManager>().enemy.id)
            {
                CardBattle.Instance.trap_list[i].transform.position = enemy.transform.position;
                CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.pos = enemy.GetComponent<EnemyManager>().current_num;
                return;
            }
        }
    }

    public void RemoveTrapEnemy(GameObject enemy)
    {
        for (int i = 0; i < CardBattle.Instance.trap_list.Count; i++)
        {
            if (CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().EnemyId == 1)
            {
                Destroy(CardBattle.Instance.trap_list[i]);
                CardBattle.Instance.trap_list.RemoveAt(i);
                return;
            }
        }

    }

    public void DrawEffect()
    {

    }

    public void CardHitEffect(GameObject card, GameObject enemy)
    {
        string name = enemy.GetComponent<EnemyManager>().enemy.enemyName;

        //If they have taken damage in a turn they don't move that turn(stunned)
        if (name == "Low Level Cleaner")
        {
            if (card.GetComponent<CardDisplay>().card.damage > 0)
            {
                enemy.GetComponent<EnemyManager>().stunned = true;
            }
        }

    }

    public void MoveEffect(GameObject enemy)
    {
        string name = enemy.GetComponent<EnemyManager>().enemy.enemyName;
        // Doesn't move on first turn
        if (name == "Cleaner With Cart")
        {
            if (CardBattle.Instance.TurnCounter == 1)
            {
                enemy.GetComponent<EnemyManager>().stunned = true;
            }
        }
    }

    public void DeadEffect(GameObject the_enemy)
    {
        string name = the_enemy.GetComponent<EnemyManager>().enemy.enemyName;
        if (name == "Bucket Hat Cleaner")
        {
            int summon_pos = the_enemy.GetComponent<EnemyManager>().current_num;
            GameObject enemy = Instantiate(CardBattle.Instance.enemyPrefab,
                CardBattle.Instance.EnemyPath[summon_pos].transform.position,
                Quaternion.identity, CardBattle.Instance.canvas);

            int id = the_enemy.GetComponent<EnemyManager>().enemy.id;
            string enemyName = "Rusty Bucket";
            string category = "Cleaners";
            string ability = "Any enemy that passes over it is moved 4 spaces forward";
            string description = "Only spawns from a defeated Bucket Hat Cleaner and is not a threat on it's own";
            int health = 4;
            int movement = 0;
            Enemy rusty_bucket = new Enemy(id, enemyName, category, ability, description, health, movement);

            enemy.GetComponent<EnemyManager>().enemy = rusty_bucket;
            enemy.GetComponent<EnemyManager>().current_num = summon_pos;
            enemy.GetComponent<EnemyManager>().target_num = summon_pos;
            enemy.GetComponent<EnemyManager>().current_pos = CardBattle.Instance.EnemyPath[summon_pos].transform.position;
            enemy.GetComponent<EnemyManager>().target_pos = CardBattle.Instance.EnemyPath[summon_pos].transform.position;
            CardBattle.Instance.EnemyObjList.Add(enemy);
            CardBattle.Instance.EnemyObjList.Sort((x, y) => x.GetComponent<EnemyManager>().enemy.movement.CompareTo(y.GetComponent<EnemyManager>().enemy.movement));
            CardBattle.Instance.EnemyDistribution[summon_pos].Add(enemy);
            CardBattle.Instance.EnemyDistribution[summon_pos].Sort((x, y) => x.GetComponent<EnemyManager>().enemy.movement.CompareTo(y.GetComponent<EnemyManager>().enemy.movement));
            InitialiseTrapEnemy(enemy);
            CardBattle.Instance.UpdateCircleState(summon_pos);
        }
        else if (name == "Rusty Bucket")
        {
            RemoveTrapEnemy(the_enemy);
        }
    }


    public void EndTurnEffect()
    {
        bool foundQuick = false;

        foreach (GameObject enemy in CardBattle.Instance.EnemyObjList)
        {
            enemy.GetComponent<EnemyManager>().stunned = false;
            enemy.GetComponent<EnemyManager>().double_trapdmg_debuff = false;
            if (enemy.GetComponent<EnemyManager>().enemy.enemyName == "Unpaid Intern")
            {
                enemy.GetComponent<EnemyManager>().enemy.health = 5;
                enemy.GetComponent<EnemyManager>().ShowEnemy();
            }
            else if (enemy.GetComponent<EnemyManager>().enemy.enemyName == "Telesales" && CardBattle.Instance.TurnCounter == 3)
            {

                int hand_num = CardBattle.Instance.Hands.Count;
                for (int i = 0; i < hand_num; i++)
                {
                    CardBattle.Instance.Discard.Add(CardBattle.Instance.Hands[0].GetComponent<CardDisplay>().card);
                    Destroy(CardBattle.Instance.Hands[0]);
                    CardBattle.Instance.Hands.RemoveAt(0);

                }
                DiscardHand = true;
                CardBattle.Instance.Energy += hand_num;
            }
            else if (enemy.GetComponent<EnemyManager>().enemy.enemyName == "Receptionist")
            {
                CardBattle.Instance.nextTurnEnergy -= 1;
                CardBattle.Instance.updateSupportText();
            }
            else if (enemy.GetComponent<EnemyManager>().enemy.enemyName == "Quark Connoisseur")
            {
                foundQuick = true;
            }else if (enemy.GetComponent<EnemyManager>().enemy.enemyName == "Disgruntled IT Guy")
            {
                if (CardBattle.Instance.blocked_space >= 0)
                {
   
                    var col = CardBattle.Instance.EnemyPath[CardBattle.Instance.blocked_space].GetComponent<Image>().color;
                    col.a = 1;
                    CardBattle.Instance.EnemyPath[CardBattle.Instance.blocked_space].GetComponent<Image>().color = col;
                }

                if (CardBattle.Instance.last_blocked_space >=0 && CardBattle.Instance.last_blocked_space != CardBattle.Instance.blocked_space)
                {
                    var col = CardBattle.Instance.EnemyPath[CardBattle.Instance.last_blocked_space].GetComponent<Image>().color;
                    col.a = 0;
                    CardBattle.Instance.EnemyPath[CardBattle.Instance.last_blocked_space].GetComponent<Image>().color = col;
                }

                CardBattle.Instance.last_blocked_space = CardBattle.Instance.blocked_space;
                CardBattle.Instance.blocked_space = enemy.GetComponent<EnemyManager>().current_num;
            }

        }
        if(foundQuick == false)
        {
            CardBattle.Instance.moveOneMoreStep = 0;
        }
    }
}
