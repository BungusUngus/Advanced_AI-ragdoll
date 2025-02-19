using UnityEngine;

public struct JointData
{
    public Joint joint;
    public float previousForce;

    public JointData(Joint newJoint)
    {
        this.joint = newJoint;
        previousForce = 0f;
    }
}

public class RagdollMe : MonoBehaviour
{
    Animator animator;
    JointData[] jointData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        GetAllJoints();
        AddChildRagdoll();
        SetChildColliders(false);

    }


    // Update is called once per frame
    private void Update()
    {
        float scoreTotal = 0f;
        for (int i = 0; i < jointData.Length; i++)
        {
            var joint = jointData[i];
            float newScore = Mathf.Abs(joint.previousForce - joint.joint.currentForce.magnitude);

            scoreTotal += newScore;

            joint.previousForce = joint.joint.currentForce.magnitude;
        }
        Debug.Log(scoreTotal);
    }

    void AddChildRagdoll()
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.gameObject.AddComponent<ChildRagdoll>();
            if (rb.gameObject != gameObject)
            {
                rb.isKinematic = true;
            }
        }
    }

    void SetChildColliders(bool enabled)
    {

        Collider currentCol = GetComponent<Collider>();
        foreach (var col in GetComponentsInChildren<Collider>())
        {
            if (col == currentCol)
            {
                currentCol.enabled = !enabled;
            }
            else
            {
                col.enabled = enabled;
            }
        }
    }

    void GetAllJoints()
    {
        Joint[] joints = GetComponentsInChildren<Joint>();
        jointData = new JointData[joints.Length];

        for (int i = 0; i < jointData.Length; i++)
        {
            Joint joint = joints[i];
            jointData[i] = new JointData(joint);
        }
    }


    public void Ragdoll(Vector3 impact)
    {

        if (impact.magnitude < 4.5f) return;
        SetChildColliders(true);
        ResetVelocities(impact);
        animator.enabled = false;
        enabled = false;
    }

    void ResetVelocities(Vector3 impact)
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.linearVelocity = -impact / rbs.Length;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        Ragdoll(collision.impulse);
    }
}
