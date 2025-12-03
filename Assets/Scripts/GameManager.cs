using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar/cambiar escenas
using UnityEngine.UI; // Necesario para trabajar con componentes de UI como 'Image'

public class GameManager : MonoBehaviour
{
    // 1. Singleton - Permite acceder a esta instancia desde cualquier script
    public static GameManager Instancia { get; private set; }

    [Header("Variables de Juego")]
    public int Puntuacion = 0;
    public int Vidas = 3;
    private int ladrillosRestantes; // Para saber cuándo ganar

    [Header("Referencias de Escena")]
    [Tooltip("Arrastra aquí el Canvas principal del juego donde se instanciará la bola.")]
    public Canvas gameCanvas;
    
    [Header("Audio")]
    [Tooltip("Arrastra aquí el clip de audio para la música de fondo.")]
    public AudioClip musicaDeFondo;
    [Tooltip("Sonido que se reproduce al romper un ladrillo.")]
    public AudioClip sonidoLadrilloRoto;
    private AudioSource audioSource;

    // ----------------------------------------------------
    
    // 2. Awake() se llama al cargar el script, antes de Start()
    private void Awake()
    {
        // Implementación del Singleton
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject); // Destruye si ya existe otra instancia
        }
        else
        {
            Instancia = this; // Se establece como la única instancia
            transform.SetParent(null); // Asegura que el GameManager sea un objeto raíz
            DontDestroyOnLoad(gameObject); // Evita que el GameManager se destruya al cargar otra escena

            // Configurar y reproducir la música de fondo
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) // Si no existe un AudioSource, lo añade
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (musicaDeFondo != null)
            {
                audioSource.clip = musicaDeFondo;
                audioSource.loop = true; // Para que la música se repita
                audioSource.Play();
            }

            // Asegurarse de que el ScoreManager exista
            if (ScoreManager.Instancia == null) { /* No hacer nada, pero la referencia fuerza su creación si está bien configurado */ }
        }
    }

    // 3. Start() se llama una vez al inicio
    private void Start()
    {
        // Encontrar todos los ladrillos al inicio del nivel
        ladrillosRestantes = FindObjectsByType<Brick>(FindObjectsSortMode.None).Length;
        // Iniciar el juego
        ReiniciarBola();
    }

    // 4. Método para instanciar la bola y prepararla para el lanzamiento
    public void ReiniciarBola()
    {
        // Destruir cualquier bola existente antes de crear una nueva
        GameObject bolaExistente = GameObject.FindWithTag("Ball"); 
        if (bolaExistente != null)
        {
            Destroy(bolaExistente);
        }

        // Crear una nueva bola desde cero
        CrearBolaConCodigo();
    }

    private void CrearBolaConCodigo()
    {
        // 1. Crear un GameObject vacío para la bola
        GameObject nuevaBola = new GameObject("Ball");
        nuevaBola.tag = "Ball"; // Asignar el tag para las colisiones

        // 2. Añadir un componente Image (en lugar de SpriteRenderer) para que sea visible en el Canvas
        Image img = nuevaBola.AddComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Sprites/ball"); // Carga el sprite desde Assets/Resources/Sprites/ball.png
        img.SetNativeSize(); // Ajusta el tamaño de la imagen al tamaño original del sprite

        // 3. Añadir los componentes de física
        CircleCollider2D collider = nuevaBola.AddComponent<CircleCollider2D>();
        Rigidbody2D rb = nuevaBola.AddComponent<Rigidbody2D>();
        collider.radius = img.rectTransform.sizeDelta.x / 2; // Ajustar el radio del collider al tamaño de la imagen
        rb.gravityScale = 0; // Sin gravedad
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Detección de colisión precisa

        // 4. Añadir y configurar el script de comportamiento de la bola
        nuevaBola.AddComponent<Ball>();

        // 5. Posicionar la bola en el Canvas asignado
        if (gameCanvas != null)
        {
            // Hacemos la bola hija del Canvas para que se renderice en la UI
            nuevaBola.transform.SetParent(gameCanvas.transform, false);
            // El componente Image necesita un RectTransform, que SetParent añade automáticamente.
            // Lo posicionamos en el centro.
            RectTransform rectTransform = nuevaBola.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    // 5. Método llamado por los ladrillos al ser destruidos
    public void LadrilloDestruido(int puntosLadrillo)
    {
        // Reproducir sonido de ladrillo roto si está asignado
        if (sonidoLadrilloRoto != null)
        {
            audioSource.PlayOneShot(sonidoLadrilloRoto);
        }

        Puntuacion += puntosLadrillo;
        ladrillosRestantes--;
        
        // Comprobar si ya no quedan ladrillos (Condición de victoria)
        if (ladrillosRestantes <= 0)
        {
            // TODO: Lógica de ganar nivel/juego
            Debug.Log("¡Ganaste! Puntuación final: " + Puntuacion);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Carga el siguiente nivel
        }
    }

    // 6. Método llamado por la 'Zona de Muerte' (DeathZone)
    public void PerderVida()
    {
        Vidas--;

        if (Vidas <= 0)
        {
            // Condición de Game Over
            Debug.Log("Game Over. Puntuación: " + Puntuacion);
            // Registra la puntuación final en el ScoreManager
            if (ScoreManager.Instancia != null)
            {
                ScoreManager.Instancia.RegistrarNuevaPuntuacion(Puntuacion);
            }
            // TODO: Cargar la escena de Game Over o reiniciar el juego.
        }
        else
        {
            // Si quedan vidas, se reinicia la bola
            ReiniciarBola(); 
        }
    }
}