using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MapClick : MonoBehaviour, IPointerClickHandler { 
    public int map_num;

    public void OnPointerClick(PointerEventData eventData)
    {
        CardBattle.Instance.TrapCardPlayConfirm(transform, map_num);

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
