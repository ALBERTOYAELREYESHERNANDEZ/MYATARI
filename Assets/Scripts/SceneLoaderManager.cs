using UnityEngine;
using UnityEngine.SceneManagement; // 👈 1. Necesitas esta librería para trabajar con escenas

public class SceneLoaderManager : MonoBehaviour
{
    // 2. Método público para que el botón pueda llamarlo
    // Le pasamos el nombre o el índice de la escena que queremos cargar
    public void CargarEscenaDeJuego(string nombreEscena)
    {
        // 3. Verifica si la escena existe (opcional, pero buena práctica)
        if (Application.CanStreamedLevelBeLoaded(nombreEscena))
        {
            // 4. Llama al gestor de escenas para cargar la escena
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            // Muestra un error si la escena no se encuentra
            Debug.LogError("La escena con el nombre '" + nombreEscena + "' no se encuentra o no está en la configuración de Build Settings.");
        }
    }
    
    // 5. Método alternativo para salir del juego (buena práctica para el botón "Salir")
    public void SalirDelJuego()
    {
        // Esto solo funciona en una aplicación compilada (no en el editor de Unity)
        Application.Quit();

        // Para el editor, se usa esta línea (comentar en la versión final)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}