using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;
    private RectTransform arrow;

    private float arrowLength;
    private float theta;
    private Vector2 arrowPosition;

    // Start is called before the first frame update
    void Start()
    {
        arrow = transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //to be modified
        endPoint = Input.mousePosition - new Vector3(960.0f, 540.0f, 0.0f);
        arrowLength = Mathf.Sqrt((endPoint.x - startPoint.x) * (endPoint.x - startPoint.x) + (endPoint.y - startPoint.y) * (endPoint.y - startPoint.y));
        arrowPosition = new Vector2((endPoint.x + startPoint.x) / 2, (endPoint.y + startPoint.y) / 2);
        theta = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));

        arrow.localPosition = arrowPosition;
        arrow.sizeDelta = new Vector2(arrowLength, arrow.sizeDelta.y);
        arrow.localEulerAngles = new Vector3(0.0f, 0.0f, theta * 180 / Mathf.PI);

    }

    public void SetStartPoint(Vector2 startPoint)
    {
        //to be modified
        this.startPoint = startPoint - new Vector2(960.0f, 540.0f);
        
    }
}
