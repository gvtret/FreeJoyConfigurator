﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidLibrary;
using Prism.Mvvm;
using System.Windows.Threading;

namespace FreeJoyConfigurator
{

    public enum ReportID : byte
    {
        JOY_REPORT = 1,
        CONFIG_IN_REPORT,
        CONFIG_OUT_REPORT,
        FIRMWARE_REPORT,
    };

    public static class ReportConverter
    {

        public static void ReportToJoystick(ref Joystick joystick, HidReport hr)
        {
            if (hr != null)
            {
                for (int i=0; i<joystick.Buttons.Count; i++)
                {
                    joystick.Buttons[i].State = (hr.Data[1 + (i & 0xF8)>>3] & (1<<(i & 0x07))) > 0 ? true : false;
                }

                for (int i = 0; i < joystick.Axes.Count; i++)
                {
                    joystick.Axes[i].Value =  (ushort) (hr.Data[17 + 2 * i] << 8 |  hr.Data[16 + 2 * i]);
                }

                for (int i = 0; i < joystick.Axes.Count; i++)
                {
                    joystick.Axes[i].RawValue = (ushort)(hr.Data[37 + 2 * i] << 8 | hr.Data[36 + 2 * i]);
                }
            }
        }
    

        public static void ReportToConfig (ref DeviceConfig config, HidReport hr)
        {
            if (hr.Data[0] == 1)
            {
                char[] chars = new char[20];

                config.FirmwareVersion = (ushort)(hr.Data[2] << 8 | hr.Data[1]);
                for (int i=0;i<20;i++)
                {
                    
                    chars[i] = (char)hr.Data[i + 3];
                    if (chars[i] == 0) break;   // end of string
                }
                config.DeviceName = new string(chars);
                config.ButtonDebounceMs = (ushort)(hr.Data[24] << 8 | hr.Data[23]);
                config.TogglePressMs = (ushort)(hr.Data[26] << 8 | hr.Data[25]);
                config.EncoderPressMs = (ushort)(hr.Data[28] << 8 | hr.Data[27]);
                config.ExchangePeriod = (ushort)(hr.Data[30] << 8 | hr.Data[29]);

                for (int i = 0; i < config.PinConfig.Count; i++)
                {
                    config.PinConfig[i] = (PinType)hr.Data[i + 32];
                }
                
            }
            else if (hr.Data[0] == 2)
            {
                config.AxisConfig[0] = new AxisConfig();
                config.AxisConfig[0].CalibMin = (ushort)(hr.Data[2] << 8 | hr.Data[1]);
                config.AxisConfig[0].CalibCenter = (ushort)(hr.Data[4] << 8 | hr.Data[3]);
                config.AxisConfig[0].CalibMax = (ushort)(hr.Data[6] << 8 | hr.Data[5]);
                config.AxisConfig[0].IsAutoCalib = Convert.ToBoolean(hr.Data[7]);
                config.AxisConfig[0].IsInverted = Convert.ToBoolean(hr.Data[8]);
                config.AxisConfig[0].FilterLevel = hr.Data[9];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[0].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[10 + i]);
                }

