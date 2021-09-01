using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class UnityEvent_float : UnityEvent<float>{};

public class FootButton2 : MonoBehaviour
{
    [SerializeField] private float treshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;
    [SerializeField] private bool UseFullPressAndRealese = false;
    [SerializeField, Range(1, 100)] private int forcePercentage;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent_float OnPressed, OnReleased, OnBetween;


    void Start() {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    void Update(){        

        //HUD.text = GetValue().ToString();
        float value = GetValue() * forcePercentage/100;


        if(UseFullPressAndRealese){
            if(!_isPressed && GetValue() + treshold >= 1){
                Pressed(value);
            }  

            if(!_isPressed && GetValue() + treshold < 1){
                Between(value);
            }  

            if(_isPressed && GetValue() - treshold <= 0){
                Released(value);
            }  
        } else if(!UseFullPressAndRealese){
            Between(value);
        }


    }


    public float GetValue() {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if(Mathf.Abs(value) < deadZone) {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed(float value){
        _isPressed = true;
        OnPressed.Invoke(value);
    }

    private void Released(float value){
        _isPressed = false;
        OnReleased.Invoke(value);
    }


    private void Between(float value){
        OnBetween.Invoke(value);
    }
}

