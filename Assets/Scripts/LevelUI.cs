using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTextMesh;

    private void Update()
    {
        UpdateLevelTextMesh();
        
    }

    private void UpdateLevelTextMesh()
    {
        levelTextMesh.text = GameManager.instance.GetLevelNumber().ToString();
    }
}
