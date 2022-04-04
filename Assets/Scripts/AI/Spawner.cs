using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IHasCooldown
{
    private List<int> Pattern;
    [SerializeField] private int id;
    [SerializeField] private float cooldownDuration;
    
    [Header("References")]
    [SerializeField] private CooldownManager cooldownManager;
    [SerializeField] private GameObject MeleePrefab;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject BeamPrefab;

    public int Id => id;
    public float CooldownDuration => cooldownDuration;

    // Start is called before the first frame update
    void Start()
    {
        id = 1;
        cooldownDuration = 2f;
        cooldownManager = GameObject.FindGameObjectWithTag("CooldownManager").GetComponent<CooldownManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownManager.IsOnCooldown(1)) { return; }

        if (Pattern.Count > 0)
        {
            StateMachine.EnemyType enemyType = Translate(Pattern[0]);
            Pattern.RemoveAt(0);

            switch (enemyType)
            {
                case StateMachine.EnemyType.Melee:
                    Instantiate(MeleePrefab, transform);
                    break;
                case StateMachine.EnemyType.Projectile:
                    Instantiate(ProjectilePrefab, transform);
                    break;
                case StateMachine.EnemyType.Beam:
                    Instantiate(BeamPrefab, transform);
                    break;
            }

            cooldownManager.PutOnCooldown(this);
        }
    }

    private StateMachine.EnemyType Translate(int enemyType)
    {
        switch (enemyType)
        {
            case 0:
                return StateMachine.EnemyType.Melee;
            case 1:
                return StateMachine.EnemyType.Projectile;
            case 2:
                return StateMachine.EnemyType.Beam;
            default:
                break;
        }

        return StateMachine.EnemyType.SpawnFail;
    }

    public void SetPattern(List<int> pattern) {
        Pattern = pattern;
    }
}
