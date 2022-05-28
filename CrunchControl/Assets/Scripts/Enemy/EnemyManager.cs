using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

public class EnemyManager : MonoBehaviour
{
    public Text NameText;
    public Text Category;
    public Text Ability;
    public Text Description;
    public Text HpText;
    public Text MoveText;



    public Enemy enemy;

    public Vector3 target_pos;
    public Vector3 current_pos;
    public int target_num ;
    public int current_num;
    public int second_destination_num; // used to indicate the destination of trap when knock back = 0
    public Vector3 second_destination_pos;

    public float speed = 200.0f;
    public bool is_trap = true;

    //Enemy condition
    public bool is_moving = false;
    public bool is_posdir = true;// indicate whether the enemy is moving in the positive direction.
    public bool stunned = false; // this indicate whether the enemy can move next turn  
    public bool double_trapdmg_debuff = false; // this indicate the enemy will receive double trap damage

    public UnityEvent CheckAtTrapEvent = new UnityEvent();
    Sprite IDImage;
    Sprite Image;


    // Start is called before the first frame update
    void Start()
    {
        CardBattle.Instance.CheckPassTrapEvent.AddListener(CheckNearTrap);
        CheckAtTrapEvent.AddListener(CheckTriggerTrap);
        ShowEnemy();
        LoadEnemyImage();
    }

    // Update is called once per frame
    void Update()
    {
        current_pos = gameObject.transform.position;
        MoveEnemy();

    }


    public void LoadEnemyImage()
    {
        string LoadPath = "Art/Enemy/" + NameText.text;
        IDImage = Resources.Load<Sprite>(LoadPath);

        string LoadPath2 = "Art/EnemyImage/" + NameText.text;

        if (Image = Resources.Load<Sprite>(LoadPath2))
        {
            gameObject.GetComponent<Image>().sprite = Image;
        }
    }

    public Sprite getEnemyImage()
    {
        return IDImage;

    }


    public void ShowEnemy()
    {
        NameText.text = enemy.enemyName.ToString();
        /*        Category.text = enemy.category.ToString();
                Ability.text = enemy.ability.ToString();*/
        Description.text = enemy.description.ToString();
        HpText.text = enemy.health.ToString();
        MoveText.text = enemy.movement.ToString();

    }

    public void MoveEnemy()
    {

       // Debug.Log(target_num);
        if (target_pos != current_pos)
        {
            is_moving = true;
            float step = speed * Time.deltaTime;
            current_pos = Vector3.MoveTowards(current_pos, target_pos, step);
            gameObject.transform.position = current_pos;

        }
        else
        {
            current_num = target_num;
            if (is_moving == true)
            {
                CheckAtTrapEvent.Invoke();
               /* is_moving = false*/;
                if (target_pos == current_pos)// stop from moving
                {
                    // Add enemy to enemyDistribution
                    is_moving = false;
                    if (CardBattle.Instance.EnemyDistribution[current_num].Count == 0)
                    {
                        // If there is no other enimies at destination, show this enemy
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        // If there is other enimies at destination, show this enemy

                        gameObject.SetActive(false);
                    }
                    // gameObject.SetActive(false);

                    EnemyEffect.Instance.UpdateTrapEnemy(gameObject);

                    //CardBattle.Instance.EnemyDistribution[current_num].Add(gameObject);
                    /*                    CardBattle.Instance.EnemyActionEvent.Invoke();
                                        CardBattle.Instance.UpdateCircleState(current_num);

                                        EnemyEffect.Instance.UpdateTrapEnemy(gameObject);*/

                    if (current_pos == CardBattle.Instance.EnemyPath[9].transform.position && enemy.health >= 0)
                    {
                        Destroy(transform.gameObject);
                        CardBattle.Instance.EnemyObjList.Remove(transform.gameObject);
                        //CardBattle.Instance.EnemyDistribution[gameObject.GetComponent<EnemyManager>().current_num].Remove(gameObject);
                        int child_num = CardBattle.Instance.HP.transform.childCount;
                        if (child_num != 0)
                        {
                            GameObject.Destroy(CardBattle.Instance.HP.transform.GetChild(child_num - 1).gameObject);
                        }
                        CardBattle.Instance.EnemyMoveCounter -= 1;
                        LoadSceneManager.HP -= 1;

                        if (LoadSceneManager.HP == 0)
                        {
                            CardBattle.Instance.FirstEnemyEscape.SetActive(false);
                            CardBattle.Instance.SecondEnemyEscape.SetActive(false);
                            CardBattle.Instance.ThirdEnemyEscape.SetActive(false);
                            CardBattle.Instance.FourthEnemyEscape.SetActive(false);
                            CardBattle.Instance.GameOver.SetActive(true);
                            CardBattle.Instance.gameOver = true;
                        }

                        else if (LoadSceneManager.HP == 1)
                        {
                            CardBattle.Instance.FirstEnemyEscape.SetActive(false);
                            CardBattle.Instance.SecondEnemyEscape.SetActive(false);
                            CardBattle.Instance.ThirdEnemyEscape.SetActive(false);
                            CardBattle.Instance.FourthEnemyEscape.SetActive(true);
                        }
                        else if (LoadSceneManager.HP == 2)
                        {
                            CardBattle.Instance.FirstEnemyEscape.SetActive(false);
                            CardBattle.Instance.SecondEnemyEscape.SetActive(false);
                            CardBattle.Instance.ThirdEnemyEscape.SetActive(true);
                        }
                        else if (LoadSceneManager.HP == 3)
                        {
                            CardBattle.Instance.FirstEnemyEscape.SetActive(false);
                            CardBattle.Instance.SecondEnemyEscape.SetActive(true);
                        }
                        else if (LoadSceneManager.HP == 4)
                        {
                            CardBattle.Instance.FirstEnemyEscape.SetActive(true);
                        }

                    }
                    else
                    {
                        CardBattle.Instance.EnemyDistribution[current_num].Add(gameObject);

                    }
                    CardBattle.Instance.EnemyActionEvent.Invoke();
                    CardBattle.Instance.UpdateCircleState(current_num);

                   


                }
            }


        }


    }

