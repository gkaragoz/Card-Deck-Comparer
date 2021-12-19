using DG.Tweening;
using UnityEngine;

namespace AnyCardGame.Entity.UI
{
    public class CustomLayoutElement : MonoBehaviour
    {
        private RectTransform _rect = null;

        private Tween _positionAnimation = null;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void PositioningTo(Vector2 targetPosition, bool shouldSimulateSlower = false)
        {
            _positionAnimation?.Kill();

            var currentPos = _rect.anchoredPosition;

            _positionAnimation = DOTween.To(() => currentPos, x => currentPos = x, targetPosition,
                shouldSimulateSlower ? 1.25F : .5F).OnUpdate(() =>
            {
                _rect.anchoredPosition = currentPos;
            });
        }

        public Vector2 AnchoredPosition
        {
            get => _rect.anchoredPosition;

            set => _rect.anchoredPosition = value;
        }
    }
}