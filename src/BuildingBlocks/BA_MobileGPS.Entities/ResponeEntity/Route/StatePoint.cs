using System;

namespace BA_MobileGPS.Entities
{
    public class StatePoint
    {
        public int StartIndex { set; get; }

        public int EndIndex { set; get; }

        public StateType State { set; get; }

        public DateTime StartTime { set; get; }

        public int Duration { set; get; }

        public string StateText { set; get; }
    }

    public enum StateType : byte
    {
        Unknown = 0,
        Normal = 1, // StateData = 0 || SecondsData > 0      bình thường
        Stop = 2, // StateData != 2                          dừng đỗ
        Loss = 3  // StateData = 2                           mất tín hiệu
    }

    /// Trang thai dung do:
    /// StateData=0 Binh thuong
    /// StateData=1: Trang thai dung do bat may
    /// StateData=2: Trang thai mat tin hieu
    /// StateData=3: Trang thai dung do tat may(Neu khong co day co chuyen thanh trang thai dung do)
}