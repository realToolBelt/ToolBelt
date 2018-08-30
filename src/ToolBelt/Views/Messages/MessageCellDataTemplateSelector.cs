using ToolBelt.Models;
using Xamarin.Forms;

namespace ToolBelt.Views.Messages
{
    public class MessageCellDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _incomingDataTemplate;

        private readonly DataTemplate _outgoingDataTemplate;

        public MessageCellDataTemplateSelector()
        {
            // Retain instances!
            this._incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this._outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ChatMessage messageVm)
            {
                return messageVm.IsIncoming ? this._incomingDataTemplate : this._outgoingDataTemplate;
            }

            return null;
        }
    }
}
