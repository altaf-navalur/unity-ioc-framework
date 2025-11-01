using XcelerateGames.Timer;
using XcelerateGames.FlutterWidget;
using XcelerateGames.UpdateManager;
using XcelerateGames.Tutorials;
using XcelerateGames.Socket;
using JungleeGames.Analytics;
using XcelerateGames.Compliance;
#if VIDEO_ENABLED
using XcelerateGames.Video;
#endif //VIDEO_ENABLED

#if FB_ENABLED
using XcelerateGames.Social.Facebook;
#endif

namespace XcelerateGames.IOC
{
    public class FrameworkBindings : BindingManager
    {
#if UMBRELLA
        protected override void Awake()
        {
            if (mFrameworkBindings == null)
                base.Awake();
            else
                Instance = mFrameworkBindings;
        }
#endif //UMBRELLA

        protected override void SetBindings()
        {
            base.SetBindings();

            #region Framework
            BindSignal<SigLoadAssetFromBundle>();
            BindSignal<SigEngineReady>();
            BindSignal<SigFrameworkInited>();
            BindSignal<SigSceneReady>();
            #endregion

            #region Flutter
            BindSignal<SigOnFlutterMessage>();
            BindSignal<SigSendMessageToFlutter>();
            BindSignal<SigSendLogToFlutter>();
            #endregion

            #region Miscellaneous
            BindSignal<SigGetVersionListHash>();
            BindSignal<SigAppResumedFromBackground>();
            BindSignal<SigShowSettings>();
            BindSignal<SigSettingsHidden>();
            BindSignal<SigCloseGameTable>();
            #endregion Miscellaneous

            #region Update Manager
            BindSignal<SigAssetUpdated>();
            BindSignal<SigCheckForAssetUpdate>();
            #endregion Update Manager

            #region Timer
            BindSignal<SigTimerRegister>();
            BindSignal<SigTimerUnregister>();
            BindSignal<SigTimerUnregisterAll>();
            #endregion Timer

            #region TutorialManager
            BindSignal<SigPlayTutorial>();
            BindSignal<SigStopTutorial>();
            BindSignal<SigPauseTutorial>();
            BindSignal<SigSetTutorialStep>();
            BindSignal<SigOnTutorialStep>();
            BindSignal<SigOnTutorialStepSetupDone>();
            BindSignal<SigTutorialStarted>();
            BindSignal<SigTutorialComplete>();
            BindSignal<SigGoToNextStep>();
            BindSignal<SigOnHandTap>();
            BindSignal<SigShowMask>();
            BindSignal<SigDestroyTutorials>();

            BindModel<TutorialModel>();
            #endregion TutorialManager

            #region Toast
            BindSignal<SigShowToast>();
            BindSignal<SigHideToast>();
            #endregion Toast

            #region Debug
            BindSignal<SigSendMailLog>();
            #endregion Debug

            #region ServerConnectionMonitor
            //BindSignal<SigServerConnectionClose>();
            BindSignal<SigOnGetPong>();
            BindSignal<SigOnPingCountUpdate>();
            BindSignal<SigOnDisconnection>();
            BindSignal<SigOnReconnect>();
            //BindSignal<SigGameQuit>();
            BindSignal<SigOnPingSend>();
            // BindSignal<SigOnSocketDisconnected>();
            #endregion

            #region Socket Related
#if USE_NATIVE_WEBSOCKET || USE_WEBSOCKET_SHARP
            BindSignal<SigConnectSocket>();
            BindSignal<SigSocketConnectionStatus>();
            BindSignal<SigDisconnectSocket>();
            BindSignal<SigOnSocketMesageReceived>();
            BindSignal<SigSendSocketMesage>();
            BindSignal<SigOnSocketMessageErrorReceived>();
            BindSignal<SigClearSocketIncomingMessage>();
            BindModel<WebSocketModel>();

            BindSignal<SigConnectSocketV2>();
            BindSignal<SigSocketConnectionStatusV2>();
            BindSignal<SigSendSocketMessageV2>();
            BindSignal<SigDisconnectSocketV2>();
            BindSignal<SigDisconnectAllSockets>();
            BindSignal<SigOnMaxConnectionRetryExceededV2>();
#endif
            #endregion

            #region Miscellaneous
            BindSignal<SigVibrate>();
            BindSignal<SigVibratePattern>();
            BindSignal<SigUploadDeviceLogs>();
            #endregion Miscellaneous

#if BACKTRACE_ENABLED
            BindSignal<SigAddBacktraceAttribute>();
#endif

#if FB_ENABLED
            BindSignal<SigFacebookLogin>();
            BindSignal<SigFacebookLogout>();
            BindSignal<SigGetCurrencyInfo>();
            BindSignal<SigGetUserData>();
            BindSignal<SigGetFriends>();
            BindSignal<SigInviteFriends>();
            BindSignal<SigGetInvitableFriends>();
            BindSignal<SigGetAppLink>();

            BindSignal<SigOnLogin>();
            BindSignal<SigOnShare>();
            BindSignal<SigOnGetInvitableFriends>();
            BindSignal<SigOnGetFriends>();
            BindSignal<SigOnSendRequest>();
            BindSignal<SigOnInviteFriends>();
            BindSignal<SigOnInviteFriend>();
            BindSignal<SigOnGetUserData>();
            BindSignal<SigOnUserPictureLoaded>();


            BindModel<FacebookModel>();
#endif
            #region Events
            BindSignal<SigSendFrameworkEvent>();
            #endregion Events

            #region ClickStream
            BindModel<ClickstreamDataModel>();
            #endregion ClickStream

            #region ZooKeeper
            BindSignal<SigFetchZooKeeperConfig>();
            BindSignal<SigZooKeeperConfigFetched>();
            #endregion ZooKeeper

            #region IAP
#if USE_IAP
            BindSignal<XcelerateGames.Purchasing.SigInitPurchaseManager>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseItem>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseStarted>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseCompleted>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseFailed>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseCancelled>();
            BindSignal<XcelerateGames.Purchasing.SigRestorePurchases>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseInitFailed>();
            BindSignal<XcelerateGames.Purchasing.SigIAPSubscriptionInfo>();
            BindSignal<XcelerateGames.Purchasing.SigPurchaseManagerReady>();
            BindSignal<XcelerateGames.Purchasing.SigCancelSubscription>();
            BindSignal<XcelerateGames.Purchasing.SigDoPurchaseVerification>();
            BindSignal<XcelerateGames.Purchasing.SigSetPurchaseVerification>();

            BindModel<XcelerateGames.Purchasing.IAPStoreModel>();
#endif //USE_IAP
            #region Video Player
#if VIDEO_ENABLED
            BindSignal<SigPlayVideo>();
            BindSignal<SigVideoPlayerCreated>();
            BindSignal<SigVideoPlayerClosed>();
            BindSignal<SigVideoPlaybackStarted>();
            BindSignal<SigVideoPlayerComplete>();
#endif //VIDEO_ENABLED
            #endregion Video Player
            #endregion IAP
            BindModel<MiscellaneousModel>();
            BindSignal<SigSendBreadcrumbEvent>();

            BindSignal<SigAssetBundleDownloaderState>();
            #region Connection Related
            BindSignal<SigShowNetworkStrength>();
            BindSignal<SigShowNetworkSpeed>();
            BindSignal<SigInitNetworkSpeed>();
            #endregion Connection Related

            #region Compliance
            BindSignal<SigInitTNOGAComplianceTimer>();
            BindSignal<SigResetTNOGAComplianceTimer>();
            BindSignal<SigStartTNOGAComplianceTimer>();
            BindSignal<SigPauseTNOGAComplianceTimer>();
            BindSignal<SigOnIntervalReachedTNOGA>();
#if !LIVE_BUILD
            BindSignal<SigOnTimerTickTNOGA>();
#endif
            #endregion Compliance

            #region Coralogix
#if CORALOGIX_REMOTE_LOGGING
            BindSignal<SigCoralogixInit>();
#endif //CORALOGIX_REMOTE_LOGGING
            #endregion Coralogix
        }

        protected override void SetFlow()
        {
            base.SetFlow();
            On<SigEngineReady>().Do<CmdInitFirebase>().Once();
            On<SigFrameworkInited>().Do<CmdInitTutorialManager>();
            On<SigLoadAssetFromBundle>().Do<CmdLoadAssetFromBundle>();

            #region Miscellaneous
            On<SigVibrate>().Do<CmdVibrate>();
            On<SigVibratePattern>().Do<CmdVibratePattern>();
            #endregion Miscellaneous

            #region Events
            On<SigSendFrameworkEvent>().Do<CmdSendFrameworkEvent>();
            #endregion Events

            #region Video Player
#if VIDEO_ENABLED
            On<SigPlayVideo>().Do<CmdPlayVideo>();
#endif //VIDEO_ENABLED
            #endregion Video Player
        }
    }
}
