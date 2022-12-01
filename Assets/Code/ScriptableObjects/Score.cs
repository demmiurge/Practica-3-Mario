using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Score")]
public class Score : ScriptableObject
{
    public float points;
    public void score()
    {
        IScoreManager score = DependencyInjector.GetDependency<IScoreManager>();
        score.addPoints(points);
    }
}
public class Coin : MonoBehaviour
{
    [SerializeField] Score score;
    private void OnTriggerEnter(Collider other)
    {
        if (score != null)
        {
            score.score();
        }
        Destroy(gameObject);
    }
}
