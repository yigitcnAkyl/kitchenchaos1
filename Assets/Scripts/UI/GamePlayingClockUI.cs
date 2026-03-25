using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image Image;

    private void Update()
    {
      Image.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
