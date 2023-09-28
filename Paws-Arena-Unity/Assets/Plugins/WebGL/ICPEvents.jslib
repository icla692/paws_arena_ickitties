mergeInto(LibraryManager.library, {
  ConnectWallet: function () {
    window.dispatchReactUnityEvent("ConnectWallet");
  },
});