using DevionGames.UIWidgets;
using UnityEngine;
using UnityEngine.Events;

namespace DevionGames.QuestSystem
{
    public static class NotificationExtension
    {
        public static void Show(this NotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (QuestManager.UI.dialogBox != null)
            {
                QuestManager.UI.dialogBox.Show(options, result, buttons);
            }
        }

        public static void Show(this NotificationOptions options, params string[] replacements)
        {
            if (QuestManager.UI.notification != null)
            {
                QuestManager.UI.notification.AddItem(options, replacements);
            }
        }
    }
}

namespace DevionGames.QuestSystem.Configuration
{
    [System.Serializable]
    public class Notifications : Settings
    {

        public override string Name
        {
            get
            {
                return "Notification";
            }
        }
        [Header("Trigger:")]
        public NotificationOptions toFarAway = new NotificationOptions()
        {
            text = "This is to far away!"
        };
        public NotificationOptions inUse = new NotificationOptions()
        {
            text = "My life is already fairly busy."
        };

        [Header("Quest:")]
        public NotificationOptions questCompleted = new NotificationOptions()
        {
            text = "{0} completed."
        };
        public NotificationOptions questFailed = new NotificationOptions()
        {
            text = "{0} failed."
        };

        public NotificationOptions taskCompleted = new NotificationOptions()
        {
            text = "{0} completed."
        };
        public NotificationOptions taskFailed = new NotificationOptions()
        {
            text = "{0} failed."
        };
        public NotificationOptions cancelQuest= new NotificationOptions()
        {
            title = "Cancel Quest",
            text = "Are you sure you want to cancel the quest?"
        };

    }
}