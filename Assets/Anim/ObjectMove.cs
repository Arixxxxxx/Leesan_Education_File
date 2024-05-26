using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] bool spin;
    [SerializeField] float spiinSpeedMultiPlyer;
    [Space]
    [SerializeField] bool bounce;
    [SerializeField] float bounceSpeedMultiplier = 1.0f;
    [SerializeField] float bounceAmount = 0.5f; // 최대 스케일 변화량
    Vector3 originalScale = Vector3.one;

    void Start()
    {
        originalScale = transform.localScale;
    }

    Vector3 rot;
    Vector3 bounceVec;
    void Update()
    {
        if (spin)
        {
            rot = transform.eulerAngles;
            rot.z = Mathf.Repeat(rot.z, 360);
            transform.Rotate(Vector3.forward * Time.deltaTime * spiinSpeedMultiPlyer);
        }

        if (bounce)
        {
            float scaleValue = Mathf.PingPong(Time.time * bounceSpeedMultiplier, bounceAmount);
            transform.localScale = originalScale + new Vector3(scaleValue, scaleValue, 0);
        }
        
    }
}
