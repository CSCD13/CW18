using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour {
    public InputActionProperty gripAction;
    public float grabRange = 0.15f;

    private Rigidbody heldObject;
    private Vector3 lastPosition;
    private Vector3 handVelocity;

    void Update() {
        handVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        float gripValue = gripAction.action.ReadValue<float>();

        if (gripValue > 0.5f && heldObject == null) {
            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, grabRange);
            foreach (Collider col in nearbyObjects) {
                if (col.CompareTag("Grabbable")) {
                    heldObject = col.GetComponent<Rigidbody>();
                    heldObject.isKinematic = true;
                    heldObject.transform.SetParent(transform); 
                    break;
                }
            }
        }
        else if (gripValue < 0.5f && heldObject != null) {
            heldObject.transform.SetParent(null); 
            heldObject.isKinematic = false; 
            heldObject.linearVelocity = handVelocity; 
            heldObject = null; 
        }
    }
}