using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimatorController : MonoBehaviour
{
    private readonly static int HasBeenHit = Animator.StringToHash("hasBeenHit");
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Initiates the hit reaction animation for the character.
    /// </summary>
    /// <param name="attackLength">The duration of the attack that caused the hit. This is used to calculate the delay before triggering the hit animation.</param>
    /// <remarks>
    /// This method starts a coroutine <see cref="RecievedHitCoroutine"/> that delays the hit animation trigger based on the attack length.
    /// The actual animation is triggered after half the attack length has passed.
    /// </remarks>
    public void RecievedHit(ref float attackLength)
    {
        StartCoroutine(RecievedHitCoroutine(attackLength));
    }

    private IEnumerator RecievedHitCoroutine(float attackLength)
    {
        yield return new WaitForSeconds(attackLength * 0.5f);
        _animator.SetTrigger(HasBeenHit);
    }
}