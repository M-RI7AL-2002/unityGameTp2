using UnityEngine;

public class TrashItem : MonoBehaviour
{
    public string trashType;  

    private bool isDragging = false;
    private Vector3 offset;
    private Plane dragPlane;
    private Vector3 startPosition;  

    private float minHeight = 0.5f;
    private float maxHeight = 6f;

    void Start()
    {
        startPosition = transform.position; 
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Plane at object’s height (initial)
        dragPlane = new Plane(Vector3.up, transform.position);

        // Calculate offset between mouse ray and object
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(camRay, out float enter))
        {
            offset = transform.position - camRay.GetPoint(enter);
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(camRay, out float enter))
        {
            Vector3 target = camRay.GetPoint(enter) + offset;

            // Add scroll wheel adjustment for height
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            target.y += scroll * 6f; // multiply for speed

            // Clamp height
            target.y = Mathf.Clamp(target.y, minHeight, maxHeight);

            transform.position = target;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Check if dropped in a bin
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
        bool droppedInBin = false;

        foreach (Collider col in hits)
        {
            Bin bin = col.GetComponent<Bin>();
            if (bin != null)
            {
                GameManager.Instance.CheckTrashDrop(this, bin);
                droppedInBin = true;
                break;
            }
        }

        // If not dropped in a bin, return to start position
        if (!droppedInBin)
        {
            transform.position = startPosition;
        }
    }
}
