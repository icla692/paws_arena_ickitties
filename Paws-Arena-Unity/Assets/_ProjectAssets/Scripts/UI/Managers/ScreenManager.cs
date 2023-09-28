using System.Collections.Generic;
using System.Linq;
using Anura.Templates.MonoSingleton;
using Anura.Templates.UIAbstractions.BaseScripts;
using UnityEngine;

namespace Anura.UI.Managers
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        private readonly Dictionary<object, BaseScreen> screens = new Dictionary<object, BaseScreen>();

        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private BaseScreen currentBaseScreen;

        protected override void Awake()
        {
            Initialization();
        }

        public BaseScreen GetScreen(object screenType)
        {
            var screen = screens.First(it => it.Key == screenType);
            return screen.Value;
        }

        public void SwitchScreen(object screenType, bool refreshScreen = true)
        {
            var lastScreen = currentBaseScreen;
            currentBaseScreen = GetScreen(screenType);

            if(refreshScreen)
            {
                lastScreen.RefreshScreen();
            }
            lastScreen.DisableScreen();

            currentBaseScreen.EnableScreen();
        }

        protected void LoadScreens()
        {
            foreach (var screen in rootCanvas.GetComponentsInChildren<BaseScreen>(true))
            {
                screens.Add(screen.GetType(), screen);
            }
        }

        private void Initialization()
        {
            LoadScreens();
            currentBaseScreen.EnableScreen();
        }
    }
}
