using UnityEngine;

public class CircleData : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private Transform player;

    [SerializeField] private float speed = 0.05f;
    [SerializeField] private int count = 100;
    [SerializeField] private float playerRadius = 0.15f;

    private Material mat;

    private Vector4[] balls;
    private Vector2[] vel;

    private Bounds localBounds;

    void Awake()
    {
        //randomize ball radius + positions
        mat = rend.material;

        balls = new Vector4[count];
        vel = new Vector2[count];
        
        localBounds = rend.localBounds;

        for (int i = 0; i < count; i++)
        {
            balls[i] = new Vector4(
                Random.value,
                Random.value,
                Random.Range(0.0025f, 0.0055f),
                0
            );

            vel[i] = Random.insideUnitCircle.normalized * speed;
        }

        Push();
    }

    void Update()
    {
        //move balls with perlin noise
        float dt = Time.deltaTime;

        for (int i = 0; i < count; i++)
        {
            Vector4 b = balls[i];

            Vector2 pos = new Vector2(b.x, b.y);

            Vector2 noise = new Vector2(
                Mathf.PerlinNoise(Time.time * 0.5f, i * 10.1f) - 0.5f,
                Mathf.PerlinNoise(i * 20.2f, Time.time * 0.5f) - 0.5f
            );

            pos += (vel[i] + noise * 0.02f) * dt;

            pos = new Vector2(
                Mathf.Repeat(pos.x, 1f),
                Mathf.Repeat(pos.y, 1f)
            );

            balls[i] = new Vector4(pos.x, pos.y, b.z, 0);
        }
        
        //check if player is in bounds, send player position
        Vector3 local = rend.transform.InverseTransformPoint(player.position);

        Vector3 min = localBounds.min;
        Vector3 size = localBounds.size;

        Vector2 playerUV = new Vector2(
            (local.x - min.x) / size.x,
            (local.z - min.z) / size.z
        );
        
        playerUV = new Vector2(
            1f - playerUV.x,
            1f - playerUV.y
        );

        bool onObject =
            player.position.x > rend.bounds.min.x &&
            player.position.x < rend.bounds.max.x &&
            player.position.z > rend.bounds.min.z &&
            player.position.z < rend.bounds.max.z;

        mat.SetVector("_PlayerPos", new Vector4(playerUV.x, playerUV.y, 0, 0));
        mat.SetFloat("_PlayerRadius", playerRadius);
        mat.SetFloat("_PlayerActive", onObject ? 1f : 0f);

        Push();
    }

    void Push()
    {
        mat.SetVectorArray("_Balls", balls);
        mat.SetInt("_BallCount", count);
    }
}