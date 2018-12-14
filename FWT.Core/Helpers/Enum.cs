namespace FWTL.Core.Helpers
{
    public class Enum
    {
        public enum SortDirection
        {
            DESC = 0,
            ASC = 1,
        }

        public enum PageSize
        {
            p10 = 10,
            p25 = 25,
            p50 = 50,
            p100 = 100
        }

        public enum PeerType
        {
            User = 1,
            Chat = 2,
            Channal = 3
        }

        public enum TelegramMediaType
        {
            Unknown = 0,
            MediaContact = 1,
            Venue = 2,
            Invoice = 3,
            Geo = 4,
            Photo = 5,
            GeoLive = 6,
            WebPage = 7,
            Document = 8,
        }

        public enum TelegramEntity
        {
            Unknown = 0,
            Mention = 1,
            Hashtag = 2,
            BotCommand = 3,
            Url = 4,
            Email = 5,
            Bold = 6,
            Italic = 7,
            Code = 8,
            Pre = 9,
            TextUrl = 10,
            MentionName = 11,
            CashTag = 12,
            Phone = 13,
        }

        public enum TelegramMessageAction
        {
            Plain = 0,
            ChatCreate = 1,
            ChatEditTitle = 2,
            ChatEditPhoto = 3,
            ChatDeletePhoto = 4,
            ChatAddUser = 5,
            ChatDeleteUser = 6,
            ChatJoinedByLink = 7,
            ChannelCreate = 8,
            ChatMigrateTo = 9,
            MigrateFrom = 10,
            PinMessage = 11,
            HistoryClear = 12,
            GameScore = 13,
            PaymentSentMe = 14,
            PaymentSent = 15,
            PhoneCall = 16,
            ScreenshotTaken = 17,
            ActionCustomAction = 18,
            BotAllowed = 19,
            SecureValuesSent = 20,
            Empty = 21,
        }

        public enum JobState
        {
            Failed = 0,
            Fetching = 1,
            Done = 99
        }
    }
}