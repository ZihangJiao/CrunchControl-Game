using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyIDDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardBattle.Instance.selectedCard == null)
        {
            CardBattle.Instance.enemyIDImage.GetComponent<Image>().sprite = transform.parent.gameObject.GetComponent<EnemyManager>().getEnemyImage();
           // CardBattle.Instance.canvas.FindChild("HandArea").gameObject.SetActive(false);
            CardBattle.Instance.Hand.gameObject.SetActive(false);
            CardBattle.Instance.enemyIDImage.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // transform.localScale = cachedScale;
        CardBattle.Instance.enemyIDImage.SetActive(false);
        CardBattle.Instance.Hand.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
