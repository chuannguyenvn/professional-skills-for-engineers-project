using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Manager;
using _Scripts.Map;
using _Scripts.StateMachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Search_Bar
{
    public class SearchStateMachine : StateMachine<SearchStateMachine,AppState>
    {
        [Header("Search UI")]
        [SerializeField] private CanvasGroup _homeMenuCanvas;
        [SerializeField] private CanvasGroup _searchMenuCanvas;
        [SerializeField] private TMP_InputField _trueSearchBar;
        [SerializeField] private TMP_InputField _homeSearchBar;
        [SerializeField] private Button _backButton;
        [SerializeField] private float _transitionDuration;
        
        [Header("Search Contain")]
        [SerializeField] private VerticalLayoutGroup _foundContents;
        [SerializeField, Range(0, 10)] private int _maxFoundElement;
        private List<FoundItem> _foundSearchItems = new();

        private void Awake()
        {
            InitEvent();
            AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
            AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
        }

        private void InitEvent()
        {
            _trueSearchBar.onValueChanged.AddListener(OnSearchValueChanged);
            _backButton.onClick.AddListener(() => ApplicationManager.Instance.SetState(AppState.Home));
        }

        public void OnSearchValueChanged(string text)
        {
            _homeSearchBar.text = text;
            if (text == "")
            {
                foreach (var foundSearchItem in _foundSearchItems)
                {
                    Destroy(foundSearchItem.gameObject);
                
                }
                _foundSearchItems.Clear();
                return;
            }
            List<Building> freshFoundBuildings = MapManager.Instance.FindBuildings(text);

            // Get the list of objects to destroy
            var toDestroy = _foundSearchItems.Where(item => !freshFoundBuildings.Contains(item.GetObjectVariable())).ToList();

            // Destroy the objects to destroy
            foreach (var item in toDestroy)
            {
                Destroy(item.gameObject);
                _foundSearchItems.Remove(item);
            }

            for (int i = _foundSearchItems.Count; i < Mathf.Min( _maxFoundElement, freshFoundBuildings.Count); i++)
            {
                if (_foundSearchItems.Any(item => item.GetObjectVariable() == freshFoundBuildings[i]))
                {
                    // Object already exists, skip
                    continue;
                }

                var newItem = Instantiate(ResourceManager.Instance.foundItem, _foundContents.transform);// Create a new FoundItem object using Instantiate
                newItem.Init(freshFoundBuildings[i], "Building",freshFoundBuildings[i].gameObject.name);
                _foundSearchItems.Add(newItem); // Add the new object to the list
            }
        
        }

        
    
        public void OnSelect(AppState exitState)
        { 
            Debug.Log("Active state search");
            _searchMenuCanvas.gameObject.SetActive(true);
            
            _trueSearchBar.ActivateInputField();
            _searchMenuCanvas.interactable = true;

            _searchMenuCanvas.alpha = 0;
            DOTween.To(() => _searchMenuCanvas.alpha, x => _searchMenuCanvas.alpha = x, 1, _transitionDuration);
            
        }
    
        public void OnDeselect(AppState enterState)
        {
            _searchMenuCanvas.interactable = false;
            _trueSearchBar.DeactivateInputField();
            
            _searchMenuCanvas.alpha = 1;
            DOTween.To(() => _searchMenuCanvas.alpha, x => _searchMenuCanvas.alpha = x, 0, _transitionDuration).OnComplete(() => _searchMenuCanvas.gameObject.SetActive(false));
        
        }
    
    
    }
}
