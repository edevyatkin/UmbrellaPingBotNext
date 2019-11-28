﻿using System;
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
        private const string Attacking = "Атакующие";
        private const string Defending = "Защищающие";
        private const string AttackingCallback = "Атакуем";
        private const string DefendingCallback = "Защищаем";

        protected Poll _poll;

        public string Text { get; }
        public InlineKeyboardMarkup ReplyMarkup { get; }
        public string ActiveCallbackQueryAnswer => $"{(_poll.Pin.IsAttack() ? AttackingCallback : DefendingCallback)} {_poll.Pin.Company} !";
        public string SleepCallbackQueryAnswer => "Спокойных снов...";

        public PollView(Poll poll) {
            _poll = poll;
            Text = CreateText();
            ReplyMarkup = CreateReplyMarkup();
        }

        private string CreateText() {
            return $"{CreateTitle()}\n\n{CreateUserList()}";
        }

        internal virtual string CreateTitle() {
            string nextBattleText = $"👊 <b>Битва в {_poll.Pin.BattleHour}:00 МСК</b>";

            var chatId = _poll.Pin.ChatId.ToString().Substring(4);
            var messageId = _poll.Pin.MessageId;
            var pressPinText = $"{_poll.Pin.Type}{_poll.Pin.Company.Logo} Прожимаемся в 📌<a href='https://t.me/c/{chatId}/{messageId}'>пин</a>";
            
            return $"{nextBattleText}\n\n{pressPinText}";
        }

        private string CreateUserList() {
            if (_poll.Votes.Count == 0)
                return $"<i>{NoUsers}</i>";

            var activeVotes = GetVotesByType(VoteType.Active, v => $" ➥ {v.DisplayName}");
            var sleepVotes = GetVotesByType(VoteType.Sleep, v => $" 😴 {v.DisplayName}");

            var userListTitle = GetUserListTitle(activeVotes);

            var userList = new StringBuilder()
                .AppendJoin('\n', activeVotes)
                .Append('\n')
                .AppendJoin('\n', sleepVotes);

            return $"{userListTitle}\n{userList.ToString()}";
        }

        private IEnumerable<string> GetVotesByType(VoteType type, Func<Vote, string> voteFormatter) => 
            _poll.Votes
                .Where(v => v.Type == type)
                .Select(voteFormatter);

        private string GetUserListTitle(IEnumerable<string> activeVotes) =>
            $"<b>{(_poll.Pin.IsAttack() ? Attacking : Defending)}</b> ({activeVotes.ToList().Count}) <b>:</b>";


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
    }
}
