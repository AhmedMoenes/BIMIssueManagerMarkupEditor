using LiveChartsCore.Drawing;

namespace BIMIssueManagerMarkupsEditor.Views.User
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserSessionService _userSession;

        public ProfileViewModel(UserSessionService userSession)
        {
            _userSession = userSession;
            CurrentUser = _userSession.CurrentUser;

            LoadCharts();
        }

        [ObservableProperty]
        private CurrentUserDto currentUser;

        public IEnumerable<ISeries> CreatedIssueSeries { get; set; }
        public IEnumerable<ISeries> AssignedIssueSeries { get; set; }
        public IEnumerable<ISeries> MonthlyActivitySeries { get; set; }

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        private void LoadCharts()
        {
            CreatedIssueSeries = new ISeries[]
            {
                new PieSeries<int>
                {
                    Values = new[] { CurrentUser.CreatedIssues.Count(i => i.IsResolved) },
                    Name = "Resolved",
                    Fill = new SolidColorPaint(SKColors.SeaGreen)
                },
                new PieSeries<int>
                {
                    Values = new[] { CurrentUser.CreatedIssues.Count(i => !i.IsResolved) },
                    Name = "Unresolved",
                    Fill = new SolidColorPaint(SKColors.OrangeRed)
                }
            };

            AssignedIssueSeries = new ISeries[]
            {
                new PieSeries<int>
                {
                    Values = new[] { CurrentUser.AssignedIssues.Count(i => i.IsResolved) },
                    Name = "Resolved",
                    Fill = new SolidColorPaint(SKColors.MediumSeaGreen)
                },
                new PieSeries<int>
                {
                    Values = new[] { CurrentUser.AssignedIssues.Count(i => !i.IsResolved) },
                    Name = "Unresolved",
                    Fill = new SolidColorPaint(SKColors.IndianRed)
                }
            };

            var groupedByMonth = CurrentUser.CreatedIssues
                .GroupBy(i => i.CreatedAt.ToString("MMM yyyy"))
                .OrderBy(g => g.First().CreatedAt)
                .ToDictionary(g => g.Key, g => g.Count());

                MonthlyActivitySeries = new ISeries[]
                {
                    new ColumnSeries<int>
                    {
                        Values = groupedByMonth.Values.ToArray(),
                        Name = "Issues Created",
                        Fill = new SolidColorPaint(SKColors.SteelBlue)
                    }
                };

                XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = groupedByMonth.Keys.ToArray(),
                        LabelsRotation = 15
                    }
                };

                YAxes = new Axis[]
                {
                    new Axis
                    {
                        Labeler = value => value.ToString("N0"),
                        Name = "Issues Count",
                        MinLimit = 0,
                        MaxLimit = Math.Max(3, groupedByMonth.Values.Max() + 1),
                        LabelsAlignment = Align.Middle,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                        UnitWidth = 1,           
                        MinStep = 1              
                    }
                };
        }
    }
}
