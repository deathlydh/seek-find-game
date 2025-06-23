    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EndScreenController : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_Text _Player;
        [SerializeField]
        TMPro.TMP_Text _AI;
        [SerializeField]
        GameObject obj;

    public static int LastSecondRoundScore = 0;

    void SetText()
    {
        obj?.SetActive(true);

        if (SaveSystem.GetCount() > 0)
        {
            int currentSecondRoundScore = SaveSystem.GetSave(SaveSystem.GetCount() - 1);
            _AI.SetText(currentSecondRoundScore.ToString());
            LastSecondRoundScore = currentSecondRoundScore; // Сохраняем значение
        }
        else
        {
            _AI.SetText("0");
            LastSecondRoundScore = 0;
        }
    }

    void Awake()
    {
        Round2StateMahine.EndGame += SetText;
    }

    void OnDestroy()
    {
        Round2StateMahine.EndGame -= SetText;
    }
}
