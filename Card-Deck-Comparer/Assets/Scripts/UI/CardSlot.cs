using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AnyCardGame.Entity.UI
{
    public class CardSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform socket = null;
        [SerializeField] private Image contentImage = null;

        private Tween _positionAnimation, _rotationAnimation = null;
        private RectTransform _rect = null;
        private Animator _animator = null;

        public Action<CardSlot> OnCardDrawn;
        public Action<CardSlot, Vector2> OnCardReadyForPlace;
        public Action<CardSlot, Vector2> OnCardNonPlaced;

        private int _id;
        public int Id { get => _id; }

        private bool _hasInteracted = false;

        public bool HasDrawn { get; set; } = false;

        public void Initialize(int id, Sprite sprite)
        {
            _rect = GetComponent<RectTransform>();
            _animator = GetComponentInChildren<Animator>();

            SetCardId(id);
            SetContentImage(sprite);
        }

        private void Update()
        {
            if (HasDrawn)
                OnCardNonPlaced?.Invoke(this, _rect.anchoredPosition);
        }

        public void SetCardId(int id)
        {
            _id = id;
        }

        public void SetContentImage(Sprite sprite)
        {
            contentImage.sprite = sprite;
        }

        public void PositioningTo(Vector3 rotation, Vector3 location, bool animateRotationOnly = false)
        {
            _rotationAnimation?.Kill();

            _rotationAnimation = transform.DOLocalRotate(rotation, .25F);

            if (!animateRotationOnly)
            {
                _positionAnimation?.Kill();

                _positionAnimation = socket.DOLocalMove(location, .25F);
            }
        }

        public void OnInteraction(Vector3 direction, bool scaleOnly = false)
        {
            if (!_hasInteracted)
            {
                _hasInteracted = true;

                if (!scaleOnly)
                    _animator.SetBool(direction.x > 0 ? "shouldMoveRight" : "shouldMoveLeft", true);

                _animator.SetBool("shouldScaleUp", true);
            }
        }

        public void OnCompleteInteraction()
        {
            if (_hasInteracted)
            {
                _hasInteracted = false;

                _animator.SetBool("shouldMoveLeft", false);
                _animator.SetBool("shouldMoveRight", false);
                _animator.SetBool("shouldScaleUp", false);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var rect = GetComponent<RectTransform>();
            var currentTransform = transform;
            var position = currentTransform.position;
            currentTransform.position = eventData.position;

            if (!IsRectTransformInsideScreen(rect))
                currentTransform.position = position;

            else
            {
                if (!HasDrawn)
                {
                    OnCardDrawn?.Invoke(this);

                    HasDrawn = true;

                    PositioningTo(Vector3.zero,
                        transform.position, true);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (HasDrawn)
                OnCardReadyForPlace?.Invoke(this, _rect.anchoredPosition);
        }

        /// <summary>
        /// This methods will check is the rect transform is inside the screen or not
        /// </summary>
        /// <param name="rectTransform">Rect Trasform</param>
        /// <returns></returns>
        private bool IsRectTransformInsideScreen(RectTransform rectTransform)
        {
            var isInside = false;
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var rect = new Rect(0, 0, Screen.width, Screen.height);
            var visibleCorners = corners.Count(corner => rect.Contains(corner));
            if (visibleCorners == 4)
                isInside = true;

            return isInside;
        }
    }
}