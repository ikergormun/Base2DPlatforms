using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //Queremos modificar algo de un componente del GameObject
    Rigidbody2D rb;
    PlayerInput playerInput; //Nuevo Input
    SpriteRenderer sr; //Variable del render del sprite

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

        //Recupero el componente del sprite
        sr = GetComponent<SpriteRenderer>();

        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Antiguo input Jump
        /*if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
        */

        //movementInput = Input.GetAxis("Horizontal"); Antiguo input Movement
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>(); // Nuevo Input

        //Que el sprite se gire en direccion al movimiento
        if(movementInput.x != 0f)
        {
            if(movementInput.x > 0f) //Si va hacia la derecha
            {
                sr.flipX = false;
            }
            else //si va hacia la izquierda
            {
                sr.flipX = true;
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Started

        //Performed
        if (context.performed && isGrounded)
        {
            Debug.Log("Jump");
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        //Canceled

    }

    private void FixedUpdate()
    {
        //Gestionar el is Grounded con Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

        Debug.DrawRay(transform.position, Vector2.down, Color.red, 0.45f); //Para poder visualizar el raycast
        Debug.Log($"Distance: {hit.distance}"); //Ver la distancia a la que esta

        if(hit.collider != null && hit.distance < 0.49f && rb.linearVelocityY < 0.01f)
        {
            //Estamos tocando el suelo
            isGrounded = true;
        }

        //Aplicamos la fuerza según el input
        rb.AddForce(Vector3.right * movementInput.x * movementForce);
    }
}
