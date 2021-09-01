using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;


public class Gaze : MonoBehaviour
{    

    //private GestureRecognizer gestRec;
    public Sprite reticleTarget;
    public Sprite reticleSelection;

    public bool dynamicReticle = false;

    private GameObject reticles, retTargetObj, retSelectionObj;

    [Range(0f, 3f)] public float reticleScale = 0.05f;

    [SerializeField] private string selectableTag = "Selectable";

    [SerializeField] private Outline.Mode outlineMode;
    [SerializeField] private Color outlineColor; 
    [SerializeField, Range(0f, 10f)] private float outlineWidth = 2f;

    private Transform _selection;

    private GameObject headset;



    /*public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputTreshold = 0.1f;
    public XRNode movementSource;
    
    public GameObject debugPrefab;

    public float recognitionTreshold = 0.9f;*/



    void Awake() {

       /* gestRec = new GestureRecognizer();

        gestRec.inputSource = inputSource;
        gestRec.inputButton = inputButton;
        gestRec.inputTreshold = inputTreshold;
        gestRec.movementSource = movementSource;
        gestRec.debugPrefab = debugPrefab;
        gestRec.recognitionTreshold = recognitionTreshold;
        gestRec.LoadGesturesFile = "Assets/GestureRecognition/FootGestLib.dat";
        Text HUD = GameObject.Find("Text").GetComponent<Text>();
        gestRec.HUD = HUD;

        gestRec.LoadGest();*/

        headset = GameObject.Find("Main Camera");

        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag(selectableTag);

        foreach(GameObject go in selectableObjects){            
            go.AddComponent<Outline>();
            go.GetComponent<Outline>().OutlineMode = outlineMode;
            outlineColor.a = 1;
            go.GetComponent<Outline>().OutlineColor = outlineColor;  
            go.GetComponent<Outline>().OutlineWidth = outlineWidth;
            go.GetComponent<Outline>().enabled = false;             
        }


        CreateReticles();
        

    }

    void FixedUpdate(){
        if(_selection != null){
            var auxOutline = _selection.GetComponent<Outline>();
            auxOutline.enabled = false;
            ReticleExit();
            _selection = null;
        } /*else {
            gestRec.notMoving();
        }*/

        var posFrontHeadset = headset.transform.position; //+ headset.transform.rotation * Vector3.forward * 1.0f;

        RaycastHit hit;
        Ray rayDirection = new Ray(posFrontHeadset, headset.transform.forward);
        Debug.DrawRay(posFrontHeadset, headset.transform.forward * 5, Color.black, 0.2f);  

        ReticleOrientation(posFrontHeadset);

        if(Physics.Raycast(rayDirection, out hit)){
            
            if(hit.collider.tag == selectableTag){
                ReticleEnter();

                var selection = hit.transform;
                var auxOutline = selection.GetComponent<Outline>();
                auxOutline.enabled = true;
                _selection = selection;
            } 
        }
    }

    void Update() {         

        RaycastHit hit;
        float distance;
        
        if(dynamicReticle){
            if(Physics.Raycast(new Ray(headset.transform.position, headset.transform.rotation * Vector3.forward * 0.1f), out hit)){
                    distance = hit.distance - 0.01f;
            } else {
                distance = hit.distance - 0.01f;           
            }
        } else {
            distance = 1.0f;
        }
        var posFrontHeadset = headset.transform.position + headset.transform.rotation * Vector3.forward * distance;   
        ReticleOrientation(posFrontHeadset);

    }


    void CreateReticles() {
        reticles = new GameObject("Reticles");
        retTargetObj = new GameObject("Target");
        retSelectionObj = new GameObject("Selection");

        retTargetObj.transform.parent = reticles.transform;
        retSelectionObj.transform.parent = reticles.transform;

        retTargetObj.AddComponent<SpriteRenderer>().sprite = reticleTarget;
        retSelectionObj.AddComponent<SpriteRenderer>().sprite = reticleSelection;

        retTargetObj.transform.localScale = new Vector3(reticleScale, reticleScale, reticleScale);       
        retSelectionObj.transform.localScale = new Vector3(reticleScale, reticleScale, reticleScale);

        retTargetObj.GetComponent<SpriteRenderer>().enabled = true;
        retSelectionObj.GetComponent<SpriteRenderer>().enabled = false;

    }

    public void ReticleEnter(){
        retTargetObj.GetComponent<SpriteRenderer>().enabled = false;
        retSelectionObj.GetComponent<SpriteRenderer>().enabled = true;

        //gestRec.OnInteraction();

        //gestRec.getGestures();

    } 

    public void ReticleExit(){
        retTargetObj.GetComponent<SpriteRenderer>().enabled = true;
        retSelectionObj.GetComponent<SpriteRenderer>().enabled = false;
        //gestRec.notMoving();
    }

    public void ReticleOrientation(Vector3 posFrontHeadset){
        retTargetObj.transform.LookAt(headset.transform.position);
        retTargetObj.transform.position = posFrontHeadset;

        retSelectionObj.transform.LookAt(headset.transform.position);
        retSelectionObj.transform.position = posFrontHeadset;  
    }

}
