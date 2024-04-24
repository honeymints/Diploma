using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class CardGameController : MonoBehaviour
{
    [SerializeField] private CardGameData _cardGameData;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _panel;
    
    [SerializeField] private int rotationSpeed;
    [SerializeField] private Sprite initialSprite;

    public Sprite[] cardSprites;
    public List<Sprite> usedCards = new List<Sprite>();
    public List<Button> cardPrefabs = new List<Button>();

    private bool _firstCardClick, _secondCardClick;
    private int _firstIndex, _secondIndex;
    private int _correctGuesses, _totalGuesses;
    
    [SerializeField] private PanelView _panelView;
    
    void Awake()
    {
        cardSprites = Resources.LoadAll<Sprite>("Sprites/CardGameSprites");
    }

    void Start()
    {
        CreateCards();
        AddListener();
        AddSprites();
        Randomize(usedCards);
        initialSprite = cardPrefabs[0].image.sprite;
    }
    
    private void CreateCards()
    {
        for (int i = 0; i < _cardGameData.sizeOfColumns * _cardGameData.sizeOfRows; i++)
        {
            var card = Instantiate(_cardPrefab, _cardPrefab.transform.position, Quaternion.identity);
            card.transform.SetParent(_panel.transform);
            card.name = $"{_cardPrefab.name} #{i + 1}";
            card.transform.localScale = Vector3.one;

            cardPrefabs.Add(card.GetComponent<Button>());
        }

        _panel.GetComponent<GridLayoutGroup>().constraintCount = _cardGameData.sizeOfColumns;
    }

    public void AddSprites()
    {
        int index = 0;
        for (int i = 0; i < cardPrefabs.Count; i++)
        {
            if (index == cardPrefabs.Count / 2)
            {
                index = 0;
            }
            usedCards.Add(cardSprites[index]);
            index++;
        }
    }
    
    public void AddListener()
    {
        for (int i=0;i< cardPrefabs.Count;i++)
        {
            cardPrefabs[i].onClick.AddListener(()=>Click());
        }
    }

    public void Click()
    {
        Button card;
        if (!_firstCardClick)
        {
            _firstCardClick = true;
            card=EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            _firstIndex = cardPrefabs.FindIndex(x => x == card);
            StartCoroutine(TurnCardAround(cardPrefabs[_firstIndex], _firstIndex));
        }
        else if (!_secondCardClick)
        {
            _secondCardClick = true;
            card=EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            _secondIndex = cardPrefabs.FindIndex(x => x == card);
            StartCoroutine(TurnCardAround(cardPrefabs[_secondIndex], _secondIndex));
        }

        StartCoroutine(CheckIfMatched());
    }

    public IEnumerator CheckIfMatched()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (_firstCardClick && _secondCardClick)
        {
            if (usedCards[_firstIndex]== usedCards[_secondIndex] && _firstIndex!=_secondIndex)
            {
                yield return new WaitForSeconds(0.5f);
                cardPrefabs[_firstIndex].interactable = false;
                cardPrefabs[_secondIndex].interactable = false;
                _correctGuesses++;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(TurnCardAroundBack(cardPrefabs[_firstIndex]));
                StartCoroutine(TurnCardAroundBack(cardPrefabs[_secondIndex]));
            }

            _totalGuesses++;
            _secondCardClick = _firstCardClick = false;
        }
        
        CountGuesses();
    }

    private IEnumerator TurnCardAround(Button card, int index)
    {
        float angleY = 0f;
        while (Math.Abs(angleY - 90f) >0)
        {
            var eulerAngles = card.transform.eulerAngles;
            angleY = Mathf.MoveTowards(eulerAngles.y, 90f, rotationSpeed*Time.deltaTime);
                
                eulerAngles = new Vector3(eulerAngles.x, angleY, eulerAngles.z);
                card.transform.eulerAngles = eulerAngles;
                yield return null;
        }
        
        card.image.sprite =  usedCards[index];
        Debug.Log($"here is index of current card {index} and sprite itself {usedCards[index].name}");
        
        while (Math.Abs(angleY - 180f) >0)
        {
            var eulerAngles = card.transform.eulerAngles;
            angleY = Mathf.MoveTowards(eulerAngles.y, 180f, rotationSpeed*Time.deltaTime);
                
            eulerAngles = new Vector3(eulerAngles.x, angleY, eulerAngles.z);
            card.transform.eulerAngles = eulerAngles;
            yield return null;
        }
        
    }

    private IEnumerator TurnCardAroundBack(Button card1)
    {
        float angleY = 180f;
        while (Math.Abs(angleY - 270f) >0)
        {
            var eulerAngles = card1.transform.eulerAngles;
            angleY = Mathf.MoveTowards(eulerAngles.y, 270f, rotationSpeed*Time.deltaTime);
                
            eulerAngles = new Vector3(eulerAngles.x, angleY, eulerAngles.z);
            card1.transform.eulerAngles = eulerAngles;
            yield return null;
        }
        
        card1.image.sprite = initialSprite;
        
        while (Math.Abs(angleY - 360f) >0)
        {
            var eulerAngles = card1.transform.eulerAngles;
            angleY = Mathf.MoveTowards(eulerAngles.y, 360f, rotationSpeed*Time.deltaTime);
                
            eulerAngles = new Vector3(eulerAngles.x, angleY, eulerAngles.z);
            card1.transform.eulerAngles = eulerAngles;
            yield return null;
        }
    }

    private void Randomize(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CountGuesses()
    {
        if (_correctGuesses == cardPrefabs.Count/2)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log($"Total guesses {_totalGuesses}");
        Debug.Log($"Correct guesses {_correctGuesses}");
        _panelView.ShowPanel();
        GetComponent<CardGameController>().enabled = false;
    }
}
