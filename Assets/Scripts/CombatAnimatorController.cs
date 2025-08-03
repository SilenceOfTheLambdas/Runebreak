using UnityEngine;

public class CombatAnimatorController : MonoBehaviour
{
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Triggered when the character receives a hit.
    /// </summary>
    public void RecievedHit(AnimationClip attackingClip)
    {
        yield return new WaitForSeconds(attackingClip.length * 0.5f);
        _animator.SetTrigger("hasBeenHit");
    }
}
