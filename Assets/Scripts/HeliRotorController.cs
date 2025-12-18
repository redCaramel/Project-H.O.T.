using System;
using UnityEngine;

//rename
public class HeliRotorController : MonoBehaviour
{
	public enum Axis
	{
		X,
		Y,
		Z,
	}
	public Axis RotateAxis;
	public AudioSource audioSource;
	public AudioClip audioClip;
    private float _rotarSpeed;
    public float RotarSpeed
    {
        get { return _rotarSpeed; }
        set { _rotarSpeed = Mathf.Clamp(value,0,3000); }
    }

    private float rotateDegree;
    private Vector3 OriginalRotate;

    void Start ()
	{
        OriginalRotate = transform.localEulerAngles;
	}

	void Update ()
	{
		setAudio();
        rotateDegree += RotarSpeed * Time.deltaTime;
	    rotateDegree = rotateDegree%360;

		switch (RotateAxis)
		{
		    case Axis.Y:
		        transform.localRotation = Quaternion.Euler(OriginalRotate.x, rotateDegree, OriginalRotate.z);
		        break;
		    case Axis.Z:
		        transform.localRotation = Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateDegree);
		        break;
		    default:
		        transform.localRotation = Quaternion.Euler(rotateDegree, OriginalRotate.y, OriginalRotate.z);
		        break;
		}
	}
	void setAudio()
    {
        if (rotateDegree > 0)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = audioClip;
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
    }
}
