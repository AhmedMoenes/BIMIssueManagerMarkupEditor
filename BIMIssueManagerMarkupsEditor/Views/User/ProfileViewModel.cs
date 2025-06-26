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

        [ObservableProperty] private IEnumerable<ISeries> createdIssueSeries;
        public bool HasCreatedIssueData => CreatedIssueSeries?.OfType<PieSeries<int>>()
            .Any(s => s.Values?.Sum() > 0) == true;
        partial void OnCreatedIssueSeriesChanged(IEnumerable<ISeries> value)
        {
            OnPropertyChanged(nameof(HasCreatedIssueData));
        }

        [ObservableProperty] private IEnumerable<ISeries> assignedIssueSeries;
        public bool HasAssignedIssueData => AssignedIssueSeries?.OfType<PieSeries<int>>()
            .Any(s => s.Values?.Sum() > 0) == true;
        partial void OnAssignedIssueSeriesChanged(IEnumerable<ISeries> value)
        {
            OnPropertyChanged(nameof(HasAssignedIssueData));
        }
        [ObservableProperty] private IEnumerable<ISeries> monthlyActivitySeries;
        public bool HasMonthlyActivityData => MonthlyActivitySeries?.OfType<ColumnSeries<int>>()
            .Any(s => s.Values?.Sum() > 0) == true;
        partial void OnMonthlyActivitySeriesChanged(IEnumerable<ISeries> value)
        {
            OnPropertyChanged(nameof(HasMonthlyActivityData));
        }
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

            var monthValues = groupedByMonth.Values.ToArray();
            var monthLabels = groupedByMonth.Keys.ToArray();

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

                int maxValue = monthValues.DefaultIfEmpty(0).Max();

            YAxes = new Axis[]
                {
                    new Axis
                    {
                        Labeler = value => value.ToString("N0"),
                        Name = "Issues Count",
                        NamePaint = new SolidColorPaint(SKColors.SlateGray)
                        {
                            SKTypeface = SKTypeface.FromFamilyName(
                                null, 
                                SKFontStyle.BoldItalic)
                        }, 
                        NameTextSize = 16,
                        MinLimit = 0,
                        MaxLimit = Math.Max(3, maxValue + 1),
                        LabelsAlignment = Align.Middle,
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                        UnitWidth = 1,
                        MinStep = 1
                    }
                };
        }
    }
}
