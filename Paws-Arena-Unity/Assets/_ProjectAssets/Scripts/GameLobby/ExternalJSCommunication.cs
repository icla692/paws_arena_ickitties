using Anura.ConfigurationModule.Managers;
using Anura.Templates.MonoSingleton;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
internal class NFTsPayload
{
    [SerializeField]
    public List<NFTPayload> nfts;
}

[System.Serializable]
internal class NFTPayload
{
    [SerializeField]
    public string url;
}

public class ExternalJSCommunication : MonoSingleton<ExternalJSCommunication>
{
    public event Action onWalletConnected;
    public event Action onNFTsReceived;

    [DllImport("__Internal")]
    private static extern void ConnectWallet();

    public void TryConnectWallet()
    {

        if (ConfigurationManager.Instance.GameConfig.env == GameEnvironment.DEV)
        {
            MockConnectWallet();
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
            ConnectWallet();
#else
        MockConnectWallet();
#endif
    }


    private async void MockConnectWallet()
    {
        await UniTask.Delay(1000);
        WalletConnected();

        await UniTask.Delay(1000);

        GameState.principalId = "u4s77-qtma7-sriuf-r7rzc-d2new-penyr-qhaap-z3lrx-b2u7e-d4wmv-gqe-dev-dev";

        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?&tokenid=hvtag-6ykor-uwiaa-aaaaa-cqace-eaqca-aaabd-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/jairp-5ykor-uwiaa-aaaaa-cqace-eaqca-aacdq-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/4gya5-nakor-uwiaa-aaaaa-cqace-eaqca-aaabu-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/jxrpt-fikor-uwiaa-aaaaa-cqace-eaqca-aaajx-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/hs6xe-7ikor-uwiaa-aaaaa-cqace-eaqca-aaaad-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/ivtko-hikor-uwiaa-aaaaa-cqace-eaqca-aaaql-q" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://images.entrepot.app/tnc/rw7qm-eiaaa-aaaak-aaiqq-cai/xtmbl-nqkor-uwiaa-aaaaa-cqace-eaqca-aaacj-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=cfbp6-cikor-uwiaa-aaaaa-cqace-eaqca-aadkb-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=v3yzq-4ykor-uwiaa-aaaaa-cqace-eaqca-aaa5u-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=e7yl4-fqkor-uwiaa-aaaaa-cqace-eaqca-aaeeg-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=4cp2w-bykor-uwiaa-aaaaa-cqace-eaqca-aacz4-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=jairp-5ykor-uwiaa-aaaaa-cqace-eaqca-aacdq-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=xtmbl-nqkor-uwiaa-aaaaa-cqace-eaqca-aaacj-a" });
        GameState.nfts.Add(new NFT() { imageUrl = "https://rw7qm-eiaaa-aaaak-aaiqq-cai.raw.ic0.app/?tokenid=4gya5-nakor-uwiaa-aaaaa-cqace-eaqca-aaabu-a" });

        onNFTsReceived?.Invoke();
    }

    [ContextMenu("Connect Wallet")]
    public void WalletConnected()
    {
        onWalletConnected?.Invoke();
    }

    public void ProvidePrincipalId(string principalId)
    {
        GameState.principalId = principalId;
    }
    public void ProvideNFTs(string nftsString)
    {
        GameState.nfts.Clear();

        NFTsPayload payload = JsonUtility.FromJson<NFTsPayload>(nftsString);
        foreach (NFTPayload nft in payload.nfts)
        {
            GameState.nfts.Add(new NFT() { imageUrl = nft.url });
        }

        onNFTsReceived?.Invoke();
    }
}
