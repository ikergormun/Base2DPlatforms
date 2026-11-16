using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerInput playerInput; //Nuevo Input

    [SerializeField] float movementForce = 10.0f;
    [SerializeField] float jumpForce = 5f;

    Vector2 movementInput;
    public bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Recuperamos los componentes
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>(); //Recuperar Nuevo Input

        //Modo para evitar colisionar con su collider
        Physics2D.queriesStartInColliders = false;

        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
        //movementInput = Input.GetAxis("Horizontal"); Nuevo input
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        //Gestionar el is Grounded con Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

        Debug.DrawRay(transform.position, Vector2.down, Color.red, 0.45f);
        Debug.Log($"Distance: {hit.distance}");

        if(hit.collider != null && hit.distance < 0.49f && rb.linearVelocityY < 0.01f)
        {
            //Estamos tocando el suelo
            isGrounded = true;
        }

        //Aplicamos la fuerza según el input
        rb.AddForce(Vector3.right * movementInput.x * movementForce);
    }
}
