using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text score;
    private void Start()
    {
        DependencyInjector.GetDependency<IScoreManager>().scoreChangedDelegate += updateScore;
    }
    private void OnDestroy()
    {
        DependencyInjector.GetDependency<IScoreManager>().scoreChangedDelegate -= updateScore;
    }
    public void updateScore(IScoreManager scoreManager)
    {
        score.text = "Score: " + scoreManager.getPoints().ToString("0");
    }
}
