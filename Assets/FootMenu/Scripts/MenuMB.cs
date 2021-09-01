using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class MenuMB : MonoBehaviour
{
    private Dummy.Representation rep;


    private float radius = 1f;
    

    private Menu FirstMenu;
    public Menu Data;
    public MenuCakePiece MenuCakePiecePrefab;
    public float GapWidthDegree = 1f;

    protected MenuCakePiece[] Pieces;
    protected MenuMB Parent;



    private GameObject colOld, highOld;

    List<string> colNames;

    private bool canPerform, piecesIsActive = false;

    private bool isfirstInter = true;


    private XRNode headset = XRNode.Head;
    private XRNode foot = XRNode.HardwareTracker;

    private XRNode controller = XRNode.RightHand;

    private GameObject auxIndirect;

    private bool menuFollowingPlayer;

    private Vector3 positionForMenu;
    private GameObject fingerAux;



    void Start()
    {      
        rep = this.transform.parent.parent.GetComponent<Dummy>().representation; 
        menuFollowingPlayer = this.transform.parent.parent.GetComponent<Dummy>().menuFollowingPlayer;

        InstantiateMenuRepresentation();

        //if(rep != Dummy.Representation.Controller)
        //{
            CreateColliders(Data.degrees, Data.Elements.Length);
            
            colOld.gameObject.SetActive(false);
            highOld.gameObject.SetActive(false);
        //}    
        
        canPerform = true;

        if(isfirstInter)
            FirstMenu = Data;

    
        if(rep == Dummy.Representation.Indirect){
            auxIndirect = new GameObject();
            auxIndirect.name = "auxIndirectActivationMenu";
            auxIndirect.AddComponent<BoxCollider>();
            auxIndirect.GetComponent<BoxCollider>().isTrigger = true;
            auxIndirect.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 0.7f);
            auxIndirect.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);
            auxIndirect.AddComponent<TriggerCollision>();

            if(!isfirstInter)
                auxIndirect.SetActive(false);
        }
    }

    
    void Update()
    {
        InputDevices.GetDeviceAtXRNode(headset).TryGetFeatureValue(CommonUsages.deviceRotation, out var rotHead);
        InputDevices.GetDeviceAtXRNode(headset).TryGetFeatureValue(CommonUsages.devicePosition, out var posHead);
        InputDevices.GetDeviceAtXRNode(foot).TryGetFeatureValue(CommonUsages.devicePosition, out var posFoot);
        InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.deviceRotation, out var rotCont);
        InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.primary2DAxis, out var posFinger);
        InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var fingerPress);


        // Menu Instatiate on the same position of foot or headset
        if(menuFollowingPlayer)
        {
            if(rep == Dummy.Representation.Direct)
            {
               /*if(isfirstInter)
                {*/
                    this.transform.parent.transform.localPosition = new Vector3(posHead.x, this.transform.parent.transform.localPosition.y, posHead.z);
                    colOld.transform.localPosition = new Vector3(posHead.x, colOld.transform.localPosition.y, posHead.z);
                    highOld.transform.localPosition = new Vector3(posHead.x, highOld.transform.localPosition.y, posHead.z);       
                    /*positionForMenu = posFoot;
                }       
                else
                {
                    this.transform.parent.transform.localPosition = new Vector3(positionForMenu.x, this.transform.parent.transform.localPosition.y, positionForMenu.z);
                    colOld.transform.localPosition = new Vector3(positionForMenu.x, colOld.transform.localPosition.y, positionForMenu.z);
                    highOld.transform.localPosition = new Vector3(positionForMenu.x, highOld.transform.localPosition.y, positionForMenu.z);
                } */  
            }       
            else if(rep == Dummy.Representation.Indirect)   
            {
                colOld.transform.localPosition = new Vector3(posHead.x, colOld.transform.localPosition.y, posHead.z);
                highOld.transform.localPosition = new Vector3(posHead.x, highOld.transform.localPosition.y, posHead.z); 
            }
        }

        /*if(useController)
        {
            InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.primary2DAxis, out var trackpad);
            InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var press);

            var stepLength = 360f / Data.Elements.Length;
            var fingerPos = NormalizeAngle(Vector3.SignedAngle(Vector3.up, trackpad, Vector3.forward));
            var activeElem = (int)(fingerPos / stepLength);

            for(int i=0; i<Data.Elements.Length; i++)
            {
                if(i == activeElem)
                    Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.80f);
                else
                    Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }*/
        
        
        

        if(canPerform) {
            if(rep == Dummy.Representation.Direct)
                InvokeDirectMenuRep(rotHead, posHead);
            else if(rep == Dummy.Representation.Indirect)
            {
                auxIndirect.transform.position = new Vector3(posHead.x, 0, posHead.z) + rotHead * Vector3.forward * 0.1f;
                auxIndirect.transform.rotation = Quaternion.Euler(auxIndirect.transform.rotation.eulerAngles.x, rotHead.eulerAngles.y, auxIndirect.transform.rotation.eulerAngles.z);
                
                if(isfirstInter){
                    bool auxIndMenu = auxIndirect.GetComponent<TriggerCollision>().selected;
                    InvokeIndirectMenuRep(auxIndMenu);
                } else {
                    teste();                    
                }

                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, rotHead.eulerAngles.y);
            }
            /*else if(rep == Dummy.Representation.Controller)
            {
                InvokeControllerRep();

                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, -rotHead.eulerAngles.y);
                
                var step = 360 / Data.Elements.Length;

                Debug.Log(posFinger);
                

                if(Mathf.Pow(posFinger.x , 2) + Mathf.Pow(posFinger.y , 2) >= Mathf.Pow(0.5f , 2))
                {
                    float anglePos = Vector3.SignedAngle(Vector3.up, posFinger, Vector3.forward);
                    
                    var activeElement = (int)(anglePos / step);
                    Debug.Log(activeElement);

                    if(fingerPress)
                    {
                        Debug.Log(Data.Elements[activeElement].Name);
                        
                    }                    
                }
            }*/
        }

        
            


        if(canPerform && piecesIsActive)
        {
            for(int i=0; i < colOld.transform.childCount; i++)
            {                
                bool auxS = colOld.transform.GetChild(i).GetComponent<TriggerCollision>().selected;
                if(auxS)
                {
                    colOld.transform.GetChild(i).GetComponent<TriggerCollision>().selected = false;

                    if(Data.Elements.Length == 1)
                    {
                        if(Data.Elements[0].NextMenu != null)
                        {
                            var newSubMenu = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                            newSubMenu.FirstMenu = this.FirstMenu;
                            newSubMenu.isfirstInter = false;
                            newSubMenu.Parent = this;
                            newSubMenu.positionForMenu = this.positionForMenu;
                            for(int j=0; j < newSubMenu.transform.childCount; j++)
                            {
                                Destroy(newSubMenu.transform.GetChild(j).gameObject);
                                Destroy(colOld);
                                Destroy(highOld);
                                if(rep == Dummy.Representation.Indirect)
                                    Destroy(auxIndirect);                                
                            }
                            newSubMenu.Data = Data.Elements[0].NextMenu;
                        } 
                        else 
                        {
                            var firstMenuAppear = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                            firstMenuAppear.isfirstInter = true;
                            firstMenuAppear.Parent = this;

                            for(int j=0; j < firstMenuAppear.transform.childCount; j++)
                            {
                                Destroy(firstMenuAppear.transform.GetChild(j).gameObject);
                                Destroy(colOld);
                                Destroy(highOld);
                                if(rep == Dummy.Representation.Indirect)
                                    Destroy(auxIndirect);
                                
                            }

                            firstMenuAppear.Data = FirstMenu;
                        }
                    }
                    else if(Data.Elements.Length == 2)
                    {
                        int auxChoice;

                        if(Data.degrees == Menu.Degrees._360){
                            
                            if(i < 3)
                                auxChoice = 0;
                            else
                                auxChoice = 1;
                        }
                        else
                        {
                            auxChoice = i;
                        }                       
                        

                        if(Data.Elements[auxChoice].NextMenu != null)
                        {
                            var newSubMenu = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                            newSubMenu.FirstMenu = this.FirstMenu;
                            newSubMenu.isfirstInter = false;
                            newSubMenu.Parent = this;
                            newSubMenu.positionForMenu = this.positionForMenu;
                            for(int j=0; j < newSubMenu.transform.childCount; j++){
                                Destroy(newSubMenu.transform.GetChild(j).gameObject);
                                Destroy(colOld);
                                Destroy(highOld);
                                if(rep == Dummy.Representation.Indirect)
                                    Destroy(auxIndirect);
                                
                            }
                            newSubMenu.Data = Data.Elements[auxChoice].NextMenu;
                        }
                        else 
                        {
                            var firstMenuAppear = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                            firstMenuAppear.isfirstInter = true;
                            firstMenuAppear.Parent = this;

                            for(int j=0; j < firstMenuAppear.transform.childCount; j++)
                            {
                                Destroy(firstMenuAppear.transform.GetChild(j).gameObject);
                                Destroy(colOld);
                                Destroy(highOld);
                                if(rep == Dummy.Representation.Indirect)
                                    Destroy(auxIndirect);                                
                            }
                            firstMenuAppear.Data = FirstMenu;
                        }
                    }
                    else
                    {
                        Debug.Log(Data.Elements[i].Name);
                        if(Data.Elements[i].NextMenu != null)
                        {
                            var newSubMenu = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                            newSubMenu.FirstMenu = this.FirstMenu;
                            newSubMenu.isfirstInter = false;
                            newSubMenu.Parent = this;
                            newSubMenu.positionForMenu = this.positionForMenu;
                            for(int j=0; j < newSubMenu.transform.childCount; j++){
                                Destroy(newSubMenu.transform.GetChild(j).gameObject);
                                Destroy(colOld);
                                Destroy(highOld);
                                if(rep == Dummy.Representation.Indirect)
                                    Destroy(auxIndirect);
                                
                            }
                            newSubMenu.Data = Data.Elements[i].NextMenu;
                        } 
                        else 
                        {
                            var firstMenuAppear = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                            firstMenuAppear.isfirstInter = true;
                            firstMenuAppear.Parent = this;

                            for(int j=0; j < firstMenuAppear.transform.childCount; j++)
                            {
                                Destroy(firstMenuAppear.transform.GetChild(j).gameObject);
                                Destroy(colOld);
                                Destroy(highOld);
                                if(rep == Dummy.Representation.Indirect)
                                    Destroy(auxIndirect);
                                
                            }
                            firstMenuAppear.Data = FirstMenu;                            
                        }
                    }
                    //gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }

    }

    
    private void CreateColliders(Menu.Degrees type, int numElems)
    {
        var iconDist = Vector3.Distance(MenuCakePiecePrefab.Icon.transform.position, MenuCakePiecePrefab.CakePiece.transform.position);

        var MenuParent = GameObject.Find("MenuGO");

        GameObject collGO = new GameObject();
        collGO.name = "colliders";
        GameObject highGO = new GameObject();
        highGO.name = "highlighters";

        if(type == Menu.Degrees._180)
        {
            if(numElems == 1) 
            {
                float angle = Mathf.PI / 3;
                float colSize = Mathf.Sqrt( Mathf.Pow(1-Mathf.Cos(angle),2) + Mathf.Pow(Mathf.Sin(angle),2));
                
                for(int i=1; i <= 3; i++)
                {
                    float currAngle = Mathf.PI * i / 3;
                    float x = Mathf.Cos(currAngle - angle/2);
                    float z = Mathf.Sin(currAngle - angle/2);

                    
                    Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                    float angleDegrees = - (currAngle - angle/2) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                    GameObject Sequetion = new GameObject();
                    GameObject SeqHigh = new GameObject();

                    Sequetion.transform.SetParent(collGO.transform, true);
                    SeqHigh.transform.SetParent(highGO.transform, true);
                    Sequetion.name = Data.Elements[0].Name + "_col";
                    SeqHigh.name = Data.Elements[0].Name + "_high";

                    Sequetion.AddComponent<BoxCollider>();
                    SeqHigh.AddComponent<BoxCollider>();

                    Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                    SeqHigh.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);

                    Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius/2, 1f, colSize - 0.15f);   
                    SeqHigh.GetComponent<BoxCollider>().size = new Vector3(radius/4, 1f, colSize - 0.15f);  

                    Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                    
                    Sequetion.AddComponent<TriggerCollision>();
                    Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[0].Action;

                    SeqHigh.AddComponent<HighliterCollider>();
                    SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[0];

                    Sequetion.transform.position = pos;
                    Sequetion.transform.rotation = rot;

                    SeqHigh.transform.position = pos;
                    SeqHigh.transform.rotation = rot;
                    
                }
            }
            else
            {
                float angle = Mathf.PI / numElems;
                float colSize = Mathf.Sqrt( Mathf.Pow(1-Mathf.Cos(angle),2) + Mathf.Pow(Mathf.Sin(angle),2));
                
                for(int i=1; i <= numElems; i++)
                {
                    float currAngle = Mathf.PI * i / numElems;
                    float x = Mathf.Cos(currAngle - angle/2);
                    float z = Mathf.Sin(currAngle - angle/2);
                    
                    Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                    float angleDegrees = - (currAngle - angle/2) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                    GameObject Sequetion = new GameObject();
                    GameObject SeqHigh = new GameObject();

                    Sequetion.transform.SetParent(collGO.transform, true);
                    SeqHigh.transform.SetParent(highGO.transform, true);
                    Sequetion.name = Data.Elements[i-1].Name + "_col";
                    SeqHigh.name = Data.Elements[i-1].Name + "_high";
                    
                    if(!(Data.Elements[i-1].Name.Equals("Empty") || Data.Elements[i-1].Name.Equals("empty")))
                    {
                        Sequetion.AddComponent<BoxCollider>();
                        SeqHigh.AddComponent<BoxCollider>();

                        Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                        SeqHigh.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);

                        Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius/2, 1f, colSize - 0.15f);       
                        SeqHigh.GetComponent<BoxCollider>().size = new Vector3(radius/4, 1f, colSize - 0.15f);  
                
                        Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                        
                        Sequetion.AddComponent<TriggerCollision>();
                        Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[i-1].Action;

                        SeqHigh.AddComponent<HighliterCollider>();
                        SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[i-1];

                        Sequetion.transform.position = pos;
                        Sequetion.transform.rotation = rot;

                        SeqHigh.transform.position = pos;
                        SeqHigh.transform.rotation = rot;
                    }
                    
                }
            }
        } 
        else if(type == Menu.Degrees._360)
        {
            if(numElems == 1) 
            {
                float angle = 2*Mathf.PI / 6;
                float colSize = Mathf.Sqrt( Mathf.Pow(1-Mathf.Cos(angle),2) + Mathf.Pow(Mathf.Sin(angle),2));
                
                for(int i=1; i <= 6; i++)
                {
                    float currAngle = 2*Mathf.PI * i / 6;
                    float x = Mathf.Cos(currAngle);
                    float z = Mathf.Sin(currAngle);

                    
                    Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                    float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                    GameObject Sequetion = new GameObject();
                    GameObject SeqHigh = new GameObject();

                    Sequetion.transform.SetParent(collGO.transform, true);
                    SeqHigh.transform.SetParent(highGO.transform, true);
                    Sequetion.name = Data.Elements[0].Name + "_col";
                    SeqHigh.name = Data.Elements[0].Name + "_high";


                    Sequetion.AddComponent<BoxCollider>();
                    SeqHigh.AddComponent<BoxCollider>();

                    Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                    SeqHigh.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);

                    Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius/2, 1f, colSize - 0.15f);  
                    SeqHigh.GetComponent<BoxCollider>().size = new Vector3(radius/4, 1f, colSize - 0.15f);

                    Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                    
                    Sequetion.AddComponent<TriggerCollision>();
                    Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[0].Action;

                    SeqHigh.AddComponent<HighliterCollider>();
                    SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[0];

                    Sequetion.transform.position = pos;
                    Sequetion.transform.rotation = rot;

                    SeqHigh.transform.position = pos;
                    SeqHigh.transform.rotation = rot;
                }
            }     
            else if(numElems == 2)
            {
                float angle = 2*Mathf.PI / 6;
                float colSize = Mathf.Sqrt( Mathf.Pow(1-Mathf.Cos(angle),2) + Mathf.Pow(Mathf.Sin(angle),2));
                
                for(int i=1; i <= 6; i++)
                {
                    float currAngle = (2*Mathf.PI * i / 6) - (2*Mathf.PI/3);
                    float x = Mathf.Cos(currAngle);
                    float z = Mathf.Sin(currAngle);

                    
                    Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                    float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                    GameObject Sequetion = new GameObject();
                    GameObject SeqHigh = new GameObject();

                    Sequetion.transform.SetParent(collGO.transform, true);
                    SeqHigh.transform.SetParent(highGO.transform, true);

                    if(i <= 3)
                    {
                        Sequetion.name = Data.Elements[0].Name + "_col";
                        SeqHigh.name = Data.Elements[0].Name + "_high";
                    }
                    else
                    {
                        Sequetion.name = Data.Elements[1].Name + "_col";
                        SeqHigh.name = Data.Elements[1].Name + "_high";
                    }

                    Sequetion.AddComponent<BoxCollider>();
                    SeqHigh.AddComponent<BoxCollider>();

                    Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                    SeqHigh.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);

                    Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius/2, 1f, colSize - 0.15f);
                    SeqHigh.GetComponent<BoxCollider>().size = new Vector3(radius/4, 1f, colSize - 0.15f);

                    Sequetion.GetComponent<BoxCollider>().isTrigger = true;
                    
                    Sequetion.AddComponent<TriggerCollision>();
                    SeqHigh.AddComponent<HighliterCollider>();
                    if(i <= 3)                    
                    {    
                        Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[0].Action;
                        SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[0];
                    }
                    else
                    {
                        Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[1].Action;
                        SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[1];
                    }
                    



                    Sequetion.transform.position = pos;
                    Sequetion.transform.rotation = rot;

                    SeqHigh.transform.position = pos;
                    SeqHigh.transform.rotation = rot;
                }
            }    
            else 
            {
                float angle = 2*Mathf.PI / numElems;
                float colSize = Mathf.Sqrt( Mathf.Pow(1-Mathf.Cos(angle),2) + Mathf.Pow(Mathf.Sin(angle),2));

                if(numElems % 2 == 0)
                {
                    for(int i=0; i < numElems; i++)
                    {
                        float currAngle = 2*Mathf.PI * i / numElems;
                        float x = Mathf.Cos(currAngle);
                        float z = Mathf.Sin(currAngle);

                        
                        Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                        float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                        Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                        GameObject Sequetion = new GameObject();
                        GameObject SeqHigh = new GameObject();

                        Sequetion.transform.SetParent(collGO.transform, true);
                        SeqHigh.transform.SetParent(highGO.transform, true);                        
                        Sequetion.name = Data.Elements[i].Name + "_col";
                        SeqHigh.name = Data.Elements[i].Name + "_high";

                        if(!(Data.Elements[i].Name.Equals("Empty") || Data.Elements[i].Name.Equals("empty")))
                        {
                            Sequetion.AddComponent<BoxCollider>();
                            SeqHigh.AddComponent<BoxCollider>();

                            Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                            SeqHigh.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);

                            Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius/2, 1f, colSize - 0.15f);
                            SeqHigh.GetComponent<BoxCollider>().size = new Vector3(radius/4, 1f, colSize - 0.15f);

                            Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                            
                            Sequetion.AddComponent<TriggerCollision>();
                            Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[i].Action;

                            SeqHigh.AddComponent<HighliterCollider>();
                            SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[i];

                            Sequetion.transform.position = pos;
                            Sequetion.transform.rotation = rot;

                            SeqHigh.transform.position = pos;
                            SeqHigh.transform.rotation = rot;
                        }
                        else
                        {
                            Sequetion.AddComponent<TriggerCollision>();   
                        }
                        
                    }
                }
                else 
                {
                    for(int i=0; i < numElems; i++)
                    {
                        float currAngle = 2*Mathf.PI * i / numElems + Mathf.PI/2;
                        float x = Mathf.Cos(currAngle);
                        float z = Mathf.Sin(currAngle);

                        
                        Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                        float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                        Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                        GameObject Sequetion = new GameObject();
                        GameObject SeqHigh = new GameObject();

                        Sequetion.transform.SetParent(collGO.transform, true);
                        SeqHigh.transform.SetParent(highGO.transform, true);                        
                        Sequetion.name = Data.Elements[i].Name + "_col";
                        SeqHigh.name = Data.Elements[i].Name + "_high";

                        if(!(Data.Elements[i].Name.Equals("Empty") || Data.Elements[i].Name.Equals("empty")))
                        {
                            Sequetion.AddComponent<BoxCollider>();
                            SeqHigh.AddComponent<BoxCollider>();

                            Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                            SeqHigh.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);

                            Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius/2, 1f, colSize - 0.15f);
                            SeqHigh.GetComponent<BoxCollider>().size = new Vector3(radius/4, 1f, colSize - 0.15f);

                            Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                            
                            Sequetion.AddComponent<TriggerCollision>();
                            Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[i].Action;

                            SeqHigh.AddComponent<HighliterCollider>();
                            SeqHigh.GetComponent<HighliterCollider>().Element = Pieces[i];

                            Sequetion.transform.position = pos;
                            Sequetion.transform.rotation = rot;

                            SeqHigh.transform.position = pos;
                            SeqHigh.transform.rotation = rot;
                        }
                        else
                        {
                            Sequetion.AddComponent<TriggerCollision>();
                        }
                    }
                }              
            }   
        }

        collGO.transform.localScale = new Vector3(iconDist/1000, 1, iconDist/1000);
        colOld = collGO;

        highGO.transform.localScale = new Vector3(iconDist/1500, 1, iconDist/1500);
        highOld = highGO;

    }


    private void InstantiateMenuRepresentation()
    {
        var iconDist = Vector3.Distance(MenuCakePiecePrefab.Icon.transform.position, MenuCakePiecePrefab.CakePiece.transform.position);

        Pieces = new MenuCakePiece[Data.Elements.Length];

        if(Data.degrees == Menu.Degrees._180){
            var fill = 0.5f / Data.Elements.Length - GapWidthDegree / 180;
            var stepLength = 180 / Data.Elements.Length;

            for(int i = 0; i < Data.Elements.Length; i++){
                
                //Instancieate the Pieces
                Pieces[i] = Instantiate(MenuCakePiecePrefab, transform);

                // set root element
                Pieces[i].transform.name = Data.Elements[i].Name;
                Pieces[i].transform.localPosition = Vector3.zero;
                Pieces[i].transform.localRotation = Quaternion.Euler(0,0, (stepLength/2)+GapWidthDegree/2);


                //set cake Piece
                if((string.Compare(Data.Elements[i].Name, "Empty") != 0) || (string.Compare(Data.Elements[i].Name, "empty") != 0))
                {
                    Pieces[i].CakePiece.fillAmount = fill;     
                    Pieces[i].CakePiece.transform.localPosition = Vector3.zero;
                    Pieces[i].CakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
                    Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
                } 
                else 
                {
                    Pieces[i].CakePiece.fillAmount = 0;
                }

                //set icon
                var aux = Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up* iconDist;
                Pieces[i].Icon.transform.localRotation = Quaternion.Euler(0,0, -(stepLength/2)+GapWidthDegree/2);
                Pieces[i].Icon.transform.localPosition = new Vector2(aux.y, -aux.x);
                Pieces[i].Icon.sprite = Data.Elements[i].Icon;                
            }
        } else if (Data.degrees == Menu.Degrees._360){
            var fill = 1f / Data.Elements.Length - GapWidthDegree / 360;
            var stepLength = 360 / Data.Elements.Length;

            //Instatiate for even numb of menu elements 
            if(Data.Elements.Length % 2 == 0){

                for(int i = 0; i < Data.Elements.Length; i++){

                    //Instancieate the Pieces
                    Pieces[i] = Instantiate(MenuCakePiecePrefab, transform);

                    // set root element
                    Pieces[i].transform.name = Data.Elements[i].Name;
                    Pieces[i].transform.localPosition = Vector3.zero;
                    Pieces[i].transform.localRotation = Quaternion.identity;

                    //set cake Piece
                    if(string.Compare(Data.Elements[i].Name, "Empty") != 0)
                    {
                        Pieces[i].CakePiece.fillAmount = fill;     
                        Pieces[i].CakePiece.transform.localPosition = Vector3.zero;
                        Pieces[i].CakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
                        Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
                    } 
                    else 
                    {
                        Pieces[i].CakePiece.fillAmount = 0;
                    }

                    //set icon
                    var aux = Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
                    //Debug.Log(aux);
                    Pieces[i].Icon.transform.localPosition = new Vector2(aux.y, -aux.x);
                    
                    /*if(rep == Dummy.Representation.Direct){
                        Pieces[i].Icon.transform.localRotation = Quaternion.Euler(0,0, -90f + (360/Data.Elements.Length) * i);
                    }*/

                    if(Data.Elements[i].IconFacingMiddle)
                        Pieces[i].Icon.transform.localRotation = Quaternion.Euler(0,0, -90f + (360/Data.Elements.Length) * i);
                    else
                        Pieces[i].Icon.transform.localRotation = Quaternion.identity;
                    
                    Pieces[i].Icon.sprite = Data.Elements[i].Icon;
                }
            } 
            //Instatiate for odd numb of menu elements 
            else 
            {
                for(int i = 0; i < Data.Elements.Length; i++)
                {
                    //Instancieate the Pieces
                    Pieces[i] = Instantiate(MenuCakePiecePrefab, transform);

                    // set root element
                    Pieces[i].transform.name = Data.Elements[i].Name;
                    Pieces[i].transform.localPosition = Vector3.zero;
                    Pieces[i].transform.localRotation = Quaternion.Euler(0, 0, 90f);

                    //set cake Piece
                    if(string.Compare(Data.Elements[i].Name, "Empty") != 0)
                    {
                        Pieces[i].CakePiece.fillAmount = fill;     
                        Pieces[i].CakePiece.transform.localPosition = Vector3.zero;
                        Pieces[i].CakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
                        Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
                    } 
                    else 
                    {
                        Pieces[i].CakePiece.fillAmount = 0;
                    }

                    //set icon
                    var aux = Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
                    //Debug.Log(aux);
                    Pieces[i].Icon.transform.localPosition = new Vector2(aux.y, -aux.x);

                    if(rep == Dummy.Representation.Direct || rep == Dummy.Representation.Controller)
                    {
                         if(Data.Elements[i].IconFacingMiddle)
                            Pieces[i].Icon.transform.localRotation = Quaternion.Euler(0,0, -90f + (360/Data.Elements.Length) * i);
                        else
                            Pieces[i].Icon.transform.localRotation = Quaternion.Euler(0,0,-90f);
                    } 
                    else if(rep == Dummy.Representation.Indirect)
                    {
                        Pieces[i].Icon.transform.localRotation = Quaternion.Euler(0,0, -90f);
                    }

                    Pieces[i].Icon.sprite = Data.Elements[i].Icon;
                }
            }
        }
    }


    private void InvokeDirectMenuRep(Quaternion rot, Vector3 pos){
        if(rot.eulerAngles.x >= 20 && rot.eulerAngles.x <= 90)
        {
            for(int i=0; i < Pieces.Length; i++)
                Pieces[i].gameObject.SetActive(true);
            
            colOld.gameObject.SetActive(true);
            highOld.gameObject.SetActive(true);

            piecesIsActive = true;       
            //menuFollowingPlayer = false; 
        }            
        else 
        {
            for(int i=0; i < Pieces.Length; i++)
                Pieces[i].gameObject.SetActive(false);
            
            colOld.gameObject.SetActive(false);
            highOld.gameObject.SetActive(false);

            piecesIsActive = false;
            //menuFollowingPlayer = true; 
        }
    }

    /*IEnumerator waiterIndirect(int sec){

        for(int i=0; i < Pieces.Length; i++)
            Pieces[i].gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(sec);

        colOld.gameObject.SetActive(true);
        highOld.gameObject.SetActive(true);

        piecesIsActive = true; 
    }*/

    void teste() {
        for(int i=0; i < Pieces.Length; i++)
            Pieces[i].gameObject.SetActive(true);

        colOld.gameObject.SetActive(true);
        highOld.gameObject.SetActive(true);

        piecesIsActive = true;
    }


    private void InvokeIndirectMenuRep(bool auxIndMenu){
        if(auxIndMenu)
        {    
            //StartCoroutine(waiterIndirect(1));
            teste();

        }
        else 
        {
            for(int i=0; i < Pieces.Length; i++)
            {
                Pieces[i].gameObject.SetActive(false);
            }
            
            colOld.gameObject.SetActive(false);
            highOld.gameObject.SetActive(false);
        }   
    }


    private void InvokeControllerRep()
    {
        for(int i=0; i < Pieces.Length; i++){
            Pieces[i].gameObject.SetActive(true);
        }
    }

}




