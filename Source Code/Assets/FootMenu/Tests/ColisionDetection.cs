using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColisionDetection : MonoBehaviour
{
    public UnityEvent action;

    public MenuElement Element;

    public string nameCol;

    private void OnCollisionEnter(Collision col) {
        nameCol = this.transform.name;
        Debug.Log(col.transform.name);
    }
}
