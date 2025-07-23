using System.Collections;
using UnityEngine;

public class AnimatedObjectDestroy : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }
}