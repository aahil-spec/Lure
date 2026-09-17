using UnityEngine;

public class PlayerInteract:MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange=3f;
    public KeyCode interactKey=KeyCode.E;

    private InventoryManager inventory;

    void Start()
    {
        inventory=GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Transform cam=Camera.main.transform;
            if (Physics.Raycast(cam.position,cam.forward,out RaycastHit hit,interactRange))
            {
                ItemPickup item=hit.collider.GetComponent<ItemPickup>();
                if (item!=null) item.PickUp(inventory);

            ObjectiveMachine machine=hit.collider.GetComponent<ObjectiveMachine>();
            if (machine!=null) machine.Interact(inventory);
            }
        }
    }
    
}
