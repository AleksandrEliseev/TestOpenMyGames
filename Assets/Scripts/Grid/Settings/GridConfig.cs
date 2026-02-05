using UnityEngine;



namespace Grid.Settings
{
    [CreateAssetMenu(fileName = "GridPaddingConfig", menuName = "Configs/Grid Config", order = 0)]
    public class GridConfig : ScriptableObject
    {
        [SerializeField] private float _topPadding = 1.0f;
        [SerializeField] private float _bottomPadding = 1.0f;
        [SerializeField] private float _sidePadding = 0.5f;
        
        public float TopPadding => _topPadding;
        public float BottomPadding => _bottomPadding;
        public float SidePadding => _sidePadding;
    }
}