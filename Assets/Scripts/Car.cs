using UnityEngine;

namespace Assets.Scripts
{
    public class Car : MonoBehaviour
    {
        public string Company = "Moskvich";
        public int Year = 2000;
        public Color Color = Color.yellow;
        public int Coins;
        [SerializeField] private int _inputCoins;

        public void Start()
        {
            Coins = 100;
        }

        [ContextMenu("Add Coins")] //контекстное меню
        public void AddCoins()
        {
            Coins += _inputCoins;
        }

    }
}