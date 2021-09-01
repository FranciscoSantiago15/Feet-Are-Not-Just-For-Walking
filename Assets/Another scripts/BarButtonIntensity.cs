using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarButtonIntensity : MonoBehaviour
{

    public FootButton2 button;

    private Color originalColor;

    public bool ButtonIntensityShow;

    private Slider slider;


    void Start()
    {
        originalColor = button.gameObject.GetComponentInChildren<MeshRenderer>().material.color;
        slider = this.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 a = new Vector3(0.1f, 0.1f, 0.1f);
        transform.localScale = new Vector3(0.1f, 0.1f * button.GetValue(), 0.1f);*/

        slider.value = button.GetValue();



        if(ButtonIntensityShow){
            //Debug.Log(originalColor.r + " | "+ originalColor.g + " | "+ originalColor.b + " | " + originalColor.g * button.GetValue());
            button.gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(originalColor.r * button.GetValue(), originalColor.g * button.GetValue(), originalColor.b * button.GetValue(), originalColor.g);
        }

    }
}
