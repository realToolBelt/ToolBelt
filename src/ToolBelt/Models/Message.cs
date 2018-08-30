using System;

namespace ToolBelt.Models
{
    public class ChatMessage : MessageBase
    {
        public ChatMessage(string text, DateTime createDate, string from, bool isIncoming) : base(text, createDate)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            IsIncoming = isIncoming;
        }

        public string From { get; }

        public bool IsIncoming { get; }
    }

    public abstract class MessageBase
    {
        public MessageBase(string text, DateTime createDate)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            CreateDate = createDate;
        }

        public DateTime CreateDate { get; }

        public string Text { get; }
    }
}
