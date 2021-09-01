using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{


    public Material skybox_default, skybox1, skybox2, skybox3, skybox4, skybox5, skybox6, skybox7;

    public GameObject Headset, Tracker, button1, button2;

    private bool auxGo = false;

    private GameObject auxGOInst, auxTrackerInst;

    public void ChangeSkyDefault() {
        RenderSettings.skybox = skybox_default;
    }

    public void ChangeSky1() {
        RenderSettings.skybox = skybox1;
    }

    public void ChangeSky2() {
        RenderSettings.skybox = skybox2;
    }

    public void ChangeSky3() {
        RenderSettings.skybox = skybox3;
    }

    public void ChangeSky4() {
        RenderSettings.skybox = skybox4;
    }

    public void ChangeSky5() {
        RenderSettings.skybox = skybox5;
    }

    public void ChangeSky6() {
        RenderSettings.skybox = skybox6;
    }

    public void ChangeSky7() {
        RenderSettings.skybox = skybox7;
    }
    
    public void InstatiateHeadset(){
        Destroy(auxGOInst);
        Destroy(auxTrackerInst);
        Headset.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        auxGOInst = Instantiate(Headset, new Vector3(-0.6f, 1f, -1), Quaternion.Euler(0, 215f, 0));
    }

    public void DeleteHeadset() {
        Destroy(auxGOInst);
    }


    public void InstantiateTracker() {
        Destroy(auxGOInst);
        Destroy(auxTrackerInst);
        Tracker.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        auxTrackerInst = Instantiate(Tracker, new Vector3(-0.6f, 1f, -1), Quaternion.Euler(0, 215f, 0));
        auxTrackerInst.transform.Rotate(Vector3.up);
    }

    public void DeleteTracker() {        
        Destroy(auxTrackerInst);       
    }
}
