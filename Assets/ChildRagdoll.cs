using UnityEngine;

public class ChildRagdoll : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        RagdollMe other = collision.gameObject.GetComponentInParent<RagdollMe>();
        RagdollMe self = GetComponentInParent<RagdollMe>();

        if(self != null && self != other)
        {
            self.Ragdoll(collision.impulse);
            enabled = false;
        }
    }
}
