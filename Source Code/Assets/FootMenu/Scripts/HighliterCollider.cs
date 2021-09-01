using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighliterCollider : MonoBehaviour
{

    public MenuCakePiece Element;

    private Color originalColor;

    private void OnCollisionEnter(Collision col) {
        if(string.Equals(col.transform.name, "FootRepresentation"))
            Element.CakePiece.color = new Color(1f, 1f, 1f, 0.80f);
    }

    private void OnCollisionExit(Collision col) {
        if(string.Equals(col.transform.name, "FootRepresentation"))
            Element.CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
    }
}
