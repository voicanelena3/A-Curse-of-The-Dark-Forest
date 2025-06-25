using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ProgressBar healthBar;

    public float Speed;
    public float RotationSpeed;
    public float JumpForce;
    public float CameraRotationLimit;
    public float maxPlayerHP;
    public float currentPlayerMP;
    private Rigidbody rb;
    private GameObject MainCamera;
    private float cameraRotation = 0;
    private DialogueManager dialogueManager;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        MainCamera = GameObject.Find("Main Camera");
        healthBar.MaxValue = maxPlayerHP;
        healthBar.CurrentValue = currentPlayerMP;
        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 input = new Vector3(horizontalInput, 0, verticalInput);
        this.transform.Translate(input * Speed * Time.deltaTime);
        float turn = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
        this.transform.Rotate(Vector3.up, turn);

        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPlayerMP -= 5;
            healthBar.CurrentValue -= 5;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPlayerMP += 5;
            healthBar.CurrentValue += 5;
        }

        float cameraTurn = Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime;
        cameraRotation -= cameraTurn;
        cameraRotation = Mathf.Clamp(cameraRotation, -CameraRotationLimit, CameraRotationLimit);
        MainCamera.transform.rotation = Quaternion.Euler(cameraRotation, this.transform.eulerAngles.y, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Deer") == true && DialogueManager.isActive==false)
        {
            var dialogueTrigger = collision.gameObject.GetComponents<DialogueTrigger>().FirstOrDefault(component => component.enabled);
            if (dialogueTrigger.isEnded == false)
            {
                dialogueTrigger.StartDialogue();
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        currentPlayerMP -= dmg;
        Debug.Log("Player took damage! Remaining: " + currentPlayerMP);
        if (currentPlayerMP <= 0)
        {
            
        }
    }
}
