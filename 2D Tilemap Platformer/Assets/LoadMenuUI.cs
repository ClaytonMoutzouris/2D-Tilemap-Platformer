using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenuUI : UIScrollMenu
{
    public PlayerMenuTabUI menuTab;
    public MenuOption optionPrefab;

    List<string> characters = new List<string>();
    PlayerSaveData saveData;
    // Start is called before the first frame update
    public override void OnEnable()
    {
        base.OnEnable();
        //put something in to reload the known stats
        LoadCharacters();
        LoadMenuOptions();
    }

    public void LoadCharacters()
    {
        characters.Clear();
        //MenuOptionSelectorUI menuSelectorNode = menuOptions[(int)MenuOptionIndex.Map];
        //menuSelectorNode.ClearOptions();
        string path = Path.Combine(Application.streamingAssetsPath, "GameData", "Characters", "");

        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.player");
        Debug.Log(path);

        foreach (FileInfo file in info)
        {
            Debug.Log(file.Name);
            characters.Add(Path.GetFileNameWithoutExtension(file.Name));
        }

    }

    public override void LoadMenuOptions()
    {
        base.LoadMenuOptions();

        foreach (string character in characters)
        {
            MenuOption characterOption = Instantiate(optionPrefab, container.transform);


            characterOption.Init();
            characterOption.name = character;
            characterOption.SetOptionName(character);
            AddOption(characterOption);
        }

        SetNavigation();
        SetCurrentNode(0);
        GetComponent<PlayerMenuTabUI>().anchorObject = currentNode.gameObject;
        menuTab.SetAnchor();
    }

    public void LoadCharacter()
    {
        CharacterSelectMenu.instance.characterSelectScreens[menuTab.playerIndex].LoadCharacter(saveData);
    }

    public void SelectNode()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "GameData", "Characters", currentNode.name + ".player");

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);

            PlayerSaveData loadData = JsonConvert.DeserializeObject<PlayerSaveData>(loadJson);

            //gameGrid.SetWorldTiles(loadData.tiles, true, true);

            //mapName = Path.GetFileNameWithoutExtension(path);
            saveData = loadData;
            //CharacterSelectMenu.instance.characterSelectScreens[menuTab.playerIndex].LoadCharacter(saveData);
        }
        else
        {
            Debug.LogError("Save file not found: " + path);
        }
    }

    public override void SetCurrentNode(MenuOption node)
    {
        //currentNode = node;
        //scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoViewVertical(currentNode.GetComponent<RectTransform>());
        base.SetCurrentNode(node);

        //LoadCharacter();
        SelectNode();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
