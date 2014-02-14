using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Track_Merger.ExerciseData {
    class CommonExerciseData {
        public UserData UserData { get; private set; }
        public CommonExerciseData(PolarHRM.PolarHRMFile polarHRM) {
            UserData = createUserData(polarHRM);
            DataPoints = polarHRM.GetDataPointsInMetricSystem();
            var PolarTrip = polarHRM.GetTripDataInMetricSystem();
            var PolarLaps = polarHRM.GetLapsInMetricSystem();
            createLapsFromPolarLaps(PolarLaps);
            createSummaryFromPolarTrip(PolarTrip);
            Totals.Time = new DateTimeRange(polarHRM.StartTime, polarHRM.Duration);
            correctLapsFromDataPoints();
            correctTotalsFromDataPoints();
            UpdateCaloriesData();
            correctTotalsTemperatureFromLaps();
            setDataAvailabilityFields(polarHRM);
            Totals.Note = polarHRM.Note;
        }

        public void UpdateUserData(ExerciseData.UserData data, bool currentDataHasPriority) {
            if (data == null) return;
            if (UserData == null) {
                UserData = new UserData();
            }
            replaceValue(ref UserData.Age, ref data.Age, currentDataHasPriority);
            if (data.Gender != null && !(currentDataHasPriority && UserData.Gender != null)) {
                UserData.Gender = data.Gender;
            }
            replaceValue(ref UserData.MaxHR, ref data.MaxHR, currentDataHasPriority);
            replaceValue(ref UserData.RestHR, ref data.RestHR, currentDataHasPriority);
            replaceValue(ref UserData.VO2Max, ref data.VO2Max, currentDataHasPriority);
            replaceValue(ref UserData.Weight, ref data.Weight, currentDataHasPriority);
        }
        
        public static bool NotNullAndNotDefault<T>(Nullable<T> val) where T : struct{
            return val.HasValue && !val.Value.Equals(default(T));
        }
        private void replaceValue<T>(ref T? first, ref T? second, bool firstPriority) where T:struct {
            if (NotNullAndNotDefault(second) && !(firstPriority && NotNullAndNotDefault(first))) {
                first = second;
            }
        }
        private ExerciseData.UserData createUserData(PolarHRM.PolarHRMFile polarHRM) {
            var data = new UserData() {
                Age = polarHRM.UserSettings.Age,
                MaxHR = polarHRM.UserSettings.MaxHR,
                RestHR = polarHRM.UserSettings.RestHR,
                VO2Max = polarHRM.UserSettings.VO2Max,
                Weight = polarHRM.UserSettings.Weight
            };
            return data;
        }

        private void setDataAvailabilityFields(PolarHRM.PolarHRMFile polarHRM) {
            IsAirPressureDataAvailable = polarHRM.IsAirPressureDataAvailable;
            IsAltitudeDataAvailable = polarHRM.IsAltitudeDataAvailable;
            IsBalanceDataAvailable = polarHRM.IsBalanceDataAvailable;
            IsCadenceDataAvailable = polarHRM.IsCadenceDataAvailable;
            IsCyclingDataAvailable = polarHRM.IsCyclingDataAvailable;
            IsPedallingIndexDataAvailable = polarHRM.IsPedallingIndexDataAvailable;
            IsPowerDataAvailable = polarHRM.IsPowerDataAvailable;
            IsSpeedDataAvailable = polarHRM.IsSpeedDataAvailable;
        }

        private void correctTotalsTemperatureFromLaps() {
            double temp = 0;
            foreach (var lap in Laps) {
                temp += lap.Totals.Temperature.Avg * lap.Totals.Time.Duration.TotalSeconds;
            }
            Totals.Temperature = new Range<double>(
                Laps.Min(lap => lap.Totals.Temperature.Min),
                temp / Totals.Time.Duration.TotalSeconds,
                Laps.Max(lap => lap.Totals.Temperature.Max)
                );
        }

        private void correctTotalsFromDataPoints() {
            var sum = calculateSummaryData(Totals.Time);
            if (Totals.Altitude.Max > sum.Altitude.Max) {
                sum.Altitude.Max = Totals.Altitude.Max;
            }
            if (Totals.Speed.Max > sum.Speed.Max) {
                sum.Speed.Max = Totals.Speed.Max;
            }
            if (Totals.Ascent > sum.Ascent) {
                sum.Ascent = Totals.Ascent;
            }
            if (Totals.Distance > sum.Distance) {
                sum.Distance = Totals.Distance;
            }
            Totals = sum;
        }

        private void correctLapsFromDataPoints() {
            var temp = Laps[0].Totals.Temperature.Min;
            foreach (var lap in Laps) {
                var sum = calculateSummaryData(lap.Totals.Time);
                var temp2 = lap.Totals.Temperature.Min;
                sum.Temperature = new Range<double>(
                    Math.Min(temp, temp2),
                    (temp + temp2) / 2,
                    Math.Max(temp, temp2));
                temp = temp2;

                if (lap.Totals.HeartRate.Max > sum.HeartRate.Max) {
                    sum.HeartRate.Max = lap.Totals.HeartRate.Max;
                }
                if (lap.Totals.HeartRate.Min > 0 && lap.Totals.HeartRate.Min < sum.HeartRate.Min) {
                    sum.HeartRate.Min = lap.Totals.HeartRate.Min;
                }
                lap.Totals = sum;
            }
        }
        public void UpdateCaloriesData() {
            Totals.Calories = 0;
            foreach (var lap in Laps) {
                lap.Totals.Calories = GetCaloriesFromDataPoints(lap.Totals.Time);
                Totals.Calories += lap.Totals.Calories;
            }
        }
        public double GetCaloriesFromDataPoints(DateTimeRange range) {
            var points = DataPoints.FindAll((point) => (point.Time >= range.Start && point.Time <= range.End));
            if (points[0].Time > range.Start) {
                points.Insert(0, GetDataPointWithInterpolation(range.Start));
            }
            if (points.Last().Time < range.End) {
                points.Add(GetDataPointWithInterpolation(range.End));
            }
            double calories = 0;
            for (int i = 1; i < points.Count; ++i) {
                calories += calculateCalories((points[i].HeartRate + points[i - 1].HeartRate) / 2, points[i].Time - points[i - 1].Time);
            }
            return calories;
        }
        public Summary calculateSummaryData(DateTimeRange range) {
            var points = DataPoints.FindAll((point) => (point.Time >= range.Start && point.Time <= range.End));
            points.Sort();
            if (points[0].Time > range.Start) {
                points.Insert(0, GetDataPointWithInterpolation(range.Start));
            }
            if (points.Last().Time < range.End) {
                points.Add(GetDataPointWithInterpolation(range.End));
            }
            Summary result = new Summary();
            double dist = 0;
            Range<double> Altitude = new Range<double>(0, 0, 0);
            double ascent = 0;
            Range<double> Cadence = new Range<double>(0, 0, 0);
            double cadenceTime = 0;
            Range<double> HeartRate = new Range<double>(0, 0, 0);
            Range<double> Power = new Range<double>(0, 0, 0);
            Range<double> PowerBalance = new Range<double>(0, 0, 0);
            Range<double> Speed = new Range<double>(0, 0, 0);
            double speedTime = 0;
            for (int i = 1; i < points.Count; ++i) {
                var timeDiff = (points[i].Time - points[i - 1].Time).TotalSeconds;
                dist += (points[i].Speed + points[i - 1].Speed) * timeDiff / 2 / 3600;
                Altitude.Avg += (points[i].Altitude + points[i - 1].Altitude) * timeDiff / 2;
                if (points[i].Altitude > points[i - 1].Altitude) {
                    ascent += points[i].Altitude - points[i - 1].Altitude;
                }
                if (points[i].Cadence > 0 && points[i - 1].Cadence > 0) {
                    Cadence.Avg += (points[i].Cadence + points[i - 1].Cadence) * timeDiff / 2;
                    cadenceTime += timeDiff;
                }
                HeartRate.Avg += (points[i].HeartRate + points[i - 1].HeartRate) * timeDiff / 2;
                Power.Avg += (points[i].Power + points[i - 1].Power) * timeDiff / 2;
                PowerBalance.Avg += (points[i].PowerBalance + points[i - 1].PowerBalance) * timeDiff / 2;
                if (points[i].Speed > 0 && points[i - 1].Speed > 0) {
                    Speed.Avg += (points[i].Speed + points[i - 1].Speed) * timeDiff / 2;
                    speedTime += timeDiff;
                }

            }
            Altitude.Avg = Altitude.Avg / range.Duration.TotalSeconds;
            Cadence.Avg = Cadence.Avg / cadenceTime;
            HeartRate.Avg = HeartRate.Avg / range.Duration.TotalSeconds;
            Power.Avg = Power.Avg / range.Duration.TotalSeconds;
            PowerBalance.Avg = PowerBalance.Avg / range.Duration.TotalSeconds;
            Speed.Avg = Speed.Avg / speedTime;

            Altitude.Min = points.Min(point => point.Altitude);
            Altitude.Max = points.Max(point => point.Altitude);

            Cadence.Min = points.Min(point => point.Cadence > 0 ? point.Cadence : Double.PositiveInfinity);
            Cadence.Max = points.Max(point => point.Cadence > 0 ? point.Cadence : Double.NegativeInfinity);

            HeartRate.Min = points.Min(point => point.HeartRate);
            HeartRate.Max = points.Max(point => point.HeartRate);

            Power.Min = points.Min(point => point.Power);
            Power.Max = points.Max(point => point.Power);

            PowerBalance.Min = points.Min(point => point.PowerBalance);
            PowerBalance.Max = points.Max(point => point.PowerBalance);

            Speed.Min = points.Min(point => point.Speed);
            Speed.Max = points.Max(point => point.Speed);

            return new Summary() {
                Altitude = Altitude,
                Ascent = ascent,
                Cadence = Cadence,
                Calories = GetCaloriesFromDataPoints(range),
                Distance = dist,
                HeartRate = HeartRate,
                Power = Power,
                Speed = Speed,
                Time = range
            };
        }

        internal double calculateCalories(double heartRate, TimeSpan time) {
            /*
             * This calculator is based on the equations (shown below) derived by LR Keytel, 
             * JH Goedecke, TD Noakes, H Hiiloskorpi, R Laukkanen, L van der Merwe, and EV Lambert 
             * for their study titled "Prediction of energy expenditure from heart rate monitoring during submaximal exercise."
             */
            if (UserData.Age == null || UserData.Gender == null || UserData.Weight == null)
                return 0;

            double result = 0;
            if (UserData.VO2Max != null && UserData.VO2Max != 0) {
                if (UserData.Gender == Gender.Male) {
                    result = (0.634 * heartRate + 0.404 * UserData.VO2Max.Value + 0.394 * UserData.Weight.Value + 0.271 * UserData.Age.Value - 95.7735);
                }
                else {
                    result = (0.45 * heartRate + 0.380 * UserData.VO2Max.Value + 0.103 * UserData.Weight.Value + 0.274 * UserData.Age.Value - 59.3954);
                }
            }
            else {
                if (UserData.Gender == Gender.Male) {
                    result = (0.6303 * heartRate + 0.1988 * UserData.Weight.Value + 0.2017 * UserData.Age.Value - 55.0969);
                }
                else {
                    result = (0.4472 * heartRate + 0.1263 * UserData.Weight.Value + 0.074 * UserData.Age.Value - 20.4022);
                }
            }
            return result * 60 * time.TotalHours / 4.184;
        }

        private void createSummaryFromPolarTrip(PolarHRM.PolarHRMFile.TripData PolarTrip) {
            Totals = new Summary() {
                Altitude = new Range<double>(0, PolarTrip.AvgAltitude, PolarTrip.MaxAltitude),
                Ascent = PolarTrip.Ascent,
                Distance = PolarTrip.Distance,
                Speed = new Range<double>(0, PolarTrip.AvgSpeed, PolarTrip.MaxSpeed),
            };
        }

        private void createLapsFromPolarLaps(List<PolarHRM.Lap> PolarLaps) {
            foreach (var lap in PolarLaps) {
                if (!DataPointExists(lap.StartTime + lap.Duration)) {
                    var point = GetDataPointWithInterpolation(lap.StartTime + lap.Duration);
                    point.Altitude = lap.Altitude;
                    point.Cadence = lap.Cadence;
                    point.HeartRate = lap.HR;
                    point.Power = lap.Power;
                    point.PowerBalance = 0;
                    point.Speed = lap.Speed;
                    InsertDataPoint(point);
                }
            }
            Laps = new List<Lap>(PolarLaps.Count);
            foreach (var lap in PolarLaps) {
                Laps.Add(new Lap() {
                    Totals = new Summary() {
                        Distance = lap.Distance,
                        HeartRate = new Range<double>() {
                            Min = lap.HRmin,
                            Avg = lap.HRavg,
                            Max = lap.HRmax
                        },
                        Temperature = new Range<double>() {
                            Min = lap.Temperature,
                            Avg = lap.Temperature,
                            Max = lap.Temperature
                        },
                        Time = new DateTimeRange(lap.StartTime, lap.Duration),
                        Note = lap.Note
                    }
                });
            }
        }
        public bool DataPointExists(DateTime time) {
            return DataPoints.Any(point => point.Time == time);
        }
        public void InsertDataPoint(DataPoint point) {
            var idx = DataPoints.BinarySearch(point);
            if (idx > 0) return;
            DataPoints.Insert(~idx, point);
        }
        public DataPoint GetDataPointWithInterpolation(DateTime time) {
            var idx = DataPoints.BinarySearch(new DataPoint() { Time = time });
            if (idx >= 0) {
                return (DataPoint)DataPoints[idx].Clone();
            }
            idx = ~idx;
            DataPoint point;
            if (idx == 0) {
                point = (DataPoint)DataPoints[0].Clone();
                point.Time = time;
            }
            else if (idx == DataPoints.Count) {
                point = (DataPoint)DataPoints.Last().Clone();
                point.Time = time;
            }
            else {
                point = DataPoint.Interpolate(DataPoints[idx - 1], DataPoints[idx], time);
            }
            return point;
        }

        public List<Lap> Laps { get; private set; }
        public Summary Totals { get; private set; }
        public List<DataPoint> DataPoints { get; private set; }

        public void CalculateDataPointDistances() {
            for (int i = 1; i < DataPoints.Count; ++i) {
                DataPoints[i].Distance = DataPoints[i - 1].Distance + (DataPoints[i].Speed + DataPoints[i - 1].Speed) / 2
                    * ((DataPoints[i].Time - DataPoints[i - 1].Time).TotalSeconds / 3600);
            }
        }
        public void AddGPSData(GPXFile gpxFile, TimeSpan offset) {
            var track = new TrackPointsCollection(gpxFile.GetTrackPoints(offset));
            foreach (var point in DataPoints) {
                var tPoint = track.GetTrackPointAtTime(point.Time);
                point.Latitude = tPoint.latitude;
                point.Longitude = tPoint.longitude;
            }
            var trackPointInRange = track.TrackPoints.FindAll(
                point => point.time >= Totals.Time.Start && point.time <= Totals.Time.End);
            var pointsToAdd = new List<DataPoint>();
            foreach (var point in trackPointInRange) {
                if (DataPointExists(point.time)) continue;
                var dataPoint = GetDataPointWithInterpolation(point.time);
                dataPoint.Longitude = point.longitude;
                dataPoint.Latitude = point.latitude;
                pointsToAdd.Add(dataPoint);
            }
            pointsToAdd.ForEach(point => InsertDataPoint(point));
            IsNavigationDataAvailable = true;
        }

        public bool IsCadenceDataAvailable { get; private set; }

        public bool IsAltitudeDataAvailable { get; private set; }

        public bool IsCyclingDataAvailable { get; private set; }

        public bool IsAirPressureDataAvailable { get; private set; }

        public bool IsSpeedDataAvailable { get; private set; }

        public bool IsPowerDataAvailable { get; private set; }

        public bool IsBalanceDataAvailable { get; private set; }

        public bool IsPedallingIndexDataAvailable { get; private set; }

        public bool IsNavigationDataAvailable { get; private set; }

        public GarminTCX.TCXFile ConvertToTCX() {

            var tcxFile = new GarminTCX.TCXFile();
            var activity = new GarminTCX.Activity() {
                Creator = new GarminTCX.Creator() {
                    Name = "Unknown Device",
                    ProductID = 0,
                    UnitID = 0,
                    Version = new uint[] { 0, 0, 0, 0 }
                },
                Id = Totals.Time.Start,
                Laps = new List<GarminTCX.Lap>(Laps.Count),
                Notes = Totals.Note,
                Sport = GarminTCX.Sport.Other,
            };
            tcxFile.Activities.Add(activity);
            foreach (var lap in Laps) {
                var tcxLap = new GarminTCX.Lap() {
                    StartTime = lap.Totals.Time.Start,
                    TotalTimeSeconds = lap.Totals.Time.Duration.TotalSeconds,
                    DistanceMeters = lap.Totals.Distance * 1000,
                    Calories = (byte)lap.Totals.Calories,
                    Intensity = "Active",
                    TriggerMethod = "Manual",
                    Notes = lap.Totals.Note,
                    AverageHeartRateBpm = new GarminTCX.HeartRate(Utility.RoundByte(lap.Totals.HeartRate.Avg)),
                    MaximumHeartRateBpm = new GarminTCX.HeartRate(Utility.RoundByte(lap.Totals.HeartRate.Max)),
                    Track = new List<GarminTCX.TrackPoint>()
                };
                if (IsCadenceDataAvailable || IsPowerDataAvailable || IsSpeedDataAvailable) {
                    if (tcxLap.Extension == null) {
                        tcxLap.Extension = new GarminTCX.LapExtension();
                    }
                }
                if (IsCadenceDataAvailable) {
                    tcxLap.Cadence = Utility.RoundByte(lap.Totals.Cadence.Avg);
                    tcxLap.Extension.MaxBikeCadence = Utility.RoundUInt(lap.Totals.Cadence.Max);
                }
                if (IsPowerDataAvailable) {
                    tcxLap.Extension.AvgWatts = Utility.RoundUInt(lap.Totals.Power.Avg);
                    tcxLap.Extension.MaxWatts = Utility.RoundUInt(lap.Totals.Power.Max);
                }
                if (IsSpeedDataAvailable) {
                    tcxLap.MaximumSpeed = Utility.KMHToMPS(lap.Totals.Speed.Max);
                    tcxLap.Extension.AvgSpeed = Utility.KMHToMPS(lap.Totals.Speed.Avg);
                }
                foreach (var trkPoint in DataPoints.FindAll(point => (
                    point.Time >= lap.Totals.Time.Start
                    && (point.Time < lap.Totals.Time.End
                        || (lap == Laps.Last() && point.Time == lap.Totals.Time.End)
                    )
                    ))) {

                    var tcxPoint = new GarminTCX.TrackPoint() {
                        Time = trkPoint.Time,
                        HeartRateBpm = new GarminTCX.HeartRate(Utility.RoundByte(trkPoint.HeartRate)),
                    };
                    if (IsPowerDataAvailable || IsSpeedDataAvailable) {
                        tcxPoint.Extension = new GarminTCX.TrackPointExtension();
                    }
                    if (IsAltitudeDataAvailable) {
                        tcxPoint.AltitudeMeters = trkPoint.Altitude;
                    }
                    if (IsCadenceDataAvailable) {
                        tcxPoint.Cadence = Utility.RoundByte(trkPoint.Cadence);
                    }
                    if (IsNavigationDataAvailable) {
                        tcxPoint.Position = new GarminTCX.Position(trkPoint.Latitude, trkPoint.Longitude);
                    }
                    if (IsPowerDataAvailable) {
                        tcxPoint.Extension.Watts = Utility.RoundUInt(trkPoint.Power);
                    }
                    if (IsSpeedDataAvailable) {
                        tcxPoint.DistanceMeters = trkPoint.Distance * 1000;
                        tcxPoint.Extension.Speed = Utility.KMHToMPS(trkPoint.Speed);
                    }
                    tcxLap.Track.Add(tcxPoint);
                }

                activity.Laps.Add(tcxLap);
            }
            return tcxFile;
        }
    }
}
