using Foundation;
using System;
using UIKit;
using Rent.Shared.Models;
using Rent.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Alert;
using System.Threading.Tasks;

namespace RentApp
{
    public partial class LocationHourVC : UITableViewController
    {
        public LocationHourVC (IntPtr handle) : base (handle) { }

        HoursRepository _hourRepository = AppDelegate.ServiceProvider.GetService<HoursRepository>();

        int _locationID;

        LocationHours _locationHours;

        bool updateable = true;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _hourRepository.Get(_locationID,(obj) => {
                _locationHours = obj;

                DifferentWeeks.On = _locationHours.DifferentWeeks;
                DifferentWeeksChanged(_locationHours.DifferentWeeks);

                SetStartValue(L_Mon, _locationHours.L_Mon);
                SetStartValue(L_Tue, _locationHours.L_Tue);
                SetStartValue(L_Wed, _locationHours.L_Wed);
                SetStartValue(L_Thu, _locationHours.L_Thu);
                SetStartValue(L_Fri, _locationHours.L_Fri);
                SetStartValue(L_Sat, _locationHours.L_Sat);
                SetStartValue(L_Sun, _locationHours.L_Sun);

                SetStartValue(U_Mon, _locationHours.U_Mon);
                SetStartValue(U_Tue, _locationHours.U_Tue);
                SetStartValue(U_Wed, _locationHours.U_Wed);
                SetStartValue(U_Thu, _locationHours.U_Thu);
                SetStartValue(U_Fri, _locationHours.U_Fri);
                SetStartValue(U_Sat, _locationHours.U_Sat);
                SetStartValue(U_Sun, _locationHours.U_Sun);

                NewyearsDay.On = _locationHours.NewyearsDay;
                Palmsunday.On = _locationHours.Palmsunday;
                MaundyThursday.On = _locationHours.MaundyThursday;
                GoodFriday.On = _locationHours.GoodFriday;
                EasterDay.On = _locationHours.EasterDay;
                SecondEasterDay.On = _locationHours.SecondEasterDay;
                PrayerDay.On = _locationHours.PrayerDay;
                ChristAscension.On = _locationHours.ChristAscension;
                WhitSunday.On = _locationHours.WhitSunday;
                SndPentecost.On = _locationHours.SndPentecost;
                ChristmasEve.On = _locationHours.ChristmasEve;
                ChristmasDay.On = _locationHours.ChristmasDay;
                SndChristmasDay.On = _locationHours.SndChristmasDay;
                NewyearsEve.On = _locationHours.NewyearsEve;

                UpdateTotal();
            }).LoadingOverlay(this);

