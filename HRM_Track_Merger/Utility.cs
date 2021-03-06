﻿using System;

namespace HRM_Track_Merger {
    public static class Utility {
        public const double MILE = 1.609344d;
        public const double FEET = 0.3048d;
        public static double MilesToKilometers(double miles) {
            return miles * MILE;
        }
        public static double FeetsToMeters(double feets) {
            return feets * FEET;
        }
        public static double KMHToMPS(double kmh) {
            return kmh / 3.6;
        }
        public static byte RoundByte(double val) {
            return (byte)Math.Round(val, MidpointRounding.AwayFromZero);
        }
        public static uint RoundUInt(double val) {
            return (uint)Math.Round(val, MidpointRounding.AwayFromZero);
        }
        public static double FahrenheitToCelsius(double val) {
            return (val - 32) * 5 / 9;
        }
    }
}
