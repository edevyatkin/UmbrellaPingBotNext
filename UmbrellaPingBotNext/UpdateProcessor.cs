using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UmbrellaPingBotNext.Rules;

namespace UmbrellaPingBotNext
{
    internal class UpdateProcessor
    {
        private const string _rulesNamespace = "UmbrellaPingBotNext.Rules";
        private static Dictionary<string, IUpdateRule> _rules = new Dictionary<string, IUpdateRule>();

        static UpdateProcessor() {
            try {
                Console.WriteLine("Loading rules...");
                Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => string.Equals(t.Namespace, _rulesNamespace, StringComparison.Ordinal) && t.IsClass)
                    .ToArray();
                Array.ForEach(types, t => _rules.Add(t.FullName, (IUpdateRule)Activator.CreateInstance(t)));
                Console.WriteLine("Rules loaded!");
            }
            catch (Exception e) {
                throw e;
            }
        }

        internal static void Process(Update update) {
            if (_rules.Count == 0)
                return;
            foreach (IUpdateRule rule in _rules.Values) {
                if (rule.IsMatch(update)) {
                    rule.Process(update);
                }
            }
        }

        public static T GetRule<T>() => (T)_rules[typeof(T).FullName];
    }
}
