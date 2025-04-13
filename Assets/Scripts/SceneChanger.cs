using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void CambiarEscena(string escena, float retraso)
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.CambiarEscenaCoroutine(escena, retraso));
        }
    }

    private IEnumerator CambiarEscenaCoroutine(string escena, float retraso)
    {
        yield return new WaitForSeconds(retraso);
        SceneManager.LoadScene(escena);
    }
}