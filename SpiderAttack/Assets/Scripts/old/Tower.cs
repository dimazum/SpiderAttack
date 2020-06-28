using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public Text damageTextTower;
    public Slider sliderHpTower;
    public SpriteRenderer cracks;
    public SpriteRenderer cracks2;
    public Sprite crack;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        cracks.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Damage(float damageEnemy)
    {
        sliderHpTower.value = sliderHpTower.value - damageEnemy;
        damageTextTower.text = damageEnemy.ToString();
        animator.Play("TextDamageTower");
        if ((sliderHpTower.value / sliderHpTower.maxValue)<=0.9f )
        {
            cracks.sprite = crack;
            if ((sliderHpTower.value / sliderHpTower.maxValue) <= 0.8f)
            {
                cracks2.sprite = crack;
            }
        }
         
    }
    public void FixTower()
    {
        sliderHpTower.value += 200f;
        gameState.currentPlayer.money-=20;
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Udar"))
    //    {


    //        Debug.Log("Udar");
    //        sliderHpTower.value = sliderHpTower.value - 0.05f;
    //        if (sliderHpTower.value <= 900f)
    //        {
    //            cracks.sprite = crack;
    //        }
    //    }
    
}