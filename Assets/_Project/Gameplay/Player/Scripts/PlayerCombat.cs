using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Fist")]
    [SerializeField] private float punchRange = 2.5f;
    [SerializeField] private float punchDamage = 25f;
    [SerializeField] private float punchKnockback = 4f;
    [SerializeField] private float punchCooldown = 0.4f;

    [Header("Throw-out")]
    [SerializeField] private float grabRange = 2.5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private Vector3 carryOffset = new Vector3(0f, -0.3f, 1.5f);

    private PlayerStatsHolder playerStatsHolder;
    private Transform playerCameraTransform;
    private InputHandler inputScript;
    private float nextPunchTime;
    private RudeCustomer carriedCustomer;

    void Awake()
    {
        playerStatsHolder = GetComponent<PlayerStatsHolder>();
        playerCameraTransform = GetComponentInChildren<Camera>().transform;
    }

    void Start()
    {
        inputScript = InputHandler.Instance;
        inputScript.onAttack += OnAttackPressed;
        inputScript.onInteract += OnInteract;
    }

    void Update()
    {
        // hold left click to keep punching, paced by punchCooldown
        if (playerStatsHolder.playerState != PlayerState.Brawl) return;
        if (carriedCustomer != null) return;       // carrying someone -> a click throws them (see OnAttackPressed)
        if (!inputScript.isAttackHeld) return;
        if (Time.time < nextPunchTime) return;

        nextPunchTime = Time.time + punchCooldown;
        Punch();
    }

    private void OnAttackPressed()
    {
        if (playerStatsHolder.playerState != PlayerState.Brawl) return;

        if (carriedCustomer != null) // holding someone -> a single click throws them out
            ThrowCarried();
    }

    private void OnInteract()
    {
        if (playerStatsHolder.playerState != PlayerState.Brawl) return;
        if (carriedCustomer != null) return; // already carrying

        if (!Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, grabRange))
            return;

        RudeCustomer rude = hit.collider.GetComponentInParent<RudeCustomer>();
        if (rude == null) return;

        if (rude.TryGetCarried())
        {
            carriedCustomer = rude;
            rude.transform.SetParent(playerCameraTransform);
            rude.transform.localPosition = carryOffset;
            rude.transform.localRotation = Quaternion.identity;
        }
    }

    private void ThrowCarried()
    {
        RudeCustomer thrown = carriedCustomer;
        carriedCustomer = null;

        thrown.transform.SetParent(null);
        thrown.GetThrownOut(playerCameraTransform.forward, throwForce);
    }

    private void Punch()
    {
        if (!Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit hit, punchRange))
            return;

        RudeCustomer rude = hit.collider.GetComponentInParent<RudeCustomer>();
        if (rude == null) return;

        Vector3 knockDir = rude.transform.position - transform.position;
        knockDir.y = 0;

        rude.TakeHit(punchDamage, knockDir, punchKnockback);
    }
}
