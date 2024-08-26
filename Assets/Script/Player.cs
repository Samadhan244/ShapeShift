using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jump, moneyyy, death, checkpoint, victory;
    public bool isAlive = true, inWater;
    public int money, style = 1, tries = 1; public Text moneyText, triesText;
    public float speed, jumpHeight = 2f, verticalVelocity, textDelayTime;
    [SerializeField] GameObject sphere, cube, sphereDeath, cubeDeath;
    [SerializeField] Animator sphereDeathAnim, cubeDeathAnim;
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform cam;
    Button button; Button2 button2;
    Vector3 spawnPoint;
    [SerializeField] Text text;
    string[] deathTexts =
{
    "Try again!",
    "Never give up!",
    "You had it!",
    "Almost there!",
    "Don't lose hope!",
    "Keep pushing!",
    "You'll get it next time!",
    "Stay determined!",
    "Not bad, keep going!",
    "Just a little more!",
    "Believe in yourself!",
    "Practice makes perfect!",
    "Learn from your mistakes!",
    "Keep your head up!",
    "Stay strong!",
    "You can do it!",
    "Keep going!",
    "Don't stop now!",
    "Persistence pays off!",
    "Stay focused!",
    "You're getting closer!",
    "Success is near!",
    "One step at a time!",
    "Every failure is a lesson!",
    "You're improving!",
    "You're on the right track!",
    "You're unstoppable!",
    "The journey continues!",
    "Victory is within reach!",
    "Embrace the challenge!"
};

    string[] moneyTexts =
    {
    "Money can't buy happiness, but it sure makes life easier!",
    "In money we trust!",
    "More money, more fun!",
    "Cha-ching!",
    "Money talks!",
    "Cash rules everything around me, C.R.E.A.M. get the money!",
    "Money makes the world go round!",
    "Show me the money!",
    "Money grows on trees in video games!",
    "Money is the root of all joy!",
    "Rolling in dough!",
    "Money isn't everything, but it sure beats whatever is in second place!",
    "I work hard for the money, so hard for it, honey!",
    "Money doesn't buy happiness, but it buys things that make me happy!",
    "Money talks, wealth whispers!"
};

    void Start()
    {
        spawnPoint = transform.position;
    }

    void Update()
    {
        textDelayTime -= Time.deltaTime;
        if (textDelayTime <= 0) text.text = null;
        Movements();
    }

    void Movements()
    {
        if (isAlive)
        {
            speed = (!inWater) ? 8f : 4f;
            Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            // Jump & Gravity
            if (!inWater)
            {
                if (characterController.isGrounded && Input.GetButtonDown("Jump")) audioSource.PlayOneShot(jump);
                if (characterController.isGrounded) verticalVelocity = (Input.GetButtonDown("Jump")) ? (style == 1 ? jumpHeight : jumpHeight / 1.5f) : -1;
                else verticalVelocity -= 5 * Time.deltaTime;
            }
            else
            {
                if (style == 1) verticalVelocity = 0.4f;
                else verticalVelocity = -1;
            }
            movementDirection.y = verticalVelocity;

            // Movements & Rotation
            characterController.Move(movementDirection * speed * Time.deltaTime);
            if (movementDirection.x != 0 || movementDirection.z != 0) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(-movementDirection.z, 0f, movementDirection.x)), Time.deltaTime * 10f);

            // Camera follow Player
            cam.position = new Vector3(transform.position.x, transform.position.y + 6f, -10f);

            // ShapeShift
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (style == 1)
                {
                    style = 2;
                    sphere.SetActive(false);
                    cube.SetActive(true);
                    if (button) button.Open();
                    if (button2) button2.Open();
                }
                else
                {
                    style = 1;
                    sphere.SetActive(true);
                    cube.SetActive(false);
                    if (button) button.Close();
                }

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            audioSource.PlayOneShot(moneyyy);
            Destroy(other.gameObject);
            money += 1;
            moneyText.text = money + " /15";
            textDelayTime = 3f;
            text.text = moneyTexts[Random.Range(0, moneyTexts.Length)];
        }
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            audioSource.PlayOneShot(checkpoint);
            spawnPoint = other.gameObject.transform.position;
            textDelayTime = 3f;
            text.text = "Checkpoint";
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
        else if (other.gameObject.CompareTag("Button"))
        {
            if (!button) button = other.GetComponent<Button>();
            if (style == 2) button.Open();
        }
        else if (other.gameObject.CompareTag("Button2"))
        {
            button2 = other.GetComponent<Button2>();
            if (style == 2) button2.Open();
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            inWater = true;
        }
        else if (other.gameObject.CompareTag("Ending"))
        {
            audioSource.clip = null;
            audioSource.PlayOneShot(victory);
            isAlive = false;
            textDelayTime = 11f;
            text.text = "Victory is yours!";
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
            this.Wait(11f, () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Exit Play mode in the Editor
#else
        Application.Quit(); // Quit the application (works for standalone builds)
#endif
            });
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
            if (style == 2)
            {
                other.gameObject.SetActive(false);
                this.Wait(1f, () => other.gameObject.SetActive(true));
            }
        }
        else if (other.gameObject.CompareTag("Breakable2"))
        {
            if (style == 1)
            {
                other.gameObject.SetActive(false);
                this.Wait(1f, () => other.gameObject.SetActive(true));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Button"))
        {
            button.Close();
            button = null;
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            inWater = false;
        }
    }

    void Death()
    {
        if (!isAlive) return;  // Prevent bug, where death happens twice, so it gives two "Tries" instead of one
        audioSource.PlayOneShot(death);
        isAlive = false;
        if (style == 1)
        {
            sphere.gameObject.SetActive(false);
            sphereDeath.gameObject.SetActive(true);
            sphereDeathAnim.Play("Death");
        }
        else
        {
            cube.gameObject.SetActive(false);
            cubeDeath.gameObject.SetActive(true);
            cubeDeathAnim.Play("Death");
        }
        this.Wait(1f, () =>
        {
            if (style == 1)
            {
                sphere.gameObject.SetActive(true);
                sphereDeath.gameObject.SetActive(false);
            }
            else
            {
                cube.gameObject.SetActive(true);
                cubeDeath.gameObject.SetActive(false);
            }
            // Ensure the CharacterController updates its position
            characterController.enabled = false;
            transform.position = spawnPoint;
            characterController.enabled = true;

            // Optionally, you can use characterController.Move to ensure proper update
            characterController.Move(Vector3.zero);
            isAlive = true;
            tries += 1;
            triesText.text = "Tries: " + tries;
        });

        textDelayTime = 3f;
        text.text = deathTexts[Random.Range(0, deathTexts.Length)];
    }
}