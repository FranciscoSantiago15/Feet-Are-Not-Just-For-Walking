using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuColliders : MonoBehaviour
{
    private GameObject MenuParent;
    public Menu Data;

    public float radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateCollidersForSequection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstantiateCollidersForSequection() {         
        
        MenuParent = GameObject.Find("Menu");

        GameObject collGO = new GameObject();
        collGO.name = "colliders";
        collGO.transform.SetParent(MenuParent.transform, true);

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
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            //Debug.Log("IT "+i+": ang = " + angleDegrees);
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);


            GameObject Sequetion = new GameObject();
            Sequetion.transform.SetParent(collGO.transform, true);
            Sequetion.name = Data.Elements[i].Name + "_col";
            Sequetion.AddComponent<BoxCollider>();
            Sequetion.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.5f, colSize - 0.03f);
            Sequetion.GetComponent<BoxCollider>().center = Vector3.zero;
            //Sequetion.transform.localScale = new Vector3(0.1f, 0.1f, colSize);
            Sequetion.transform.position = pos;
            Sequetion.transform.rotation = rot;
        }

        if(Data.degrees == Menu.Degrees._360 && isOdd)
            collGO.transform.localRotation = Quaternion.Euler(0,-90f,0);

        collGO.transform.localScale = new Vector3(radius/3.5f, 1, radius/3.5f);

        //collGO.transform.SetParent(this.transform , true);
         
    }
}
