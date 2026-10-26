using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy SO", menuName = "ScriptableData/Enemy")]
public class Enemy_SO : ScriptableObject
{
    public Color enemyColor = new Color(0,0,0);
    public int maxHealth = 100;
}
