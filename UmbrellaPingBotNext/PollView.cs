using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace UmbrellaPingBotNext
{
    internal class PollView
    {
        private const string NoUsers = "Еще никто не прожался";
        private const string PinIsPressed = "🗡Я прожался!";
        private const string Sleep = "😴Сплю";
        private const string Attacking = "⚔️Атакующие";
        private const string Defending = "🛡Защищающие";
        private const string AttackingCallback = "Атакуем";
        private const string DefendingCallback = "Защищаем";

        protected Poll _poll;

        public string Text { get; }
        public InlineKeyboardMarkup ReplyMarkup { get; }
        public string ActiveCallbackQueryAnswer => CreateActiveCallbackQueryAnswer();
        public string SleepCallbackQueryAnswer => CreateSleepCallbackQueryAnswer();

        public PollView(Poll poll) {
            _poll = poll;
            Text = CreateText();
            ReplyMarkup = CreateReplyMarkup();
        }

        private string CreateText() {
            return $"{CreateTitle()}\n\n{CreateUserList()}";
        }

        internal virtual string CreateTitle() {
            string nextBattleText = $"👊<b>Битва в {_poll.Pin.BattleHour}:00 МСК</b>";

            var chatId = _poll.Pin.ChatId.ToString().Substring(4);
            var messageId = _poll.Pin.MessageId;
            var pressPinText = $"Прожимаемся в 📌<a href='https://t.me/c/{chatId}/{messageId}'>пин</a>";
            
            return $"{nextBattleText}\n\n{pressPinText}";
        }

        private string CreateUserList() {
            if (_poll.Votes.Count == 0)
                return $"<i>{NoUsers}</i>";

            var userListTitle = $"<b>{(_poll.Pin.IsAttack() ? Attacking : Defending)}</b> " +
                                $"({_poll.Votes.Where(u => u.Status == PollUserStatus.Active).ToList().Count})";
            var userList = new StringBuilder();
            foreach (var user in _poll.Votes.Where(u => u.Status == PollUserStatus.Active))
                userList.Append($" ➥ {user}\n");
            userList.Append('\n');
            foreach (var user in _poll.Votes.Where(u => u.Status == PollUserStatus.Sleep))
                userList.Append($" 😴 {user}\n");

            return $"{userListTitle}\n{userList}";
        }

        private InlineKeyboardMarkup CreateReplyMarkup() {
            var pinButton = new InlineKeyboardButton() {
                CallbackData = "pin_is_pressed",
                Text = PinIsPressed
            };

            var sleepButton = new InlineKeyboardButton() {
                CallbackData = "sleep_is_pressed",
                Text = Sleep
            };

            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>() { pinButton, sleepButton });
        }

        private string CreateActiveCallbackQueryAnswer() => $"{(_poll.Pin.IsAttack() ? AttackingCallback : DefendingCallback)} {_poll.Pin.Company} !";

        private string CreateSleepCallbackQueryAnswer() => "Спокойных снов...";
    }
}