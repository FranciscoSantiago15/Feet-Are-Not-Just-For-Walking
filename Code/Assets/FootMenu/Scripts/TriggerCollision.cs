using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerCollision : MonoBehaviour
{
    
    public bool selected = false;

    public UnityEvent action = new UnityEvent();

    private void OnTriggerExit(Collider other) {
        if(string.Equals(other.name, "FootRepresentation")){
            selected = true;
            action.Invoke();
        }
        
    } 

}
