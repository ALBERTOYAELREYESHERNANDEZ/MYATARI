using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar/cambiar escenas

public class GameManager : MonoBehaviour
{
    // 1. Singleton - Permite acceder a esta instancia desde cualquier script
    public static GameManager Instancia { get; private set; }

    [Header("Variables de Juego")]
    public int Puntuacion = 0;
    public int Vidas = 3;
    private int ladrillosRestantes; // Para saber cuándo ganar
    
    // Referencia al prefab de la bola
    public GameObject prefabBola;
    // Posición de inicio de la bola/paleta
    public Vector3 posicionInicialBola; 

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
        }
    }

    // 3. Start() se llama una vez al inicio
    private void Start()
    {
        // Encontrar todos los ladrillos al inicio del nivel
        ladrillosRestantes = FindObjectsOfType<Brick>().Length;
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

        // Crear una nueva bola en la posición inicial (encima de la paleta)
        Instantiate(prefabBola, posicionInicialBola, Quaternion.identity);
    }

    // 5. Método llamado por los ladrillos al ser destruidos
    public void LadrilloDestruido(int puntosLadrillo)
    {
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
            // TODO: Mostrar pantalla de Game Over, reiniciar juego
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reinicia el nivel actual
        }
        else
        {
            // Si quedan vidas, se reinicia la bola
            ReiniciarBola(); 
        }
    }
}