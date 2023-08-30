using UnityEngine;

public class WibblerGO : MonoBehaviour
{
    public float wibbleSpeed;

    private void Start()
    {
        wibbleSpeed = Random.Range(1f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        position = new Vector3(position.x,
            Mathf.PerlinNoise1D(Time.time + position.x) * wibbleSpeed, position.z);
        transform.position = position;
    }
}
