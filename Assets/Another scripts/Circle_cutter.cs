using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle_cutter : MonoBehaviour
{
    public enum Degrees { _180, _360 }
    public Degrees degrees;
    public int numElems;
    
    void Start()
    {
        if(degrees == Degrees._180){
            if(numElems == 1) 
            {
                float angle = Mathf.PI / 3;
                float colSize = Mathf.Sqrt( Mathf.Pow(1-Mathf.Cos(angle),2) + Mathf.Pow(Mathf.Sin(angle),2));
                
                for(int i=1; i <= 3; i++)
                {
                    float currAngle = Mathf.PI * i / 3;
                    float x = Mathf.Cos(currAngle - angle/2);
                    float z = Mathf.Sin(currAngle - angle/2);

                    float angleDegrees = - (currAngle - angle/2) * Mathf.Rad2Deg;    
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(0.5f, 1f, colSize - 0.15f);

                    cube.transform.position = new Vector3(x, 0, z);
                    cube.transform.rotation = Quaternion.Euler(0, angleDegrees, 0);

                    var cubeRender = cube.GetComponent<Renderer>();
                    cubeRender.material.SetColor("_Color", Color.green);
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
                    float angleDegrees = - (currAngle - angle/2) * Mathf.Rad2Deg;    
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(0.5f, 1f, colSize - 0.15f);

                    cube.transform.position = new Vector3(x, 0, z);
                    cube.transform.rotation = Quaternion.Euler(0, angleDegrees, 0);

                    var cubeRender = cube.GetComponent<Renderer>();
                    cubeRender.material.SetColor("_Color", Color.green);
                }
            } 
        }
        else if(degrees == Degrees._360)
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

                    float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(0.5f, 1f, colSize - 0.15f);

                    cube.transform.position = new Vector3(x, 0, z);
                    cube.transform.rotation = Quaternion.Euler(0, angleDegrees, 0);

                    var cubeRender = cube.GetComponent<Renderer>();
                    cubeRender.material.SetColor("_Color", Color.green);
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

                    float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 
                    
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(0.5f, 1f, colSize - 0.15f);

                    cube.transform.position = new Vector3(x, 0, z);
                    cube.transform.rotation = Quaternion.Euler(0, angleDegrees, 0);

                    var cubeRender = cube.GetComponent<Renderer>();
                    cubeRender.material.SetColor("_Color", Color.green);
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

                        float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                        Quaternion rot = Quaternion.Euler(0, angleDegrees, 0); 
                        
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.localScale = new Vector3(0.5f, 1f, colSize - 0.15f);

                        cube.transform.position = new Vector3(x, 0, z);
                        cube.transform.rotation = Quaternion.Euler(0, angleDegrees, 0);

                        var cubeRender = cube.GetComponent<Renderer>();
                        cubeRender.material.SetColor("_Color", Color.green);
                    }
                }
                else 
                {
                    for(int i=0; i < numElems; i++)
                    {
                        float currAngle = 2*Mathf.PI * i / numElems + Mathf.PI/2;
                        float x = Mathf.Cos(currAngle);
                        float z = Mathf.Sin(currAngle);

                        float angleDegrees = - (currAngle) * Mathf.Rad2Deg;
                        Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);

                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.localScale = new Vector3(0.5f, 1f, colSize - 0.15f);

                        cube.transform.position = new Vector3(x, 0, z);
                        cube.transform.rotation = Quaternion.Euler(0, angleDegrees, 0);

                        var cubeRender = cube.GetComponent<Renderer>();
                        cubeRender.material.SetColor("_Color", Color.green);
                    }
                }
            }
        }
    }
}

/*
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
        }*/


