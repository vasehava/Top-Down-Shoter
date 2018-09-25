using UnityEngine.UI;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{

    private float deltaTime;
    private Text fps;

    private void Awake()
    {
        #if UNITY_ANDROID
            Application.targetFrameRate = 60;
        #endif
    }
    private void Start()
    {
        fps = GetComponent<Text>();
    }
    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        // fps.text = (1 / deltaTime).ToString();
        fps.text = string.Format("{0:0.#}", (1 / deltaTime));
    }
}
