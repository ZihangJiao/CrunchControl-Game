using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO;

public enum GamePhase
{
    playerDraw, playerAction, enemyAction
}


public class CardBattle : MonoSingleton<CardBattle>
{

    public static CardBattle Instance;


    // Data used to load enemy.
    public TextAsset EnemyData;
    //List of enemies
    public List<Enemy> EnemyList = new List<Enemy>();
    public List<GameObject> EnemyObjList = new List<GameObject>();
    public List<GameObject> EnemyPath;
    public GameObject enemyPrefab;
    public GameObject IndicatorCircle;
    public GameObject HighlightedCircle;
    public GameObject CircleIndicator;
    public List<GameObject> CircleIndiList;

    public List<List<GameObject>> EnemyDistribution = new List<List<GameObject>>();// Distribution of enemy on the map
    public List<int> CurrentViewEnemyList;// indicate current enemy look at when there are multiple enemies at same gird

    // Data used to load deck.
    public TextAsset DeckData;
    public int EncounterInt;
    //Deck
    public List<Card> Deck = new List<Card>();
    public Text DeckCount;
    //DiscardPile
    public List<Card> Discard = new List<Card>();
    public Text DiscardCount;
    // Hand area.
    public RectTransform Hand;
    public List<GameObject> Hands = new List<GameObject>();

    //Card prefab joins the hand
    public GameObject cardPrefab;

    //array of location of traps
    public List<GameObject> trap_list;
    public GameObject trapPrefab;

    public GamePhase GamePhase = GamePhase.playerAction;

    public UnityEvent phaseChangeEvent = new UnityEvent();
    public UnityEvent EnemyActionEvent = new UnityEvent();
    public UnityEvent NewTurnEvent = new UnityEvent();
    public UnityEvent CheckPassTrapEvent = new UnityEvent();
    public UnityEvent CardSpacingEvent = new UnityEvent();
    public UnityEvent CircleStateEvent = new UnityEvent();

    // Turn counter
    public int TurnCounter = 1;

    //Arrow when select cards
    public GameObject ArrowPrefab;
    private GameObject arrow;
    public Transform canvas;

    public Text EndturnButtonText;
    //energy system
    public Text EnergyCount;
    public int Energy = 7;

    public GameObject selectedCard;
    public GameObject enlarged_cardshown;

    public int EnemyMoveCounter;//use to control which enemy is moving
    public bool EnemyIsMoving = false;

    public GameObject enemyOnMap;

    public float directDamageMultiplier = 1.0f;
    public float trapDamageMultiplier = 1.0f;

    // next turn effect stuff;
    public GameObject effectBoard;
    public bool nextTurnEffect = false;
    public float nextTurndirectDamageMultiplier = 1.0f;
    public float nextTuentrapDamageMultiplier = 1.0f;
    public int nextTurnEnergy = 7;

    public int moveOneMoreStep = 0;
    public int blocked_space = 0;
    public int last_blocked_space = -1;

    public GameObject enemyIDImage;
    public Text SupportEffectText;

