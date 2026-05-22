using UnityEngine;
using UnityEngine.UI;

public class RudeCustomerHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage; // Image Type = Filled, Fill Method = Horizontal
    [SerializeField] private RudeCustomer owner;
    [SerializeField] private bool hideWhenFull = false;

    private Transform cam;
    private Canvas barCanvas;

    void Awake()
    {
        if (owner == null) owner = GetComponentInParent<RudeCustomer>();
        barCanvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        if (PlayerLocator.PlayerTransform != null)
        {
            Camera playerCam = PlayerLocator.PlayerTransform.GetComponentInChildren<Camera>();
            if (playerCam != null) cam = playerCam.transform;
        }
    }

    void LateUpdate()
    {
        if (owner == null || fillImage == null) return;

        float hp = owner.HPNormalized;
        fillImage.fillAmount = hp;

        // hide the bar's rendering at full HP, but keep this script running so it can come back
        if (hideWhenFull && barCanvas != null)
            barCanvas.enabled = hp < 1f;

        if (cam != null)
            transform.forward = cam.forward; // billboard: stay flat to the camera so it's always readable
    }
}
