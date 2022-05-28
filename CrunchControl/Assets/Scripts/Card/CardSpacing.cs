using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSpacing : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;
    public float dotWidth = 200f;
    public RectTransform lengthRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        CardBattle.Instance.CardSpacingEvent.AddListener(changeSpacing);
    }

    // Update is called once per frame
    void Update()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
        lengthRectTransform = GetComponent<RectTransform>();
    }

    public void changeSpacing()
    {

        int dotCount = transform.childCount;
        if (EnemyEffect.Instance.DiscardHand == true)
        {
            dotCount = CardBattle.Instance.Energy;
        }
        layoutGroup.spacing = (lengthRectTransform.rect.width - (dotCount * dotWidth)) / (dotCount - 1);
        if (layoutGroup.spacing > 20)
        {
            layoutGroup.spacing = 20;

        }

    }
}
