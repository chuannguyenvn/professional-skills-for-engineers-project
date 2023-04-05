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
        [SerializeField] private CanvasGroup homeMenuCanvas;
        [SerializeField] private CanvasGroup searchMenuCanvas;
        [SerializeField] private TMP_InputField trueSearchBar;
        [SerializeField] private TMP_InputField homeSearchBar;
        [SerializeField] private float transitionDuration;

        [SerializeField] private VerticalLayoutGroup foundContents;
        [SerializeField, Range(0, 10)] private int maxFoundElement;
        private List<FoundItem> _foundSearchItems = new();

        private void Awake()
        {
            AddToFunctionQueue(OnSelect, StateEvent.OnEnter);
            AddToFunctionQueue(OnDeselect, StateEvent.OnExit);
        }

        public void OnChangeValue()
        {
            homeSearchBar.text = trueSearchBar.text;
            if (trueSearchBar.text == "")
            {
                foreach (var foundSearchItem in _foundSearchItems)
                {
                    Destroy(foundSearchItem.gameObject);
                
                }
                _foundSearchItems.Clear();
            
                return;
            
            }
            List<Building> freshFoundBuildings = MapManager.Instance.FindBuildings(trueSearchBar.text);

            // Get the list of objects to destroy
            var toDestroy = _foundSearchItems.Where(item => !freshFoundBuildings.Contains(item.GetObjectVariable())).ToList();

            // Destroy the objects to destroy
            foreach (var item in toDestroy)
            {
                Destroy(item.gameObject);
                _foundSearchItems.Remove(item);
            }

            for (int i = _foundSearchItems.Count; i < Mathf.Min( maxFoundElement, freshFoundBuildings.Count); i++)
            {
                if (_foundSearchItems.Any(item => item.GetObjectVariable() == freshFoundBuildings[i]))
                {
                    // Object already exists, skip
                    continue;
                }

                var newItem = Instantiate(ResourceManager.Instance.foundItem, foundContents.transform);// Create a new FoundItem object using Instantiate
                newItem.Init(freshFoundBuildings[i], "Building",freshFoundBuildings[i].gameObject.name);
                _foundSearchItems.Add(newItem); // Add the new object to the list
            }
        
        }

        
    
        public void OnSelect()
        { 
            searchMenuCanvas.gameObject.SetActive(true);
        
            homeMenuCanvas.interactable = false;

            trueSearchBar.ActivateInputField();
            searchMenuCanvas.interactable = true;

            searchMenuCanvas.alpha = 0;
            DOTween.To(() => searchMenuCanvas.alpha, x => searchMenuCanvas.alpha = x, 1, transitionDuration).OnComplete(() => homeMenuCanvas.gameObject.SetActive(false));

            //homeMenuCanvas.alpha = 1;
            //DOTween.To(() => homeMenuCanvas.alpha, x => homeMenuCanvas.alpha = x, 0, transitionDuration).OnComplete(() => homeMenuCanvas.gameObject.SetActive(false));
        
        }
    
        public void OnDeselect()
        { 
            homeMenuCanvas.gameObject.SetActive(true);
        
            searchMenuCanvas.interactable = false;

            trueSearchBar.DeactivateInputField();
            homeMenuCanvas.interactable = true;

            //homeMenuCanvas.alpha = 0;
            //DOTween.To(() => homeMenuCanvas.alpha, x => homeMenuCanvas.alpha = x, 1, transitionDuration).OnComplete(() => searchMenuCanvas.gameObject.SetActive(false));

            searchMenuCanvas.alpha = 1;
            DOTween.To(() => searchMenuCanvas.alpha, x => searchMenuCanvas.alpha = x, 0, transitionDuration).OnComplete(() => searchMenuCanvas.gameObject.SetActive(false));
        
        }
    
    
    }
}