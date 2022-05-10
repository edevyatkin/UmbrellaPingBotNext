using System.Collections.Generic;
using System.Linq;

namespace WebhookApp.Services.Smoothie;

public class Smoothie {
    public static readonly Smoothie Empty = new();
    public int[] Ingredients { get; }
    private static Dictionary<int,string> _numbersIngredient = new Dictionary<int, string>() {
        [0] = "🍋",
        [1] = "🍇",
        [2] = "🍏",
        [3] = "🥕",
        [4] = "🍅",
    };

    private Smoothie() {
        Ingredients = new int[5];
    }

    public Smoothie(int[] ingredients) {
        Ingredients = ingredients;
    }

    public string ToStringDebug() {
        return $"[{string.Join(' ', Ingredients)}]";
    }

    public override string ToString() {
        return Ingredients.Aggregate(string.Empty, (acc,i) => acc + _numbersIngredient[i]);
    }
}
