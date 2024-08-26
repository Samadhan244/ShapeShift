using UnityEngine;

[SelectionBase]
public class Mover : MonoBehaviour
{
    [SerializeField] float moveX, moveY, moveZ, touchDistance = 1f;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(moveX, moveY, moveZ), out hit, touchDistance))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                moveX = -moveX;
                moveY = -moveY;
                moveZ = -moveZ;
            }
        }
        transform.position += new Vector3(moveX, moveY, moveZ) * Time.deltaTime;
    }
}