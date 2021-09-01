using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuColiders : MonoBehaviour
{
    public enum Degrees { _180, _360 }
    [SerializeField] Degrees degrees;

    public int N_Division;

    public float radius = 5f;

    private GameObject MenuParent;
    
    void Start()
    {

        MenuParent = GameObject.Find("Menu");

        int piMult = 0;
        int auxDiv = 0;
        if(degrees == Degrees._180){
           piMult = 1;
            auxDiv = 1;
        } else if (degrees == Degrees._360){
            piMult = 2;
            auxDiv = 0;
        }

        float colSize = (Mathf.PI*piMult/(N_Division - auxDiv)) * radius;
        

         for(int i=0; i < N_Division; i++){
            float angle = i * Mathf.PI * piMult / (N_Division - auxDiv);                
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            //Debug.Log("IT "+i+": ang = " + angleDegrees);
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);


            GameObject Sequetion = new GameObject();
            Sequetion.transform.SetParent(MenuParent.transform, true);
            Sequetion.name = "Seq_" + i;
            Sequetion.AddComponent<BoxCollider>();
            Sequetion.GetComponent<BoxCollider>().size = new Vector3(0.1f, 3f, colSize - 0.03f);
            Sequetion.GetComponent<BoxCollider>().center = Vector3.zero;
            //Sequetion.transform.localScale = new Vector3(0.1f, 0.1f, colSize);
            Sequetion.transform.position = new Vector3(pos.x, pos.y + 0.05f, pos.z);
            Sequetion.transform.rotation = rot;

            

            /*GameObject Sequetion = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Sequetion.transform.SetParent(MenuParent.transform, true);
            Sequetion.name = "Seq_" + i;
            Sequetion.GetComponent<Renderer>().material.color = Color.blue;
            Sequetion.transform.localScale = new Vector3(0.1f, 0.1f, colSize);
            Sequetion.transform.position = new Vector3(pos.x, pos.y + 0.05f, pos.z);
            Sequetion.transform.rotation = rot;
            //cube.AddComponent<BoxCollider>();
            
            Sequetion.GetComponent<BoxCollider>().size = new Vector3(1f, 20f, 1f);
            Sequetion.GetComponent<BoxCollider>().center = new Vector3(0, 9.5f, 0); */
        

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
