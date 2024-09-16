using UnityEngine;

public class CardValue : MonoBehaviour
{
    [SerializeField] int points;
    public int Points { get { return points; } set { points = value; } }
}
