using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperBadge : MonoBehaviour {

    public Sprite none;
    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;

    private SpriteRenderer spriteRenderer;
    private Image image;
    private GroundTile tile;

    private void Awake() {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        image = transform.GetComponent<Image>();
    }

    private void Start() {
		tile = transform.GetComponentInParent<GroundTile> ();

		if (tile != null) {
			ChangeBadge ();
		}
	}

    public void ChangeBadge() {
		Achivement achivement = tile.achivements;

		if (achivement.mission1_accomplished && achivement.mission2_accomplished && achivement.mission3_accomplished) {
			SwitchToGold ();
		} 
		else if ((achivement.mission1_accomplished && achivement.mission2_accomplished) || (achivement.mission1_accomplished && achivement.mission3_accomplished || (achivement.mission2_accomplished && achivement.mission3_accomplished))) {
			SwitchToSilver ();
		} 
		else if (achivement.mission1_accomplished || achivement.mission2_accomplished || achivement.mission3_accomplished) {
			SwitchToBronze ();
		}
	}

    public void SwitchToBronze() {
		spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		spriteRenderer.sprite = bronze;
    }

    public void SwitchToSilver() {
		spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		spriteRenderer.sprite = silver;
    }

    public void SwitchToGold() {
		spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		spriteRenderer.sprite = gold;
    }
}
