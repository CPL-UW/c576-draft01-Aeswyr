using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    [SerializeField] private InputHandler input;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private JumpHandler jump;
    [SerializeField] private MovementHandler move;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private GameObject towerIndicatorPrefab;
    [SerializeField] private PrefabList towerList;
    [SerializeField] private ModeHandler mode;
    [SerializeField] private LayerMask interactMask;
    private GameObject towerIndicator;
    private bool modeSelect;


    private bool grounded;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.Gamemode = Mode.MOVE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool prevGrounded = grounded;
        grounded = ground.CheckGrounded();
        if (grounded && !prevGrounded)
            animator.SetTrigger("land");

        if (!modeSelect && input.dir != 0) {
            sprite.flipX = input.dir < 0;
            move.UpdateMovement(input.dir);
        } else {
            animator.SetBool("running", false);
        }

        if (input.move.pressed) {
            if (modeSelect) {
                mode.Increment((int)input.dir);
            } else {
                move.StartAcceleration(input.dir);
                animator.SetBool("running", true);
            }

        }

        if (input.move.released) {
            if (!modeSelect)
                move.StartDeceleration();
        }

        if (input.jump.pressed) {
            animator.SetTrigger("jump");
            jump.StartJump();
            animator.SetBool("grounded", false);
        } else {
            animator.SetBool("grounded", grounded && rbody.velocity.y < 1);
        }

        if (input.mode.pressed)
            HandleModePress();
        if (input.mode.released)
            HandleModeRelease();

        if (towerIndicator != null && !grounded) {
            Destroy(towerIndicator);
            towerIndicator = null;
            mode.ForceMode(Mode.MOVE);
        }

        if (towerIndicator != null) {
            Grid levelGrid = GameController.Instance.GetLevelGrid();
            towerIndicator.transform.position = levelGrid.CellToWorld(levelGrid.WorldToCell(this.transform.position)) + new Vector3(0, 1, 0);
        }
        
        if (input.interact.pressed) {
            if (GameController.Instance.Gamemode == Mode.PLACE
                && GameController.Instance.TrySpendParts(1)) {
                GameObject newTower = Instantiate(towerList.Get("beam"), towerIndicator.transform.position, towerList.Get("beam").transform.rotation);
                newTower.transform.SetParent(GameController.Instance.GetLevelObjectParent().transform);
            }
            if (GameController.Instance.Gamemode == Mode.MOVE) {
                Collider2D obj = Physics2D.OverlapPoint(transform.position, interactMask);
                if (obj != null) {
                    obj.GetComponent<InteractController>().OnInteract();
                }
            }
        }
    }

    private void HandleModePress() {
        if (GameController.Instance.Gamemode == Mode.PLACE) {
            Destroy(towerIndicator);
            towerIndicator = null;
        }
        move.StartDeceleration();
        modeSelect = true;
        mode.gameObject.SetActive(true);
    }

    private void HandleModeRelease() {
        mode.gameObject.SetActive(false);
        modeSelect = false;
        GameController.Instance.Gamemode = mode.GetMode();
        if (GameController.Instance.Gamemode == Mode.PLACE) {
            towerIndicator = Instantiate(towerIndicatorPrefab);
        }
    }
}