            L_Mon.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Mon = n);
            L_Tue.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Tue = n);
            L_Wed.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Wed = n);
            L_Thu.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Thu = n);
            L_Fri.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Fri = n);
            L_Sat.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Sat = n);
            L_Sun.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.L_Sun = n);

            U_Mon.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Mon = n);
            U_Tue.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Tue = n);
            U_Wed.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Wed = n);
            U_Thu.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Thu = n);
            U_Fri.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Fri = n);
            U_Sat.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Sat = n);
            U_Sun.EditingChanged += (sender, e) => TryParseFloat(((UITextField)sender), (n) => _locationHours.U_Sun = n);

            NewyearsDay.ValueChanged += (sender, e) => _locationHours.NewyearsDay = ((UISwitch)sender).On;
            Palmsunday.ValueChanged += (sender, e) => _locationHours.Palmsunday = ((UISwitch)sender).On;
            MaundyThursday.ValueChanged += (sender, e) => _locationHours.MaundyThursday = ((UISwitch)sender).On;
            GoodFriday.ValueChanged += (sender, e) => _locationHours.GoodFriday = ((UISwitch)sender).On;
            EasterDay.ValueChanged += (sender, e) => _locationHours.EasterDay = ((UISwitch)sender).On;
            SecondEasterDay.ValueChanged += (sender, e) => _locationHours.SecondEasterDay = ((UISwitch)sender).On;
            PrayerDay.ValueChanged += (sender, e) => _locationHours.PrayerDay = ((UISwitch)sender).On;
            ChristAscension.ValueChanged += (sender, e) => _locationHours.ChristAscension = ((UISwitch)sender).On;
            WhitSunday.ValueChanged += (sender, e) => _locationHours.WhitSunday = ((UISwitch)sender).On;
            SndPentecost.ValueChanged += (sender, e) => _locationHours.SndPentecost = ((UISwitch)sender).On;
            ChristmasEve.ValueChanged += (sender, e) => _locationHours.ChristmasEve = ((UISwitch)sender).On;
            ChristmasDay.ValueChanged += (sender, e) => _locationHours.ChristmasDay = ((UISwitch)sender).On;
            SndChristmasDay.ValueChanged += (sender, e) => _locationHours.SndChristmasDay = ((UISwitch)sender).On;
            NewyearsEve.ValueChanged += (sender, e) => _locationHours.NewyearsEve = ((UISwitch)sender).On;

            DifferentWeeks.ValueChanged += (sender, e) =>
            {
                DifferentWeeksChanged(DifferentWeeks.On);
                UpdateTotal();
            };
        }

        void DifferentWeeksChanged(bool different)
        {
            _locationHours.DifferentWeeks = different;
            Update();
            UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveEaseOut, () => {
                View.LayoutIfNeeded();
                U_Mon.Hidden = !different;
                U_Tue.Hidden = !different;
                U_Wed.Hidden = !different;
                U_Thu.Hidden = !different;
                U_Fri.Hidden = !different;
                U_Sat.Hidden = !different;
                U_Sun.Hidden = !different;
                UnevenWeeksLabel.Hidden = !different;
            }, () => { });
        }

        void TryParseFloat(UITextField tf, Action<float> success = null)
        {
            var text = tf.Text.Replace('.', ',');
            if (string.IsNullOrEmpty(text))
                text = "0";

            if (float.TryParse(text, out var result))
            {
                if (result == 0)
                    tf.Text = "";

                tf.TextColor = UIColor.Black;

                if(success != null)
                {
                    success(result);
                    Update();
                }
                UpdateTotal();
            } else {
                tf.TextColor = UIColor.Red;
            }
        }

        void SetStartValue(UITextField tf, float n)
        {
            if (n != 0)
                tf.Text = n + "";
        }

        void UpdateTotal()
        {
            var total = 0.0;
            total += _locationHours.L_Mon;
            total += _locationHours.L_Tue;
            total += _locationHours.L_Wed;
            total += _locationHours.L_Thu;
            total += _locationHours.L_Fri;
            total += _locationHours.L_Sat;
            total += _locationHours.L_Sun;

            if(_locationHours.DifferentWeeks)
            {
                var totalTwo = 0.0;
                totalTwo += _locationHours.U_Mon;
                totalTwo += _locationHours.U_Tue;
                totalTwo += _locationHours.U_Wed;
                totalTwo += _locationHours.U_Thu;
                totalTwo += _locationHours.U_Fri;
                totalTwo += _locationHours.U_Sat;
                totalTwo += _locationHours.U_Sun;

                total = (total + totalTwo) / 2;
            }

            WeeklyTotal.Text = total.ToString("0.00");
        }

        async void Update(bool overrule = false)
        {
            if (!updateable && !overrule)
                return;
            updateable = false;
            await _hourRepository.Update(_locationHours, () => {
                System.Diagnostics.Debug.WriteLine("success");
            }, () => {
                System.Diagnostics.Debug.WriteLine("error");
            });
            await Task.Delay(5000);
            updateable = true;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            Update(true);
        }

        public void Setup(int locationID)
        {
            _locationID = locationID;
        }
    }
}