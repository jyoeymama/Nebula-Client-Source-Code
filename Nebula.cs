// To anyone wondering about this script, This is an unfinished test of a hack client
// I suck at programming and I couldent get it to work
// I tried downloading unity vr and decompiling the APK for the game I was attempting to mod
// This was just a basic test on making a gui and trying to make it popup within the real game
// I failed yet agian :( 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NebulaClientUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject clientUI;           
    public CanvasGroup canvasGroup;       
    public Text fpsText;
    public Text pingText;

    [Header("Camera Rig Tracking")]
    public Transform cameraRig;           
    public Vector3 offset = new Vector3(0, 1.5f, 2f);

    private float deltaTime = 0.0f;
    private bool isVisible = false;

    void Start()
    {
        clientUI.SetActive(false);
        canvasGroup.alpha = 0f;
        StartCoroutine(UpdatePing());
    }

    void Update()
    {

        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            isVisible = !isVisible;
            StopAllCoroutines();
            StartCoroutine(FadeUI(isVisible));
            if (isVisible)
                StartCoroutine(UpdatePing());
        }


        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.Ceil(fps)}";


        if (isVisible)
        {
            Vector3 targetPosition = cameraRig.position + cameraRig.forward * offset.z + Vector3.up * offset.y;
            clientUI.transform.position = targetPosition;
            clientUI.transform.LookAt(cameraRig.position);
            clientUI.transform.Rotate(0, 180, 0); 
        }
    }

    IEnumerator UpdatePing()
    {
        while (isVisible)
        {
            Ping ping = new Ping("8.8.8.8");
            yield return new WaitForSeconds(0.5f);
            pingText.text = $"Ping: {ping.time} ms";
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator FadeUI(bool show)
    {
        if (show)
            clientUI.SetActive(true);

        float target = show ? 1f : 0f;
        float start = canvasGroup.alpha;
        float duration = 0.25f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }

        canvasGroup.alpha = target;
        if (!show)
            clientUI.SetActive(false);
    }
}