    public void CheckNearTrap()
    {
        if (target_num != current_num && enemy.enemyName != "Length Contractor")
        {
            for (int i = 0; i < CardBattle.Instance.trap_list.Count; i++)
            {
                int trap_num = CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.pos;
                Vector3 trap_pos = CardBattle.Instance.trap_list[i].transform.position;
                int knockback = CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback;


                /*              target_num = trap_num;
                              target_pos = trap_pos;*/


                if (current_num < trap_num && trap_num <= target_num)//step on trap while moving forward
                {
                    if (knockback == 0)
                    {
                        second_destination_num = target_num;
                        second_destination_pos = target_pos;
                    }
                    target_num = trap_num;
                    target_pos = trap_pos;
                }
                else if (current_num > trap_num && trap_num >= target_num)//step on trap while moving backward
                {
                    if (knockback == 0)
                    {
                        second_destination_num = target_num;
                        second_destination_pos = target_pos;
                    }
                    target_num = trap_num;
                    target_pos = trap_pos;
                }
            }
        }

    }

    public void CheckTriggerTrap()
    {
        if (CardBattle.Instance.trap_list.Count != 0 && enemy.enemyName != "Length Contractor")
        {
            for (int i = 0; i < CardBattle.Instance.trap_list.Count; i++)
            {
                // if the enemy step on the trap
                if (CardBattle.Instance.trap_list[i].transform.position == current_pos)
                {
                    CardEffect.Instance.CheckTrapEffect(CardBattle.Instance.trap_list[i], is_posdir, gameObject);
                    if (double_trapdmg_debuff)
                    {
                        CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.damage *= 2;// the trrp deals double damage if enemy is in debuff
                    }
                    // deal damage

                    enemy.health -= (int)(CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.damage * CardBattle.Instance.trapDamageMultiplier);
                    // remove the enemy if he dies
                    if (enemy.health <= 0)
                    {
                        EnemyEffect.Instance.DeadEffect(transform.gameObject);
                        Destroy(transform.gameObject);
                        CardBattle.Instance.EnemyObjList.Remove(transform.gameObject);
                        CardBattle.Instance.UpdateCircleState(current_num);
                        CardBattle.Instance.EnemyMoveCounter -= 1;


                    }

                    //2. push


                    // determin whether the enemy is moving forwards or backwords
                    if (CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback >= 0)
                    {
                        is_posdir = true;
                    }
                    else
                    {
                        is_posdir = false;
                    }
                    if (CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback != 0) // determine wheather the knockback of trap is 0
                    {
                        if (target_num - CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback >= 0 &&
                        target_num - CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback < CardBattle.Instance.EnemyDistribution.Count)
                        {
                            target_num -= CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback;
                            target_pos = CardBattle.Instance.EnemyPath[target_num].transform.position;
                            CheckNearTrap();
                        }
                        else if (target_num - CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().trap.knockback >= CardBattle.Instance.EnemyDistribution.Count)
                        {
                            target_num = CardBattle.Instance.EnemyDistribution.Count - 1;
                            target_pos = CardBattle.Instance.EnemyPath[CardBattle.Instance.EnemyDistribution.Count - 1].transform.position;
                            CheckNearTrap();
                        }
                        else
                        {
                            target_num = 0;
                            target_pos = CardBattle.Instance.EnemyPath[0].transform.position;
                            CheckNearTrap();
                        }
                    }
                    else
                    {

                        {
                            target_num = second_destination_num;
                            target_pos = CardBattle.Instance.EnemyPath[target_num].transform.position;
                            if (current_pos != target_pos)
                            {
                                CheckNearTrap();
                            }
                            //CheckNearTrap();
                        }

                        
                    }

                    if (CardBattle.Instance.trap_list[i].GetComponent<TrapDisplay>().EnemyId == 0)// if the trap is not enemy trap
                    {
                        Destroy(CardBattle.Instance.trap_list[i]);
                        CardBattle.Instance.trap_list.RemoveAt(i);
                    }
                    ShowEnemy();

                    if (enemy.health <= 0)
                    {
                        CardBattle.Instance.EnemyActionEvent.Invoke();
                    }
                }


            }
        }
    }



}
