using System;
using System.Collections.Generic;
using System.Linq;
using AnyCardGame.Enums;
using DG.Tweening;
using UnityEngine;

namespace AnyCardGame.Entity.UI
{
    public class DeckController : MonoBehaviour
    {
        [Header("@UI References")]
        [SerializeField] private CustomLayout layout = null;
        [SerializeField] private CardSlot cardSlotPrefab = null;

        private List<CardSlot> _cards = new List<CardSlot>();

        [SerializeField]
        [Range(0F, 1F)] private float _arcStep = 0F;

        private float _arcHeightRate = 2.25F;

        private float _arcHeightStep = .725F;

        public float ArcHeightStep
        {
            get => _arcHeightStep;

            set => _arcHeightStep = value;
        }

        public float ArcStep
        {
            get => _arcStep;

            set => _arcStep = value;
        }

        private const int MIN_SPACE_LIMIT = 5;
        private const int MAX_SPACE_LIMIT = 150;

        private Sprite[] _currentTheme;

        public void Initialize(int[] ids, Sprite[] contentSprites)
        {
            _currentTheme = contentSprites;

            _cards = new List<CardSlot>();

            for (int ii = 0; ii < ids.Length; ii++)
            {
                var id = ids[ii];
                var newCardSlot = CreateCardSlotInstance(id, GetThemeById(id));
                _cards.Add(newCardSlot);

                // Subscribe Card's Actions
                newCardSlot.OnCardDrawn += OnAnyCardDrawn;
                newCardSlot.OnCardReadyForPlace += OnAnyCardReadyForPlace;
                newCardSlot.OnCardNonPlaced += OnAnyCardNonPlaced;
            }

            layout.InitializeLayout();
        }

        private CardSlot CreateCardSlotInstance(int id, Sprite sprite)
        {
            var cardSlot = Instantiate(cardSlotPrefab, layout.transform);
            cardSlot.Initialize(id, sprite);

            return cardSlot;
        }

        private Sprite GetThemeById(int id)
        {
            return _currentTheme[id - 1];
        }

        public void SetTheme(Sprite[] theme)
        {
            _currentTheme = theme;

            for (int ii = 0; ii < _cards.Count; ii++)
            {
                var card = _cards[ii];
                var sprite = GetThemeById(card.Id);
                card.SetContentImage(sprite);
            }
        }

        private void OnAnyCardDrawn(CardSlot card)
        {
            layout.RemoveLayoutElement(_cards.IndexOf(card));

            _cards.Remove(card);
        }

        private void OnAnyCardReadyForPlace(CardSlot card, Vector2 pos)
        {
            var slotIndex = layout.FindClosestElement(pos);

            _cards.Insert(slotIndex, card);

            layout.AddLayoutElement(card.transform, slotIndex);

            card.HasDrawn = false;

            RefreshCards();
        }

        private void OnAnyCardNonPlaced(CardSlot card, Vector2 pos)
        {
            RefreshCards();

            var slotIndex = layout.FindClosestElement(pos, true);

            // First Card Slot
            if (slotIndex == 0)
            {
                _cards[slotIndex].OnInteraction(Vector3.right);
            }

            // Last Card Slot
            else if (slotIndex == _cards.Count - 1)
            {
                _cards[slotIndex].OnInteraction(Vector3.left);
            }

            // Others
            else
            {
                _cards[slotIndex - 1].OnInteraction(Vector3.left);

                _cards[slotIndex].OnInteraction(Vector3.left, true);
            }
        }

        private void Update()
        {
            if (CanAnimate())
            {
                layout.Spacing = Mathf.Lerp(MIN_SPACE_LIMIT, MAX_SPACE_LIMIT, _arcStep);
                _arcHeightRate = Mathf.Lerp(-5F, 5F, _arcHeightStep);

                AnimateCards();
            }
        }

        #region Animations

        private const float ANIM_DURATION = 1F;

        private bool _shouldAnimate = true;
        private Tween _deckAnimation = null;

        private void OnDestroy()
        {
            foreach (var card in _cards)
            {
                // UnSubscribe Card's Actions
                card.OnCardDrawn -= OnAnyCardDrawn;
                card.OnCardReadyForPlace -= OnAnyCardReadyForPlace;
                card.OnCardNonPlaced -= OnAnyCardNonPlaced;
            }
        }

        private bool CanAnimate()
        {
            return _shouldAnimate;
        }

        private void ShouldAnimate(bool state)
        {
            _shouldAnimate = state;
        }

        private void StopAnimation()
        {
            _deckAnimation?.Kill();
        }

        private void AnimateDeck(bool shouldOpen = true)
        {
            ShouldAnimate(false);

            StopAnimation();

            var current = _arcStep;

            _deckAnimation = DOTween.To(() => current, x => current = x, shouldOpen ? 1F : 0F, ANIM_DURATION).OnUpdate(() =>
            {
                _arcStep = current;

                layout.Spacing = Mathf.Lerp(MIN_SPACE_LIMIT, MAX_SPACE_LIMIT, _arcStep);

                AnimateCards();

            }).SetEase(Ease.InSine)
                .OnComplete(() => ShouldAnimate(true));
        }

        private void AnimateCards()
        {
            var rotationLimit = (_cards.Count - 1) * 2;

            var totalRotation = 0F;

            var transformDataset = new List<Vector2>();

            var isEven = _cards.Count % 2 == 0;

            for (var i = 0; i < _cards.Count; i++)
            {
                var targetRotationZ = Mathf.Lerp(0F, (rotationLimit - (4 * i)), _arcStep);

                var targetPositionY = Mathf.Lerp(0F, (totalRotation * _arcHeightRate), _arcStep);

                var calculatedRotation = new Vector3(0, 0, targetRotationZ);

                var calculatedPosition = new Vector3(0, targetPositionY, 0);

                _cards[i].PositioningTo(calculatedRotation, calculatedPosition);

                totalRotation += targetRotationZ;

                if (i == _cards.Count / 2)
                    break;

                if (isEven && i == _cards.Count / 2 - 1)
                    continue;

                transformDataset.Add(new Vector2(-targetRotationZ, targetPositionY));
            }

            if (transformDataset.Count > 0)
            {
                var index = transformDataset.Count - 1;

                for (var i = _cards.Count / 2 + 1; i < _cards.Count; i++)
                {
                    _cards[i].PositioningTo(new Vector3(0, 0, transformDataset[index].x),
                        new Vector3(0, transformDataset[index].y, 0));

                    index--;
                }
            }
        }

        private void RefreshCards()
        {
            foreach (var elem in _cards)
                elem.OnCompleteInteraction();
        }

        #endregion

        public void SortByResponse(int[] newIds)
        {
            var sortedCardSlots = new List<CardSlot>();

            for (int ii = 0; ii < newIds.Length; ii++)
            {
                var targetId = newIds[ii];
                var targetCardSlot = _cards.FirstOrDefault(c => c.Id == targetId);

                sortedCardSlots.Add(targetCardSlot);
            }

            _cards = sortedCardSlots;

            layout.InitializeLayout(_cards.ConvertAll((x) => x.transform));
        }
    }
}