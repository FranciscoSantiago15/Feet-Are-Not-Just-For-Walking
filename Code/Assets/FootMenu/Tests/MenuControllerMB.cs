using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MenuControllerMB : MonoBehaviour
{
    private Dummy.Representation rep;


    private float radius = 1f;
    

    private Menu FirstMenu;
    public Menu Data;
    public MenuCakePiece MenuCakePiecePrefab;
    public float GapWidthDegree = 1f;

    protected MenuCakePiece[] Pieces;
    protected MenuMB Parent;



    private GameObject colOld, highOld;

    List<string> colNames;

    private bool canPerform, piecesIsActive = false;

    private bool isfirstInter = true;


    private XRNode headset = XRNode.Head;
    private XRNode controller = XRNode.RightHand;

    private GameObject auxIndirect;

    private bool menuFollowingPlayer = false;

    private Vector3 positionForMenu;

    
    void Update() 
    {
        InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.primary2DAxis, out var trackpad);
        InputDevices.GetDeviceAtXRNode(controller).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var press);

    }

}
