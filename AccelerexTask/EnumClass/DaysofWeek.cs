namespace AccelerexTask.EnumClass;

public class DaysofWeek
{
    public static string WeekDays(int value)
    {
        return value switch
        {
            1 => "Tuesday",
            2 => "Wednesday",
            3 => "Thursday",
            4 => "Friday",
            5 => "Saturday",
            6 => "Sunday",
            _ => "Monday",
        };
    }
}
