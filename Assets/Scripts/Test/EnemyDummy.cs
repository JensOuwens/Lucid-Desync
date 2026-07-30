using UnityEngine;

public class EnemyDummy : MonoBehaviour, IDamagable
{
    public void Hurt(float damage)
    {
        Debug.Log("taken" + damage);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
