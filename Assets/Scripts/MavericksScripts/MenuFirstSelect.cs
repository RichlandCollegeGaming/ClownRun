using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuFirstSelect : MonoBehaviour
{
    [SerializeField] Button firstButton;

    private void OnEnable()
    {
        if(firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }
}
