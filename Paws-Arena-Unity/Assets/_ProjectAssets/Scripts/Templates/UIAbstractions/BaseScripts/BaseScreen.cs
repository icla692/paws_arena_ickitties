using Anura.Templates.UIAbstractions.Interfaces;
using UnityEngine;

namespace Anura.Templates.UIAbstractions.BaseScripts
{
    public class BaseScreen : MonoBehaviour, IScreen
    {
        public virtual void EnableScreen()
        {
            gameObject.SetActive(true);
        }

        public virtual void RefreshScreen()
        {

        }

        public virtual void DisableScreen()
        {
            gameObject.SetActive(false);
        }
    }
}
