using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class DataStore : MonoBehaviour
{

    public string file;

    public XRNode tracker;

    public XRNode inputSource;
    public InputHelpers.Button button;

    public class TrackerData {
        public Vector3 pos {get; set; }
        public Quaternion rot {get; set; }
        public Vector3 angV {get; set; }
        public Vector3 v {get; set; }

        public TrackerData(Vector3 pos, Quaternion rot, Vector3 angV, Vector3 v){
            this.pos = pos;
            this.rot = rot;
            this.angV = angV;
            this.v = v;
        }

        public string toString() {
            return "Position:\tX: " + this.pos.x + "  |  Y: " + this.pos.y + "  |  Z: " + this.pos.z + 
            "\nRotation:\tX: " + this.rot.eulerAngles.x + "  |  Y: " +  this.rot.eulerAngles.y + "  |  Z: " +  this.rot.eulerAngles.z + 
            "\nAng Velocity:\tX: " + this.angV.x + "  |  Y: " + this.angV.y + "  |  Z: " + this.angV.z + 
            "\nVelocity:\tX: " + this.v.x + "  |  Y: " + this.v.y + "  |  Z: " + this.v.z + 
            "\n_____________________________________________________________________________________________";
        }

        public string toCSV() {
             return this.pos.x + "," + this.pos.y + "," + this.pos.z + "," + this.rot.eulerAngles.x + "," +  this.rot.eulerAngles.y + "," +  this.rot.eulerAngles.z + "," + this.angV.x + "," + this.angV.y + "," + this.angV.z + "," + this.v.x + "," + this.v.y + "," + this.v.z;
        }

    }


    private List<TrackerData> lista;


    private bool auxPres = false; 

    private bool auxSave = false;

    private int numeroEstado = 1;


    // Start is called before the first frame update
    void Start()
    {
        lista = new List<TrackerData>();   
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 position;
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out position);

        Quaternion rotation;
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out rotation);

        Vector3 angVeloc;
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceAngularVelocity, out angVeloc);

        Vector3 veloc;
        InputDevices.GetDeviceAtXRNode(tracker).TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out veloc);


        var aux = new TrackerData(position, rotation, angVeloc, veloc);
        

        


        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), button, out bool isInputButtonPressed, 1);

        if(isInputButtonPressed) {
            Debug.Log(position);
            lista.Add(aux);
            auxSave = true;
        }


        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), InputHelpers.Button.PrimaryButton, out bool isSecPressed, 1);
        if(isSecPressed && auxSave){
            WriteString(numeroEstado, lista);
            WriteStringCSV(numeroEstado, lista);
            numeroEstado++;
            auxSave = false;
            lista.Clear();
        }

    }



    public void SaveFile(List<TrackerData> lista){
        string dest = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if(File.Exists(dest)) 
            file = File.OpenWrite(dest);
        else
            file = File.Create(dest);


        BinaryFormatter bf = new BinaryFormatter();
        
        //Debug.Log(lista[2].toString());

        bf.Serialize(file, lista);

        file.Close();
    }



    //[MenuItem("Tools/Write file")]
    static void WriteString(int num,List<TrackerData> lista){
        string path = "Assets/Resouces/ToeTap.txt";

        StreamWriter writer = new StreamWriter(path, true);
        //writer.WriteLine("************** INICIO **************");
        writer.WriteLine("\n\n::::::::: " +num+ " :::::::::\n\n");

        foreach(var order in lista){
            writer.WriteLine(order.toString());
        }

        //writer.WriteLine("************** FIM **************");
        writer.Close();

        /*AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("test");

        Debug.Log(asset.text);*/

    }

    static void WriteStringCSV(int num,List<TrackerData> lista){
        string path = "Assets/Resouces/ToeTapCSV.txt";

        StreamWriter writer = new StreamWriter(path, true);
        //writer.WriteLine("************** INICIO **************");
        writer.WriteLine("\n\n::::::::: " +num+ " :::::::::");

        writer.WriteLine("Position (X), Position (Y), Position (Z), Rotation (X), Rotation (Y), Rotation (Z), Ang Veloc (X), Ang Veloc (Y), Ang Veloc (Z), Velocity (X), Velocity (Y), Velocity (Z)");

        foreach(var order in lista){
            writer.WriteLine(order.toCSV());
        }

        //writer.WriteLine("************** FIM **************");
        writer.Close();
    }

}
