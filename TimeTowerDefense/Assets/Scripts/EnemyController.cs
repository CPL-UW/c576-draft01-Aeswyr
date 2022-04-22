using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public SpawnData spawnData;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private GroundedCheck ground;
    [SerializeField] private GameObject indParts, indAmmo, indTime;
    [SerializeField] private Animator animator;
    private long spawnTick;

    // Start is called before the first frame update
    void Start()
    {
        spawnTick = HUDController.Instance.GetTick();
        if (spawnData.carry == CarryType.AMMO)
            indAmmo.SetActive(true);
        if (spawnData.carry == CarryType.PARTS)
            indParts.SetActive(true);
        
        if (spawnData.type == EnemyType.TIME) {
            indTime.SetActive(true);
            indTime.GetComponent<TextMeshPro>().text = spawnData.data + ":00";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float sign = Mathf.Sign(GameController.Instance.GetGoal().x - transform.position.x);
        if (GameController.Instance.tickDiff > 0) {
            sign *= -1;
            if (spawnData.type == EnemyType.TIME)
            sign = 0;
        }  

        if (spawnData.type == EnemyType.TIME) {
            animator.SetBool("on", int.Parse(spawnData.data) == HUDController.Instance.GetHour());
        }
            
        rbody.velocity = new Vector2(sign * speed, rbody.velocity.y);
        if (Utils.Raycast(transform.position + new Vector3(1.5f * sign, 0, 0), new Vector2(sign, 0), 0.1f, wallMask) && ground.CheckGrounded()) {
            rbody.velocity = new Vector2(rbody.velocity.x, 50f);
        }

        if (HUDController.Instance.GetTick() == spawnTick && spawnData.type != EnemyType.TIME) {
            GameController.Instance.UnspawnEnemy(this);
            Destroy(this.gameObject);
        }
    }

    public void Die() {
        if (spawnData.type == EnemyType.TIME && int.Parse(spawnData.data) != HUDController.Instance.GetHour())
            return;
        if (spawnData.carry == CarryType.AMMO)
            GameController.Instance.AddAmmo(1);
        if (spawnData.carry == CarryType.PARTS)
            GameController.Instance.AddParts(1);
        Analytics.LogEvent("ENEMYDEFEATED", $"{spawnTick}");
        Remove();
    }

    public void Remove() {
        GameController.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
