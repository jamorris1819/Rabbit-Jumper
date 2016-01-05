using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {

    public Animator rabbitAnimator;
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public Sprite[] backing;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject storeMenu;
    public AudioClip click;
    public Text highscore;

    // options menu
    public Text musicVolumeText;
    public Slider musicVolumeSlider;
    public Text soundVolumeText;
    public Slider soundVolumeSlider;

    public Text controlTypeText;
    public Slider controlTypeSlider;
    string[] gameType = new string[] { "Tilt device to move player", "Drag to move player", "Press buttons to move player" };

    // shop menu
    List<Block> blocks;
    public Text blockName;
    public Text blockDescription;
    public Text blockButtonText;
    public GameObject blockButton;
    public Text carrotCount;
    public AudioClip buy;

    public GameObject[] blockButtons;

    private int tempSelectedBlock;

    void Start()
    {
        // temp code
        /*PlayerPrefs.SetInt("carrotCount", 100);
        PlayerPrefs.SetInt("unlockedBlockWood", 0);
        PlayerPrefs.SetInt("unlockedBlockSand", 0);
        PlayerPrefs.SetInt("unlockedBlockStone", 0);
        PlayerPrefs.SetInt("unlockedBlockSnow", 0);
        PlayerPrefs.SetInt("unlockedBlockCake", 0);*/
        // temp
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("highscore", 0);
        blocks = new List<Block>();
        blocks.Add(new Block("Grass", "They say the best things in life don't come for free - but this does!", 0, true));
        blocks.Add(new Block("Wood", "These platforms are lovingly carved. Watch out for splinters!", 25, PlayerPrefs.GetInt("unlockedBlockWood", 0) == 1));
        blocks.Add(new Block("Sand", "This sand is magically stuck together. It's messy, but it does the job.", 25, PlayerPrefs.GetInt("unlockedBlockSand", 0) == 1));
        blocks.Add(new Block("Stone", "A hard, cold block. Will probably be the safest to stand on.", 50, PlayerPrefs.GetInt("unlockedBlockStone", 0) == 1));
        blocks.Add(new Block("Snow", "There's nothing calmer than a snowy terrain! Happy snow day!", 100, PlayerPrefs.GetInt("unlockedBlockSnow", 0) == 1));
        blocks.Add(new Block("Cake", "This is literally cake, it doesn't get any sweeter than that.", 250, PlayerPrefs.GetInt("unlockedBlockCake", 0) == 1));
        blockButtons[1].GetComponent<Image>().sprite = backing[PlayerPrefs.GetInt("unlockedBlockWood", 0)];
        blockButtons[2].GetComponent<Image>().sprite = backing[PlayerPrefs.GetInt("unlockedBlockSand", 0)];
        blockButtons[3].GetComponent<Image>().sprite = backing[PlayerPrefs.GetInt("unlockedBlockStone", 0)];
        blockButtons[4].GetComponent<Image>().sprite = backing[PlayerPrefs.GetInt("unlockedBlockSnow", 0)];
        blockButtons[5].GetComponent<Image>().sprite = backing[PlayerPrefs.GetInt("unlockedBlockCake", 0)];
        carrotCount.text = PlayerPrefs.GetInt("carrotCount", 0).ToString();
        //ResetToSaved();
    }

    public void OptionsPress()
    {
        ResetToSaved();
        rabbitAnimator.SetTrigger("FlyAway");
        spriteRenderer.sprite = sprites[1];
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void StorePress()
    {
        rabbitAnimator.SetTrigger("FlyAway");
        spriteRenderer.sprite = sprites[2];
        mainMenu.SetActive(false);
        storeMenu.SetActive(true);
        Click();
    }

    public void MusicSlideChanged()
    {
        musicVolumeText.text = musicVolumeSlider.value + "%";
        Click();
    }

    public void SoundSlideChanged()
    {
        soundVolumeText.text = Mathf.FloorToInt(soundVolumeSlider.value * 100) + "%";
        if (Mathf.FloorToInt(soundVolumeSlider.value * 100) == 0)
            soundVolumeText.text = "OFF";
        GameObject.Find("Main Camera").GetComponent<AudioSource>().volume = soundVolumeSlider.value;
        PlayerPrefs.SetFloat("soundVolume", soundVolumeSlider.value);
        PlayerPrefs.Save();
        Click();
    }

    public void ControlChanged()
    {
        controlTypeText.text = gameType[(int)controlTypeSlider.value];
        Click();
    }

    public void OptionsSave()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("soundVolume", soundVolumeSlider.value);
        PlayerPrefs.SetFloat("controlType", controlTypeSlider.value);
        PlayerPrefs.Save();
        OptionsClose();
    }

    public void OptionsClose()
    {
        ResetToSaved();
        rabbitAnimator.SetTrigger("FlyBack");
        spriteRenderer.sprite = sprites[0];
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Click();
    }

    void ResetToSaved()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 50f);
        soundVolumeSlider.value = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        controlTypeSlider.value = PlayerPrefs.GetFloat("controlType", 0);
        controlTypeText.text = gameType[(int)controlTypeSlider.value];
    }

    public void BlockChosen(int index)
    {
        Click();
        tempSelectedBlock = index;
        blockName.text = blocks[index].name;
        blockDescription.text = blocks[index].description;
        blockButton.SetActive(true);
        if (blocks[index].available)
        {
            if (PlayerPrefs.GetInt("platformType", 0) == index)
                blockButtonText.text = "Selected";
            else
                blockButtonText.text = "Select";
        }
        else
            blockButtonText.text = "Buy for " + blocks[index].price;
    }

    public void BlockAction()
    {
        Click();
        if (PlayerPrefs.GetInt("unlockedBlock" + blocks[tempSelectedBlock].name, 0) == 0)
        {
            if (PlayerPrefs.GetInt("carrotCount", 0) >= blocks[tempSelectedBlock].price)
            {
                PlayerPrefs.SetInt("carrotCount", PlayerPrefs.GetInt("carrotCount") - blocks[tempSelectedBlock].price);
                blockButtons[tempSelectedBlock].GetComponent<Image>().sprite = backing[1];
                blocks[tempSelectedBlock].available = true;
                PlayerPrefs.SetInt("unlockedBlock" + blocks[tempSelectedBlock].name, 1);
                BlockChosen(tempSelectedBlock);
                carrotCount.text = PlayerPrefs.GetInt("carrotCount", 0).ToString();
                GameObject.Find("Main Camera").GetComponent<AudioManager>().Play(buy);
            }
        } else
        {
            PlayerPrefs.SetInt("platformType", tempSelectedBlock);
            blockButtonText.text = "Selected";
        }
    }

    public void CloseStore()
    {
        rabbitAnimator.SetTrigger("FlyBack");
        spriteRenderer.sprite = sprites[0];
        storeMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Click()
    {

        GameObject.Find("Main Camera").GetComponent<AudioManager>().Play(click);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}

class Block
{
    public string name;
    public string description;
    public int price;
    public bool available;

    public Block(string name, string description, int price, bool available)
    {
        this.name = name;
        this.description = description;
        this.price = price;
        this.available = available;
    }
}