/*var stepLength = 360 / Data.Elements.Length;
        var mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f);
       

        var activeElement = (int)(mouseAngle / stepLength);
        for(int i=0; i < Data.Elements.Length; i++){
            if(i == activeElement){
                Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.75f);
            } else {
                Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            var path = Path + "/" + Data.Elements[activeElement].Name;
            if(Data.Elements[activeElement].NextMenu != null){
                //Debug.Log(transform.parent);
                var newSubMenu = Instantiate(gameObject, transform.parent).GetComponent<MenuMB>();
                newSubMenu.Parent = this;
                for(int j=0; j < newSubMenu.transform.childCount; j++){
                    Destroy(newSubMenu.transform.GetChild(j).gameObject);
                    Destroy(colOld);
                }
                newSubMenu.Data = Data.Elements[activeElement].NextMenu;
                newSubMenu.Path = path;
                newSubMenu.callback = callback;
            } 
            else {
                callback?.Invoke(path);
            }
            gameObject.SetActive(false);
        }*/


/*

private void InstantiateSeqtionHighliter() {

        var MenuParent = GameObject.Find("Menu");

        GameObject highGO = new GameObject();
        highGO.name = "higliters";

        if(Data.Elements.Length == 1) {
            float colSize = radius;

            GameObject Sequetion = new GameObject();
            Sequetion.transform.SetParent(highGO.transform, true);
            Sequetion.name = Data.Elements[0].Name + "_high";
            Sequetion.AddComponent<BoxCollider>();
            Sequetion.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, colSize);
            Sequetion.GetComponent<BoxCollider>().center = new Vector3(0.1f, 0f, 0f);

            
            Sequetion.AddComponent<HighliterCollider>();
            Sequetion.GetComponent<HighliterCollider>().Element = Pieces[0];

            Sequetion.transform.position = new Vector3(0, 0, colSize + (colSize/2));
            Sequetion.transform.rotation = Quaternion.Euler(0,90f,0);

        } else if(Data.Elements.Length == 2) {
            
            float colSize = radius;

            GameObject Sequetion1 = new GameObject();
            Sequetion1.transform.SetParent(highGO.transform, true);
            Sequetion1.name = Data.Elements[0].Name + "_high";
            Sequetion1.AddComponent<BoxCollider>();
            Sequetion1.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, colSize*2);
            Sequetion1.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
            
            Sequetion1.AddComponent<HighliterCollider>();
            Sequetion1.GetComponent<HighliterCollider>().Element = Pieces[0];

            Sequetion1.transform.position = new Vector3(colSize, 0, colSize);
            Sequetion1.transform.rotation = Quaternion.Euler(0,-45f,0);

            GameObject Sequetion2 = new GameObject();
            Sequetion2.transform.SetParent(highGO.transform, true);
            Sequetion2.name = Data.Elements[1].Name + "_high";
            Sequetion2.AddComponent<BoxCollider>();
            Sequetion2.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, colSize*2);
            Sequetion2.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
            
            Sequetion2.AddComponent<HighliterCollider>();
            Sequetion2.GetComponent<HighliterCollider>().Element = Pieces[1];

            Sequetion2.transform.position = new Vector3(-colSize, 0, colSize);
            Sequetion2.transform.rotation = Quaternion.Euler(0,45,0);

        } else {
            int piMult = 0;
            int auxDiv = 0;
            bool isOdd = false;
            if(Data.degrees == Menu.Degrees._180){
                piMult = 1;
                auxDiv = 1;
            } else if (Data.degrees == Menu.Degrees._360){
                piMult = 2;
                auxDiv = 0;
                if(Data.Elements.Length % 2 != 0)
                    isOdd = true;
            }

            float colSize = (Mathf.PI*piMult/(Data.Elements.Length - auxDiv)) * radius;

            for(int i=0; i < Data.Elements.Length; i++){

                float angle = i * Mathf.PI * piMult / (Data.Elements.Length - auxDiv);                
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                //Debug.Log("IT "+i+": ang = " + angleDegrees);
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);


                GameObject Sequetion = new GameObject();
                Sequetion.transform.SetParent(highGO.transform, true);
                Sequetion.name = Data.Elements[i].Name + "_high";
                Sequetion.AddComponent<BoxCollider>();
                Sequetion.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, (colSize - 0.03f)/2);
                Sequetion.GetComponent<BoxCollider>().center = new Vector3(0.1f, 0f, 0f);

                
                Sequetion.AddComponent<HighliterCollider>();
                Sequetion.GetComponent<HighliterCollider>().Element = Pieces[i];

                if(Data.degrees == Menu.Degrees._180){
                    Sequetion.transform.position = new Vector3(pos.x, pos.y, pos.z + colSize/2);
                } else if (Data.degrees == Menu.Degrees._360){
                    Sequetion.transform.position = pos;
                }

                Sequetion.transform.rotation = rot;
                    
            }

            if(Data.degrees == Menu.Degrees._360 && isOdd)
                highGO.transform.localRotation = Quaternion.Euler(0,-90f,0);
        }

        highGO.transform.localScale = new Vector3(radius/5 , 1, radius/5);

        highOld = highGO;
    }


    private void InstantiateCollidersForSequection() {    
        
        var MenuParent = GameObject.Find("Menu");

        GameObject collGO = new GameObject();
        collGO.name = "colliders";
        //collGO.transform.SetParent(MenuParent.transform, true);

        int piMult = 0;
        int auxDiv = 0;
        bool isOdd = false;
        if(Data.degrees == Menu.Degrees._180){
            piMult = 1;
            auxDiv = 1;
        } else if (Data.degrees == Menu.Degrees._360){
            piMult = 2;
            auxDiv = 0;
            if(Data.Elements.Length % 2 != 0)
                isOdd = true;
        }

        if(Data.Elements.Length == 1) {

            if(Data.degrees == Menu.Degrees._360){
            
                float colSize = (Mathf.PI*piMult/(3 - auxDiv)) * radius;    

                for(int i=0; i < 4; i++){

                    float angle = i * Mathf.PI * piMult / (4 - auxDiv);    

                    float x = Mathf.Cos(angle) * radius;
                    float z = Mathf.Sin(angle) * radius;
                    Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                    float angleDegrees = -angle * Mathf.Rad2Deg;
                    //Debug.Log("IT "+i+": ang = " + angleDegrees);
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);       
                
                    GameObject Sequetion = new GameObject();

                    Sequetion.transform.SetParent(collGO.transform, true);
                    Sequetion.name = Data.Elements[0].Name + "_col";
                    Sequetion.AddComponent<BoxCollider>();
                    Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius, 0.5f, colSize);
                    Sequetion.GetComponent<BoxCollider>().center = new Vector3(radius/2, 0f, 0f);
                    Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                    
                    Sequetion.AddComponent<TriggerCollision>();
                    Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[0].Action;

                    Sequetion.transform.position = pos;
                    Sequetion.transform.rotation = rot;
                }
            } else if(Data.degrees == Menu.Degrees._180) {

                float colSize = (Mathf.PI*piMult/(3 - auxDiv)) * radius;    

                for(int i=0; i < 3; i++){

                    float angle = i * Mathf.PI * piMult / (3 - auxDiv);    

                    float x = Mathf.Cos(angle) * radius * 1.5f;
                    float z = Mathf.Sin(angle) * radius * 0.75f;
                    Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                    float angleDegrees = -angle * Mathf.Rad2Deg;
                    //Debug.Log("IT "+i+": ang = " + angleDegrees);
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);       
                
                    GameObject Sequetion = new GameObject();

                    Sequetion.transform.SetParent(collGO.transform, true);
                    Sequetion.name = Data.Elements[0].Name + "_col";
                    Sequetion.AddComponent<BoxCollider>();
                    Sequetion.GetComponent<BoxCollider>().size = new Vector3(radius, 0.5f, colSize);
                    Sequetion.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
                    Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                    
                    Sequetion.AddComponent<TriggerCollision>();
                    Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[0].Action;

                    Sequetion.transform.position = new Vector3(pos.x, pos.y, pos.z + colSize/2);;
                    Sequetion.transform.rotation = rot;
                }
            }

        } else if(Data.Elements.Length == 2) {

            float colSize = radius;

            GameObject Sequetion1 = new GameObject();
            Sequetion1.transform.SetParent(collGO.transform, true);
            Sequetion1.name = Data.Elements[0].Name + "_col";
            Sequetion1.AddComponent<BoxCollider>();
            Sequetion1.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, colSize*2);
            Sequetion1.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
            Sequetion1.GetComponent<BoxCollider>().isTrigger = true;
            
            Sequetion1.AddComponent<TriggerCollision>();
            Sequetion1.GetComponent<TriggerCollision>().action = Data.Elements[0].Action;

            Sequetion1.transform.position = new Vector3(colSize, 0, colSize);
            Sequetion1.transform.rotation = Quaternion.Euler(0,-45f,0);

            GameObject Sequetion2 = new GameObject();
            Sequetion2.transform.SetParent(collGO.transform, true);
            Sequetion2.name = Data.Elements[1].Name + "_col";
            Sequetion2.AddComponent<BoxCollider>();
            Sequetion2.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, colSize*2);
            Sequetion2.GetComponent<BoxCollider>().center = new Vector3(0f, 0f, 0f);
            Sequetion2.GetComponent<BoxCollider>().isTrigger = true;
            
            Sequetion2.AddComponent<TriggerCollision>();
            Sequetion2.GetComponent<TriggerCollision>().action = Data.Elements[1].Action;

            Sequetion2.transform.position = new Vector3(-colSize, 0, colSize);
            Sequetion2.transform.rotation = Quaternion.Euler(0,45f,0);

        } else {

            float colSize = (Mathf.PI*piMult/(Data.Elements.Length - auxDiv)) * radius;

            for(int i=0; i < Data.Elements.Length; i++){

                float angle = i * Mathf.PI * piMult / (Data.Elements.Length - auxDiv);    

                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = MenuParent.transform.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                //Debug.Log("IT "+i+": ang = " + angleDegrees);
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);

                GameObject Sequetion = new GameObject();
                Sequetion.transform.SetParent(collGO.transform, true);
                Sequetion.name = Data.Elements[i].Name + "_col";
                Sequetion.AddComponent<BoxCollider>();
                Sequetion.GetComponent<BoxCollider>().size = new Vector3(0.5f, 0.5f, colSize);
                Sequetion.GetComponent<BoxCollider>().center = new Vector3(0.25f, 0f, 0f);
                Sequetion.GetComponent<BoxCollider>().isTrigger = true;

                
                Sequetion.AddComponent<TriggerCollision>();
                Sequetion.GetComponent<TriggerCollision>().action = Data.Elements[i].Action;
                //Sequetion.AddComponent<ColisionDetection>();
                //Sequetion.GetComponent<ColisionDetection>().Element = Data.Elements[i];
                //Sequetion.GetComponent<ColisionDetection>().action = Data.Elements[i].Action;
                //Sequetion.transform.localScale = new Vector3(0.1f, 0.1f, colSize);

                if(Data.degrees == Menu.Degrees._180){
                    Sequetion.transform.position = new Vector3(pos.x, pos.y, pos.z + colSize/2);
                } else if (Data.degrees == Menu.Degrees._360){
                    Sequetion.transform.position = pos;
                }

                Sequetion.transform.rotation = rot;
                    
            }

            if(Data.degrees == Menu.Degrees._360 && isOdd)
                collGO.transform.localRotation = Quaternion.Euler(0,-90f,0);
        }

        collGO.transform.localScale = new Vector3(radius/3.5f, 1, radius/3.5f);

        colOld = collGO;
         
    }


*/





