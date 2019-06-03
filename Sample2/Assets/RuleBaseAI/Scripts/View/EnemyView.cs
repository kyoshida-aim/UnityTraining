using UnityEngine;

public class EnemyView : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite) {
        spriteRenderer.sprite = sprite;
    }

    public void OnDefeat() {
        Destroy(this.gameObject);
    }
}