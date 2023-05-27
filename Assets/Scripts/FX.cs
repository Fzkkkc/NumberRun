using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _spotExplosionFX;
    public static FX Instance;

    private void Awake() =>
        Instance = this;
    
    public void PlayGoalExplosionFX(Vector3 position)
    {
        _spotExplosionFX.transform.position = position;
        _spotExplosionFX.Play();
    }
}