using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovemnet : MonoBehaviour
{

    private GameObject drone;

    public void MoveUp(float value) {
        FindObject();
        drone.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y + value, drone.transform.position.z);
    }

    public void MoveDown(float value) {
        FindObject();
        drone.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y - value, drone.transform.position.z);
    }

    public void MoveRight(float value) {
        FindObject();
        drone.transform.position = new Vector3(drone.transform.position.x + value, drone.transform.position.y, drone.transform.position.z);
    }

    public void MoveLeft(float value) {
        FindObject();
        drone.transform.position = new Vector3(drone.transform.position.x - value, drone.transform.position.y, drone.transform.position.z);
    }

    public void MoveForward(float value) {
        FindObject();
        drone.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, drone.transform.position.z + value);
    }

    public void MoveBackward(float value) {
        FindObject();
        drone.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y, drone.transform.position.z - value);
    }

    private void FindObject() {
        drone = this.gameObject;        
    }


}
