using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float speed = 3f;
    public Transform follow;
    Transform _transform;

    /// <summary>
    /// Unity awake
    /// </summary>
    void Awake()
    {
        this._transform = this.transform;
    }

    /// <summary>
    /// Unity update
    /// </summary>
    void Update()
    {
        if (this.follow)
        {
            this._transform.position = Vector3.Lerp(
                this._transform.position,
                this.follow.position,
                this.speed * Time.deltaTime
            );
        }
    }
}