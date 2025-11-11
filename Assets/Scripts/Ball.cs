using UnityEngine;


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
    }

    // 2. Update() para gestionar el lanzamiento
    void Update()
    {
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
        }
    }

    // 3. OnCollisionEnter2D se llama al detectar una colisión
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Lógica opcional para un rebote más dinámico con la paleta
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Obtener el punto de contacto
            ContactPoint2D contacto = collision.contacts[0];
            
            // Calcular el desplazamiento relativo al centro de la paleta
            float desplazamientoX = contacto.point.x - collision.transform.position.x;
            
            // Normalizar el desplazamiento para obtener un factor entre -1 y 1
            float factorRebote = desplazamientoX / (collision.collider.bounds.size.x / 2);

            // Dirección de rebote: Vector2.up + factor horizontal
            Vector2 nuevaDireccion = new Vector2(factorRebote, 1).normalized;

            // Mantener la magnitud (velocidad) pero cambiar la dirección
            rb.linearVelocity = nuevaDireccion * rb.linearVelocity.magnitude;
        }
    }
}