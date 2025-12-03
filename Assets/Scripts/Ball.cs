using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public float velocidadInicial = 15f;
    public float velocidadMaxima = 20f;
    public float velocidadMinima = 10f;

    private bool lanzada = false;

    // 1. Start()
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // La bola empieza sin moverse, esperando el lanzamiento.
        // Como está en un Canvas, es mejor controlarla manualmente al inicio.
        rb.bodyType = RigidbodyType2D.Kinematic; 
    }

    // 2. Update() para gestionar el lanzamiento
    void Update()
    {
        if (!lanzada && Input.GetKeyDown(KeyCode.Space))
        {
            lanzada = true;

            // ¡IMPORTANTE! Liberamos la bola del Canvas para que las físicas funcionen en el mundo del juego.
            transform.SetParent(null);

            // Activamos las físicas y damos el impulso inicial.
            rb.bodyType = RigidbodyType2D.Dynamic; 
            rb.linearVelocity = Vector2.up * velocidadInicial;
        }

        // Si la bola ya fue lanzada, aseguramos que la velocidad se mantenga dentro de los límites
        if (lanzada)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Clamp(rb.linearVelocity.magnitude, velocidadMinima, velocidadMaxima);
        }
    }

    // 3. OnCollisionEnter2D se llama al detectar una colisión
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Lógica para un rebote más dinámico con la paleta
            ContactPoint2D contacto = collision.contacts[0];
            float desplazamientoX = contacto.point.x - collision.transform.position.x;
            float factorRebote = desplazamientoX / (collision.collider.bounds.size.x / 2);
            Vector2 nuevaDireccion = new Vector2(factorRebote, 1f).normalized;
            rb.linearVelocity = nuevaDireccion * rb.linearVelocity.magnitude;
        }
    }
}