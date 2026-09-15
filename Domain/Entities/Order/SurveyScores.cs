using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Domain.ValueObjects;

public class SurveyScore : ValueObject
{
    public int Id { get; private set; }
    public string Title { get; private set; }

    private SurveyScore(string title, int id)
    {
        Title = title;
        Id = id;
    }

    // Predefined scores
    public static SurveyScore VeryPoor => new("خیلی ضعیف", 1);
    public static SurveyScore Poor => new("ضعیف", 2);
    public static SurveyScore Average => new("متوسط", 3);
    public static SurveyScore Good => new("خوب", 4);
    public static SurveyScore Excellent => new("عالی", 5);

    // Collection
    public static IEnumerable<SurveyScore> Items => new[]
    {
        VeryPoor, Poor, Average, Good, Excellent
    };

    // Get item by title
    public static SurveyScore GetItem(string title)
    {
        return Items.FirstOrDefault(x => x.Title == title)
               ?? throw new ArgumentException($"Invalid title: {title}");
    }

    // Get ID from title
    public static int FindId(string title)
    {
        return GetItem(title).Id;
    }

    // Get item by Id
    public static SurveyScore FromId(int id)
    {
        return Items.FirstOrDefault(x => x.Id == id)
               ?? throw new ArgumentException($"Invalid score id: {id}");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Title;
    }
}
