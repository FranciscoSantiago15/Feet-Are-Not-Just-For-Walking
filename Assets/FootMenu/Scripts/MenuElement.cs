using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "MenuElement 1", menuName = "FootMenu/Element", order = 2)]
public class MenuElement : ScriptableObject
{
    public string Name;
    public Sprite Icon;

    public bool IconFacingMiddle = false;

    public UnityEvent Action = new UnityEvent();

    public Menu NextMenu;

}



