using UnityEngine;

public class UiLevelSelector : MonoBehaviour
{
    [SerializeField] private Levels levels;
    [SerializeField] private Transform grid;
    [SerializeField] private GameObject buttonPrefab;


    void Start()
    {
        for(int i = 0; i < levels.Data.Length; i++)
        {
            var button = Instantiate(buttonPrefab);
            var uiLevelButton = button.GetComponent<UiLevelButton>();
            uiLevelButton.Index = i;
            uiLevelButton.Tutorial = levels.Data[i].tutorial;
            uiLevelButton.Content = levels.Data[i].levelData;
            button.transform.SetParent(grid);
            button.transform.localScale = Vector3.one;
        }
    }
}
