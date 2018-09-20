using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Right_Stick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image joystickBG;
    private Image joystick;
    private Vector2 inputVector;
    private bool shooting = false;

    // Use this for initialization
    void Start()
    {
        joystickBG = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    { 
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = -(pos.x / joystickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBG.rectTransform.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            if (inputVector.magnitude > 1.2f)
                shooting = true;
            else shooting = false;

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystick.rectTransform.anchoredPosition = new Vector2(-inputVector.x * (joystickBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystickBG.rectTransform.sizeDelta.y / 2));
        }
        //print(pos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("OnPointerDown");
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //print("OnPointerUp");
        inputVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
        shooting = false;
    }


	
	// Update is called once per frame
	void Update () {
		
	}

    public float InputVector_X
    {
        get { return -inputVector.x; }
    }
    public float InputVector_Y
    {
        get { return inputVector.y; }
    }
    public bool Fire
    {
        get { return shooting; }
    }

}
