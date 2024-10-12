using System.Collections;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public string SceneName;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    public GameObject fadeOutPanel;
    public GameObject fadeInPanel;
    public float fadeWait;

    private void Awake()
    {
        if (fadeInPanel != null)
        {
            GameObject Panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(Panel, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStorage.initialValue = playerPosition;
            StartCoroutine(FadeCoroutine());
        }
    }

    public IEnumerator FadeCoroutine()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

}
[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initialValue;
    public Vector2 defaultValue;
    public void OnAfterDeserialize()
    {
        initialValue = defaultValue;
    }
    public void OnBeforeSerialize()
    {

    }
}