    public GameObject HP;
    public bool gameOver = false;
    public GameObject FirstEnemyEscape;
    public GameObject SecondEnemyEscape;
    public GameObject ThirdEnemyEscape;
    public GameObject FourthEnemyEscape;
    public GameObject ChangeToMap;
    public GameObject GameOver;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BattleStart();
        //EnemyActionEvent.AddListener(EnemyAction);
        EnemyActionEvent.AddListener(EnemyActionOneByOne);
        NewTurnEvent.AddListener(NewTurn);
        // CircleStateEvent.AddListener(UpdateCircleState);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            DestroyArrow();
        }


        if (EnemyObjList.Count == 0 && gameOver != true)
        {
            string filepath = Application.dataPath + "/StreamingAssets.json";

            if (File.Exists(filepath))
            {
                StreamReader sr = new StreamReader(filepath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();

                Save save = JsonUtility.FromJson<Save>(jsonStr);
                save.health = LoadSceneManager.HP;
                string saveJsonStr = JsonUtility.ToJson(save);
                try
                {
                    File.WriteAllText(filepath, saveJsonStr);

#if UNITY_EDITOR
                    Debug.Log($"Successfully saved data to {filepath}.");
#endif
                }
                catch (System.Exception exception)
                {
#if UNITY_EDITOR
                    Debug.LogError($"Failed save data to {filepath}. \n{exception}");
#endif
                }
            }


            CardBattle.Instance.FirstEnemyEscape.SetActive(false);
            CardBattle.Instance.SecondEnemyEscape.SetActive(false);
            CardBattle.Instance.ThirdEnemyEscape.SetActive(false);
            CardBattle.Instance.FourthEnemyEscape.SetActive(false);
            CardBattle.Instance.ChangeToMap.SetActive(true);
        }

    }

    // Start Phase
    public void BattleStart()
    {
        InitialiseHP();
        //1. Read Deck
        ReadDeck();
        //2. Shuffle
        Shuffle();
        //3. Draw Cards, Initialise Enemies
        if (LoadSceneManager.isStealthBattle == true)
        {
            DrawCard(4);
        }
        else
        {
            DrawCard(3);
        }
        ReadEnemy();
        InitialiseEnemy();
        // UpdateEnemyState();


    }

    public void InitialiseHP()
    {
        int HP_int = LoadSceneManager.HP;
        for(int i = 0; i < 5 - HP_int; i++)
        {
            Destroy(HP.transform.GetChild(i).gameObject);
        }
    }

    public void ReadDeck()
    {

        string[] dataRow = DeckData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "id")
            
            {
                continue;
            }
            else if (rowArray[0] != "")
            {
                int id = int.Parse(rowArray[0]);
                string cardName = rowArray[1];
                int cost = int.Parse(rowArray[2]);
                string description = rowArray[3].Replace("#", ",");

                int damage = int.Parse(rowArray[4]);
                int knockback = int.Parse(rowArray[5]);
                int type = int.Parse(rowArray[6]);
                Card card = new Card(id, cardName, cost, description, damage, knockback, type);

                Deck.Add(card);
            }
        }
        DeckCount.text = Deck.Count.ToString();
        DiscardCount.text = "0";
    }

    public void Shuffle()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < Deck.Count; i++)
        {
            // Change position of two randomly selected cards.
            int rand = Random.Range(0, Deck.Count);
            Card temp = Deck[i];
            Deck[i] = Deck[rand];
            Deck[rand] = temp;

        }
    }




    public void ReadEnemy()
    {
       int IncomingInt = LoadSceneManager.difficulty_level;
       // int IncomingInt = 1;

        Random.InitState((int)System.DateTime.Now.Ticks);
        int rand_num = Random.Range(1, 100);

        switch (IncomingInt)
        {
            case 0:
                EncounterInt = 14;
                break;
            case 1:
                if (1 <= rand_num && rand_num < 26)
                {
                    EncounterInt = 1;
                }
                else if (26 <= rand_num && rand_num < 51)
                {
                    EncounterInt = 3;
                }
                else if (51 <= rand_num && rand_num < 76)
                {
                    EncounterInt = 5;
                }
                else if (76 <= rand_num && rand_num < 101)
                {
                    EncounterInt = 11;
                }
                break;
            case 2:
                if (1 <= rand_num && rand_num < 21)
                {
                    EncounterInt = 2;
                }
                else if (21 <= rand_num && rand_num < 41)
                {
                    EncounterInt = 6;
                }
                else if (41 <= rand_num && rand_num < 61)
                {
                    EncounterInt = 7;
                }
                else if (61 <= rand_num && rand_num < 81)
                {
                    EncounterInt = 9;
                }else if (81 <= rand_num && rand_num < 101)
                {
                    EncounterInt = 10;
                }
                break;
            case 3:
                if (1 <= rand_num && rand_num < 33)
                {
                    EncounterInt = 4;
                }
                else if (33 <= rand_num && rand_num < 66)
                {
                    EncounterInt = 8;
                }
                else if (66 <= rand_num && rand_num < 101)
                {
                    EncounterInt = 12;
                }
                break;
            case 4:
                EncounterInt = 13;
                break;
        }

      //EncounterInt = 13;

        string[] dataRow = EnemyData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            //if (rowArray[0] == "id")
            if (rowArray[0] != EncounterInt.ToString())
            {
                continue;
            }
            else if (rowArray[0] != "")
            {
                int id = int.Parse(rowArray[0]);
                string enemyName = rowArray[1];
                if(enemyName == "Quark Connoisseur")
                {
                    moveOneMoreStep = 1;
                }
                string category = rowArray[2];
                string ability = rowArray[3];
                string description = rowArray[4];
                int health = int.Parse(rowArray[5]);
                int movement = int.Parse(rowArray[6]);
                Enemy enemy = new Enemy(id, enemyName, category, ability, description, health, movement);
                EnemyList.Add(enemy);

            }
        }
    }

    public void InitialiseEnemy()
    {

        for (int i = 0; i < EnemyList.Count; i++)
        {
            // Initialise enemy at the start of the path
            GameObject enemy = Instantiate(enemyPrefab, EnemyPath[0].transform.position, Quaternion.identity, canvas.Find("Enemy"));
            enemy.GetComponent<EnemyManager>().enemy = EnemyList[i];
            enemy.GetComponent<EnemyManager>().current_pos = EnemyPath[0].transform.position;
            enemy.GetComponent<EnemyManager>().target_pos = EnemyPath[0].transform.position;
            //enemy.GetComponent<EnemyManager>().initialise = true;
            EnemyObjList.Add(enemy);


        }

        EnemyObjList.Sort((x, y) => x.GetComponent<EnemyManager>().enemy.movement.CompareTo(y.GetComponent<EnemyManager>().enemy.movement));

        for (int i = 0; i < 10; i++)
        {
            CurrentViewEnemyList.Add(0);
            List<GameObject> tempList = new List<GameObject>();
            EnemyDistribution.Add(tempList);
        }

        // Trigger the Initialise effect of enemy
        foreach (GameObject the_enemy in EnemyObjList)
        {
            EnemyEffect.Instance.InitialiseEffect(the_enemy);
        }

        //initialise enemy distribution array

        for (int i = 0; i < EnemyObjList.Count; i++)
        {
            int num = EnemyObjList[i].GetComponent<EnemyManager>().target_num;
            EnemyDistribution[num].Add(EnemyObjList[i]);
            if (i == 0)
            {
                EnemyObjList[i].SetActive(true);
            }
            else
            {
                EnemyObjList[i].SetActive(false);
            }
        }

        UpdateCircleState(0);


    }


    public void UpdateCircleState(int pos_num)
    {

        if (CurrentViewEnemyList[pos_num] >= EnemyDistribution[pos_num].Count)
        {
            CurrentViewEnemyList[pos_num] = 0;
        }

        if (EnemyDistribution[pos_num].Count == 1)// if only one enemy at the grid
        {
            EnemyDistribution[pos_num][0].SetActive(true);
            GameObject leftarrow = EnemyDistribution[pos_num][0].transform.Find("leftarrow").gameObject;
            GameObject rightarrow = EnemyDistribution[pos_num][0].transform.Find("rightarrow").gameObject;
            GameObject circle = EnemyDistribution[pos_num][0].transform.Find("Circle").gameObject;
            // does not show arrows and circles
            leftarrow.SetActive(false);
            rightarrow.SetActive(false);
            circle.SetActive(false);

        }

        if (EnemyDistribution[pos_num].Count > 1)// if more than one enemy at the grid
        {

            GameObject leftarrow = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("leftarrow").gameObject;
            GameObject circle = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("Circle").gameObject;
            GameObject rightarrow = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("rightarrow").gameObject;

            // GameObject CircleIndi = CircleIndiList[pos_num];
            foreach (Transform child in circle.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            GameObject CircleIndi = Instantiate(CircleIndicator, circle.transform.position, Quaternion.identity, circle.transform);
            // if this is the first enemy in list

            if (CurrentViewEnemyList[pos_num] == 0)
            {
                leftarrow.SetActive(false);
                rightarrow.SetActive(true);

            }

            // if this is the last enemy in list
            else if (CurrentViewEnemyList[pos_num] == EnemyDistribution[pos_num].Count - 1)
            {

                leftarrow.SetActive(true);
                rightarrow.SetActive(false);
            }
            // if this is the middle enemy in list
            else
            {
                leftarrow.SetActive(true);
                rightarrow.SetActive(true);
            }


            circle.SetActive(true);

            for (int j = 0; j < EnemyDistribution[pos_num].Count; j++)// make circles
            {

                if (CurrentViewEnemyList[pos_num] == j)// green dot
                {
                    if (CircleIndi.transform.childCount < EnemyDistribution[pos_num].Count)
                    {
                        Instantiate(HighlightedCircle, CircleIndi.transform);
                    }
                    EnemyDistribution[pos_num][j].SetActive(true);
                    //  EnemyDistribution[pos_num][j].GetComponent<EnemyManager>().is_hidden = false;
                }
                else
                {
                    if (CircleIndi.transform.childCount < EnemyDistribution[pos_num].Count)
                    {
                        Instantiate(IndicatorCircle, CircleIndi.transform);
                    }
                    EnemyDistribution[pos_num][j].SetActive(false);
                    //  EnemyDistribution[pos_num][j].GetComponent<EnemyManager>().is_hidden = true;

                }

            }


        }

    }

    // Player Phase
    public void CardPlayRequest(Transform position, GameObject card)
    {
        //If the energy cost of the selected card is less than the remaining energy
        if (arrow != null)
        {
            DestroyArrow();
        }

        if (card.GetComponent<CardDisplay>().card.cost <= Energy)
        {
            CreateArrow(position, ArrowPrefab);
            selectedCard = card;
            if(card.GetComponent<CardDisplay>().card.type == 2)
            {
                effectBoard.GetComponent<Image>().color = Color.yellow;
            }
        }


    }

    public void CardPlayConfirm(Transform enemyTransform, GameObject enemy)
    {
        if (selectedCard != null)
        {
            // if the card is direct attack card.
            if (selectedCard.GetComponent<CardDisplay>().card.type == 0)
            {
                CardPlay(selectedCard, enemy);

                //Audio when card played
                FindObjectOfType<AudioController>().PlaySound("SFXAction");
            }
        }
    }

    public void CardPlay(GameObject card, GameObject enemy)
    {
        string name = enemy.GetComponent<EnemyManager>().enemy.enemyName;
        //Check Card Effect
        if (card.GetComponent<CardDisplay>().AOE == true)
        {
            //  int pos_num = enemy.GetComponent<EnemyManager>().current_num;
            // int list_count = EnemyDistribution[pos_num].Count;


            /*            for (int i = 0; i < list_count; i++)
                        {
                            EnemyDistribution[pos_num][i].SetActive(true);
                            if (name != "Time Dilater")
                            {
                                ResolveCardEffect(card, EnemyDistribution[pos_num][i]);
                            }
                        }*/
            int list_count = EnemyObjList.Count;
            for (int i = 0; i < EnemyObjList.Count; i++)
            {
                name = EnemyObjList[i].GetComponent<EnemyManager>().enemy.enemyName;
                if (name != "Time Dilater")
                {
                    EnemyObjList[i].SetActive(true);
                    ResolveCardEffect(card, EnemyObjList[i]);
                    if (list_count != EnemyObjList.Count)
                    {
                        list_count = EnemyObjList.Count;
                        i--;
                    }

                }
            }

            for(int i = 0;i < 9; i++)
            {
                UpdateCircleState(i);
            }

        }
        else
        {
           
            if (name != "Time Dilater")
            {
                ResolveCardEffect(card, enemy);
            }
        }

        //3.energy cost
        Energy -= card.GetComponent<CardDisplay>().card.cost;
        EnergyCount.text = Energy.ToString();

        // remove the card from hand and add to discard pile
        Hands.Remove(card);
        Discard.Add(card.GetComponent<CardDisplay>().card);
        DiscardCount.text = Discard.Count.ToString();
        Destroy(card);
        Destroy(arrow);
        selectedCard = null;
        //enemy.GetComponent<EnemyManager>().ShowEnemy();
        CardSpacingEvent.Invoke();


    }

    public void NextTurnEffectPlayConfirm()
    {
        if (selectedCard != null)
        {
            // if the card is next turn effect.
            if (selectedCard.GetComponent<CardDisplay>().card.type == 2)
            {
                NextTurnEffectPlay(selectedCard);

                //Audio when card played
                FindObjectOfType<AudioController>().PlaySound("SFXAction");
            }
        }
    }

    public void NextTurnEffectPlay(GameObject card)
    {
        CardEffect.Instance.CheckNextTurnEffect(card);
        //3.energy cost
        Energy -= card.GetComponent<CardDisplay>().card.cost;
        EnergyCount.text = Energy.ToString();

        // remove the card from hand and add to discard pile
        Hands.Remove(card);
        Discard.Add(card.GetComponent<CardDisplay>().card);
        DiscardCount.text = Discard.Count.ToString();
        Destroy(card);
        Destroy(arrow);
        selectedCard = null;
        CardSpacingEvent.Invoke();


    }

    public void ResolveCardEffect(GameObject card, GameObject enemy)
    {
        //resolve effect;
        //1. damage
        if (card.GetComponent<CardDisplay>().card.damage > 0)
        {
            EnemyEffect.Instance.CardHitEffect(card, enemy);
        }
        enemy.GetComponent<EnemyManager>().enemy.health -= (int)(card.GetComponent<CardDisplay>().card.damage * directDamageMultiplier);
        enemy.GetComponent<EnemyManager>().ShowEnemy();
        if (enemy.GetComponent<EnemyManager>().enemy.health <= 0)
        {
            EnemyEffect.Instance.DeadEffect(enemy);
            Destroy(enemy);
            EnemyObjList.Remove(enemy);
            //UpdateEnemyState();
            EnemyDistribution[enemy.GetComponent<EnemyManager>().current_num].Remove(enemy);
            UpdateCircleState(enemy.GetComponent<EnemyManager>().current_num);
        }

        //2. push
        // determin whether the enemy is moving forwards or backwords
        if (card.GetComponent<CardDisplay>().card.knockback >= 0)
        {
            enemy.GetComponent<EnemyManager>().is_posdir = true;
        }
        else
        {
            enemy.GetComponent<EnemyManager>().is_posdir = false;
        }

        if (card.GetComponent<CardDisplay>().card.knockback > 0 && EnemyObjList.Contains(enemy) && enemy.GetComponent<EnemyManager>().current_num != 0
            || card.GetComponent<CardDisplay>().card.knockback < 0 && EnemyObjList.Contains(enemy) && enemy.GetComponent<EnemyManager>().current_num != EnemyDistribution.Count - 1)// if enemy is moved by card
        {

            int pos_num = enemy.GetComponent<EnemyManager>().current_num;

            // remove the current enemy from enemy distribution, remove circles and arrows
            GameObject circle = EnemyDistribution[enemy.GetComponent<EnemyManager>().current_num][CurrentViewEnemyList[enemy.GetComponent<EnemyManager>().current_num]].transform.Find("Circle").gameObject;
            foreach (Transform child in circle.transform)
            {
                Destroy(child.gameObject);
            }
            GameObject leftarrow = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("leftarrow").gameObject;
            GameObject rightarrow = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("rightarrow").gameObject;
            leftarrow.SetActive(false);
            rightarrow.SetActive(false);
            EnemyDistribution[enemy.GetComponent<EnemyManager>().current_num].Remove(enemy);
            UpdateCircleState(enemy.GetComponent<EnemyManager>().current_num);
        }

        // if enemy moves in the central area
        if (enemy.GetComponent<EnemyManager>().target_num - card.GetComponent<CardDisplay>().card.knockback >= 0 &&
            enemy.GetComponent<EnemyManager>().target_num - card.GetComponent<CardDisplay>().card.knockback < EnemyDistribution.Count)
        {
            enemy.SetActive(true);

            enemy.GetComponent<EnemyManager>().target_num -= card.GetComponent<CardDisplay>().card.knockback;
            enemy.GetComponent<EnemyManager>().target_pos = EnemyPath[enemy.GetComponent<EnemyManager>().target_num].transform.position;
            CheckPassTrapEvent.Invoke();
        }// if enemy is moved to the last grid
        else if (enemy.GetComponent<EnemyManager>().target_num - card.GetComponent<CardDisplay>().card.knockback >= EnemyDistribution.Count)
        {
            enemy.SetActive(true);

            enemy.GetComponent<EnemyManager>().target_num = EnemyDistribution.Count - 1;
            enemy.GetComponent<EnemyManager>().target_pos = EnemyPath[EnemyDistribution.Count - 1].transform.position;
            CheckPassTrapEvent.Invoke();
        }
        else// if enemy is moved to the first grid
        {
            enemy.SetActive(true);

            enemy.GetComponent<EnemyManager>().target_num = 0;
            enemy.GetComponent<EnemyManager>().target_pos = EnemyPath[0].transform.position;
            CheckPassTrapEvent.Invoke();

        }
    }


    public void TrapCardPlayConfirm(Transform location, int map_num)
    {
        if (selectedCard != null)
        {
            if (selectedCard.GetComponent<CardDisplay>().card.type == 1)
            {
                // can not place trap if there is any enemy on grid.
                foreach (GameObject enemy in EnemyObjList)
                {
                    if (enemy.GetComponent<EnemyManager>().target_pos == location.position)
                    {
                        return;
                    }
                }
                // can not place trap if there is another trap on grid.
                foreach (GameObject the_trap in trap_list)
                {
                    if (the_trap.GetComponent<TrapDisplay>().trap.pos == map_num)
                    {
                        return;
                    }
                }

                if(map_num == last_blocked_space)
                {
                    return;
                }

                GameObject trap = Instantiate(trapPrefab, location.position, Quaternion.identity, canvas.Find("Enemy"));
                trap.GetComponent<TrapDisplay>().trap =
                    new Trap(selectedCard.GetComponent<CardDisplay>().card.cardName, selectedCard.GetComponent<CardDisplay>().card.damage, selectedCard.GetComponent<CardDisplay>().card.knockback
                    , map_num);

                //Audio when card played
                FindObjectOfType<AudioController>().PlaySound("SFXAction");

                trap_list.Add(trap);

                Energy -= selectedCard.GetComponent<CardDisplay>().card.cost;
                EnergyCount.text = Energy.ToString();
                Hands.Remove(selectedCard);
                Discard.Add(selectedCard.GetComponent<CardDisplay>().card);
                DiscardCount.text = Discard.Count.ToString();
                Destroy(selectedCard);
                Destroy(arrow);
                selectedCard = null;
                CardSpacingEvent.Invoke();

            }
        }
    }

    public void EnemyActionOneByOne()
    {
        // if the enemy is not at the end of the road, or have ability (if enemy move)
        if (EnemyMoveCounter < EnemyObjList.Count && EnemyIsMoving == true)
        {
            // initilise current enemy

            GameObject enemy = EnemyObjList[EnemyMoveCounter];


            int pos_num = enemy.GetComponent<EnemyManager>().current_num;
            // set the move direction positive
            enemy.GetComponent<EnemyManager>().is_posdir = true;

            //  trigger enemyMoveEffect
            EnemyEffect.Instance.MoveEffect(enemy);

            // remove the current enemy from enemy distribution, remove circles and arrows
            GameObject circle = enemy.transform.Find("Circle").gameObject;
            foreach (Transform child in circle.transform)
            {
                Destroy(child.gameObject);
            }
            //GameObject leftarrow = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("leftarrow").gameObject;
            //GameObject rightarrow = EnemyDistribution[pos_num][CurrentViewEnemyList[pos_num]].transform.Find("rightarrow").gameObject;
            GameObject leftarrow = enemy.transform.Find("leftarrow").gameObject;
            GameObject rightarrow = enemy.transform.Find("rightarrow").gameObject;
            leftarrow.SetActive(false);
            rightarrow.SetActive(false);
            circle.SetActive(false);
            // if the enemy moves, redistribute the position of enemy.
            if (enemy.GetComponent<EnemyManager>().enemy.movement != 0 &&
                enemy.GetComponent<EnemyManager>().current_num != EnemyPath.Count - 1 &&
                enemy.GetComponent<EnemyManager>().stunned == false)
            {
                EnemyDistribution[enemy.GetComponent<EnemyManager>().current_num].Remove(enemy);
            }
            else
            {
                CurrentViewEnemyList[pos_num] += 1;
            }
            UpdateCircleState(enemy.GetComponent<EnemyManager>().current_num);

            EnemyMoveCounter += 1;



            // if the enemy is not at the end of the road, or have ability (if enemy move)
            if (enemy.GetComponent<EnemyManager>().enemy.movement != 0 &&
                enemy.GetComponent<EnemyManager>().current_num != EnemyPath.Count - 1 &&
                enemy.GetComponent<EnemyManager>().stunned == false)
            {
                // cauculate the end moving position 
                if (enemy.GetComponent<EnemyManager>().target_num + enemy.GetComponent<EnemyManager>().enemy.movement + moveOneMoreStep <= EnemyPath.Count - 1)
                {


                    enemy.SetActive(true);
                    enemy.GetComponent<EnemyManager>().target_num += (enemy.GetComponent<EnemyManager>().enemy.movement + moveOneMoreStep);
                    enemy.GetComponent<EnemyManager>().target_pos = EnemyPath[enemy.GetComponent<EnemyManager>().target_num].transform.position;


                    // check trap
                    CheckPassTrapEvent.Invoke();

                }
                else
                {
                    // if the enemy have move to the end point of road
                    enemy.SetActive(true);
                    enemy.GetComponent<EnemyManager>().target_num = EnemyPath.Count - 1;
                    enemy.GetComponent<EnemyManager>().target_pos = EnemyPath[EnemyPath.Count - 1].transform.position;


                    CheckPassTrapEvent.Invoke();
                }
            }
            else
            {
                // CurrentViewEnemyList[pos_num] += 1;
                //UpdateCircleState(enemy.GetComponent<EnemyManager>().current_num);
                EnemyActionOneByOne();
            }


        }
        else
        {

            //UpdateCircleState(pos_num);
            EnemyIsMoving = false;
            EnemyMoveCounter = 0;
            //Debug.Log("action phase");


            if (GamePhase.ToString() == "enemyAction")
            {
                EnemyEffect.Instance.EndTurnEffect();
                GamePhase = GamePhase.playerAction;
                phaseChangeEvent.Invoke();
                NewTurnEvent.Invoke();
            }

        }

    }



    public void DrawCard(int draw_num)
    {

        if (draw_num <= Deck.Count)//enough card in Deck to draw
        {
            for (int i = 0; i < draw_num; i++)
            {
                GameObject card = Instantiate(cardPrefab, Hand);
                // Draw the first(top) card in deck
                card.GetComponent<CardDisplay>().card = Deck[0];
                Deck.RemoveAt(0);
                Hands.Add(card);

            }
        }
        else//not enough card in Deck to draw
        {
            int MoreToDraw = draw_num - Deck.Count;
            // draw the remaining cards
            for (int i = 0; i < Deck.Count; i++)
            {
                GameObject card = Instantiate(cardPrefab, Hand);
                // Draw the first(top) card in deck
                card.GetComponent<CardDisplay>().card = Deck[0];
                Deck.RemoveAt(0);
                Hands.Add(card);

            }

            Deck = new List<Card>(Discard); // use the discard pile as new deck
            Discard.Clear();
            Shuffle(); // shuffle the deck
            for (int i = 0; i < MoreToDraw; i++)
            {
                GameObject card = Instantiate(cardPrefab, Hand);
                // Draw the first(top) card in deck
                card.GetComponent<CardDisplay>().card = Deck[0];
                Deck.RemoveAt(0);
                Hands.Add(card);

            }

        }
        DeckCount.text = Deck.Count.ToString();
        DiscardCount.text = Discard.Count.ToString();
        CardSpacingEvent.Invoke();
    }


    public void NewTurn()
    {
        if (Energy > Deck.Count + Discard.Count)
        { Energy = Deck.Count + Discard.Count; }
        //1. draw cards
        DrawCard(Energy);

        //2. restore energy
        Energy = nextTurnEnergy;
        nextTurnEnergy = 7;
        EnergyCount.text = Energy.ToString();
        directDamageMultiplier = nextTurndirectDamageMultiplier;
        nextTurndirectDamageMultiplier = 1.0f;
        trapDamageMultiplier = nextTuentrapDamageMultiplier;
        nextTuentrapDamageMultiplier = 1.0f;
        CardBattle.Instance.updateSupportText();

        TurnCounter += 1;
        phaseChangeEvent.Invoke();

        //Special conditions
        EnemyEffect.Instance.DiscardHand = false;
        Destroy(enlarged_cardshown);

    }

    public void CreateArrow(Transform startPoint, GameObject arrowPrefab)
    {

        arrow = GameObject.Instantiate(arrowPrefab, canvas);
        arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(startPoint.position.x, startPoint.position.y));
    }

    public void DestroyArrow()
    {
        effectBoard.GetComponent<Image>().color = Color.white;
        selectedCard = null;
        Destroy(arrow);
    }

    public void OnClickEndturn()
    {

/*        if (GamePhase == GamePhase.gameStart)
        {
            EndturnButtonText.text = "End Turn";
            GamePhase = GamePhase.playerAction;
            phaseChangeEvent.Invoke();

        }
        else */
        if (GamePhase == GamePhase.playerAction)
        {
            GamePhase = GamePhase.enemyAction;
            phaseChangeEvent.Invoke();
            EnemyIsMoving = true;
            EnemyActionEvent.Invoke();
            Destroy(arrow);
            selectedCard = null;
        }
        //to be deleted
        if (EnemyObjList.Count == 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Destroy(enemyOnMap);
            SceneManager.LoadScene("TopDownScene");
        }


    }


    public void LeftClick(GameObject Enemy)
    {
        int num = Enemy.GetComponent<EnemyManager>().target_num;
        CurrentViewEnemyList[num] -= 1;
        UpdateCircleState(num);
    }

    public void RightClick(GameObject Enemy)
    {
        int num = Enemy.GetComponent<EnemyManager>().target_num;
        CurrentViewEnemyList[num] += 1;
        UpdateCircleState(num);
    }

    public void updateSupportText()
    {
        SupportEffectText.text = "Next Turn Energy: " + nextTurnEnergy.ToString() + "\n" +
            "Direct Attack damage * " + nextTurndirectDamageMultiplier.ToString() + "\n" +
            "Trap damage: * " + nextTuentrapDamageMultiplier.ToString();
    }
}
