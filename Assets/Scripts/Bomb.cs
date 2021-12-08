using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float BombTimer = 4.5f;
    public GameObject effect;

    private float OwnTimer = 0f;
    private bool isEffectCreated = false;
    private bool isDealedDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, BombTimer);
    }

    // Update is called once per frame
    void Update()
    {
        OwnTimer += Time.deltaTime;
        if (OwnTimer > BombTimer - 0.5f)
        {
            if (!isEffectCreated)
            {
                Instantiate(effect, transform.position, transform.rotation, transform);
                isEffectCreated = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isEffectCreated)
        {
           if( collision.gameObject.layer != LayerMask.NameToLayer("Obstacle") && !isDealedDamage)
            {
                collision.gameObject.SendMessage("DealDamage", SendMessageOptions.DontRequireReceiver);
                isDealedDamage = true;
            }
        }
    }

}
