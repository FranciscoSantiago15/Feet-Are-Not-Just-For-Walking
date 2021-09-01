using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Dummy : MonoBehaviour
{
    public MenuMB MainMenuPrefab;
    public enum Representation { Direct, Indirect, Controller }

    [SerializeField] public Representation representation;
    protected MenuMB MainMenuInstance;

    public bool menuFollowingPlayer = false;


    [HideInInspector] public Canvas canvas;

    private XRNode controller = XRNode.RightHand;
    private XRNode headset = XRNode.Head;



    void Start()
    {
        canvas = this.GetComponentInChildren<Canvas>();
        MainMenuInstance = Instantiate(MainMenuPrefab, canvas.transform);

        if(representation == Representation.Direct)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.transform.position = new Vector3(0, 0.001f, 0);
            canvas.transform.rotation = Quaternion.Euler(90, 0, 0);
            canvas.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            canvas.worldCamera = Camera.main;
        } 
        else if(representation == Representation.Indirect)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.planeDistance = 0.5f;
            canvas.scaleFactor = 0.5f;
            canvas.worldCamera = Camera.main;
        }
        else if(representation == Representation.Controller)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.transform.localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);
            canvas.worldCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.devicePosition, out var posCont);
        InputDevices.GetDeviceAtXRNode(headset).TryGetFeatureValue(CommonUsages.deviceRotation, out var rotHead);

        if(representation == Representation.Controller)
        {
            canvas.transform.position = posCont;
            canvas.transform.LookAt(Camera.main.transform);
        }
    }
}
