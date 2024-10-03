using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour, IPanel
{
    [SerializeField] private Button _spinBtn;
    [SerializeField] private SpinPanel _spinPanel;
    
    private bool _isOpen = false;

    private void Start()
    {
        CheckIsOpen();
    }

    private void OnEnable()
    {
        _spinBtn.onClick.AddListener(_spinPanel.Toggle);
        _spinBtn.onClick.AddListener(Toggle);
    }
    private void OnDisable()
    {
        _spinBtn.onClick.RemoveListener(_spinPanel.Toggle);
        _spinBtn.onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        gameObject.SetActive(!_isOpen);
        _isOpen = !_isOpen;
    }

    private void CheckIsOpen()
    {
        _isOpen = gameObject.activeInHierarchy;
    }
}
