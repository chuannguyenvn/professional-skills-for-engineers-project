using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace _Scripts.Calendar
{
    public class CalendarMenu : SwipeVerticalHorizontalMenu
    {
        [Header("Visualize Swipe Button UI")]
        [SerializeField] private CalendarSwipeButton [] _swipeButtons;
        private CalendarSwipeButton _currentSelectButton;
        
        [Header("Visualize Swipe Bar UI")]
        [SerializeField] private RectTransform _miniBarBelowButton;
        [SerializeField, Range(0,1)] private float _minBarMovingDuration = 0.25f;
        [SerializeField] private Ease _miniBarEase;
        private void Start()
        {
            InitSwipeButton();
        }
        

        private void InitSwipeButton()
        {
            for (var i = 0 ; i < _swipeButtons.Length; i++)
            {
                var swipeButton = _swipeButtons[i];
                var i1 = i;
                swipeButton.button.onClick.AddListener(() =>
                {
                    SetToHorizontalPage(i1);
                });
                swipeButton.VisualizeDeselect();
            }
            
            _currentSelectButton = _swipeButtons[initHorizontalIndex];
            _currentSelectButton.VisualizeSelect();
        }

        public void SelectSwipeButton(int index)
        {
            _currentSelectButton?.VisualizeDeselect();
            _currentSelectButton = _swipeButtons[index];
            _currentSelectButton.VisualizeSelect();
            _miniBarBelowButton.DOAnchorPosX(_currentSelectButton.rectTransform.anchoredPosition.x, _minBarMovingDuration).SetEase(_miniBarEase);
        }
    }
}
