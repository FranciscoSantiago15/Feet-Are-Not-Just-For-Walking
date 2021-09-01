using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class FootButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;

    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool previousPress = false;

    private float previousFootHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;
    
    protected override void Awake() {
        base.Awake();
        onHoverEntered.AddListener(StartPress);
        onHoverExited.AddListener(EndPress);
    }

    private void OnDestroy() {
        onHoverEntered.RemoveListener(StartPress);
        onHoverExited.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor) {
        hoverInteractor = interactor;
        previousFootHeight = GetLocalYPosition(hoverInteractor.transform.position);
    }

    private void EndPress(XRBaseInteractor interactor) {
        hoverInteractor = null;
        previousFootHeight = 0.0f;

        previousPress = false;
        SetYPosition(yMax);
    }

    private void Start() {
        SetMinMax();
    }

    private void SetMinMax() {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(hoverInteractor){
            float newFootHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float footDifference = previousFootHeight - newFootHeight;
            previousFootHeight = newFootHeight;

            float newPosition = transform.localPosition.y - footDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    private float GetLocalYPosition(Vector3 position) {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    private void SetYPosition(float position) {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;

    }

    private void CheckPress() {
        bool inPosition = InPosition();

        if(inPosition && inPosition != previousPress) {
            OnPress.Invoke();
        }

        previousPress = inPosition;
    }

    private bool InPosition() {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }
    
}
