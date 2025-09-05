using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeftUI : MonoBehaviour
{
   public Image hpbarimage;
   public Image ammobarimage;
   public Image enemyimage; 
   public Text hptext;
   public Text ammotext;
   public Text Scoretext;
   public Text enemiestext;
   
   private void Update()
   {
      hpbarimage.fillAmount = (float)GameManager.instance.playercurrenthp / (float)GameManager.instance.playermaxhp;
      hptext.text = $"{GameManager.instance.playercurrenthp}/{GameManager.instance.playermaxhp}";

      Scoretext.text = $"Scroe : {GameManager.instance.score}";

      ammobarimage.fillAmount = (float)GameManager.instance.ammoCount / (float)GameManager.instance.ammoMax;
      ammotext.text = $"{GameManager.instance.ammoCount}/{GameManager.instance.ammoMax}";
   }
}
