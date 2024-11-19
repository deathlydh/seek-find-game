using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    [SerializeField]
    public TMP_Text txt;
    int count;

    void Start()
    {
        SaveSystem.init();
        count = SaveSystem.GetCount();
        Debug.Log(SaveSystem.GetCount());

        if (count != 0)
        {
            Load();
        }
    }

    public void Save(int score)
    {
        SaveSystem.Save(score);
    }

    public void SaveToBuffer(int score){
        SaveSystem.SaveFirstStage(score);
    }

    public int GetFromBuffer(int score){
        return SaveSystem.GetFirstStage();
    }

    public int[] Load() {
        count = SaveSystem.count;
        return SaveSystem.GetAllScore();
    }

    public int AnaliticSoreBatterThen(){
        return SaveSystem.AnaliticSoreBatterThen();
    }
}
