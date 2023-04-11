using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Calendar
{
    public class CalendarMenu : SwipeVerticalHorizontalMenu
    {
        [Header("Visualize UI")]
        [SerializeField] private CalendarSwipeButton [] _swipeButtons;
        
        private void Start()
        {
            for (var i = 0 ; i < _swipeButtons.Length; i++)
            {
                var swipeButton = _swipeButtons[i];
                var i1 = i;
                swipeButton.button.onClick.AddListener(() =>
                {
                    SelectSwipeButton(i1);
                    SetToHorizontalPage(i1);
                });
                swipeButton.Deselect();
            }
            
            _swipeButtons[initHorizontalIndex].Select();
        
        }

        private void SelectSwipeButton(int index)
        {
            foreach (var swipeButton in _swipeButtons)
            {
                swipeButton.Deselect();
            }
            _swipeButtons[index].Select();
        }
    }
}
