using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ToeTap : MonoBehaviour
{
    private int state = -1;

    private int STATE_LOW_BEGIN = 0;
    private int STATE_RISING = 1;
    private int STATE_RISEN = 2;
    private int STATE_FALLING = 3;
    private int STATE_LOW_END = 4;

    private int lastRotX, lastRotY, lastRotZ;

    public XRNode tracker;


    void Start()
    {
        state = STATE_LOW_BEGIN;
        lastRotX = lastRotY = lastRotZ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Quaternion rotAux = new Quaternion();
        Vector3 rot = new Vector3();

        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(CommonUsages.devicePosition, out pos);
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(CommonUsages.deviceRotation, out rotAux);
        rot = rotAux.eulerAngles;

        switch(state) {
            case 0: 
                if (rot.y > 210 && rot.z < 145 && pos.y > 0 && pos.z > 0) {
                    state = STATE_RISING;
                }
                break;
            case 1:
                if (rot.y > 200 && rot.z < 150 && rot.y <= 270 && rot.z >= 100 && pos.y > 0 && pos.z > 0) {
                    // same state
                    } else if (rot.y > 270 && rot.z < 100) {
                        //this._start = time;
                        state = STATE_RISEN;
                        //console.log("RISEN",  this._index)   
                    } else {
                        // reset
                        state = STATE_LOW_BEGIN;
                        //console.log("LOW_BEGIN", this._index)   
                    }    
                break;
            case 2:
                if (rot.y > 270 && rot.z < 100 && pos.y > 0 && pos.z > 0) {
                  // same state
                } else if (rot.y < 270 && rot.z > 90 ) {
                  state = STATE_FALLING;
                  //console.log("FALLING", this._index) 
                  //this._start = time;
                }
                break;
            case 3:
                if (rot.y < 270 && rot.z > 90 && rot.y >= 220 && rot.z <=155 && pos.y > 0 && pos.z > 0) {
                    // same state
                    } else if (rot.y < 240 && rot.z > 125 ) {
                        state = STATE_LOW_BEGIN;
                        //console.log("LOW_BEGIN", this._index) 
                        Debug.Log("ToeTap");
                        //Debug.Log.log(this._data[this._index]);
                    } else {
                        state = STATE_LOW_BEGIN;
                    //console.log("LOW_BEGIN", this._index) 
                    }
                break;
        }
        
    }
}
