using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnyCardGame.Entity.UI
{
    public class CustomLayout : MonoBehaviour
    {
        [SerializeField] private float spacing = 5F;

        private List<CustomLayoutElement> _layoutElements = new List<CustomLayoutElement>();

        public float Spacing
        {
            get => spacing;

            set => spacing = value;
        }

        private float _spaceHolder = 0F;

        public void InitializeLayout()
        {
            _layoutElements.Clear();

            _layoutElements = GetComponentsInChildren<CustomLayoutElement>().ToList();

            UpdateLayout();
        }

        private void Update()
        {
            if (Math.Abs(spacing - _spaceHolder) > .01F)
            {
                UpdateLayout();

                _spaceHolder = spacing;
            }
        }

        private void UpdateLayout(bool shouldSimulateSlower = false)
        {
            var midIndex = _layoutElements.Count / 2;

            for (var i = 0; i < _layoutElements.Count; i++)
            {
                var offset = 0F;

                if (i < midIndex)
                {
                    offset = spacing * (midIndex - i);

                    var elem = _layoutElements[i];

                    elem.PositioningTo(Vector2.left * offset, shouldSimulateSlower);
                }

                else if (i > midIndex)
                {
                    offset = spacing * (i - midIndex);

                    var elem = _layoutElements[i];

                    elem.PositioningTo(Vector2.right * offset, shouldSimulateSlower);
                }

                else
                {
                    var elem = _layoutElements[i];

                    elem.PositioningTo(Vector2.zero, shouldSimulateSlower);
                }
            }
        }

        private void RefreshLayoutElements()
        {
            for (var i = 0; i < _layoutElements.Count; i++)
            {
                _layoutElements[i].transform.SetSiblingIndex(i);
            }
        }

        public int FindClosestElement(Vector2 pos, bool excludeLast = false)
        {
            var midIndex = _layoutElements.Count / 2;

            var currentOffset = pos.x;

            var maxIndex = excludeLast ? _layoutElements.Count - 1 : _layoutElements.Count;

            if (Mathf.Abs(currentOffset) > spacing * midIndex)
            {
                if (currentOffset < 0)
                    return 0;

                return maxIndex;
            }

            var targetIndex = Mathf.FloorToInt(currentOffset / spacing) + midIndex + 2;

            return Mathf.Clamp(targetIndex, 0, maxIndex);
        }

        public void InitializeLayout(List<Transform> elements)
        {
            _layoutElements.Clear();

            foreach (var elem in elements)
            {
                if (elem.TryGetComponent(out CustomLayoutElement layoutElement))
                    _layoutElements.Add(layoutElement);
            }

            RefreshLayoutElements();

            UpdateLayout(true);
        }

        public void AddLayoutElement(Transform elem, int place = -1)
        {
            if (elem.TryGetComponent(out CustomLayoutElement element))
            {
                // Place new card to bottom
                if (place == -1)
                {
                    _layoutElements.Add(element);
                }

                // Place new card to index
                else
                {
                    _layoutElements.Insert(place, element);
                }

                RefreshLayoutElements();

                UpdateLayout();
            }
        }

        public void RemoveLayoutElement(CustomLayoutElement elem)
        {
            if (_layoutElements.Contains(elem))
            {
                var index = _layoutElements.IndexOf(elem);

                RemoveLayoutElement(index);
            }
        }

        public void RemoveLayoutElement(int index)
        {
            _layoutElements.RemoveAt(index);

            RefreshLayoutElements();

            UpdateLayout();
        }
    }
}