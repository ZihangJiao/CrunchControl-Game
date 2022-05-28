using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Text CostText;
    public Text NameText;
    public Text DescriptionText;
    public Text DamegeText;
    public Text KnockbackText;
    public Text CardtypeText;
    public bool AOE = false;
    public Card card;
    
    // Start is called before the first frame update
    void Start()
    {
        ShowCard();
        LoadCardImage();
        InitialiseCardEffect();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCard()
    {
        CostText.text = card.cost.ToString();
        NameText.text = card.cardName.ToString();
        DescriptionText.text = card.description.ToString();
        DamegeText.text = card.damage.ToString();
        KnockbackText.text = card.knockback.ToString();


    }

    public void LoadCardImage()
    {
        string LoadPath = "";
        if (card.type == 0)
        {
            CardtypeText.text = "Direct Attack Card";
            LoadPath = "Art/Cards/DirectAttack/" + NameText.text;

        }
        else if (card.type == 1)
        {
            CardtypeText.text = "Trap Card";
            LoadPath = "Art/Cards/Trap/" + NameText.text;
        }

        else if (card.type == 2)
        {
            CardtypeText.text = "Support Card";
            LoadPath = "Art/Cards/Support/" + NameText.text;
        }

        Sprite image;

        if (image = Resources.Load<Sprite>(LoadPath))
        {
            gameObject.GetComponent<Image>().sprite = image;
            gameObject.GetComponent<Image>().color = Color.white;
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

        }
    } 

    public void InitialiseCardEffect()
    {
        if (card.cardName.ToString() == "Staple Gun" || card.cardName.ToString() == "Quantum Paper Plane")
        {
            AOE = true;
        }
    }

}
