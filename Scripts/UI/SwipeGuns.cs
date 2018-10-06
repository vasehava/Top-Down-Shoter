using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SwipeGuns : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private UserInput userInput = null;
    public Image wepIcon;
    public Text wepName, wepNumber;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        userInput = null;
        userInput = GameObject.Find("Player(Clone)").GetComponent<UserInput>();
        if (userInput == null)
            Debug.Log(":::NULL");
        wepIcon.sprite = userInput.weps.WeaponIcon;
        wepName.text = userInput.weps.WeaponName;
        wepNumber.text = "#"+(userInput.weps.Index + 1) + " of " + userInput.weps.weapons.Count;
        userInput.wepList = wepNumber;
    }
    public void UpdateWeaponsCount()
    {
        wepNumber.text = "#" + (userInput.weps.Index + 1) + " of " + userInput.weps.weapons.Count;
    }
    public void SwitchGun()
    {
        userInput.weps.Next();
        wepIcon.sprite = userInput.weps.WeaponIcon;
        wepName.text = userInput.weps.WeaponName;
        wepNumber.text = "#" + (userInput.weps.Index + 1) + " of " + userInput.weps.weapons.Count;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //if(eventData.delta.x > 0)
        //{
        //    userInput.weps.Next();
        //    wepIcon.sprite = userInput.weps.WeaponIcon;
        //    wepName.text = userInput.weps.WeaponName;
        //}
        //else if(eventData.delta.x < 0)
        //{
        //    userInput.weps.Prev();
        //    wepIcon.sprite = userInput.weps.WeaponIcon;
        //    wepName.text = userInput.weps.WeaponName;
        //}
    }
    public void SwipeGun()
    {
        userInput.weps.Next();
        wepIcon.sprite = userInput.weps.WeaponIcon;
        wepName.text = userInput.weps.WeaponName;
    }
    public void OnDrag(PointerEventData eventData)
    {

    }


	
	// Update is called once per frame
	void Update () {
		
	}
}
