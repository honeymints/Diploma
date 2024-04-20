using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _panel;
    [SerializeField] private AnimationCurve _curve;
    
    [SerializeField] private int sizeOfColumns;
    [SerializeField] private int sizeOfRows;
    [SerializeField] private int rotationSpeed;
    [SerializeField] private Sprite initialSprite;

    public Sprite[] cardSprites;
    public List<Sprite> usedCards = new List<Sprite>();
    public List<Button> cardPrefabs = new List<Button>();

    private bool firstCardClick, secondCardClick;
    private int firstIndex, secondIndex;
    
    void Awake()
    {
        cardSprites = Resources.LoadAll<Sprite>("Sprites/CardGameSprites");
    }

    void Start()
    {
        CreateCards();
        AddListener();
        AddSprites();
        initialSprite = cardPrefabs[0].image.sprite;
    }
    
    private void CreateCards()
    {
        for (int i = 0; i < sizeOfColumns * sizeOfRows; i++)
        {
            var card = Instantiate(_cardPrefab, _cardPrefab.transform.position, Quaternion.identity);
            card.transform.SetParent(_panel.transform);
            card.name = $"{_cardPrefab.name} #{i + 1}";
            card.transform.localScale = Vector3.one;

            cardPrefabs.Add(card.GetComponent<Button>());
        }

        _panel.GetComponent<GridLayoutGroup>().constraintCount = sizeOfColumns;
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
        if (!firstCardClick)
        {
            firstCardClick = true;
            card=EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            firstIndex = cardPrefabs.FindIndex(x => x == card);
            
            StartCoroutine(TurnCardAround(cardPrefabs[firstIndex], firstIndex));
        }
        else if (!secondCardClick)
        {
            secondCardClick = true;
            card=EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            secondIndex = cardPrefabs.FindIndex(x => x == card);

            StartCoroutine(TurnCardAround(cardPrefabs[secondIndex], secondIndex));
            
        }

        CheckIfMatched();
    }

    public void CheckIfMatched()
    {
        if (firstCardClick && secondCardClick)
        {
            if (cardPrefabs[firstIndex].image.sprite == cardPrefabs[secondIndex].image.sprite && firstIndex!=secondIndex)
            {
                Debug.Log("matched!");
            }
            else
            {
                Debug.Log("first and second card didn't match and turn around");
                StartCoroutine(TurnCardAroundBack(cardPrefabs[firstIndex]));
                StartCoroutine(TurnCardAroundBack(cardPrefabs[secondIndex]));
                secondCardClick = false;
                secondCardClick = false;
            }
        }
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
        card.image.sprite = cardSprites[index];
        
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
        float angleY = 0f;
        while (Math.Abs(angleY - 90f) >0)
        {
            var eulerAngles = card1.transform.eulerAngles;
            angleY = Mathf.MoveTowards(eulerAngles.y, 90f, rotationSpeed*Time.deltaTime);
                
            eulerAngles = new Vector3(eulerAngles.x, angleY, eulerAngles.z);
            card1.transform.eulerAngles = eulerAngles;
            yield return null;
        }
        
        card1.image.sprite = initialSprite;
        
        while (Math.Abs(angleY - 180f) >0)
        {
            var eulerAngles = card1.transform.eulerAngles;
            angleY = Mathf.MoveTowards(eulerAngles.y, 180f, rotationSpeed*Time.deltaTime);
                
            eulerAngles = new Vector3(eulerAngles.x, angleY, eulerAngles.z);
            card1.transform.eulerAngles = eulerAngles;
            yield return null;
        }
    }
}
