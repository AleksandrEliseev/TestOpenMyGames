using UnityEngine;

namespace GameCamera
{
    public class CameraContainer : MonoBehaviour
    {
        [SerializeField] private Camera _gameCamera;

        public Camera GameCamera => _gameCamera;
    }
}