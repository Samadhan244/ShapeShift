using UnityEngine;

[SelectionBase]
public class Spinner : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void Randomizer()
    {
        rotationSpeed = Random.Range(100f, 500f);  // რენდომად აარჩევს სიჩქარეს 100-500 ჩათვლით
        rotationSpeed = Random.Range(1, 3) == 1 ? rotationSpeed : -rotationSpeed;  // არჩეული სიჩქარე ან დადებითში იქნება, ან უარყოფითში (50-50% შანსი)
    }
}