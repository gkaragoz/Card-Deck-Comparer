using AnyCardGame.Entity.Cards;
using AnyCardGame.Entity.UI;
using AnyCardGame.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnyCardGame.Entity.Players
{
    public class PlayerController : MonoBehaviour
    {
        [Header("@References")]
        [SerializeField]
        private UIManager _uiManager = null;

        private Player _player;

        private void Start()
        {
            _uiManager.OnSortByClicked += OnSortByClickedListener;

            _player = new Player(new List<Card>
            {
                new Card("H01"),
                new Card("S02"),
                new Card("D05"),
                new Card("H04"),
                new Card("S01"),
                new Card("D03"),
                new Card("C04"),
                new Card("S04"),
                new Card("D01"),
                new Card("S03"),
                new Card("D04"),
            });

            _uiManager.Initialize(_player.GetGrouppedDeckCardIds());
        }

        private void OnDestroy()
        {
            _uiManager.OnSortByClicked -= OnSortByClickedListener;
        }

        private void OnSortByClickedListener(GroupType groupType, Action<int[]> responseCallback)
        {
            _player.SortDeck(groupType);

            responseCallback?.Invoke(_player.GetGrouppedDeckCardIds());
        }
    }
}