using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; } 

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button InteractButton;
    [SerializeField] private Button InteractAltButton;
    [SerializeField] private Button sprintButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamePadInteractButton;
    [SerializeField] private Button gamePadInteractAltButton;
    [SerializeField] private Button gamePadsprintButton;
    [SerializeField] private Button gamePadpauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI InteractText;
    [SerializeField] private TextMeshProUGUI InteractAltText;
    [SerializeField] private TextMeshProUGUI sprintText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamePadInteractText;
    [SerializeField] private TextMeshProUGUI gamePadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamePadsprintText;
    [SerializeField] private TextMeshProUGUI gamePadpauseText;
    [SerializeField] private Transform preesAKeyTransform;

    private Action onCloseButtonAction;

    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() => {

            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        }); 
        
        musicButton.onClick.AddListener(() => { 
        MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });


        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up);});
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left);});
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down);});
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right);});
        sprintButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Sprint);});
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause);});
        InteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact);});
        InteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Alternate);});
        gamePadsprintButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Sprint);});
        gamePadpauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Pause);});
        gamePadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Interact);});
        gamePadInteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Interact_Alternate);});
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();

        
        Hide();
        PressaKeyyHide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        UpdateVisual();
        Hide();
    }

    private void UpdateVisual() 
    {
        soundEffectsText.text = "Sound Effects : " + MathF.Round(SoundManager.Instance.GetVolume()* 10f);
        musicText.text = "Music : " + MathF.Round(MusicManager.Instance.GetVolume()* 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        InteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        InteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        sprintText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Sprint);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamePadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        gamePadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact_Alternate);
        gamePadsprintText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Sprint);
        gamePadpauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);
    }
    public void Show(Action onCloseButtonAction) 
    {
        this.onCloseButtonAction = onCloseButtonAction;
     gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    private void Hide() 
    {
     gameObject.SetActive(false);
        
    }

    private void PressaKeyyShow()   
    {
    preesAKeyTransform.gameObject.SetActive(true);
    }

    private void PressaKeyyHide() 
    {
    preesAKeyTransform.gameObject.SetActive(false);
    }


    private void RebindBinding(GameInput.Binding binding) 
    {
    PressaKeyyShow();
        GameInput.Instance.RebindBinding(binding,() => { 
            
            PressaKeyyHide();
            UpdateVisual();
    });
    }
}