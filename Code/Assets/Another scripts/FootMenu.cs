using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootMenu : MonoBehaviour
{

    public List<MenuButton> buttons = new List<MenuButton>();
    //private Vector3 trackerPosition;
    private Vector2 MousePosition;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 CenterCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public int MenuItens;
    public int CurrentMenuItem;
    private int OldMenuItem;
    
    void Start()
    {
        MenuItens = buttons.Count;
        foreach(MenuButton button in buttons){
            button.sceneimage.color = button.NormalColor;
        }

        CurrentMenuItem = 0;
        OldMenuItem = 0;

    }

    // Update is called once per frame
    void Update()
    {
        getCurrentMenuItem();
        
        if(Input.GetButtonDown("Fire1"))
            ButtonAction();
        
    }

    public void getCurrentMenuItem() {
        MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        toVector2M = new Vector2(MousePosition.x/Screen.width, MousePosition.y/Screen.width);
        float angle = (Mathf.Atan2(fromVector2M.y - CenterCircle.y, fromVector2M.x - CenterCircle.x) - Mathf.Atan2(toVector2M.y - CenterCircle.y, toVector2M.x - CenterCircle.x)) * Mathf.Rad2Deg;
    
        if(angle < 0)
            angle += 360;

        CurrentMenuItem = (int)(angle / (360 / MenuItens));

        if(CurrentMenuItem != OldMenuItem){
            buttons[OldMenuItem].sceneimage.color = buttons[OldMenuItem].NormalColor;
            OldMenuItem = CurrentMenuItem;
            buttons[CurrentMenuItem].sceneimage.color = buttons[CurrentMenuItem].HighlightedColor;
        }
    }


    public void ButtonAction() {
        buttons[CurrentMenuItem].sceneimage.color = buttons[CurrentMenuItem].PressedColor;

        if(CurrentMenuItem == 0)
            print("Pressed 0");
    }
}


[System.Serializable]
public class MenuButton
{
    public string Name;
    public Image sceneimage;
    public Color NormalColor = Color.white;
    public Color HighlightedColor = Color.grey;
    public Color PressedColor = Color.gray;
} 
