using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoSingleton<CardEffect>
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTrapEffect(GameObject trap, bool forward, GameObject enemy)
    {
        string name = trap.GetComponent<TrapDisplay>().trap.trap_name;
        // If enemy passes through backwards then 2 additional attack but no knockback
        if (name == "Broken Printer")
        {
            if (forward == false)
            {
                trap.GetComponent<TrapDisplay>().trap.damage += 2;
                trap.GetComponent<TrapDisplay>().trap.knockback = 0;
            }

        }
        // If enemy passes through backwards then double damage

       else if (name == "Bonus Pay Slip")
        {
            if (forward == false)
            {
                trap.GetComponent<TrapDisplay>().trap.damage *= 2;
            }
        }
        // If enemy passes through backwards then no knockback. If enemy passes through forwards then no damage
        else if (name == "Disappointing Memo")
        {
            if (forward == false)
            {
                trap.GetComponent<TrapDisplay>().trap.damage = 0;
            }
            else
            {
                trap.GetComponent<TrapDisplay>().trap.knockback = 0;
                
            }
        }

        // If enemy passes through backwards then double damage

        else if (name == "Spare Lego Bricks")
        {
            if (forward == false)
            {
                trap.GetComponent<TrapDisplay>().trap.damage += 3;
            }
        }

        else if (name == "Office Fan")
        {
            if (forward == false)
            {
                trap.GetComponent<TrapDisplay>().trap.knockback -= 4;
            }
        }

        // Any other traps activated this turn on this enemy deal double damage
        else if (name == "Jar Of Marbles" || name == "Sign In Sheet")
        {
            enemy.GetComponent<EnemyManager>().double_trapdmg_debuff = true;
        }

    }


/*    public void CheckCardEffect(GameObject card, GameObject enemy)
    {
        string name = card.GetComponent<CardDisplay>().card.cardName;
        if (name == "Staple Gun"|| name == "Quantum Paper Plane") {
            foreach (GameObject the_enemy in CardBattle.Instance.EnemyObjList)
            {
                if (the_enemy != enemy)
                {   // other cards at the same place
                    if (the_enemy.GetComponent<EnemyManager>().current_num == enemy.GetComponent<EnemyManager>().current_num)
                    {
                       
                    }
            }
            } 
        }
    }*/

    public void CheckNextTurnEffect(GameObject card)
    {
        string name = card.GetComponent<CardDisplay>().card.cardName;
        if (name == "Progress Report")
        {
            CardBattle.Instance.nextTurnEnergy += 1;
            CardBattle.Instance.updateSupportText();
        }else if (name == "Unplanned Meeting")
        {
            CardBattle.Instance.nextTurnEnergy += 2;
            CardBattle.Instance.updateSupportText();
        }
        else if (name == "Motivational Speech")
        {
            CardBattle.Instance.nextTurndirectDamageMultiplier += 0.5f;
            CardBattle.Instance.updateSupportText();
        }
        else if (name == "Survival Guide")
        {
            CardBattle.Instance.nextTuentrapDamageMultiplier += 0.5f;
            CardBattle.Instance.updateSupportText();
        }

    }
    public void EndTurnEffect()
    {

    }
}
