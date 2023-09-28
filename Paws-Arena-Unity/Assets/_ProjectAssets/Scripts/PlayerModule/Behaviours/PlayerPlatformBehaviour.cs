using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformBehaviour : MonoBehaviour
{
    public PlayerCustomization playerCustomization;
    public bool isMyCat = true;

    private void OnEnable()
    {
        if (isMyCat)
        {
            playerCustomization.SetCat(GameState.selectedNFT.imageUrl, GameState.selectedNFT.ids);
        }
        else
        {
            playerCustomization.wrapper.SetActive(false);
        }
    }


    public async void SetCat(string imageUrl)
    {
        NFT nft = new NFT()
        {
            imageUrl = imageUrl
        };

        playerCustomization.wrapper.SetActive(false);

        await nft.GrabImage();

        playerCustomization.wrapper.SetActive(true);
        playerCustomization.SetCat(nft.imageUrl, nft.ids);
    }

}
