using UnityEngine;

public class EnemyView : MonoBehaviour {
    public GameObject enemy;
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = enemy.GetComponent<SpriteRenderer>();
    }

    public void setSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

    public void OnDefeat() {
        Destroy(enemy);
    }
}