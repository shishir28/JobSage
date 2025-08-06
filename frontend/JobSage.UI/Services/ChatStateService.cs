using System;

namespace JobSage.UI.Services
{
    public interface IChatStateService
    {
        string? GetConversationId(Guid jobId);
        void SetConversationId(Guid jobId, string conversationId);
        void ClearConversationId(Guid jobId);
    }

    public class ChatStateService : IChatStateService
    {
        private readonly Dictionary<Guid, string> _conversationIds = new();

        public string? GetConversationId(Guid jobId)
        {
            return _conversationIds.TryGetValue(jobId, out var conversationId) ? conversationId : null;
        }

        public void SetConversationId(Guid jobId, string conversationId)
        {
            _conversationIds[jobId] = conversationId;
        }

        public void ClearConversationId(Guid jobId)
        {
            _conversationIds.Remove(jobId);
        }
    }
}
