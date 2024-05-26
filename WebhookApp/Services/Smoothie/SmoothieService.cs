using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace WebhookApp.Services.Smoothie;

// Алгоритм
/*
 * 1. Формирование общего списка возможных комбинаций (набор) (3125 шт.)
 * 2. Выбор случайной комбинации из набора (кандидат) для поиска необходимой (цель).
 * 3. Проверка кандидата, получение одного из 5 результатов:
 *  -- плохой (0,1 совпадений)
 *  -- нормальный (2)
 *  -- хороший (3)
 *  -- отличный (4)
 *  -- лучший (5)
 * 4. В случае результата "лучший" прекращение работы и возврат кандидата.
 * 5. В остальных случаях фильтрация.
 * 5.1. Поиск всех комбинаций в наборе, где количество совпадений с кандидатом больше или равно минимуму
 *      и меньше или равно максимуму.
 * 5.2. Исключение кандидата из набора, если он не единственный, а, следовательно, является целью.
 * 5.3. Замена набора найденными комбинациями.
 * 
 */

public class SmoothieService : ISmoothieService {
    private  List<Smoothie> _allSmoothie;
    public int ElapsedCombinations => _allSmoothie.Count;
    public Smoothie BestSmoothie { get; private set; } = Smoothie.Empty;
    public SmoothieStatus BestSmoothieStatus { get; private set; } = SmoothieStatus.Pending;
    public string BestSmoothieDescription { get; private set; } = string.Empty;


    public SmoothieService() {
        _allSmoothie = new List<Smoothie>();
        GenerateCombinations(Smoothie.Empty, 0);
    }

    private void GenerateCombinations(Smoothie smoothie, int pos) {
        if (pos == 5) {
            var ning = new int[5];
            Array.Copy(smoothie.Ingredients, ning, 5);
            var sm = new Smoothie(ning);
            _allSmoothie.Add(sm);
            return;
        }
        for (int ingredient = 0; ingredient < 5; ingredient++) {
            smoothie.Ingredients[pos] = ingredient;
            GenerateCombinations(smoothie, pos + 1);
        }
    }

    public void Filter(Smoothie candidate, SmoothieStatus status) {
        if (BestSmoothieStatus == SmoothieStatus.Best)
            return;

        switch (status) {
            case SmoothieStatus.Best:
                FilterSmoothie(candidate,5,5);
                break;
            case SmoothieStatus.Excellent:
                FilterSmoothie(candidate,4,4);
                break;
            case SmoothieStatus.Good:
                FilterSmoothie(candidate, 3,3);
                break;
            case SmoothieStatus.Normal:
                FilterSmoothie(candidate, 2,2);
                break;
            case SmoothieStatus.Poor:
                FilterSmoothie(candidate, 0,1);
                break;
            case SmoothieStatus.Pending:
                throw new Exception("Smoothie hasn't been checked");
        }

        if (BestSmoothieStatus > status) {
            BestSmoothie = candidate;
            BestSmoothieStatus = status;
            BestSmoothieDescription = BestSmoothie.Description;
        }
    }

    public Smoothie Peek() {
        int index = RandomNumberGenerator.GetInt32(_allSmoothie.Count);
        return _allSmoothie[index];
    }
    
    public List<Smoothie> Peek(int count) {
        if (count >= _allSmoothie.Count)
            return _allSmoothie;
        var selected = new List<Smoothie>();
        while (count > 0)
        {
            int index = RandomNumberGenerator.GetInt32(_allSmoothie.Count);
            if (selected.Contains(_allSmoothie[index])) 
                continue;
            selected.Add(_allSmoothie[index]);
            count--;
        }
        return selected;
    }

    public void Reset() {
        _allSmoothie.Clear();
        GenerateCombinations(Smoothie.Empty, 0);
        BestSmoothie = Smoothie.Empty;
        BestSmoothieStatus = SmoothieStatus.Pending;
        BestSmoothieDescription = string.Empty;
    }

    private void FilterSmoothie(Smoothie candidate, int min, int max) {
        var newAllSmoothie = new List<Smoothie>();
        foreach (var s in _allSmoothie) {
            int matches = 0;
            for (int i = 0; i < 5; i++) {
                if (s.Ingredients[i] == candidate.Ingredients[i])
                    matches++;
            }
            if (matches >= min && matches <= max)
                newAllSmoothie.Add(s);
        }
        if (newAllSmoothie.Count > 1)
            newAllSmoothie.Remove(candidate);
        _allSmoothie = newAllSmoothie;
    }
}
