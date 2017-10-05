using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toaster.Interfaces;

namespace Toaster.Device
{
    public class Toaster : IToaster
    {
        double _setting;
        string _content;
        bool _toasting;
        string _color;
        double _time;
        double _rate = 0.5;
        double _heat = 1.0;
        double _doneness;

        static readonly string[] _colors =
        {
            "White", "#EFEBE9", "#D7CCC8", "#BCAAA4", "#A1887F", "#8D6E63", "#795548", "#6D4C41", "#5D4037", "#4E342E",
            "#3E2723", "#212121", "Black"
        };

        public double Setting
        {
            get { return _setting; }
            set
            {
                if (_setting != value)
                {
                    _setting = value;
                    OnPropertyChanged("Setting");
                }
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (_toasting)
                {
                    throw new InvalidOperationException("Cannot set content while toasting");
                }
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        public bool Toasting
        {
            get { return _toasting; }
            set
            {
                if (_toasting == value)
                {
                    throw new InvalidOperationException(_toasting ? "Already toasting" : "Already not toasting");
                }
                _toasting = value;
                if (_toasting)
                {
                    Start();
                }
                OnPropertyChanged("Toasting");
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    _doneness = Math.Max(0, Array.IndexOf(_colors, _color));
                    OnPropertyChanged("Color");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Start()
        {
            _time = 0;
            Next();
        }

        void Next()
        {
            Task.Delay((int)(_rate * 1000))
                .ContinueWith(t =>
                {
                    Update();
                });
        }

        void Update()
        {
            if (_toasting)
            {
                _time += _rate;
                _doneness += _heat;
                int colorIndex = Math.Min((int)_doneness, _colors.Length - 1);
                Color = _colors[colorIndex];
                if (_time >= _setting)
                {
                    Toasting = false;
                }
                else
                {
                    Next();
                }
            }
        }
    }
}
