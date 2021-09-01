using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;


public class BarFacingHeadset : MonoBehaviour
{
    public enum TypeOfRepresentation { OnObject, OnController, OnHeadset, None }
    
    [Header("FeedBack")]
    [SerializeField] TypeOfRepresentation TypeOfFeedback;

    private List<UnityEngine.XR.InputDevice> allDevices = new List<UnityEngine.XR.InputDevice>();
    
    private InputDevice headset;

    private InputDevice controller;

    private Canvas canvas;

    private Slider slider;

    private Text text;

    public FootButton2 footButton;

    private Vector3 originalPosition, originalScale;

    private Color originalColor;

    private string footButtonName;

    private bool auxState = false;


    void Start()
    {

        UnityEngine.XR.InputDevices.GetDevices(allDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, allDevices);
        foreach (var device in allDevices){
            //Debug.Log(string.Format("NAME: '{0}' with role '{1}'", device.name, device.characteristics));
            headset = device;
        }     

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, allDevices);
        foreach (var device in allDevices){
            //Debug.Log(string.Format("NAME: '{0}' with role '{1}'", device.name, device.characteristics));
            controller = device;
        }             

        canvas = gameObject.GetComponentInChildren<Canvas>(); 
        slider = gameObject.GetComponentInChildren<Slider>();
        text = gameObject.GetComponentInChildren<Text>();
        originalPosition = slider.transform.position;
        originalScale = slider.transform.localScale;

        originalColor = footButton.gameObject.GetComponentInChildren<MeshRenderer>().material.color;
        footButtonName = transform.parent.name;
    }

    // Update is called once per frame
    void Update()
    {   
        text.text = string.Format("{0}       {1:#0.00}%",footButtonName, footButton.GetValue() * 100);
        slider.value = footButton.GetValue();

        Vector3 headsetPos = new Vector3();
        headset.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out headsetPos);
        Quaternion headsetRot = new Quaternion();
        headset.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out headsetRot);

        CheckButtonIsUsed(auxState);


        if(TypeOfFeedback == TypeOfRepresentation.OnObject){    
            if(auxState) {
                canvas.enabled = true;
                footButton.gameObject.GetComponentInChildren<MeshRenderer>().material.color = originalColor;
                auxState = false;
            }

            slider.transform.position = originalPosition;    
            slider.transform.LookAt(headsetPos);
            slider.transform.localScale = originalScale;

        }else if(TypeOfFeedback == TypeOfRepresentation.OnController){
            if(auxState) {
                canvas.enabled = true;
                footButton.gameObject.GetComponentInChildren<MeshRenderer>().material.color = originalColor;
                auxState = false;
            }

            Vector3 contPos = new Vector3(); 
            Quaternion contRot = new Quaternion();
                
            controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out contPos);
            controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out contRot);

            slider.transform.localScale = originalScale * 0.3f;
            slider.transform.position = new Vector3(contPos.x, contPos.y + 0.1f, contPos.z);
            slider.transform.LookAt(headsetPos);

            //transform.localEulerAngles = new Vector3(contRot.eulerAngles.x, contRot.eulerAngles.y - 180, contRot.eulerAngles.z);
        } else if (TypeOfFeedback == TypeOfRepresentation.OnHeadset){
            if(auxState) {
                canvas.enabled = true;
                footButton.gameObject.GetComponentInChildren<MeshRenderer>().material.color = originalColor;
                auxState = false;
            }

            var posFrontHeadset = headsetPos + headsetRot * Vector3.forward * 0.2f;
            slider.transform.position = posFrontHeadset;
            slider.transform.LookAt(headsetPos);
            slider.transform.localScale = originalScale * 0.3f;
        } else {
            auxState = true;
            footButton.gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(1* footButton.GetValue(), 0, 0, 1);
        }
        
    }

    void CheckButtonIsUsed(bool auxState) {
        if(!auxState){
            if(footButton.GetValue() == 0){
                canvas.enabled = false;
            } else {
                canvas.enabled = true;
            }
        }
    }

}