                config.AxisConfig[1] = new AxisConfig();
                config.AxisConfig[1].CalibMin = (ushort)(hr.Data[32] << 8 | hr.Data[31]);
                config.AxisConfig[1].CalibCenter = (ushort)(hr.Data[34] << 8 | hr.Data[33]);
                config.AxisConfig[1].CalibMax = (ushort)(hr.Data[36] << 8 | hr.Data[35]);
                config.AxisConfig[1].IsAutoCalib = Convert.ToBoolean(hr.Data[37]);
                config.AxisConfig[1].IsInverted = Convert.ToBoolean(hr.Data[38]);
                config.AxisConfig[1].FilterLevel = hr.Data[39];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[1].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[40 + i]);
                }

            }
            else if (hr.Data[0] == 3)
            {
                config.AxisConfig[2] = new AxisConfig();
                config.AxisConfig[2].CalibMin = (ushort)(hr.Data[2] << 8 | hr.Data[1]);
                config.AxisConfig[2].CalibCenter = (ushort)(hr.Data[4] << 8 | hr.Data[3]);
                config.AxisConfig[2].CalibMax = (ushort)(hr.Data[6] << 8 | hr.Data[5]);
                config.AxisConfig[2].IsAutoCalib = Convert.ToBoolean(hr.Data[7]);
                config.AxisConfig[2].IsInverted = Convert.ToBoolean(hr.Data[8]);
                config.AxisConfig[2].FilterLevel = hr.Data[9];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[2].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[10 + i]);
                }

                config.AxisConfig[3] = new AxisConfig();
                config.AxisConfig[3].CalibMin = (ushort)(hr.Data[32] << 8 | hr.Data[31]);
                config.AxisConfig[3].CalibCenter = (ushort)(hr.Data[34] << 8 | hr.Data[33]);
                config.AxisConfig[3].CalibMax = (ushort)(hr.Data[36] << 8 | hr.Data[35]);
                config.AxisConfig[3].IsAutoCalib = Convert.ToBoolean(hr.Data[37]);
                config.AxisConfig[3].IsInverted = Convert.ToBoolean(hr.Data[38]);
                config.AxisConfig[3].FilterLevel = hr.Data[39];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[3].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[40 + i]);
                }               
            }
            else if (hr.Data[0] == 4)
            {
                config.AxisConfig[4] = new AxisConfig();
                config.AxisConfig[4].CalibMin = (ushort)(hr.Data[2] << 8 | hr.Data[1]);
                config.AxisConfig[4].CalibCenter = (ushort)(hr.Data[4] << 8 | hr.Data[3]);
                config.AxisConfig[4].CalibMax = (ushort)(hr.Data[6] << 8 | hr.Data[5]);
                config.AxisConfig[4].IsAutoCalib = Convert.ToBoolean(hr.Data[7]);
                config.AxisConfig[4].IsInverted = Convert.ToBoolean(hr.Data[8]);
                config.AxisConfig[4].FilterLevel = hr.Data[9];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[4].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[10 + i]);
                }

                config.AxisConfig[5] = new AxisConfig();
                config.AxisConfig[5].CalibMin = (ushort)(hr.Data[32] << 8 | hr.Data[31]);
                config.AxisConfig[5].CalibCenter = (ushort)(hr.Data[34] << 8 | hr.Data[33]);
                config.AxisConfig[5].CalibMax = (ushort)(hr.Data[36] << 8 | hr.Data[35]);
                config.AxisConfig[5].IsAutoCalib = Convert.ToBoolean(hr.Data[37]);
                config.AxisConfig[5].IsInverted = Convert.ToBoolean(hr.Data[38]);
                config.AxisConfig[5].FilterLevel = hr.Data[39];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[5].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[40 + i]);
                }
            }
            else if (hr.Data[0] == 5)
            {
                config.AxisConfig[6] = new AxisConfig();
                config.AxisConfig[6].CalibMin = (ushort)(hr.Data[2] << 8 | hr.Data[1]);
                config.AxisConfig[6].CalibCenter = (ushort)(hr.Data[4] << 8 | hr.Data[3]);
                config.AxisConfig[6].CalibMax = (ushort)(hr.Data[6] << 8 | hr.Data[5]);
                config.AxisConfig[6].IsAutoCalib = Convert.ToBoolean(hr.Data[7]);
                config.AxisConfig[6].IsInverted = Convert.ToBoolean(hr.Data[8]);
                config.AxisConfig[6].FilterLevel = hr.Data[9];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[6].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[10 + i]);
                }

                config.AxisConfig[7] = new AxisConfig();
                config.AxisConfig[7].CalibMin = (ushort)(hr.Data[32] << 8 | hr.Data[31]);
                config.AxisConfig[7].CalibCenter = (ushort)(hr.Data[34] << 8 | hr.Data[33]);
                config.AxisConfig[7].CalibMax = (ushort)(hr.Data[36] << 8 | hr.Data[35]);
                config.AxisConfig[7].IsAutoCalib = Convert.ToBoolean(hr.Data[37]);
                config.AxisConfig[7].IsInverted = Convert.ToBoolean(hr.Data[38]);
                config.AxisConfig[7].FilterLevel = hr.Data[39];
                for (int i = 0; i < 10; i++)
                {
                    config.AxisConfig[7].CurveShape[i] = new System.Windows.Point(i, (sbyte)hr.Data[40 + i]);
                }
            }
            else if (hr.Data[0] == 6)
            {
                // buttons group 1
                for (int i=0;i<62;i++)
                {
                    config.ButtonConfig[i].Type = (ButtonType)hr.Data[i + 1];
                }
            }
            else if (hr.Data[0] == 7)
            {
                // buttons group 2
                for (int i = 0; i < 62; i++)
                {
                    config.ButtonConfig[i + 62].Type = (ButtonType)hr.Data[i + 1];
                }
            }
            else if (hr.Data[0] == 8)
            {
                // buttons group 3
                for (int i = 0; i < 4; i++)
                {
                    config.ButtonConfig[i + 124].Type = (ButtonType)hr.Data[i + 1];
                }

                // axes to buttons group 1
                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[0].Points[i] = (sbyte)hr.Data[5 + i];
                }
                config.AxisToButtonsConfig[0].ButtonsCnt = (byte)hr.Data[17];
                config.AxisToButtonsConfig[0].IsAnalogEnabled = (hr.Data[18] > 0) ? true : false;

                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[1].Points[i] = (sbyte)hr.Data[19 + i];
                }
                config.AxisToButtonsConfig[1].ButtonsCnt = (byte)hr.Data[31];
                config.AxisToButtonsConfig[1].IsAnalogEnabled = (hr.Data[32] > 0) ? true : false;

                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[2].Points[i] = (sbyte)hr.Data[33 + i];
                }
                config.AxisToButtonsConfig[2].ButtonsCnt = (byte)hr.Data[45];
                config.AxisToButtonsConfig[2].IsAnalogEnabled = (hr.Data[46] > 0) ? true : false;

                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[3].Points[i] = (sbyte)hr.Data[47 + i];
                }
                config.AxisToButtonsConfig[3].ButtonsCnt = (byte)hr.Data[59];
                config.AxisToButtonsConfig[3].IsAnalogEnabled = (hr.Data[60] > 0) ? true : false;
            }
            else if (hr.Data[0] == 9)
            {
                // axes to buttons group 2
                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[4].Points[i] = (sbyte)hr.Data[1 + i];
                }
                config.AxisToButtonsConfig[4].ButtonsCnt = (byte)hr.Data[13];
                config.AxisToButtonsConfig[4].IsAnalogEnabled = (hr.Data[14] > 0) ? true : false;

                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[5].Points[i] = (sbyte)hr.Data[15 + i];
                }
                config.AxisToButtonsConfig[5].ButtonsCnt = (byte)hr.Data[27];
                config.AxisToButtonsConfig[5].IsAnalogEnabled = (hr.Data[28] > 0) ? true : false;

                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[6].Points[i] = (sbyte)hr.Data[29 + i];
                }
                config.AxisToButtonsConfig[6].ButtonsCnt = (byte)hr.Data[41];
                config.AxisToButtonsConfig[6].IsAnalogEnabled = (hr.Data[42] > 0) ? true : false;

                for (int i = 0; i < 12; i++)
                {
                    config.AxisToButtonsConfig[7].Points[i] = (sbyte)hr.Data[43 + i];
                }
                config.AxisToButtonsConfig[7].ButtonsCnt = (byte)hr.Data[55];
                config.AxisToButtonsConfig[7].IsAnalogEnabled = (hr.Data[56] > 0) ? true : false;
            }
            else if (hr.Data[0] == 10)
            {

            }
        }

        public static List<HidReport> ConfigToReports (DeviceConfig config)
        {
            List<HidReport> hidReports = new List<HidReport>();
            byte[] buffer = new byte[64];
            byte[] chars;

            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = (byte) 0x01;
            buffer[2] = (byte)(config.FirmwareVersion & 0xFF);
            buffer[3] = (byte) (config.FirmwareVersion >> 8);            
            chars = Encoding.ASCII.GetBytes(config.DeviceName);
            Array.ConstrainedCopy(chars, 0, buffer, 4, (chars.Length > 20) ? 20 : chars.Length);
            buffer[24] = (byte)(config.ButtonDebounceMs & 0xFF);
            buffer[25] = (byte)(config.ButtonDebounceMs >> 8);
            buffer[26] = (byte)(config.TogglePressMs & 0xFF);
            buffer[27] = (byte)(config.TogglePressMs >> 8);
            buffer[28] = (byte)(config.EncoderPressMs & 0xFF);
            buffer[29] = (byte)(config.EncoderPressMs >> 8);
            buffer[30] = (byte)(config.ExchangePeriod & 0xFF);
            buffer[31] = (byte)(config.ExchangePeriod >> 8);           
            for (int i = 0; i < 30; i++)
            {
                buffer[i + 33] = (byte)config.PinConfig[i];
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x02;
            buffer[2] = (byte)(config.AxisConfig[0].CalibMin & 0xFF);
            buffer[3] = (byte)(config.AxisConfig[0].CalibMin >> 8);
            buffer[4] = (byte)(config.AxisConfig[0].CalibCenter & 0xFF);
            buffer[5] = (byte)(config.AxisConfig[0].CalibCenter >> 8);
            buffer[6] = (byte)(config.AxisConfig[0].CalibMax & 0xFF);
            buffer[7] = (byte)(config.AxisConfig[0].CalibMax >> 8);            
            buffer[8] = (byte)(config.AxisConfig[0].IsAutoCalib ? 0x01 : 0x00);
            buffer[9] = (byte)(config.AxisConfig[0].IsInverted ? 0x01 : 0x00);
            buffer[10] = (byte)(config.AxisConfig[0].FilterLevel);
            for (int i=0; i<10; i++)
            {
                buffer[i + 11] = (byte)config.AxisConfig[0].CurveShape[i].Y;
            }
            buffer[32] = (byte)(config.AxisConfig[1].CalibMin & 0xFF);
            buffer[33] = (byte)(config.AxisConfig[1].CalibMin >> 8);
            buffer[34] = (byte)(config.AxisConfig[1].CalibCenter & 0xFF);
            buffer[35] = (byte)(config.AxisConfig[1].CalibCenter >> 8);
            buffer[36] = (byte)(config.AxisConfig[1].CalibMax & 0xFF);
            buffer[37] = (byte)(config.AxisConfig[1].CalibMax >> 8);            
            buffer[38] = (byte)(config.AxisConfig[1].IsAutoCalib ? 0x01 : 0x00);
            buffer[39] = (byte)(config.AxisConfig[1].IsInverted ? 0x01 : 0x00);
            buffer[40] = (byte)(config.AxisConfig[1].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 41] = (byte)config.AxisConfig[1].CurveShape[i].Y;
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x03;
            buffer[2] = (byte)(config.AxisConfig[2].CalibMin & 0xFF);
            buffer[3] = (byte)(config.AxisConfig[2].CalibMin >> 8);
            buffer[4] = (byte)(config.AxisConfig[2].CalibCenter & 0xFF);
            buffer[5] = (byte)(config.AxisConfig[2].CalibCenter >> 8);
            buffer[6] = (byte)(config.AxisConfig[2].CalibMax & 0xFF);
            buffer[7] = (byte)(config.AxisConfig[2].CalibMax >> 8);
            buffer[8] = (byte)(config.AxisConfig[2].IsAutoCalib ? 0x01 : 0x00);
            buffer[9] = (byte)(config.AxisConfig[2].IsInverted ? 0x01 : 0x00);
            buffer[10] = (byte)(config.AxisConfig[2].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 11] = (byte)config.AxisConfig[2].CurveShape[i].Y;
            }
            buffer[32] = (byte)(config.AxisConfig[3].CalibMin & 0xFF);
            buffer[33] = (byte)(config.AxisConfig[3].CalibMin >> 8);
            buffer[34] = (byte)(config.AxisConfig[3].CalibCenter & 0xFF);
            buffer[35] = (byte)(config.AxisConfig[3].CalibCenter >> 8);
            buffer[36] = (byte)(config.AxisConfig[3].CalibMax & 0xFF);
            buffer[37] = (byte)(config.AxisConfig[3].CalibMax >> 8);
            buffer[38] = (byte)(config.AxisConfig[3].IsAutoCalib ? 0x01 : 0x00);
            buffer[39] = (byte)(config.AxisConfig[3].IsInverted ? 0x01 : 0x00);
            buffer[40] = (byte)(config.AxisConfig[3].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 41] = (byte)config.AxisConfig[3].CurveShape[i].Y;
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x04;
            buffer[2] = (byte)(config.AxisConfig[4].CalibMin & 0xFF);
            buffer[3] = (byte)(config.AxisConfig[4].CalibMin >> 8);
            buffer[4] = (byte)(config.AxisConfig[4].CalibCenter & 0xFF);
            buffer[5] = (byte)(config.AxisConfig[4].CalibCenter >> 8);
            buffer[6] = (byte)(config.AxisConfig[4].CalibMax & 0xFF);
            buffer[7] = (byte)(config.AxisConfig[4].CalibMax >> 8);
            buffer[8] = (byte)(config.AxisConfig[4].IsAutoCalib ? 0x01 : 0x00);
            buffer[9] = (byte)(config.AxisConfig[4].IsInverted ? 0x01 : 0x00);
            buffer[10] = (byte)(config.AxisConfig[4].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 11] = (byte)config.AxisConfig[4].CurveShape[i].Y;
            }
            buffer[32] = (byte)(config.AxisConfig[5].CalibMin & 0xFF);
            buffer[33] = (byte)(config.AxisConfig[5].CalibMin >> 8);
            buffer[34] = (byte)(config.AxisConfig[5].CalibCenter & 0xFF);
            buffer[35] = (byte)(config.AxisConfig[5].CalibCenter >> 8);
            buffer[36] = (byte)(config.AxisConfig[5].CalibMax & 0xFF);
            buffer[37] = (byte)(config.AxisConfig[5].CalibMax >> 8);
            buffer[38] = (byte)(config.AxisConfig[5].IsAutoCalib ? 0x01 : 0x00);
            buffer[39] = (byte)(config.AxisConfig[5].IsInverted ? 0x01 : 0x00);
            buffer[40] = (byte)(config.AxisConfig[5].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 41] = (byte)config.AxisConfig[5].CurveShape[i].Y;
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x05;
            buffer[2] = (byte)(config.AxisConfig[6].CalibMin & 0xFF);
            buffer[3] = (byte)(config.AxisConfig[6].CalibMin >> 8);
            buffer[4] = (byte)(config.AxisConfig[6].CalibCenter & 0xFF);
            buffer[5] = (byte)(config.AxisConfig[6].CalibCenter >> 8);
            buffer[6] = (byte)(config.AxisConfig[6].CalibMax & 0xFF);
            buffer[7] = (byte)(config.AxisConfig[6].CalibMax >> 8);
            buffer[8] = (byte)(config.AxisConfig[6].IsAutoCalib ? 0x01 : 0x00);
            buffer[9] = (byte)(config.AxisConfig[6].IsInverted ? 0x01 : 0x00);
            buffer[10] = (byte)(config.AxisConfig[6].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 11] = (byte)config.AxisConfig[6].CurveShape[i].Y;
            }
            buffer[32] = (byte)(config.AxisConfig[7].CalibMin & 0xFF);
            buffer[33] = (byte)(config.AxisConfig[7].CalibMin >> 8);
            buffer[34] = (byte)(config.AxisConfig[7].CalibCenter & 0xFF);
            buffer[35] = (byte)(config.AxisConfig[7].CalibCenter >> 8);
            buffer[36] = (byte)(config.AxisConfig[7].CalibMax & 0xFF);
            buffer[37] = (byte)(config.AxisConfig[7].CalibMax >> 8);
            buffer[38] = (byte)(config.AxisConfig[7].IsAutoCalib ? 0x01 : 0x00);
            buffer[39] = (byte)(config.AxisConfig[7].IsInverted ? 0x01 : 0x00);
            buffer[40] = (byte)(config.AxisConfig[7].FilterLevel);
            for (int i = 0; i < 10; i++)
            {
                buffer[i + 41] = (byte)config.AxisConfig[7].CurveShape[i].Y;
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x06;
            for (int i=0; i<62; i++)
            {
                buffer[i + 2] = (byte) config.ButtonConfig[i].Type;
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x07;
            for (int i = 0; i < 62; i++)
            {
                buffer[i + 2] = (byte)config.ButtonConfig[i + 62].Type;
            }
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x08;
            for (int i = 0; i < 4; i++)
            {
                buffer[i + 2] = (byte)config.ButtonConfig[i + 124].Type;
            }
            // axes to buttons 1
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 6] = (byte)config.AxisToButtonsConfig[0].Points[i];
            }
            buffer[18] = (byte)config.AxisToButtonsConfig[0].ButtonsCnt;
            buffer[19] = (byte)(config.AxisToButtonsConfig[0].IsAnalogEnabled ? 0x01 : 0x00);
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 20] = (byte)config.AxisToButtonsConfig[1].Points[i];
            }
            buffer[32] = (byte)config.AxisToButtonsConfig[1].ButtonsCnt;
            buffer[33] = (byte)(config.AxisToButtonsConfig[1].IsAnalogEnabled ? 0x01 : 0x00);
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 34] = (byte)config.AxisToButtonsConfig[2].Points[i];
            }
            buffer[46] = (byte)config.AxisToButtonsConfig[2].ButtonsCnt;
            buffer[47] = (byte)(config.AxisToButtonsConfig[2].IsAnalogEnabled ? 0x01 : 0x00);
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 48] = (byte)config.AxisToButtonsConfig[3].Points[i];
            }
            buffer[60] = (byte)config.AxisToButtonsConfig[3].ButtonsCnt;
            buffer[61] = (byte)(config.AxisToButtonsConfig[3].IsAnalogEnabled ? 0x01 : 0x00);

            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x09;

            // axes to buttons
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 2] = (byte)config.AxisToButtonsConfig[4].Points[i];
            }
            buffer[14] = (byte)config.AxisToButtonsConfig[4].ButtonsCnt;
            buffer[15] = (byte)(config.AxisToButtonsConfig[4].IsAnalogEnabled ? 0x01 : 0x00);
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 16] = (byte)config.AxisToButtonsConfig[5].Points[i];
            }
            buffer[28] = (byte)config.AxisToButtonsConfig[5].ButtonsCnt;
            buffer[29] = (byte)(config.AxisToButtonsConfig[5].IsAnalogEnabled ? 0x01 : 0x00);
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 30] = (byte)config.AxisToButtonsConfig[6].Points[i];
            }
            buffer[42] = (byte)config.AxisToButtonsConfig[6].ButtonsCnt;
            buffer[43] = (byte)(config.AxisToButtonsConfig[6].IsAnalogEnabled ? 0x01 : 0x00);
            for (int i = 0; i < 12; i++)
            {
                buffer[i + 44] = (byte)config.AxisToButtonsConfig[7].Points[i];
            }
            buffer[56] = (byte)config.AxisToButtonsConfig[7].ButtonsCnt;
            buffer[57] = (byte)(config.AxisToButtonsConfig[7].IsAnalogEnabled ? 0x01 : 0x00);
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            buffer.Initialize();
            buffer[0] = (byte)ReportID.CONFIG_OUT_REPORT;
            buffer[1] = 0x0A;
            hidReports.Add(new HidReport(64, new HidDeviceData(buffer, HidDeviceData.ReadStatus.Success)));

            return hidReports;
        }
    }
}