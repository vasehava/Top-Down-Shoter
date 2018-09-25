using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Weapons
{
    
    public List<Weapon> weapons = new List<Weapon>();
    private int index = 0;
    private Animator animator;
    public Shooting shooting;

    public void Next()
    {
        index = index == weapons.Count - 1 ? 0 : index + 1;
        int prev = index == 0 ? weapons.Count - 1 : index - 1;
        weapons[prev].RemoveGun();
        weapons[index].SetUpGun(animator);
        shooting = weapons[index].GetShooting;
    }
    public void Prev()
    {
        index = index == 0 ? weapons.Count - 1 : index - 1;
        int prev = index == 0 ? weapons.Count - 1 : index - 1;
        weapons[prev].RemoveGun();
        weapons[index].SetUpGun(animator);
        shooting = weapons[index].GetShooting;
    }
    public void SetUpWeapons(Animator anim)
    {
        animator = anim;
        weapons[index].SetUpGun(animator);
        shooting = weapons[index].GetShooting;
    }

    public string WeaponName
    {
        get
        {
            return weapons[index].weaponName;
        }
    }
    public Sprite WeaponIcon
    {
        get
        {
            return weapons[index].weaponIcon;
        }
    }
}
//This is the script that we write everything that has to do with the User Input
public class UserInput : MonoBehaviour
{

    public float speed = 5;
    public bool hidemouse;
    public AudioClip stepClip;
    public Weapons weps;



    private AudioSource audioSource;
    private Transform cam;
    private Vector3 camForward; //stores the forward vector of the cam
    private Vector3 move;
    private Vector3 moveInput;

    private Image powerUpIcon;
    private Slider powerUpSlider;

    private float forwardAmount;
    private float turnAmount;
    private bool speedBoosted;
    private Right_Stick r_stick;
    private Left_Stick l_stick;
    private Vector3 lookPos;
    private Animator anim;
    private Rigidbody rigidBody;
    [HideInInspector]
    public ReloadUI reloadUI;

    void Start()
    {
 
        try
        {
            SetUpAnimator();
            weps.SetUpWeapons(anim);
            Debug.Log(weps.WeaponIcon.name);
            cam = Camera.main.transform;

            anim = GetComponent<Animator>();

            rigidBody = GetComponent<Rigidbody>();

            audioSource = GetComponent<AudioSource>();
            GameObject obj = GameObject.Find("GameplayCanvas");//.GetComponentInChildren<Right_Stick>();
            r_stick = obj.GetComponentInChildren<Right_Stick>();
            l_stick = obj.GetComponentInChildren<Left_Stick>();

            powerUpIcon = obj.GetComponentInChildren<PowerUpsUI>().GetComponentInChildren<Image>(true);
            powerUpSlider = obj.GetComponentInChildren<PowerUpsUI>().GetComponentInChildren<Slider>(true);

            reloadUI = obj.GetComponentInChildren<ReloadUI>();
        }
        catch
        {

        }
        StartCoroutine("FootSteps");
    }

    void Update()
    {
        try
        {
            if (r_stick.Fire)
                weps.shooting.Invoke();


            Vector3 lookDir = new Vector3(r_stick.InputVector_X, 0, r_stick.InputVector_Y);
            lookDir.y = 0;
            transform.LookAt(transform.position + lookDir, Vector3.up);
        }
        catch
        {
            
        }
    }

    void FixedUpdate()
    {
        float horizontal = l_stick.InputVector_X;   //Input.GetAxis("Horizontal");
        float vertical = l_stick.InputVector_Y; //Input.GetAxis("Vertical");

        if (cam != null) 
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
			move = vertical * camForward + horizontal * cam.right;
        }
        else
        {
            move = vertical * Vector3.forward + horizontal * Vector3.right;
        }

        if (move.magnitude > 1) 
            move.Normalize();

        Move(move);

        Vector3 movement = new Vector3(horizontal, 0, vertical);
      
        if (horizontal == 0 && vertical == 0)
        {
            rigidBody.velocity = Vector3.zero * 0.5f;
        }
       

        rigidBody.AddForce(movement * speed / Time.deltaTime);
        
        
    }

    IEnumerator FootSteps()
    {
        while (true)
        {
            if (rigidBody.velocity.magnitude > 2)
                audioSource.PlayOneShot(stepClip);
            yield return new WaitForSeconds(speed);
        }
    }
    void Move(Vector3 move)
    {
        //Vector3 move is the input in word space
        if (move.magnitude > 1)
            move.Normalize();

        this.moveInput = move; //store the move

        ConvertMoveInput();
        UpdateAnimator();

    }

    void ConvertMoveInput()
    {

        Vector3 localMove = transform.InverseTransformDirection(moveInput);

        turnAmount = localMove.x;

   
        //Improves sideways speed
        if (localMove.z < -0.3f)
        {
            localMove.z = -1;
        }
        else if (localMove.z > 0.3f)
        {
            localMove.z = 1;
        }
        
        forwardAmount = localMove.z;
    }

    void UpdateAnimator()
    {
        anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Sideways", turnAmount, 0.1f, Time.deltaTime);
    }

    void SetUpAnimator()
    {
        // this is a ref to the animator component on the root.
        anim = GetComponent<Animator>();

        // we use avatar from a child animator component if present
        // this is to enable easy swapping of the character model as a child node
        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break; //if you find the first animator, stop searching
            }
        }
    }

    public bool SpeedBoost(float time, Sprite icon)
    {
        if (!speedBoosted)
        {
            powerUpIcon.sprite = icon;
            powerUpIcon.gameObject.SetActive(true);
            StartCoroutine(_SpeedBoost(time));
            return true;
        }
        return false;
    }

    private IEnumerator _SpeedBoost(float time)
    {
        speedBoosted = true;
        this.speed = 0.7f;
        powerUpSlider.value = 0;
        powerUpSlider.gameObject.SetActive(true);
        for (int i = 0; i <= powerUpSlider.maxValue; i++)
        {
            powerUpSlider.value += 1;
            yield return new WaitForSeconds(time/powerUpSlider.maxValue);

        }
        speed = 0.5f;
        powerUpIcon.gameObject.SetActive(false);
        powerUpSlider.gameObject.SetActive(false);
        speedBoosted = false;
    }

    void OnDisable()
    {
        
    }
    
}

   