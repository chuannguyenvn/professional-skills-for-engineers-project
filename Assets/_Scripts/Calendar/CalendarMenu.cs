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
                    Debug.Log("SET SWIPE TO "+ i1 + " "+ swipeButton.gameObject.name);
                    SetToHorizontalPage(i1);
                });
                swipeButton.Deselect();
            }
            
            _swipeButtons[initHorizontalIndex].Select();
        
        }
    }
}
