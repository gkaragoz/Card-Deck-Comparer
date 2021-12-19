using AnyCardGame.Enums;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AnyCardGame.Entity.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("@UI References")]
        [SerializeField] private Slider arcStepSlider = null;
        [SerializeField] private Slider arcHeightStepSlider = null;
        [SerializeField] private Button sortSameKindButton = null;
        [SerializeField] private Button sortStraightButton = null;
        [SerializeField] private Button sortSmartButton = null;
        [SerializeField] private Button themeButtonT1 = null;
        [SerializeField] private Button themeButtonT2 = null;

        [Header("@References")]
        [SerializeField] private DeckController deckController = null;

        public Action<GroupType, Action<int[]>> OnSortByClicked { get; set; }

        private bool _shouldAnimate = false;
        private Sprite[] _themeT1 = null;
        private Sprite[] _themeT2 = null;

        private const string THEME_PREFIX = "card_list_t";

        public void Initialize(int[] cardIds)
        {
            LoadThemes();

            deckController.Initialize(cardIds, _themeT1);

            SetInteractability(false);

            DOVirtual.DelayedCall(1F, () =>
            {
                var current = 0F;

                DOTween.To(() => current, x => current = x, 1F, .5F).OnUpdate(() =>
                {
                    arcStepSlider.value = current;

                }).OnComplete(() =>
                {
                    _shouldAnimate = true;

                    SetInteractability(true);
                });
            });
        }

        private void Update()
        {
            if (_shouldAnimate)
            {
                if (arcStepSlider == null ||
                   arcHeightStepSlider == null ||
                   deckController == null)
                    return;

                deckController.ArcStep = arcStepSlider.value;
                deckController.ArcHeightStep = arcHeightStepSlider.value;
            }
        }

        private void SetInteractability(bool state)
        {
            arcStepSlider.interactable = state;
            arcHeightStepSlider.interactable = state;
            sortSameKindButton.interactable = state;
            sortStraightButton.interactable = state;
            sortSmartButton.interactable = state;
            themeButtonT1.interactable = state;
            themeButtonT2.interactable = state;
        }

        private void LoadThemes()
        {
            string theme01ImageName = THEME_PREFIX + 1;
            _themeT1 = Resources.LoadAll<Sprite>("Textures/" + theme01ImageName);
            string theme02ImageName = THEME_PREFIX + 2;
            _themeT2 = Resources.LoadAll<Sprite>("Textures/" + theme02ImageName);
        }

        private void OnSortByClickedResponseListener(int[] ids)
        {
            deckController.SortByResponse(ids);
        }

        public void OnClick_SetTheme(int themeId)
        {
            deckController.SetTheme(themeId == 1 ? _themeT1 : _themeT2);
        }

        public void SortByStraight()
        {
            OnSortByClicked?.Invoke(GroupType.Straight, OnSortByClickedResponseListener);
        }

        public void SortBySameKind()
        {
            OnSortByClicked?.Invoke(GroupType.SameKind, OnSortByClickedResponseListener);
        }

        public void SortBySmart()
        {
            OnSortByClicked?.Invoke(GroupType.Smart, OnSortByClickedResponseListener);
        }
    }
}