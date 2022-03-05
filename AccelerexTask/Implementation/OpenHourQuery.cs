namespace AccelerexTask.Implementation;

public static class OpenHourQuery
{
    public class Command : IRequest<Dictionary<string, string>>
    { 
        public ICollection<OpenHours> Monday { get; set; }
        public ICollection<OpenHours> Tuesday { get; set; }
        public ICollection<OpenHours> Wednesday { get; set; }
        public ICollection<OpenHours> Thursday { get; set; }
        public ICollection<OpenHours> Friday { get; set; }
        public ICollection<OpenHours> Saturday { get; set; }
        public ICollection<OpenHours> Sunday { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, Dictionary<string, string>>
    {
        public async Task<Dictionary<string, string>> Handle(Command request, CancellationToken cancellationToken)
        {
            Dictionary<string, string> output = new();
            string? getTime = string.Empty;

            List<ICollection<OpenHours>> queries = new()
            {
                request.Monday,
                request.Tuesday,
                request.Wednesday,
                request.Thursday,
                request.Friday,
                request.Saturday,
                request.Sunday
            };

            var sortQueries = SortJsonInput(queries);

            return await Task.Run(() =>
            {
                for (int i = 0; i < sortQueries.Count; i++)
                {
                    List<OpenHours?> getOpenHours = sortQueries[i].ToList();
                    getTime = (getOpenHours.Count != 0) 
                        ? PrintMultipleOpening(getOpenHours)
                        : "Closed";
                    output.Add(DaysofWeek.WeekDays(i), getTime);
                }
                return output;
            }, cancellationToken);
        }
        private static string PrintMultipleOpening(List<OpenHours?> openHours)
        {
            string timeOpened = string.Empty;
            for (int i = 0; i < openHours.Count - 1; i++)
            {
                timeOpened += TimeConverter(openHours[i].Value, openHours[i + 1].Value);
                timeOpened += (i + 1 != openHours.Count - 1) ? ", " : string.Empty;
                i += 1;
            }
            return timeOpened;
        }

        private static List<ICollection<OpenHours>> SortJsonInput(List<ICollection<OpenHours>> jsonInput)
        {
            for (int i = 0; i < jsonInput.Count; i++)
            {
                OpenHours? firstClose = jsonInput[i].FirstOrDefault();
                if (string.Equals(firstClose?.Type, OpeningHour.close.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    jsonInput[i].Remove(firstClose);
                    if (i == 0)
                    {
                        jsonInput[^1].Add(firstClose);
                        continue;
                    }
                    jsonInput[i - 1].Add(firstClose);
                }
            }
            return jsonInput;
        }

        private static string TimeConverter(uint openTime, uint closeTime)
        {
            uint oPenTimeModulus = openTime % 3600;
            uint closeTimeModulus = closeTime % 3600;

            uint openTimeHour =  (openTime - oPenTimeModulus) / 3600;
            uint closeTimeHour = (closeTime - closeTimeModulus) / 3600;

            decimal openTimeMin = Math.Floor((decimal)oPenTimeModulus / 60);
            decimal closeTimeMin = Math.Floor((decimal)closeTimeModulus / 60);

            /*uint openTimeSec = (oPenTimeModulus % 60);
            uint closeTimeSec = (closeTimeModulus % 60);*/

            string openTimeFilter = (openTimeHour > 11 && openTimeHour < 23 ) 
                ? TimeFilter.PM.ToString() 
                : TimeFilter.AM.ToString();

            string closeTimeFilter = (closeTimeHour > 11 && closeTimeHour < 23) 
                ? TimeFilter.PM.ToString() 
                : TimeFilter.AM.ToString();

            return $"{(openTimeHour > 12 ? openTimeHour - 12 : openTimeHour) }" +
                $":{(openTimeMin.ToString().Length != 2 ? string.Concat("0", openTimeMin) : openTimeMin)} {openTimeFilter} - " +
                $"{(closeTimeHour > 12 ? closeTimeHour - 12 : closeTimeHour)}" +
                $":{(closeTimeMin.ToString().Length != 2 ? string.Concat("0", closeTimeMin) : closeTimeMin)} {closeTimeFilter}";
        }

    }
}

