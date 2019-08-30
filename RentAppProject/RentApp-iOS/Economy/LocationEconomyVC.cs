using Foundation;
using System;
using UIKit;
using Rent.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Rent.Shared.Repositories;
using ModuleLibraryiOS.Alert;
using System.Threading.Tasks;
using System.Globalization;
using Nager.Date;
using System.Linq;
using System.Collections.Generic;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class LocationEconomyVC : UITableViewController
    {
        public LocationEconomyVC (IntPtr handle) : base (handle) { }

        EconomyRepository _economyRepository = AppDelegate.ServiceProvider.GetService<EconomyRepository>();
        HoursRepository _hourRepository = AppDelegate.ServiceProvider.GetService<HoursRepository>();

        int? LocationID;
        int? CustomerID;
        List<LocationEconomy> _locationEconomys;
        List<LocationHours> _locationHours;

        bool updateable = true;

        List<DateTime> holidays;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CultureInfo.CurrentCulture = new CultureInfo("da-DK");

            holidays = DateSystem.GetPublicHoliday("DK", DateTime.Now.Year).Select(h => h.Date).ToList();

            var palmSunday = holidays[3].AddDays(-7);
            var christmasEve = holidays[9].AddDays(-1);
            var newYearseve = new DateTime(DateTime.Now.Year, 12, 31);

            holidays.Insert(3, palmSunday);
            holidays.Insert(10, christmasEve);
            holidays.Insert(13, newYearseve);

            if(LocationID != null)
            {
                _hourRepository.Get((int)LocationID, async (hours) =>
                {
                    _locationHours = new List<LocationHours>() { hours };
                    await _economyRepository.GetForLocation((int)LocationID, (obj) => {
                        _locationEconomys = new List<LocationEconomy>() { obj };

                        PriceCleaning.Text = obj.PriceRegularCleaning + "";
                        PriceWindowCleaning.Text = obj.PriceWindowCleaning + "";
                        PricePerHour.Text = GetPricePerHour(obj.PricePerHourCategory) + "";
                        PriceCategory.Text = obj.PricePerHourCategory.ToString();
                        StartDate.Text = obj.StartDate?.ToString("dd-MM-yy");
                        UpdateResult();
                    });
                }).LoadingOverlay(this);

                PriceCleaning.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationEconomys[0].PriceRegularCleaning = n);
                PriceWindowCleaning.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationEconomys[0].PriceWindowCleaning = n);
                PricePerHour.EditingDidBegin += (sender, e) => 
                {
                    PricePerHour.ResignFirstResponder();
                    this.Start<SimplePickerVC>().Setup("Vælg brugerrolle", new [] { "Drift " + GetPricePerHour((PricePerHourCategory)0), "Hotel " + GetPricePerHour((PricePerHourCategory)1), "Rent " + GetPricePerHour((PricePerHourCategory)2), "UL " + GetPricePerHour((PricePerHourCategory)3) }, (index) =>
                    {
                        _locationEconomys[0].PricePerHourCategory = (PricePerHourCategory) index;
                        PriceCategory.Text = _locationEconomys[0].PricePerHourCategory.ToString();
                        PricePerHour.Text = GetPricePerHour(_locationEconomys[0].PricePerHourCategory) + "";
                        Update();
                        UpdateResult();
                    });
                };
                StartDate.EditingDidBegin += (sender, e) =>
                {
                    StartDate.ResignFirstResponder();
                    this.Start<SimpleDatePickerVC>().Setup("Vælg startDato", (obj) =>
                    {
                        obj.RightNavigationButton("Fjern", () =>
                        {
                            this.NavigationController.PopViewController(true);
                            StartDate.Text = "-";
                            _locationEconomys[0].StartDate = null;
                            Update();
                            UpdateResult();
                        });
                    }, (obj) =>
                    {
                        StartDate.Text = obj.ToString("dd-MM-yy");
                        _locationEconomys[0].StartDate = obj;
                        Update();
                        UpdateResult();
                    });
                };
            }
            else {
                PriceCleaning.UserInteractionEnabled = false;
                PriceWindowCleaning.UserInteractionEnabled = false;
                PricePerHour.UserInteractionEnabled = false;
                PriceCategory.UserInteractionEnabled = false;
                //ProductPercentage.UserInteractionEnabled = false;
                StartDate.UserInteractionEnabled = false;

                _hourRepository.GetForCustomer((int)CustomerID, async (hours) =>
                {
                    _locationHours = hours.ToList();
                    await _economyRepository.GetForCustomer((int)CustomerID, (economies) => {
                        _locationEconomys = economies.ToList();

                        PriceCleaning.Text = economies.Sum(e => e.PriceRegularCleaning) + "";
                        PriceWindowCleaning.Text = economies.Sum(e => e.PriceWindowCleaning) + "";
                        PricePerHour.Text = "-";
                        //ProductPercentage.Text = "-" + economies.Sum(e => (e.PriceRegularCleaning + e.PriceWindowCleaning) * 0.03) + "";
                        UpdateResult();
                    });
                }).LoadingOverlay(this);
            }
        }

        void TryParseFloat(UITextField tf, Action<float> success)
        {
            if (float.TryParse(tf.Text, out var result))
            {
                tf.TextColor = UIColor.Black;
                success(result);
                Update();
                UpdateResult();
            }
            else
            {
                tf.TextColor = UIColor.Red;
            }
        }

        public void UpdateResult()
        {
            float[][][] months = new float[_locationHours.Count][][];
            for (int i = 0; i < _locationHours.Count; i++)
            {
                months[i] = GetMonthsForLocation(_locationHours[i], _locationEconomys[i]);
            }

            SetValue(Jan, (months.Sum(m => m[0][1]) * 0.97 - months.Sum(m => m[0][0])));
            SetValue(Feb, (months.Sum(m => m[1][1]) * 0.97 - months.Sum(m => m[1][0])));
            SetValue(Mar, (months.Sum(m => m[2][1]) * 0.97 - months.Sum(m => m[2][0])));
            SetValue(Apr, (months.Sum(m => m[3][1]) * 0.97 - months.Sum(m => m[3][0])));
            SetValue(May, (months.Sum(m => m[4][1]) * 0.97 - months.Sum(m => m[4][0])));
            SetValue(Jun, (months.Sum(m => m[5][1]) * 0.97 - months.Sum(m => m[5][0])));
            SetValue(Jul, (months.Sum(m => m[6][1]) * 0.97 - months.Sum(m => m[6][0])));
            SetValue(Aug, (months.Sum(m => m[7][1]) * 0.97 - months.Sum(m => m[7][0])));
            SetValue(Sep, (months.Sum(m => m[8][1]) * 0.97 - months.Sum(m => m[8][0])));
            SetValue(Oct, (months.Sum(m => m[9][1]) * 0.97 - months.Sum(m => m[9][0])));
            SetValue(Nov, (months.Sum(m => m[10][1]) * 0.97 - months.Sum(m => m[10][0])));
            SetValue(Dec, (months.Sum(m => m[11][1]) * 0.97 - months.Sum(m => m[11][0])));

            var monthlyPrice = _locationEconomys.Sum(e => e.PriceRegularCleaning + e.PriceWindowCleaning);
            var yearlyPrice = months.Sum(m => m.Sum(e => e[1]));
            var wageCost = months.Sum(m => m.Sum(e => e[0]));
            var productPercentage = yearlyPrice * 0.03f;
            var totalExpense = wageCost + productPercentage;
            var earnings = yearlyPrice - totalExpense;
            var dg = earnings / yearlyPrice;

            SetValue(TotalMonthlyPrice, monthlyPrice);
            SetValue(TotalYearlyPrice, yearlyPrice);
            SetValue(WageCost, -wageCost);
            SetValue(ProductPercentage, -productPercentage);
            SetValue(TotalExpense, -totalExpense);
            SetValue(Earning, earnings);
            SetValue(DG, dg, "P");
        }

        void SetValue(UILabel label, double value, string format = "C")
        {
            if (value > 0)
                label.TextColor = UIColor.FromRGB(50, 122, 42);
            else if (value < 0)
                label.TextColor = UIColor.Red;
            else label.TextColor = UIColor.Black;

            label.Text = value.ToString(format);
        }

        float[][] GetMonthsForLocation(LocationHours hours, LocationEconomy economy)
        {
            float[][] months = new float[12][];

            var date = new DateTime(DateTime.Now.Year, 1, 1);
            var thisYear = date.Year;

            while (date.Year == thisYear)
            {
                int[] week = new int[14];

                int thisMonth = date.Month;

                int daysThisMonth = 0;
                int daysChargedThisMonth = 0;

                while (date.Month == thisMonth && date.Year == thisYear)
                {
                    daysThisMonth++;
                    if (Include(date, hours) && (economy.StartDate == null || date >= economy.StartDate))
                    {
						daysChargedThisMonth++;
						week[((int)date.DayOfWeek) + (7 * (GetIso8601WeekOfYear(date) % 2 * (hours.DifferentWeeks ? 1 : 0)))]++;
                    }
                    date = date.AddDays(1);
                }

                float totalHours = 0;

                for (int i = 0; i < 14; i++)
                {
                    int days = week[i];

                    if (i == 0) totalHours += days * hours.L_Sun;
                    if (i == 1) totalHours += days * hours.L_Mon;
                    if (i == 2) totalHours += days * hours.L_Tue;
                    if (i == 3) totalHours += days * hours.L_Wed;
                    if (i == 4) totalHours += days * hours.L_Thu;
                    if (i == 5) totalHours += days * hours.L_Fri;
                    if (i == 6) totalHours += days * hours.L_Sat;
                    if (i == 7) totalHours += days * hours.U_Sun;
                    if (i == 8) totalHours += days * hours.U_Mon;
                    if (i == 9) totalHours += days * hours.U_Tue;
                    if (i == 10) totalHours += days * hours.U_Wed;
                    if (i == 11) totalHours += days * hours.U_Thu;
                    if (i == 12) totalHours += days * hours.U_Fri;
                    if (i == 13) totalHours += days * hours.U_Sat;
                }
                months[thisMonth - 1] = new float[2];
                months[thisMonth - 1][0] = totalHours * GetPricePerHour(economy.PricePerHourCategory);
                months[thisMonth - 1][1] = ((float)daysChargedThisMonth / (float)daysThisMonth) * (economy.PriceRegularCleaning + economy.PriceWindowCleaning);
                System.Diagnostics.Debug.WriteLine(thisMonth + "  " +  totalHours + " - " + months[thisMonth - 1][0] + " % " + months[thisMonth - 1][1]);
            }
            return months;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            Update(true);
        }

        async void Update(bool overrule = false)
        {
            if (!updateable && !overrule)
                return;
            updateable = false;
            foreach(var e in _locationEconomys)
                await _economyRepository.Update(e, () => { });
            await Task.Delay(5000);
            updateable = true;
        }

        public void Setup(int? locationID, int? customerID)
        {
            LocationID = locationID;
            CustomerID = customerID;
        }




        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        } 

        public  bool Include(DateTime date, LocationHours hours)
        {
            if(!hours.NewyearsDay && SameDate(date, holidays[0])) return false;
            if(!hours.Palmsunday && SameDate(date, holidays[1])) return false;
            if(!hours.MaundyThursday && SameDate(date, holidays[2])) return false;
            if(!hours.GoodFriday && SameDate(date, holidays[3])) return false;
            if(!hours.EasterDay && SameDate(date, holidays[4])) return false;
            if(!hours.SecondEasterDay && SameDate(date, holidays[5])) return false;
            if(!hours.PrayerDay && SameDate(date, holidays[6])) return false;
            if(!hours.ChristAscension && SameDate(date, holidays[7])) return false;
            if(!hours.WhitSunday && SameDate(date, holidays[8])) return false;
            if(!hours.SndPentecost && SameDate(date, holidays[9])) return false;
            if(!hours.ChristmasEve && SameDate(date, holidays[10])) return false;
            if(!hours.ChristmasDay && SameDate(date, holidays[11])) return false;
            if(!hours.SndChristmasDay && SameDate(date, holidays[12])) return false;
            if(!hours.NewyearsEve && SameDate(date, holidays[13])) return false;

            return true;
        }

        bool SameDate(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
        }

        public int GetPricePerHour(PricePerHourCategory category)
        {
            if (category == Rent.Shared.Models.PricePerHourCategory.Drift)
                return 205;
            if (category == Rent.Shared.Models.PricePerHourCategory.Hotel)
                return 175;
            if (category == Rent.Shared.Models.PricePerHourCategory.Rent)
                return 180;
            if (category == Rent.Shared.Models.PricePerHourCategory.UL)
                return 175;
            return 0;
        }
    }
}