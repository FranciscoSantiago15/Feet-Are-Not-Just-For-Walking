using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FootScale : MonoBehaviour
{
    public XRNode footSource;

    public XRNode inputSource;
    public InputHelpers.Button inputButton;


    private GameObject heel;
    private Vector3 posHeel;
    private bool isHeelCreated = false;


    // Start is called before the first frame update
    void Start()
    {        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHeelCreated){
            Vector3 auxPos = heel.transform.position;
            InputDevices.GetDeviceAtXRNode(footSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 auxP);
            InputDevices.GetDeviceAtXRNode(footSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion auxR);



            heel.transform.position = auxP + posHeel;
            heel.transform.rotation = auxR;

            
        } else {
            InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isInputButtonPressed, 0.1f);
            if(isInputButtonPressed){
                heel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                heel.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                InputDevices.GetDeviceAtXRNode(inputSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out posHeel);

                heel.transform.position = posHeel;

                isHeelCreated = true;

            }
        }
        
    }
}
