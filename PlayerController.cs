using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerController : MonoBehaviour, ITakeDamage
{
    
    public Transform weaponTransform;
    
    [SerializeField] private Transform weaponPosition;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float jumpCheckDistance;
    [SerializeField] private int health;
    [SerializeField] private AudioClip clip;
    private int currentHealth;
    [SerializeField] private bool isInvincible;
    [SerializeField] private float invincibilityTimer;
    [SerializeField] private Animator animator;

    [SerializeField] private IInteractable interactableObject;

    private Rigidbody2D rb;
    private GameInput gameInput;

    private float PlayerScaleX;
    private float RayPosX;
    
    private Vector2 gravityVector;
    private KittenStateLogic KittenStateLogic;

    // Start is called before the first frame update
    void Start()
    {
        clip = Resources.Load<AudioClip>("Sounds/PlayerHit/SFX_Hurt01");
        
        KittenStateLogic = new KittenStateLogic();
        KittenStateLogic.StatesOfKittens = new FirstKitten();
        KittenStateLogic.GunsInitializing(Objects.instance.Claws, Objects.instance.LightRifle, Objects.instance.HeavyRifle);

        PlayerScaleX = gameObject.transform.localScale.x;
        RayPosX = weaponTransform.GetChild(1).localPosition.x;
        
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        currentHealth = health;
        //weaponLogic = weaponTransform.gameObject.GetComponent<IWeapon>();

        rb = GetComponent<Rigidbody2D>();
        gameInput = Objects.instance.gameInput;
        gameInput.OnJumpActionPerformed += GameInput_OnJumpActionPerformed;
        gameInput.OnShootActionPerformed += GameInput_OnShootActionPerformed;
        gameInput.OnInteractActionPerformed += GameInput_OnInteractActionPerformed;
        gameInput.OnFirstKittenChosePerformed += GameInput_OnFirstKittenChosePerformed;
        gameInput.OnSecondKittenChosePerformed += GameInput_OnSecondKittenChosePerformed;
        gameInput.OnThirdKittenChosePerformed += GameInput_OnThirdKittenChosePerformed;
        gameInput.OnEscClickedPerformed += GameInput_OnEscClickedPerformed;
    }

    private void GameInput_OnInteractActionPerformed(object sender, System.EventArgs e)
    {
        HandleInteraction();
    }

    private void GameInput_OnShootActionPerformed(object sender, System.EventArgs e)
    {
        HandleShooting(KittenStateLogic.WeaponLogic);
    }

    private void GameInput_OnJumpActionPerformed(object sender, System.EventArgs e)
    {
        HandleJumping();
    }
    
    private void GameInput_OnFirstKittenChosePerformed(object sender, System.EventArgs e)
    {
        KittenStateLogic.StatesOfKittens.CallingTheFirstKitten(KittenStateLogic);
        Debug.Log("First!");
    }

    private void GameInput_OnSecondKittenChosePerformed(object sender, System.EventArgs e)
    {
        KittenStateLogic.StatesOfKittens.CallingTheSecondKitten(KittenStateLogic);
        Debug.Log("Second!");
    }

    private void GameInput_OnThirdKittenChosePerformed(object sender, System.EventArgs e)
    {
        KittenStateLogic.StatesOfKittens.CallingTheThirdKitten(KittenStateLogic);
        Debug.Log("Third!");
    }

    private void GameInput_OnEscClickedPerformed(object sender, System.EventArgs e)
    {
        bool isActive = Objects.instance.PausePanel.gameObject.activeSelf;
        Objects.instance.PausePanel.SetActive(!isActive);
        if (isActive == true)
        {
            Time.timeScale = 1;
            //Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 0;
            //Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        ChangeDirection(weaponTransform, weaponPosition);
    }

    private void HandleInteraction()
    {
        Debug.Log("InteractAction");
        if (interactableObject != null) interactableObject.Interact(gameObject);
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        // Vector3 moveDir = new Vector3(inputVector.x, inputVector.y);
        inputVector *= moveSpeed;
        rb.velocity = new Vector2(inputVector.x, rb.velocity.y);
        if (inputVector.x != 0) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
        //transform.position += moveDir * moveSpeed * Time.deltaTime;

        //if (rb.velocity.y < 0)
        //{
        //    rb.velocity -= gravityVector * fallMultiplier * Time.deltaTime;
        //}
    }

    private void HandleJumping()
    {
        RaycastHit2D rchit = Physics2D.BoxCast(transform.position, new Vector2(0.99f, 1f), 0f, Vector2.down, jumpCheckDistance, ground);
        //RaycastHit2D rchit = Physics2D.Raycast(transform.position, Vector2.down, jumpCheckDistance, ground);
        if (rchit.collider != null)
        {
            Debug.Log(rchit.collider.name);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleShooting(IWeapon weaponLogic)
    {
        weaponLogic.Recoil(gameObject, weaponPosition);
        weaponLogic.Attack(weaponTransform);
    }

    public void ChangeDirection(Transform Gun, Transform GunPos)
    {
        Vector2 direct = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GunPos.position;
        var angle = Mathf.Atan(direct.y / direct.x);
        Gun.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        
        if (direct.x < 0)
        {
            Gun.Rotate(0, 180, 0);
        }
        
        Gun.position = GunPos.position + new Vector3(direct.x * Gun.localScale.x,
            direct.y * Gun.localScale.x).normalized / 3;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > gameObject.transform.position.x)
        {
            transform.localScale = new Vector3(PlayerScaleX, gameObject.transform.localScale.y);
            if (Gun.GetChild(0).name == "Direction")
            {
                Gun.GetChild(0).localPosition = new Vector3(0.5f, 0f);
                Gun.GetChild(1).localPosition = new Vector3(RayPosX, 0);
                //Gun.localScale = new Vector3(Gun.localScale.x, Gun.localScale.y);
            }
        }
        else
        {
            transform.localScale = new Vector3(-PlayerScaleX, gameObject.transform.localScale.y);
            if (Gun.GetChild(0).name == "Direction")
            {
                Gun.GetChild(0).localPosition = new Vector3(-0.5f, 0f);
                Gun.GetChild(1).localPosition = new Vector3(-RayPosX, 0);
                //Gun.localScale = new Vector3(-Gun.localScale.x, Gun.localScale.y);
            }
        }
    }

    public void TakeDamage()
    {
        if (!isInvincible)
        {
            currentHealth--;
            Objects.instance.HealthPanels[currentHealth].SetActive(false);
            Events.onDoSound?.Invoke(clip);
            if (currentHealth <= 0)
            {
                GetDestroyed();
            }
            StartCoroutine(InvincibilityTimer(invincibilityTimer));
            
        }
    }

    public void GetDestroyed()
    {
        Events.onPlayerDeath?.Invoke();
    }

    public IEnumerator InvincibilityTimer(float time)
    {
        if (isInvincible) yield break;
        isInvincible = true;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            yield return null;
        }
        isInvincible = false;
    }

    public void Respawn()
    {
        currentHealth = health;
        for (int i = 0; i < Objects.instance.HealthPanels.Length; i++)
        {
            Objects.instance.HealthPanels[i].SetActive(true);
        }
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactableObject = collision.gameObject.GetComponent<IInteractable>();
        if (interactableObject != null)
        {
            this.interactableObject = interactableObject;
            Debug.Log($"set object {collision.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactableObject = collision.gameObject.GetComponent<IInteractable>();
        if (interactableObject != null)
        {
            this.interactableObject = null;
            Debug.Log($"set object {collision.name}");
        }
    }
    
}
