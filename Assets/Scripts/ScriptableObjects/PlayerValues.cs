using UnityEngine;

//Written by Mandy

[CreateAssetMenu(fileName = "PlayerValues", menuName = "ScriptableObjects/PlayerValuesComponent", order = 2)]
public class PlayerValues : ScriptableObject
{
    public int Lives;
    public int Scores;
}
