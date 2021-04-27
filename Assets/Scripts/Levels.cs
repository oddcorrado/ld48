using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Levels", order = 1)]
public class Levels : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        public string name;
        [TextArea(20, 40)]
        public string levelData;
        [TextArea(2, 4)]
        public string tutorial;
    }
    [SerializeField] private Level[] levels;
    public Level[] Data { get { return levels; } }
}