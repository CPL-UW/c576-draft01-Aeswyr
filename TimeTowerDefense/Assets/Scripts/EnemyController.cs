using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public SpawnData spawnData;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private GameObject indParts, indAmmo;
    private long spawnTick;

    // Start is called before the first frame update
    void Start()
    {
        spawnTick = HUDController.Instance.GetTick();
        if (spawnData.carry == CarryType.AMMO)
            indAmmo.SetActive(true);
        if (spawnData.carry == CarryType.PARTS)
            indParts.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float sign = Mathf.Sign(GameController.Instance.GetGoal().x - transform.position.x);
        if (GameController.Instance.tickDiff > 0)
            sign *= -1;
        rbody.velocity = new Vector2(sign * speed, rbody.velocity.y);
        if (Utils.Raycast(transform.position + new Vector3(1.5f * sign, 0, 0), new Vector2(sign, 0), 0.1f, wallMask) && ground.CheckGrounded()) {
            rbody.velocity = new Vector2(rbody.velocity.x, 50f);
        }

        if (HUDController.Instance.GetTick() == spawnTick) {
            GameController.Instance.UnspawnEnemy(this);
            Destroy(this.gameObject);
        }
    }

    public void Die() {
        if (spawnData.carry == CarryType.AMMO)
            GameController.Instance.AddAmmo(1);
        if (spawnData.carry == CarryType.PARTS)
            GameController.Instance.AddParts(1);
        Remove();
    }

    public void Remove() {
        GameController.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
