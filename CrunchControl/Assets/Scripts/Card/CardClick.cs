using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BattleCardState
{
    inHand, inDiscardPile, inDeck
}
public class CardClick : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cachedScale;
    public Transform canvas;
    GameObject Card_show;
    public GameObject EnlargePlace;
    public GameObject CardViewPrefab;

    public BattleCardState state = BattleCardState.inHand;
    public void OnPointerDown(PointerEventData eventData)
    {

        //1.Clicked from hand
        if (state == BattleCardState.inHand)
        {
            if (CardBattle.Instance.GamePhase.ToString() == "playerAction")
            {
                CardBattle.Instance.CardPlayRequest(transform, transform.gameObject);
            }
        }
        //2.Clicked from deck/discard pile
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Card_show = Instantiate(CardViewPrefab, transform.position, Quaternion.identity, transform.parent.parent);
        Card_show.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Card_show.GetComponent<CardDisplay>().card = gameObject.GetComponent<CardDisplay>().card;
        CardBattle.Instance.enlarged_cardshown = Card_show;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // transform.localScale = cachedScale;
        Destroy(Card_show);
    }

    // Start is called before the first frame update
    void Start()
    {
        cachedScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
