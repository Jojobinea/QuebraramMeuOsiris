using System.Collections;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private Collider2D[] _col;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Box" || collision.gameObject.tag == "Sarcofago")
        {
            AudioSystem.Instance.PlaySFX("BoxBreak");
            for (int i = 0; i < _sprites.Length; i++)
            {
                _sprites[i].enabled = false;
                _col[i].enabled = false;
            }
            _particles.Play();
            StartCoroutine(DestroyWall());
        }
    }

    private IEnumerator DestroyWall()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}
