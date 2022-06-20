import './App.css';
import { Unity, useUnityContext } from 'react-unity-webgl';
import PlugConnect from '@psychedelic/plug-connect';
import { Principal } from '@dfinity/principal'
import { getNFTActor, NFTActor, getNFTInfo, getUserCollectionTokens, DABCollection, NFTCollection } from '@psychedelic/dab-js';
import fetch from 'cross-fetch';
import { HttpAgent } from '@dfinity/agent'
function App() {

  const { unityProvider } = useUnityContext({
    loaderUrl: './unity/Build/WebApp.loader.js',
    dataUrl: './unity/Build/WebApp.data',
    frameworkUrl: './unity/Build/WebApp.framework.js',
    codeUrl: './unity/Build/WebApp.wasm'
  });

  const getNFTs = async function () {
    console.log("Show me!");

    console.log(window.ic);
    if (typeof window.ic === 'undefined' || typeof window.ic.plug === 'undefined') {
      throw new Error("We cannot detect a Plug Wallet in your browser extensions");
    }
    
    const canisterId = 'rw7qm-eiaaa-aaaak-aaiqq-cai';
    const whitelist = [
      'rw7qm-eiaaa-aaaak-aaiqq-cai'
    ]
    
    const host = "https://mainnet.dfinity.network";
    
    await window.ic.plug.requestConnect({
      whitelist,
      host
    });
    
    console.log("Requested connect!");
    var principal = await window.ic.plug.agent.getPrincipal();
    console.log("Got principal!", principal.toText());
    let principalText = principal.toText();
    
    const DEFAULT_AGENT = new HttpAgent({ fetch, host: "https://mainnet.dfinity.network" });
    const kittiesCollection = await getNFTInfo({ nftCanisterId: canisterId, agent: DEFAULT_AGENT });

    const NFTActor = getNFTActor(
      {
        canisterId: kittiesCollection.principal_id.toString(),
        agent: DEFAULT_AGENT,
        standard: kittiesCollection.standard
      }
    );
    const tokenList = await NFTActor.getUserTokens(Principal.fromText(principalText));
    //TODO: have fun!
  }




  return (
    <>
      <PlugConnect whitelist={['rw7qm-eiaaa-aaaak-aaiqq-cai']}
        onConnectCallback={() => { console.log("Boom!", window.ic.plug.agent.getPrincipal()); }}
      />
      <button onClick={() => getNFTs()}>NFTs?</button>
      <Unity className={"unity-wrapper"} unityProvider={unityProvider} />
    </>
  );
}

export default App;
