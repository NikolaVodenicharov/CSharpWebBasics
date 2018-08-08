﻿namespace SimpleMvc.Framework.Models
{
    using System;
    using System.Collections.Generic;

    public class ViewModel
    {
        public ViewModel()
        {
            this.Data = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data { get; }

        public string this[string key]
        {
            get => this.Data[key];
            set => this.Data[key] = value;
        }
    }
}
