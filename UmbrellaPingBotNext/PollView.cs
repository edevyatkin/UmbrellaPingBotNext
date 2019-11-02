using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace UmbrellaPingBotNext
{
    internal class PollView
    {
        private const string NoUsers = "Еще никто не прожался";
        private const string PinIsPressed = "Я прожался!";
        private const string Attacking = "⚔️Атакующие";
        private const string Defending = "🛡Защищающие";
        private const string AttackingCallback = "Атакуем";
        private const string DefendingCallback = "Защищаем";

        private Poll _poll;
        private string _text;
        private InlineKeyboardMarkup _replyMarkup;

        public string Text => _text;
        public InlineKeyboardMarkup ReplyMarkup => _replyMarkup;
        public string CallbackQueryAnswer => CreateCallbackQueryAnswer();

        public PollView(Poll poll) {
            _poll = poll;
            _text = CreateText();
            _replyMarkup = CreateReplyMarkup();
        }

        private string CreateText() {
            return $"{CreateTitle()}\n\n{CreateUserList()}";
        }

        private string CreateTitle() {
            var chatId = _poll.Pin.ChatId.ToString().Substring(4);
            var messageId = _poll.Pin.MessageId;
            return $"👊<b>Прожимаемся в</b> <a href='https://t.me/c/{chatId}/{messageId}'>пин</a>";
        }

        private string CreateUserList() {
            if (_poll.UsersPressedPin.Count == 0)
                return $"<i>{NoUsers}</i>";

            var userListTitle = $"{(_poll.Pin.IsAttack() ? Attacking : Defending)} " +
                                $"({_poll.UsersPressedPin.Count})";
            var userList = string.Join("\n ", _poll.UsersPressedPin);
            return $"{userListTitle}\n\n{userList}";
        }

        private InlineKeyboardMarkup CreateReplyMarkup() {
            var button = new InlineKeyboardButton();
            button.CallbackData = "pin_is_pressed";
            button.Text = PinIsPressed;
            return new InlineKeyboardMarkup(button);
        }

        private string CreateCallbackQueryAnswer() {
            return $"{(_poll.Pin.IsAttack() ? AttackingCallback : DefendingCallback)} {_poll.Pin.Company} !";
        }
    }
}