using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Calendar
{
    [RequireComponent(typeof(Button))]
    public class CalendarSwipeButton : MonoBehaviour, IPointerDownHandler
    {
        public Button button;
        [SerializeField] private CalendarMenu _calendarMenu;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _selectSprite, _idleSprite;
        [SerializeField] private Color _selectColor, _idleColor;


        public void Select()
        {
            _icon.sprite = _selectSprite;
            _icon.color = _selectColor;
        }

        public void Deselect()
        {
            _icon.sprite = _idleSprite;
            _icon.color = _idleColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _calendarMenu.OnPointerDown(eventData);
        }
    }
}
