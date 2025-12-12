using UnityEngine;

<<<<<<< Updated upstream

// Requiere que el objeto tenga un Rigidbody2D
[RequireComponent(typeof(Rigidbody2D))] 
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocidadInicial = 12f;
    public float velocidadMaxima = 20f;
    public float velocidadMinima = 10f;
    private bool lanzada = false;

    // 1. Start()
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
=======
[RequireComponent(typeof(Rigidbody2D))] // Ahora requerimos el Rigidbody 2D
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb; // Cambiamos a Rigidbody2D
    public float velocidadMaxima = 20f;
    public float velocidadMinima = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtenemos el Rigidbody2D
        
        // --- FORZAR CONFIGURACIÓN DEL RIGIDBODY DESDE CÓDIGO ---
        // Esto anula cualquier configuración incorrecta en el Inspector.
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0; // Sin gravedad.
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Solo congelamos la rotación.
        
        LanzarBola();
>>>>>>> Stashed changes
    }

    private void LanzarBola()
    {
<<<<<<< Updated upstream
        // Si la bola no ha sido lanzada y se presiona el espacio
        if (!lanzada && Input.GetKeyDown(KeyCode.Space))
        {
            lanzada = true;
            // Impulso inicial (por ejemplo, hacia arriba)
            rb.linearVelocity = Vector2.up * velocidadInicial; 
        }

        // Si la bola ya fue lanzada, aseguramos que la velocidad se mantenga dentro de los límites
        if (lanzada)
        {
            // Normalizar la velocidad (dirección) y luego multiplicar por la velocidad deseada
            rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Clamp(rb.linearVelocity.magnitude, velocidadMinima, velocidadMaxima);
=======
        float direccionX = Random.value < 0.5f ? -1f : 1f; // Dirección horizontal aleatoria

        // Usamos Vector2 para la dirección en 2D.
        Vector2 direccion = new Vector2(direccionX * 0.5f, 1).normalized;

        rb.linearVelocity = direccion * velocidadMinima;

        rb.WakeUp(); // Aseguramos que el Rigidbody esté activo
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0)
        {
            // Mantenemos la velocidad dentro de los límites
            rb.linearVelocity = rb.linearVelocity.normalized * 
                Mathf.Clamp(rb.linearVelocity.magnitude, velocidadMinima, velocidadMaxima);
>>>>>>> Stashed changes
        }
    }

    // Cambiamos a OnCollisionEnter2D para colisiones 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Lógica opcional para un rebote más dinámico con la paleta
        if (collision.gameObject.CompareTag("Paddle"))
        {
<<<<<<< Updated upstream
            // Obtener el punto de contacto
            ContactPoint2D contacto = collision.contacts[0];
            
            // Calcular el desplazamiento relativo al centro de la paleta
            float desplazamientoX = contacto.point.x - collision.transform.position.x;
            
            // Normalizar el desplazamiento para obtener un factor entre -1 y 1
            float factorRebote = desplazamientoX / (collision.collider.bounds.size.x / 2);

            // Dirección de rebote: Vector2.up + factor horizontal
            Vector2 nuevaDireccion = new Vector2(factorRebote, 1).normalized;

            // Mantener la magnitud (velocidad) pero cambiar la dirección
=======
            ContactPoint2D contacto = collision.GetContact(0);

            float desplazamientoX =
                (contacto.point.x - collision.transform.position.x)
                / collision.collider.bounds.size.x;

            // Nueva dirección de rebote en 2D
            Vector2 nuevaDireccion = new Vector2(desplazamientoX, 1f).normalized;

>>>>>>> Stashed changes
            rb.linearVelocity = nuevaDireccion * rb.linearVelocity.magnitude;
        }
    }
}