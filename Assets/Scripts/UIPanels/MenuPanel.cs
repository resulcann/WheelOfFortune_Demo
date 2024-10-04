using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour, IPanel
{
    [SerializeField] private GameObject _innerPanel;
    [SerializeField] private Button _spinBtn;
    [SerializeField] private SpinPanel _spinPanel;
    
    public void Init()
    {
        _spinBtn?.onClick.AddListener(UIManager.Instance.Open_SpinPanel);
    }
    public void OnDestroyProcess()
    {
        _spinBtn?.onClick.RemoveListener(UIManager.Instance.Open_SpinPanel);
    }
    
    public void OpenPanel() => _innerPanel.SetActive(true);

    public void ClosePanel() => _innerPanel.SetActive(false);
}
