using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SwipeGuns : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private UserInput userInput = null;
    public Image wepIcon;
    public Text wepName;

    // Use this for initialization
    void Start()
    {
        userInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UserInput>();
        wepIcon.sprite = userInput.weps.WeaponIcon;
        wepName.text = userInput.weps.WeaponName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.delta.x > 0)
        {
            userInput.weps.Next();
            wepIcon.sprite = userInput.weps.WeaponIcon;
            wepName.text = userInput.weps.WeaponName;
        }
        else if(eventData.delta.x < 0)
        {
            userInput.weps.Prev();
            wepIcon.sprite = userInput.weps.WeaponIcon;
            wepName.text = userInput.weps.WeaponName;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
