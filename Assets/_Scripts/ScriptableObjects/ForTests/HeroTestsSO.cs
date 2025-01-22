using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTestsSO : HeroSO
{
    
    public void SetName(string name) => _name = name;
    public void SetMaxHealth(int amount) => _maxHealth = amount;
    public void SetCurrentHealth(int amount) => _currentHealth = amount;
    public void SetHeroClass(HeroClass heroClass) => _heroClass = heroClass;
    public void SetHeroLevel(int level) => _heroLevel = level;
}
