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
    
        void Start()
        {
        }

        public void Select()
        {
            _icon.sprite = _selectSprite;
            _icon.color = new Color(27, 124, 210);
        }

        public void Deselect()
        {
            _icon.sprite = _idleSprite;
            _icon.color = Color.black;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _calendarMenu.OnPointerDown(eventData);
        }
    }
}
