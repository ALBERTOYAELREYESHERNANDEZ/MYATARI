using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad de movimiento
    public float limiteHorizontal = 8f; // Límite para que la paleta no salga de la pantalla

    // 1. Update() se llama en cada frame
    void Update()
    {
        // 2. Input del teclado (eje horizontal: flechas, A/D)
        float inputHorizontal = Input.GetAxis("Horizontal");

        // 3. Calcular el movimiento (dirección * velocidad * tiempo)
        // Time.deltaTime asegura que el movimiento sea independiente de la velocidad de fotogramas
        float movimiento = inputHorizontal * velocidad * Time.deltaTime;

        // 4. Nueva posición
        Vector3 nuevaPosicion = transform.position + new Vector3(movimiento, 0, 0);

        // 5. Limitar la posición horizontal (Clamp)
        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, -limiteHorizontal, limiteHorizontal);

        // 6. Aplicar la nueva posición
        transform.position = nuevaPosicion;

        // NOTA: Para un control más preciso con físicas (Rigidbody2D), se usaría FixedUpdate() 
        // y se manipularía la velocidad o la posición del Rigidbody.
    }
}