using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{

    public Text HUD;

    private GameObject ObjectToControl;

    public float MovementRatio;

    public XRNode tracker;

    private GameObject leftSelection, rightSelection;

    private Vector3 inicialAng;

    private Vector3 position;
    private Quaternion rotation;

    public GameObject cube;

    public bool TrackerRot = false;

    public bool onFoot = false;

    private int interval = 21;

    private List<float> rotX, rotY, rotZ;

    private int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        ObjectToControl = this.gameObject;

        position = ObjectToControl.transform.position;
        rotation = ObjectToControl.transform.localRotation;

        /*inicialAng = new Vector3(0, 160f, 132f);
        
        leftSelection = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftSelection.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        

        rightSelection = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightSelection.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);*/

        rotX = new List<float>();
        rotY = new List<float>();
        rotZ = new List<float>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = new Vector3();
        Vector3 rot = new Vector3();
        Quaternion auxRot = new Quaternion();
        Vector3 angVeloc = new Vector3();


        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(CommonUsages.devicePosition, out p);
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(CommonUsages.deviceRotation, out auxRot);
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out angVeloc);
        rot = auxRot.eulerAngles;       


        HUD.text = rot.ToString() + "\n" + angVeloc;

        var cubeRot = cube.transform.rotation;

        ObjectToControl.transform.rotation = cubeRot;

        if(TrackerRot)
            cube.transform.rotation = auxRot;

        if(onFoot)
            cube.transform.position = p;


        if(Time.frameCount % interval == 0) {
            rotX.Add(rot.x);
            rotY.Add(rot.y);
            rotZ.Add(rot.z);
        }


        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), InputHelpers.Button.PrimaryButton, out bool isInputButtonPressed, 1);

        if(isInputButtonPressed){
            //Debug.Log(rot.ToString());
            Debug.Log(rotX + "\n" + rotY + "\n" + rotZ);
            rotX.Clear();
            rotY.Clear();
            rotZ.Clear();
        }
            

        if((rot.y >= 42.3 && rot.y <= 62.1) && (rot.z <= 310.5 && rot.z >= 285.1)){
            ObjectToControl.transform.position = new Vector3(ObjectToControl.transform.position.x + 0.01f, ObjectToControl.transform.position.y, ObjectToControl.transform.position.z);
        } 

        if((rot.y <= 309.5 && rot.y >= 284.6) && (rot.z >= 53.5 && rot.z <= 79.3)){
            ObjectToControl.transform.position = new Vector3(ObjectToControl.transform.position.x - 0.01f, ObjectToControl.transform.position.y, ObjectToControl.transform.position.z);
        }

        
    }

}
