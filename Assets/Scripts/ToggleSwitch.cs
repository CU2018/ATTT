// Fall 2021 - CS294-137 HW3
// Siyu Zhang

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    public GameController gameController;
    public PlaceGameBoard placeGameBoard;
    public Text messageText;

    [SerializeField]
    private bool _isOn = false;
    public bool isOn
    {
        get
        {
             return _isOn;
        }
    }

    [SerializeField]
    private RectTransform toggleIndicator;
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;
    private float offX;
    private float onX;
    [SerializeField]
    private float tweenTime = 0.25f;

    private AudioSource audioSource;
    
    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;
    // Start is called before the first frame update
    void Start()
    {
        offX = toggleIndicator.anchoredPosition.x; // start position
        onX = backgroundImage.rectTransform.rect.width - toggleIndicator.rect.width;
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //Toggle(isOn);
    }

    public void Toggle(bool value, bool playSFX = true)
    {
        if(value != isOn && placeGameBoard.Placed())
        {
            _isOn = value;
            ToggleColor(isOn);
            MoveIndicator(isOn);

            if(playSFX)
                audioSource.Play();

            if(valueChanged != null)
                valueChanged(isOn);

            // enable or disable the augmented mode
            gameController.AtttEnabled = isOn;
            gameController.showStartMessage();
        }
        else
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "You need to place the board first~";
        }
    }

    public void ToggleColor(bool value)
    {
        if(value)
            backgroundImage.DOColor(onColor, tweenTime);
        else
            backgroundImage.DOColor(offColor, tweenTime);
    }

    public void MoveIndicator(bool value)
    {
        if(value)
            toggleIndicator.DOAnchorPosX(onX, tweenTime);
        else
            toggleIndicator.DOAnchorPosX(offX, tweenTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!isOn);
    }

    // function for changing game mode when the target image is detected
    public void OnImageDetected()
    {
        Toggle(!isOn);
        
        for (int a = 0; a < transform.childCount; a++)
        {
            transform.GetChild(a).gameObject.SetActive(true);
        }
    }
}
