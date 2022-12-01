using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreManager 
{
    void addPoints(float f);
    float getPoints();
    event ScoreChanged scoreChangedDelegate;
}
public delegate void ScoreChanged(IScoreManager scoreManager);
