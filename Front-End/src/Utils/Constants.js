export let isDev = false;

let prodUrl = "https://colorfulcoding-portofolio.s3.eu-central-1.amazonaws.com/nftcat/";
let stagingUrl = "https://colorfulcoding-portofolio.s3.eu-central-1.amazonaws.com/nftcat-staging/";

export function GetUnityBaseUrl(){
    return isDev ? stagingUrl : prodUrl;
}