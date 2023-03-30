using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class SearchBarManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CanvasGroup homeMenuCanvas;
    [SerializeField] private CanvasGroup searchMenuCanvas;
    [SerializeField] private TMP_InputField trueSearchBar;
    [SerializeField] private TMP_InputField homeSearchBar;
    [SerializeField] private float transitionDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect()
    { 
        searchMenuCanvas.gameObject.SetActive(true);
        
        homeMenuCanvas.interactable = false;

        trueSearchBar.ActivateInputField();
        searchMenuCanvas.interactable = true;

        searchMenuCanvas.alpha = 0;
        DOTween.To(() => searchMenuCanvas.alpha, x => searchMenuCanvas.alpha = x, 1, transitionDuration);

        homeMenuCanvas.alpha = 1;
        DOTween.To(() => homeMenuCanvas.alpha, x => homeMenuCanvas.alpha = x, 0, transitionDuration).OnComplete(() => homeMenuCanvas.gameObject.SetActive(false));
        
    }
    
    public void OnDeselect()
    { 
        homeMenuCanvas.gameObject.SetActive(true);
        
        searchMenuCanvas.interactable = false;

        trueSearchBar.DeactivateInputField();
        homeMenuCanvas.interactable = true;

        homeMenuCanvas.alpha = 0;
        DOTween.To(() => homeMenuCanvas.alpha, x => homeMenuCanvas.alpha = x, 1, transitionDuration);

        searchMenuCanvas.alpha = 1;
        DOTween.To(() => searchMenuCanvas.alpha, x => searchMenuCanvas.alpha = x, 0, transitionDuration).OnComplete(() => searchMenuCanvas.gameObject.SetActive(false));
        
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
