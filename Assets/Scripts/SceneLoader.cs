using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName; // Имя сцены для загрузки
    [SerializeField] private Button loadButton; // Кнопка для запуска сцены

    private void Start()
    {
        // Подписываемся на событие нажатия кнопки
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadScene);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName); // Загрузка указанной сцены
    }
}
