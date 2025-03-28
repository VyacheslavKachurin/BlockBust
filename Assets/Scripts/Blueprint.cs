using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Blueprint : MonoBehaviour
    {
        public event Action<Blueprint> OnClicked; 

        private void OnMouseDown()
        {
            OnClicked?.Invoke(this);
            Debug.Log($"Blueprint clicked");
        }

        public void Show(bool show = true)
        {
            gameObject.SetActive(show);
        }




    }
}