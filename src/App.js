import './App.css'
import { Unity, useUnityContext } from 'react-unity-webgl'
import PlugConnect from '@psychedelic/plug-connect'
import { Principal } from '@dfinity/principal'
import {
  getNFTActor,
  NFTActor,
  getNFTInfo,
  getUserCollectionTokens,
  DABCollection,
  NFTCollection,
} from '@psychedelic/dab-js'
import fetch from 'cross-fetch'
import { HttpAgent } from '@dfinity/agent'
import { useEffect, useState, useCallback } from 'react'
import { StoicIdentity } from 'ic-stoic-identity'
import { GetUnityBaseUrl } from './Utils/Constants'

function App() {
  useEffect(() => {
    document.title = 'Paws Arena';
  }, []);
  //create principal whitelist for the NFT actor
  // const principalWhitelist = [
  //   'vdeac-sem4r-b24tn-46byj-mc55d-wot2o-zd4mu-n7ch4-333wi-z4pw5-rae',
  //   'gogf7-nq7co-2dy6i-z6x22-qdobx-l7uoc-tgmtd-qjsj7-ob5j4-32ogc-vae',
  //   'cd6tk-lb6fi-e3ylu-spfqx-y2dyl-pme6b-aacni-akms7-sjjdw-fosdm-uae',
  //   'byonl-gsuqx-7tkhz-hgeev-34jd7-zug5j-fleeu-mnzdo-z2fzj-htgyc-bae',
  //   'u4s77-qtma7-sriuf-r7rzc-d2new-penyr-qhaap-z3lrx-b2u7e-d4wmv-gqe',
  // ]

  const [readyToConnect, setReadyToConnect] = useState(false)
  const [connected, setConnected] = useState(false)
  const [gotNFTs, setGotNFTs] = useState(false)
  const host = 'https://knbkj-fiaaa-aaaan-qaadq-cai.ic0.app/'
  // function handleCacheControl(url) {
  //   if (
  //     url.match(/\.jpg/) ||
  //     url.match(/\.png/) ||
  //     url.match(/\.data/) ||
  //     url.match(/\.wasm/)
  //   ) {
  //     return 'immutable'
  //   }
  //   return 'no-store'
  // }

  const {unityProvider,
    sendMessage,
    addEventListener,
    removeEventListener,
    loadingProgression,
    isLoaded } = useUnityContext({
    loaderUrl: './unity/Build/pawsArenaBuild.loader.js',
    dataUrl: 'https://webapiwithssl20230210160824.azurewebsites.net/download/pawsArenaBuild.data.br',
    frameworkUrl: 'https://webapiwithssl20230210160824.azurewebsites.net/download/pawsArenaBuild.framework.js.br',
    codeUrl: 'https://webapiwithssl20230210160824.azurewebsites.net/download/pawsArenaBuild.wasm.br'
    // dataUrl: 'https://localhost:44300/download/pawsArenaBuild.data.br',
    // frameworkUrl: 'https://localhost:44300/download/pawsArenaBuild.framework.js.br',
    // codeUrl: 'https://localhost:44300/download/pawsArenaBuild.wasm.br'
  });

  const loadingPercentage = Math.round(loadingProgression * 100)

  // We'll use a state to store the device pixel ratio.
  // const [devicePixelRatio, setDevicePixelRatio] = useState(
  //   window.devicePixelRatio,
  // )
  // const handleChangePixelRatio = useCallback(() => {
  //   // A function which will update the device pixel ratio of the Unity
  //   // Application to match the device pixel ratio of the browser.
  //   const updateDevicePixelRatio = function () {
  //     setDevicePixelRatio(window.devicePixelRatio)
  //   }
  //   // A media matcher which watches for changes in the device pixel ratio.
  //   const mediaMatcher = window.matchMedia(
  //     `screen and (resolution: ${devicePixelRatio}dppx)`,
  //   )
  //   if (mediaMatcher.matches) {
  //     updateDevicePixelRatio()
  //   }
  //   // Adding an event listener to the media matcher which will update the
  //   // device pixel ratio of the Unity Application when the device pixel
  //   // ratio changes.
  //   mediaMatcher.addEventListener('change', updateDevicePixelRatio)
  //   return function () {
  //     // Removing the event listener when the component unmounts.
  //     mediaMatcher.removeEventListener('change', updateDevicePixelRatio)
  //   }
  // }, [devicePixelRatio])

  const connectStoic = () => {
    console.log('try to connect')
    var canisterId = 'rw7qm-eiaaa-aaaak-aaiqq-cai'
    const whitelist = ['rw7qm-eiaaa-aaaak-aaiqq-cai']
    //
    StoicIdentity.load().then(async (identity) => {
      console.log('StoicIdentity.load()')
      //check if we have an existing connection
      if (identity !== false) {
        console.log('identity !== false')

        //ID is a already connected wallet!
      } else {
        //No existing connection, lets make one!
        console.log('StoicIdentity.connect()')

        identity = await StoicIdentity.connect()
      }
      // if (principalWhitelist.some((e) => e === identity.getPrincipal().toText())) {
      setConnected(true)

      let agent = new HttpAgent({
        identity,
        host: host,
      })
      // agent.fetchRootKey().catch((err) => {
      //   console.warn(
      //     "Unable to fetch root key. Check to ensure that your local replica is running",
      //   );
      //   console.error(err);
      // });

      const kittiesCollection = await getNFTInfo({
        nftCanisterId: canisterId,
        agent: agent,
      })
      // get Nft Actor for agent and canisterId with kittiesCollection standard

      const NFTActor = getNFTActor({
        canisterId: kittiesCollection.principal_id.toString(),
        agent: agent,
        standard: kittiesCollection.standard,
      })
      const tokenList = await NFTActor.getUserTokens(identity.getPrincipal())
      let nftList = {
        nfts: tokenList.map((obj) => {
          return { url: obj.url }
        }),
      }
      console.log('Token list', tokenList)

      let nftsString = JSON.stringify(nftList)
      sendMessage(
        'ExternalJSCommunication',
        'ProvidePrincipalId',
        identity.getPrincipal().toText(),
      )
      sendMessage('ExternalJSCommunication', 'ProvideNFTs', nftsString)
      setGotNFTs(true)
      // } else {
      //   alert('You are not whitelisted to use this app')
      //   StoicIdentity.disconnect();
      // }
    })
  }

  const getNFTs = async function () {
    if (
      typeof window.ic === 'undefined' ||
      typeof window.ic.plug === 'undefined'
    ) {
      throw new Error(
        'We cannot detect a Plug Wallet in your browser extensions',
      )
    }

    // const HOST ="https://ic0.app";
    const whitelist = ['rw7qm-eiaaa-aaaak-aaiqq-cai']
    var canisterId = 'rw7qm-eiaaa-aaaak-aaiqq-cai'
    const host = 'https://mainnet.dfinity.network'

    await window.ic.plug.requestConnect({
      whitelist,
      host,
    })

    if (!window.ic?.plug?.agent) {
      console.warn('no agent found')
      const result = await window.ic?.plug?.createAgent({
        whitelist,
        host,
      })
      result
        ? console.log('agent created')
        : console.warn('agent creation failed')
    }
    const principal = await window.ic.plug.agent.getPrincipal()
    // if (principalWhitelist.some((e) => e === principal.toText())) {
    //get NFT info for canisterId
    const kittiesCollection = await getNFTInfo({
      nftCanisterId: canisterId,
      agent: window.ic.plug.agent,
    })
    console.log('Token list', kittiesCollection)

    const NFTActor = getNFTActor({
      canisterId: kittiesCollection.principal_id.toString(),
      agent: window.ic.plug.agent,
      standard: kittiesCollection.standard,
    })

    //get token list for principal
    const tokenList = await NFTActor.getUserTokens(principal)
    console.log({ principalId: principal.toText() })
    //create nft list
    let nftList = {
      nfts: tokenList.map((obj) => {
        return { url: obj.url }
      }),
    }
    console.log('Token list', tokenList)

    let nftsString = JSON.stringify(nftList)
    sendMessage(
      'ExternalJSCommunication',
      'ProvidePrincipalId',
      principal.toText(),
    )
    sendMessage('ExternalJSCommunication', 'ProvideNFTs', nftsString)
    setGotNFTs(true)
    //   } else {
    //   alert('You are not whitelisted to use this app');
    //   window.ic.plug.disconnect();
    // }
  }

  useEffect(() => {
    addEventListener('ConnectWallet', () => {
      console.log('Ready to connect wallet...')
      setReadyToConnect(true)
    })
  })

  return (
    <>
      {isLoaded === false && (
        <div className="loading-overlay background-image">
          <div class="divTable loader">
            <div className="plug-button">
              <button>
                <div>
                  <span class="stoic-span">
                    Loading... ({loadingPercentage}%)
                  </span>
                </div>
              </button>
            </div>
          </div>
        </div>
      )}
      {readyToConnect && !connected && (
        <div class="divTable wallet-buttons">
          <div class="divTableBody">
            <div class="divTableRow">
              <div class="divTableCell">
                <div className="plug-button">
                  <PlugConnect
                    whitelist={['rw7qm-eiaaa-aaaak-aaiqq-cai']}
                    onConnectCallback={() => {
                      sendMessage(
                        'ExternalJSCommunication',
                        'OnWalletConnected',
                      )
                      setConnected(true)
                      getNFTs()
                    }}
                  />
                </div>
              </div>
              <div class="divTableCell">
                <div className="plug-button">
                  <button class="c-ipvTzC" onClick={connectStoic}>
                    <div>
                      <img
                        class="stoic"
                        src={require('./stoic.png')}
                        alt="stoic"
                      />
                      <span class="stoic-span">Connect to Stoic</span>
                    </div>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
      <Unity
        className={'unity-wrapper'}
        unityProvider={unityProvider}
        devicePixelRatio={devicePixelRatio}
      />
    </>
  )
}

export default App
