using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NFT
{
    public string imageUrl;
    public string furType;
    public List<string> ids;
    public Texture2D imageTex;
    private XmlDocument doc;
    private DateTime recoveryEndDate;

    public bool CanFight => RecoveryEndDate < DateTime.UtcNow;
    public int MinutesUntilHealed => (int)(RecoveryEndDate - DateTime.UtcNow).TotalMinutes;

    public TimeSpan TimeUntilHealed => RecoveryEndDate - DateTime.UtcNow;

    public Action UpdatedRecoveryTime;

    public DateTime RecoveryEndDate
    {
        get
        {
            return recoveryEndDate;
        }
        set
        {
            recoveryEndDate = value;
            UpdatedRecoveryTime?.Invoke();
        }
    }

    public async UniTask GrabImage()
    {
        doc = await NFTImageLoader.LoadSVGXML(imageUrl);
        if (imageTex == null)
        {
            //imageTex = NFTImageLoader.LoadNFT(doc);
            imageTex = NFTImageLoader.LoadNFTLocal(doc);
        }
        if (furType == null)
        {
            furType = NFTImageLoader.GetFurType(doc);
        }
        if (ids == null)
        {
            ids = NFTImageLoader.GetIds(doc);
        }
    }
}
