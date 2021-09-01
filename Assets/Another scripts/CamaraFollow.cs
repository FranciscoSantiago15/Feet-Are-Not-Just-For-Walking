using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class CamaraFollow : MonoBehaviour
{

    public bool RotationMenu;

    private List<UnityEngine.XR.InputDevice> allDevices = new List<UnityEngine.XR.InputDevice>();

    private UnityEngine.XR.InputDevice headset;

    private Transform ObjectThatFollow;
    

    void Start()
    {
        ObjectThatFollow = this.transform;

        UnityEngine.XR.InputDevices.GetDevices(allDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, allDevices);
        foreach (var device in allDevices){
            //Debug.Log(string.Format("NAME: '{0}' with role '{1}'", device.name, device.characteristics));
            headset = device;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HEADSET: " + headet.transform.position);
        Vector3 HeadsetPos = new Vector3();
        headset.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out HeadsetPos);

        ObjectThatFollow.position = new Vector3(HeadsetPos.x, ObjectThatFollow.position.y, HeadsetPos.z);
        
        if(RotationMenu == true){
            Quaternion q;
            headset.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out q);

            ObjectThatFollow.rotation = Quaternion.Euler(ObjectThatFollow.rotation.eulerAngles.x, q.eulerAngles.y, ObjectThatFollow.rotation.eulerAngles.z);

            
        }       
    }
}
