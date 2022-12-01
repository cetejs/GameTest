using System.Collections;
using UnityEngine;

public class DagdollPreview : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float loopTime = 3.0f;
    
    private IEnumerator Start()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(loopTime);
        while (true)
        {
            anim.enabled = false;
            yield return waitForSeconds;
            anim.enabled = true;
            yield return new WaitForSeconds(Random.Range(0f, 1.0f));
        }
    }
}
