using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreField;
    [SerializeField] Image[] _health;

    // Start is called before the first frame update
    void Start()
    {
        _scoreField.text = "0000";
    }

    // Update is called once per frame
    void Update()
    {
        _scoreField.text = GameController.Instance.Score.ToString().PadLeft(4, '0');

        //its too late and I'm already drinking
        switch (PlayerController.Instance.Health)
        {
            case 2:
                _health[2].enabled = false;
                break;
            case 1:
                _health[1].enabled = false;
                break;
            case 0 :
                _health[0].enabled = false;
                break;
            default: break;
        }
    }
}
