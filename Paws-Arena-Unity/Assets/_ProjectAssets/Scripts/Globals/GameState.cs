using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static string nickname = "Neinitialized Name";
    public static string principalId;
    //NFT
    public static Action<NFT> onSelectedNFT;
    public static string walletId;
    public static List<NFT> nfts { get; private set; }
    public static NFT selectedNFT { get; private set; }

    //Settings
    public static GameSettings gameSettings;

    //Networking
    public static string roomName = "";

    /***Inter-scene needed data ***/
    //Finding Match => In-game
    public static BotInformation botInfo = new BotInformation()
    {
        nickname = "Nick",
        l = 3
    };


    //In-Game => After Game
    public static GameResolveState gameResolveState = GameResolveState.DRAW;
    internal static LeaderboardPostResponseEntity pointsChange;

    static GameState(){
        nfts = new List<NFT>();
        gameSettings = GameSettings.Default();
    }


    public static void SetSelectedNFT(NFT nft)
    {
        selectedNFT = nft;
        onSelectedNFT?.Invoke(nft);
    }

}
