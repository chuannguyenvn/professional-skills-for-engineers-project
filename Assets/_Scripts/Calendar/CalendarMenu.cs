using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Calendar
{
    public class CalendarMenu : SwipeVerticalHorizontalMenu
    {
        [Header("Visualize UI")]
        [SerializeField] private CalendarSwipeButton [] _swipeButtons;
        private CalendarSwipeButton _currentSelectButton;
        private void Start()
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
        }
    }
}
