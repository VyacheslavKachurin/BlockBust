using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Shape : MonoBehaviour
{
    [SerializeField] private List<Transform> _tiles;
    public List<Transform> Tiles => _tiles;


}
