using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Runtime.InteropServices;



public class GestureRecognizer : MonoBehaviour
{    

    private Vector3 posInic;
    private bool aux = false;

    private GameObject auxPrefab;

    private Text HUDText;

    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputTreshold = 0.1f;
    public XRNode movementSource;
    
    public GameObject debugPrefab;

    public float recognitionTreshold = 0.9f;


    private List<Vector3> positionList = new List<Vector3>();


    //_____________________________________________________________________________________________________________________________________
    private GameObject active_controller = null;

    private GestureRecognition gr = new GestureRecognition();
    
    public Text HUD;
    
    
    private int recording_gesture = -1;


    // Temporary storage for objects to display the gesture stroke.
    List<string> stroke = new List<string>(); 

    // Temporary counter variable when creating objects for the stroke display:
    int stroke_index = 0; 

    GCHandle me;

    [SerializeField] public string LoadGesturesFile;



    void Start()
    {
        LoadGest();
    }

    public void LoadGest()
    {
        HUD = GameObject.Find("Text").GetComponent<Text>();

        int ret = gr.loadFromFile(LoadGesturesFile);
        
        if(ret != 0){
            Debug.Log("Failed to load foot gestures database file [Missing/Not Found file .dat]");
            return;
        } else {
            //HUD.text = printAllGesturesNames();
            //Debug.Log(printAllGesturesNames());
        }
    }

    void Update() {
        OnInteraction();
        //getGestures();
    }


    public void OnInteraction()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isInputButtonPressed, inputTreshold);
        //InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), InputHelpers.Button.PrimaryButton, out bool isPrimaryButtonPressed, inputTreshold);


        if(active_controller == null){
            if(isInputButtonPressed) {
                active_controller = GameObject.Find("RightHand Controller");
            }

            GameObject hmd = GameObject.Find("Main Camera"); // alternative: Camera.main.gameObject
            Vector3 hmd_p = hmd.transform.position;
            Quaternion hmd_q = hmd.transform.rotation;
            gr.startStroke(hmd_p, hmd_q, recording_gesture);

        }


        if(isInputButtonPressed){

            Vector3 p = new Vector3();
            Quaternion q = new Quaternion();

            InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out p);
            InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out q);

            gr.contdStrokeQ(p,q);

            if(debugPrefab){
                Destroy(Instantiate(debugPrefab, p, q), 3);
            }
            return;
        }

        active_controller = null;

        double similarity = 0; // This will receive a value of how similar the performed gesture was to previous recordings.
        Vector3 pos = Vector3.zero; // This will receive the position where the gesture was performed.
        double scale = 0; // This will receive the scale at which the gesture was performed.
        Vector3 dir0 = Vector3.zero; // This will receive the primary direction in which the gesture was performed (greatest expansion).
        Vector3 dir1 = Vector3.zero; // This will receive the secondary direction of the gesture.
        Vector3 dir2 = Vector3.zero; // This will receive the minor direction of the gesture (direction of smallest expansion).
        int gesture_id = gr.endStroke(ref similarity, ref pos, ref scale, ref dir0, ref dir1, ref dir2);

        double similarity2 = 0;
        int indentified_gesture =  gr.endStroke(ref similarity2);

        //Debug.Log(gesture_id + "\n" + similarity);

        for(int i = 0; i < gr.getListLenghtGestures(); i++){
            if(i == gesture_id) { 
                HUD.text = gr.getGestureName(i);
                Debug.Log(gr.getGestureName(i) + "\t [" + similarity + "]");
            }
        }      
    }


    string printAllGesturesNames(){
        string gestList = "Gestures supported by the component: \n";
        for(int i = 0; i < gr.getListLenghtGestures(); i++){
            gestList += " - " + gr.getGestureName(i) + "\n";
        }

        return gestList;
    }


    public void getGestures(){
        InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out Vector3 veloc);

        InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 p);
        InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion q);

        if((veloc.x <= -0.1 || veloc.x >= 0.1 ) || (veloc.y <= -0.1 || veloc.y >= 0.1 ) || (veloc.z <= -0.1 || veloc.z >= 0.1 )){
            Debug.Log("is Moving");
            if(aux){
                HUD.text = veloc.x + "\n" + veloc.y + "\n" + veloc.z;
                MovUpdate();
            } else {
                posInic = p;
                auxPrefab = Instantiate(debugPrefab, p, q);
                aux = true;
            }
        } 
    }

    void MovUpdate() {

        GameObject hmd = GameObject.Find("Main Camera"); // alternative: Camera.main.gameObject
        Vector3 hmd_p = hmd.transform.position;
        Quaternion hmd_q = hmd.transform.rotation;
        gr.startStroke(hmd_p, hmd_q, recording_gesture);

        Vector3 p = new Vector3();
        Quaternion q = new Quaternion();

        InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out p);
        InputDevices.GetDeviceAtXRNode(movementSource).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out q);

        gr.contdStrokeQ(p,q);
    }


    void RecMov() {

        double similarity = 0; // This will receive a value of how similar the performed gesture was to previous recordings.
        Vector3 pos = Vector3.zero; // This will receive the position where the gesture was performed.
        double scale = 0; // This will receive the scale at which the gesture was performed.
        Vector3 dir0 = Vector3.zero; // This will receive the primary direction in which the gesture was performed (greatest expansion).
        Vector3 dir1 = Vector3.zero; // This will receive the secondary direction of the gesture.
        Vector3 dir2 = Vector3.zero; // This will receive the minor direction of the gesture (direction of smallest expansion).
        int gesture_id = gr.endStroke(ref similarity, ref pos, ref scale, ref dir0, ref dir1, ref dir2);

        double similarity2 = 0;
        int indentified_gesture =  gr.endStroke(ref similarity2);

        //Debug.Log(gesture_id + "\n" + similarity);

        for(int i = 0; i < gr.getListLenghtGestures(); i++){
            if(i == gesture_id) { 
                HUD.text = gr.getGestureName(i) + "\t [" + similarity + "]";
                //Debug.Log(gr.getGestureName(i) + "\t [" + similarity + "]");
            }
        }      
    }


    public void notMoving() {
        Destroy(auxPrefab, 3);
        aux = false;
        posInic = Vector3.zero;
    }
}
