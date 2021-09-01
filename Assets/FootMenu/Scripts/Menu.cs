using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


[CreateAssetMenu(fileName = "Menu 1", menuName = "FootMenu/Menu", order = 1)]
public class Menu : ScriptableObject
{
    public enum Degrees { _180, _360 }
    public Degrees degrees;
    public MenuElement[] Elements;   

}


