using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Possibly a place to set options and their defaults
//Might just use a bunch of enums for things with set number of options, we'll see
[CreateAssetMenu(fileName = "AppearanceMenuOptions", menuName = "ScriptableObjects/GameData/AppearanceMenuOptions")]
public class AppearanceMenuOptions : ScriptableObject
{

    public List<Color> skinColors;
    public List<Color> hoodPrimaryColors;
    public List<Color> hoodSecondaryColors;
    public List<Color> shirtPrimaryColors;
    public List<Color> shirtSecondaryColors;
    public List<Color> pantsColors;
    public List<Color> shoesColors;



}