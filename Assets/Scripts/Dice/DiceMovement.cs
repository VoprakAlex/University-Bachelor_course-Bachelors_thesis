using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
/*
public class DiceMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler//, IPointerDownHandler
{

    [SerializeField] private RectTransform MainRectTransform;

    [SerializeField] private bool CurrentState = false;

    [SerializeField] private Vector3 OriginalScale;

    [SerializeField] private GameObject Hand;

    [SerializeField] private HandManager HandManager;

    [SerializeField] private GameObject Battle;

    [SerializeField] private float SelectScale = 1.1f;

    [SerializeField] private GameObject GlowEffect;

    [SerializeField] private GameObject Dice;

    [SerializeField] private BattleSystem BattleSystem;


    void Awake()
    {
        MainRectTransform = GetComponent<RectTransform>();
        Hand = GameObject.Find("HandPosition");
        Battle = GameObject.Find("BattlePosition");
        HandManager = FindFirstObjectByType<HandManager>();
        BattleSystem = FindFirstObjectByType<BattleSystem>();

        OriginalScale = MainRectTransform.localScale;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState) 
        {
           HandleHoverState();   
        }
    }

    private void TransitionToState0()
    {
        CurrentState = false;
        MainRectTransform.localScale = OriginalScale;
        GlowEffect.SetActive(false);
    }

    private void HandleHoverState()
    {
        GlowEffect.SetActive(true);
        MainRectTransform.localScale = OriginalScale * SelectScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentState == false)
        {           
            CurrentState = true;
            
            HandleHoverState();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentState == true)
        {
            TransitionToState0();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BattleSystem.Battleinprogress)
        {
            return;
        }

        if (HandManager.BattleReady)
        {

            if (MainRectTransform.parent == Battle.transform)
            {
                HandManager.BattleReady = false;
                MainRectTransform.SetParent(Hand.transform);
            }
            else if (MainRectTransform.parent == Hand.transform)
            {
                HandManager.BattleDiceObject.transform.SetParent(Hand.transform);
                HandManager.BattleDiceObject = Dice;
                MainRectTransform.SetParent(Battle.transform);
                
            }
        }
        else
        {
            HandManager.BattleReady = true;
            MainRectTransform.SetParent(Battle.transform);
            HandManager.BattleDiceObject = Dice;
        }
    }
}
*/